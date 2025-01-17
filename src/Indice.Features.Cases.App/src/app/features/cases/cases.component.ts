import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { BaseListComponent, Icons, IResultSet, ListViewType, MenuOption, ModalService, RouterViewAction, ViewAction } from '@indice/ng-components';
import { SearchOption } from '@indice/ng-components/lib/controls/advanced-search/models';
import { forkJoin, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { CasePartial, CasePartialResultSet, CasesApiService, } from 'src/app/core/services/cases-api.service';
import { QueriesModalComponent } from 'src/app/shared/components/query-modal/query-modal.component';

@Component({
    selector: 'app-cases',
    templateUrl: './cases.component.html'
})
export class CasesComponent extends BaseListComponent<CasePartial> implements OnInit {
    public newItemLink = 'new-case';
    public formActions: ViewAction[] = [
        new RouterViewAction(Icons.EntryView, 'queries', 'rightpane', 'Οι αναζητήσεις μου', 'Οι αναζητήσεις μου')
    ];
    public queryParamsHasFilter = false;

    constructor(
        private _route: ActivatedRoute,
        private _router: Router,
        private _api: CasesApiService,
        private _modalService: ModalService
    ) {
        super(_route, _router);
        this.view = ListViewType.Table;
        this.pageSize = 10;
        this.sort = 'createdByWhen';
        this.sortdir = 'asc';
        this.sortOptions = [
            new MenuOption('Ημ. Υποβολής', 'createdByWhen')
        ];
    }

    public ngOnInit(): void {
        // Are there any filters in queryParams?
        this._route.queryParams.subscribe((params: Params) => {
            this.queryParamsHasFilter = params['filter'] ? true : false;
        });
        forkJoin({
            caseTypes: this._api.getCaseTypes(),
            checkpointTypes: this._api.getDistinctCheckpointCodes()
        }).pipe(take(1)).subscribe(({ caseTypes, checkpointTypes }) => {
            const caseTypeSearchOption: SearchOption = {
                field: 'caseTypeCodes',
                name: 'ΤΥΠΟΣ ΑΙΤΗΣΗΣ',
                dataType: 'array',
                options: [],
                multiTerm: true
            }
            for (let caseType of caseTypes.items!) { // fill caseTypeSearchOption's SelectInputOptions
                caseTypeSearchOption.options?.push({ value: caseType.code, label: caseType?.title! })
            }
            const checkpointTypeSearchOption: SearchOption = {
                field: 'checkpointTypeCodes',
                name: 'ΤΡΕΧΟΝ ΣΗΜΕΙΟ ΕΛΕΓΧΟΥ',
                dataType: 'array',
                options: [],
                multiTerm: true
            }
            for (let checkpointType of checkpointTypes) { // fill checkpointTypeSearchOption's SelectInputOptions
                checkpointTypeSearchOption.options?.push({ value: checkpointType, label: checkpointType })
            }
            this.searchOptions = [
                {
                    field: 'customerId',
                    name: 'ΚΩΔΙΚΟΣ ΠΕΛΑΤΗ',
                    dataType: 'string'
                },
                {
                    field: 'customerName',
                    name: 'ΟΝΟΜΑ ΠΕΛΑΤΗ',
                    dataType: 'string'
                },
                {
                    field: 'TaxId', // this must be exactly the same "case-wise" with db's json property!
                    name: 'Α.Φ.Μ. ΠΕΛΑΤΗ',
                    dataType: 'string'
                },
                {
                    field: 'groupIds',
                    name: 'ΑΡΙΘΜΟΣ ΚΑΤΑΣΤΗΜΑΤΟΣ',
                    dataType: 'string',
                    multiTerm: true
                },
                {
                    field: 'dateRange',
                    name: 'ΗΜ. ΥΠΟΒΟΛΗΣ',
                    dataType: 'daterange'
                },
                caseTypeSearchOption,
                checkpointTypeSearchOption
            ];
            // now that we have the searchOptions, call parent's ngOnInit!
            super.ngOnInit();
        });

        // independent call to fetch the case Types that the user can select for Case Creation
        this._api.getCaseTypes(true)
            .pipe(take(1))
            .subscribe(
                (caseTypesForCaseCreation: CasePartialResultSet) => {
                    if (caseTypesForCaseCreation.count !== 0) {
                        this.formActions.unshift(
                            new RouterViewAction(Icons.Add, this.newItemLink, 'rightpane', 'Υποβολή νέας αίτησης', 'Νέα αίτηση')
                        );
                    }
                }
            );
    }

    openQueryModal(): void {
        this._modalService.show(QueriesModalComponent, {
            backdrop: 'static',
            keyboard: false
        });
    }

    loadItems(): Observable<IResultSet<CasePartial> | null | undefined> {
        let customerId = this.filters?.find(f => f.member === 'customerId')?.value;
        let customerName = this.filters?.find(f => f.member === 'customerName')?.value;
        let groupIds: string[] = [];
        this.filters?.filter(f => f.member === 'groupIds')?.forEach(f => groupIds?.push(f.value));
        let from = this.filters?.find(f => f.member === 'from')?.value;
        let to = this.filters?.find(f => f.member === 'to')?.value;
        let caseTypeCodes: string[] = [];
        this.filters?.filter(f => f.member === 'caseTypeCodes')?.forEach(f => caseTypeCodes?.push(f.value));
        let checkpointTypeCodes: string[] = [];
        this.filters?.filter(f => f.member === 'checkpointTypeCodes')?.forEach(f => checkpointTypeCodes?.push(f.value));
        let filterMetadata: string[] = [];
        this.filters?.filter(f => f.member === 'TaxId')?.forEach(f => filterMetadata?.push(`metadata.${f.member}::eq::(${f.dataType})${f.value}`)); // this is the form that the server accepts
        return this._api
            .getCases(
                customerId,
                customerName,
                from ? new Date(from) : undefined,
                to ? new Date(to) : undefined,
                caseTypeCodes,
                checkpointTypeCodes,
                groupIds,
                filterMetadata,
                this.page,
                this.pageSize,
                this.sortdir === 'asc' ? this.sort! : this.sort + '-',
                this.search || undefined,
                undefined
            )
            .pipe(
                take(1),
                map((result: CasePartialResultSet) => (result as IResultSet<CasePartial>))
            );
    }
}
