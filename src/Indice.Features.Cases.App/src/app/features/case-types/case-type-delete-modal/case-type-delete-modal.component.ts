import { catchError, tap } from 'rxjs/operators';
import { Component, OnInit } from '@angular/core';
import { Modal, ToasterService, ToastType } from '@indice/ng-components';
import { CasesApiService } from 'src/app/core/services/cases-api.service';
import { EMPTY } from 'rxjs';

@Component({
  selector: 'app-case-type-delete-modal',
  templateUrl: './case-type-delete-modal.component.html',
  styleUrls: ['./case-type-delete-modal.component.scss']
})
export class CaseTypeDeleteModalComponent implements OnInit {

  public id: string = '';
  public name?: string;
  constructor(private modal: Modal, private _api: CasesApiService, private toaster: ToasterService) { }

  ngOnInit(): void {
  }

  deleteCaseType() {
    this._api.deleteCaseType(this.id).pipe(
      tap(_ => {
        this.toaster.show(ToastType.Success, "Επιτυχία!", "Η διαγραφή του τύπου αίτησης ολοκληρώθηκε");
        this.closeModal(true);
      }),
      catchError(err => {
        this.toaster.show(ToastType.Error, "Whoops!", err.detail);
        this.closeModal(false);
        return EMPTY
      })
    ).subscribe();
  }

  public closeModal(result: any) {
    this.modal.hide(result);
  }
}
