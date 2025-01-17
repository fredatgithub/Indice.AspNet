import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ApprovalButtonsComponent } from './components/approval-buttons/approval-buttons.component';
import { PageIllustrationComponent } from './components/page-illustration/page-illustration.component';
import { RadioButtonsListComponent } from './components/radio-buttons-list/radio-buttons-list.component';
import { ToggleButtonComponent } from './components/toggle-button/toggle-button.component';
import { BeautifyBooleanPipe } from './pipes.services';
import { TailwindFrameworkComponent } from './ajsf/json-schema-frameworks/tailwind-framework/tailwind-framework.component';
import { JsonSchemaFormModule } from '@ajsf-extended/core';
import { SubmitWidgetComponent } from './ajsf/json-schema-frameworks/tailwind-framework/submit-widget/submit-widget.component';
import { SelectCaseTypeComponent } from './components/select-case-type/select-case-type.component';
import { SearchCustomerComponent } from './components/search-customer/search-customer.component';
import { IndiceComponentsModule } from '@indice/ng-components';
import { SelectWidgetComponent } from './ajsf/json-schema-frameworks/tailwind-framework/select-widget/select-widget.component';
import { CurrencyWidgetComponent } from './ajsf/json-schema-frameworks/tailwind-framework/currency-widget/currency-widget.component';
import { NgxMaskModule } from 'ngx-mask'
import { DateWidgetComponent } from './ajsf/json-schema-frameworks/tailwind-framework/date-widget/date-widget.component';
import { LookupWidgetComponent } from './ajsf/json-schema-frameworks/tailwind-framework/lookup-widget/lookup-widget.component';
import { InputWidgetComponent } from './ajsf/json-schema-frameworks/tailwind-framework/input-widget/input-widget.component';
import { TextAreaWidgetComponent } from './ajsf/json-schema-frameworks/tailwind-framework/text-area-widget/text-area-widget.component';
import { FileWidgetComponent } from './ajsf/json-schema-frameworks/tailwind-framework/file-widget/file-widget.component';
import { CaseDetailInfoComponent } from './components/case-detail-info/case-detail-info.component';
import { CaseCustomActionComponent } from './components/case-custom-action/case-custom-action.component';
import { LookupSelectorWidgetComponent } from './ajsf/json-schema-frameworks/tailwind-framework/lookup-selector-widget/lookup-selector-widget.component';



@NgModule({
  declarations: [
    // components
    ApprovalButtonsComponent,
    PageIllustrationComponent,
    RadioButtonsListComponent,
    ToggleButtonComponent,
    SelectCaseTypeComponent,
    SearchCustomerComponent,
    CaseDetailInfoComponent,
    CaseCustomActionComponent,
    // ajsf
    FileWidgetComponent,
    TailwindFrameworkComponent,
    SelectWidgetComponent,
    SubmitWidgetComponent,
    CurrencyWidgetComponent,
    DateWidgetComponent,
    LookupWidgetComponent,
    InputWidgetComponent,
    TextAreaWidgetComponent,
    LookupSelectorWidgetComponent,
    // pipes
    BeautifyBooleanPipe
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    JsonSchemaFormModule,
    IndiceComponentsModule,
    NgxMaskModule.forRoot()
  ],
  exports: [
    // components
    ApprovalButtonsComponent,
    PageIllustrationComponent,
    RadioButtonsListComponent,
    ToggleButtonComponent,
    SelectCaseTypeComponent,
    SearchCustomerComponent,
    CaseDetailInfoComponent,
    // ajsf
    TailwindFrameworkComponent,
    // pipes
    BeautifyBooleanPipe,
    CaseCustomActionComponent
  ]
})
export class SharedModule { }
