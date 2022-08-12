import { ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { ToasterService, ToastType } from '@indice/ng-components';
import { EMPTY, forkJoin, Observable } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { CaseDetails, CasesApiService, CasesAttachmentLink, EditCaseRequest } from 'src/app/core/services/cases-api.service';
import { UploadFileWidgetService } from 'src/app/core/services/file-upload.service';
import { JSFFileWidgetComponent } from 'src/app/shared/ajsf/jsf-file-widget.component';
import { TailwindSubmitWidgetComponent } from 'src/app/shared/ajsf/json-schema-frameworks/tailwind-framework/submit-widget/submit-widget.component';
import { TailwindFrameworkComponent } from 'src/app/shared/ajsf/json-schema-frameworks/tailwind-framework/tailwind-framework.component';
import { SelectWidgetComponent } from 'src/app/shared/ajsf/json-schema-frameworks/tailwind-framework/select-widget/select-widget.component';
import { get, isEqual } from 'lodash';
import { CurrencyWidgetComponent } from 'src/app/shared/ajsf/json-schema-frameworks/tailwind-framework/currency-widget/currency-widget.component';
import { DateWidgetComponent } from 'src/app/shared/ajsf/json-schema-frameworks/tailwind-framework/date-widget/date-widget.component';
import { LookupWidgetComponent } from 'src/app/shared/ajsf/json-schema-frameworks/tailwind-framework/lookup-widget/lookup-widget.component';

@Component({
  selector: 'app-case-form',
  templateUrl: './case-form.component.html',
  styleUrls: ['./case-form.component.scss']
})
export class CaseFormComponent implements OnChanges, OnInit {
  private latestModel: any;
  private latestIsValid: boolean = false;
  // form is editable?
  @Input() formEditable: boolean | undefined;
  @Input() case: CaseDetails | undefined;
  // inform parent that data should be refreshed
  @Output() updateDataEvent = new EventEmitter<{ draft: boolean }>();
  @Output() formIsValid = new EventEmitter<boolean>();
  @Output() unSavedChanges = new EventEmitter<boolean>();
  // Add custom widget
  public widgets = {
    "file": JSFFileWidgetComponent,
    "submit": TailwindSubmitWidgetComponent,
    "select": SelectWidgetComponent,
    "lookup": LookupWidgetComponent,
    "date": DateWidgetComponent,
    "currency": CurrencyWidgetComponent
  };
  // Add custom framework
  public framework = {
    framework: TailwindFrameworkComponent
  };
  public jsonFormOptions: any = {
    addSubmit: false, // Don't show a submit button if layout does not have one
    setSchemaDefaults: false, // Always use schema defaults for empty fields
    draft: false
  };
  private copiedLayout: any;
  private initialData: any;
  public formIsDirty: boolean = false;
  public showForm = true;

  public data: any;
  public schema: any;
  public layout: any;
  public lastChange: any;
  onChangeCallbackDictionary: any = {};

  @ViewChild('formContainer') formContainer: ElementRef | undefined;

  constructor(
    private _toaster: ToasterService,
    private _api: CasesApiService,
    private uploadFileWidgetService: UploadFileWidgetService,
    private changeDetector: ChangeDetectorRef
  ) { }

  ngOnChanges(changes: SimpleChanges) {
    if (!this.case) { // no case -> no point to continue execution
      return;
    }
    if (changes.hasOwnProperty('case')) {
      // deep copy layout
      this.copiedLayout = JSON.parse(this.case.caseType?.layout!);
      /**
       * delete empty strings and clear potential empty objects in data
       * for ajsf (so that onChanges can work as expected - ajsf, at initialization, deletes empty objects and strings)
       */
      if (this.case.data) {
        this.initialData = this.removeEmptyObjects(this.deleteEmptyStringProperties(JSON.parse(this.case.data!)));
      }
    }
    // extract layout from case type
    let layout = JSON.parse(this.case.caseType?.layout!);
    // transform layout only when formEditable input is changed
    if (changes.hasOwnProperty('formEditable')) {
      this.transformLayout(layout, !this.formEditable, this.case.draft);
      // here we need to check if case has become editable: we need to "enable" checkboxes
      // note: this won't affect checkboxes that have "disableCheckbox" class in their layout!
      if (this.formContainer?.nativeElement && (this.formEditable || this.case?.draft)) {
        this.formContainer.nativeElement.classList.remove('disableCheckbox');
      }
    }
    this.jsonFormOptions.draft = this.case.draft;
    this.jsonFormOptions.setSchemaDefaults = this.case.draft; // use schema defaults only for draft cases!
    this.jsonFormOptions.addSubmit = this.case.draft || this.formEditable;
    this.schema = JSON.parse(this.case.caseType?.dataSchema!);
    this.layout = layout;
    this.data = this.initialData;
    this.populateForm(this.data);
    this.changeDetector.detectChanges();
  }

  ngOnInit(): void { }

  public ngAfterViewInit(): void {
    // check if we need to "disable" case's checkboxes
    if (!this.formEditable && !this.case?.draft) {
      this.formContainer?.nativeElement.classList.add('disableCheckbox');
    }
  }

  public onSubmit(event: any): void {
    if (this.case?.draft) {
      // we have to send user's document(s) to the server
      const callsDict: { [key: string]: Observable<CasesAttachmentLink> } = {};
      // what is the key of the dictionary here? the dataPointer (e.g. "/homeAddress/attachmentId") that was added in the file-upload widget
      for (const key in this.uploadFileWidgetService.files) {
        if (this.uploadFileWidgetService.files.hasOwnProperty(key)) {
          const fileParam = { data: this.uploadFileWidgetService.files[key], fileName: this.uploadFileWidgetService.files[key].name };
          callsDict[key] = this._api.uploadAdminCaseAttachment(this.case.id!, undefined, fileParam);
        }
      }
      forkJoin(callsDict)
        .pipe(
          tap(
            // we can match the received attachment guids to the respective fields with the help of the keys/dataPointers!
            (response: { [key: string]: CasesAttachmentLink }) => {
              let stringifiedData = JSON.stringify(event);
              for (const key in response) {
                if (response.hasOwnProperty(key)) {
                  // we simply replace the dataPointer with the guid that we received from the server
                  stringifiedData = stringifiedData.replace(key, response[key].id!);
                }
              }
              // finally, submit the case
              this._api.submitAdminCase(this.case?.id!, stringifiedData)
                .pipe(
                  tap(() => {
                    this.uploadFileWidgetService.reset();
                    this._toaster.show(ToastType.Success, 'Επιτυχής Επεξεργασία', `Η επεξεργασία της αίτησης ολοκληρώθηκε.`, 5000);
                    this.updateDataEvent.emit({ draft: true });
                  })
                )
                .subscribe();
            }))
        .subscribe();
    } else {
      const editCaseRequest = new EditCaseRequest({ data: JSON.stringify(event) });
      this._api.editCase(this.case?.id!, undefined, editCaseRequest)
        .pipe(
          tap(() => {
            this.initialData = event; // initial data are, now, the saved data
            this.formIsDirty = false;
            this.unSavedChanges.emit(this.formIsDirty);
            this.updateDataEvent.emit({ draft: false });
            this._toaster.show(ToastType.Success, 'Επιτυχής αποθήκευση', `Οι αλλαγές σας αποθηκεύτηκαν με επιτυχία.`, 5000);
            this.uploadFileWidgetService.reset();
          }),
          catchError(() => {
            this._toaster.show(ToastType.Error, 'Αποτυχία αποθήκευσης', `Δεν κατέστη εφικτό να αποθηκευτούν οι αλλαγές σας.`, 5000);
            return EMPTY;
          })
        )
        .subscribe();
    }
  }

  // property-level onChange implementation from:
  // https://github.com/hamzahamidi/ajsf/issues/37
  onFormChanges(entity: any) {
    if (!this.case?.draft) { // this logic applies only for non-draft cases
      // this will only work if setSchemaDefaults is set to FALSE, because, at initialization, ajsf adds to data the properties with default values from schema!
      if (!isEqual(entity, this.initialData)) {
        this.formIsDirty = true;
        this.unSavedChanges.emit(this.formIsDirty);
      } else {
        this.formIsDirty = false;
        this.unSavedChanges.emit(this.formIsDirty);
      }
    }

    // ajsf's (onChanges) is very trigger happy and often is invoked with a fake empty
    // object, so ignore it
    if (Object.keys(entity).length === 0 && entity.constructor === Object) {
      return;
    }

    for (const [key, onChange] of Object.entries(this.onChangeCallbackDictionary)) {
      if (get(entity, key) === get(this.lastChange, key)) {
        // no changes, no need to invoke onChange
        continue;
      }

      // check needed so typescript won't complain when compiling
      if (typeof onChange === 'function') {
        onChange(get(entity, key), entity);
      }
    }
    this.lastChange = entity;
  }

  public isValid(event: boolean) {
    this.formIsValid.emit(event);
    this.latestIsValid = event;
  }

  onSubmitExternal(): void {
    if (this.latestIsValid) {
      this.onSubmit(this.latestModel);
    }
  }

  onCancel(): void {
    this.showForm = false;
    this.changeDetector.detectChanges(); // enforce the instantaneous deletion of form     
    this.schema = JSON.parse(this.case?.caseType?.dataSchema!);
    this.layout = JSON.parse(this.case?.caseType?.layout!);
    this.data = this.initialData;
    this.populateForm(this.data);
    this.showForm = true;
    this.changeDetector.detectChanges();
  }

  private transformLayout(layout: any, viewOnlyMode: boolean = true, draft?: boolean): void {
    // first check for draft, and override json layout
    if (draft) {
      // case is draft, we need all form properties available and ignore layout restrictions (eg readonly, or html class disable)
      this.removeReadonlyProperties(layout);
      return;
    }
    // case is not draft, check for editability
    if (!viewOnlyMode) {
      // form is editable      
      layout = this.copiedLayout;
      this.jsonFormOptions.addSubmit = true;
    } else {
      this.addReadonlyProperties(layout);
    }
  }

  private removeReadonlyProperties(layout: any): void {
    const activateElement = (element: any) => {
      delete element.readonly;
      if (element.htmlClass && element.htmlClass.indexOf('disableCheckbox') > -1) {
        element.htmlClass = element.htmlClass.replace('disableCheckbox', '');
      }
    }
    layout.forEach((element: any) => {
      activateElement(element);
      if (element.hasOwnProperty('items')) { // ajsf sections have items!
        element.items.forEach((item: any) => {
          activateElement(item);
          if (item.hasOwnProperty('items')) { // ajsf flex containers have items!
            item.items.forEach((i: any) => {
              activateElement(i);
            });
          }
        });
      }
    });
  }

  private addReadonlyProperties(layout: any): void {
    // form is view-only -> add readonly property to all objects of layout object!
    layout.forEach((element: any) => {
      element.readonly = "true";
      if (element.hasOwnProperty('items')) { // ajsf sections have items!
        element.items.forEach((item: any) => {
          item.readonly = "true";
          if (item.hasOwnProperty('items')) { // ajsf flex containers have items!
            item.items.forEach((i: any) => {
              i.readonly = "true";
            });
          }
        });
      }
    });
  }

  private deleteEmptyStringProperties(obj: any) {
    for (var k in obj) {
      if (!obj[k] || typeof obj[k] !== "object") {
        if (obj[k] === '') {
          delete obj[k];
        }
        continue // If null or not an object, skip to the next iteration
      }

      // The property is an object
      this.deleteEmptyStringProperties(obj[k]); // <-- Make a recursive call on the nested object
    }
    return obj;
  }

  private removeEmptyObjects(obj: any) {
    for (var k in obj) {
      if (!obj[k] || typeof obj[k] !== "object") {
        continue // If null or not an object, skip to the next iteration
      }

      // The property is an object
      this.removeEmptyObjects(obj[k]); // <-- Make a recursive call on the nested object
      if (Object.keys(obj[k]).length === 0) {
        delete obj[k]; // The object had no properties, so delete that property
      }
    }
    return obj;
  }

  private populateForm(entity: any) {
    this.lastChange = entity;
    const onInitCallbackDictionary: any = {};

    if (this.layout) {
      // recursively traverse the form json
      const traverse = (item: any) => {
        if (item.items) {
          item.items.map(traverse);
        } else if (item.tabs) {
          item.tabs.map(traverse);
        }

        // keep track of onChange callbacks
        if (item.onChange) {
          this.onChangeCallbackDictionary[item.key] = Function('"use strict";return (' + item.onChange + ')')();
        }

        if (item.onInit) {
          onInitCallbackDictionary[item.key] = Function('"use strict";return (' + item.onInit + ')')();
        }
      };
      this.layout.map(traverse);
    }

    for (const [key, onInit] of Object.entries(onInitCallbackDictionary)) {
      if (typeof onInit === 'function') {
        onInit(get(entity, key), entity);
      }
    }
  }

}