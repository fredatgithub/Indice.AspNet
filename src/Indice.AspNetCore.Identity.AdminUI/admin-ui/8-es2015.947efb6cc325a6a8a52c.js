(window.webpackJsonp=window.webpackJsonp||[]).push([[8],{fQZA:function(e,t,i){"use strict";i.d(t,"a",(function(){return J}));var o=i("xgIS"),n=i("lJxs"),r=i("pLZG"),c=i("7o/Q"),s=i("D0XW");class a{constructor(e,t){this.dueTime=e,this.scheduler=t}call(e,t){return t.subscribe(new l(e,this.dueTime,this.scheduler))}}class l extends c.a{constructor(e,t,i){super(e),this.dueTime=t,this.scheduler=i,this.debouncedSubscription=null,this.lastValue=null,this.hasValue=!1}_next(e){this.clearDebounce(),this.lastValue=e,this.hasValue=!0,this.add(this.debouncedSubscription=this.scheduler.schedule(u,this.dueTime,this))}_complete(){this.debouncedNext(),this.destination.complete()}debouncedNext(){if(this.clearDebounce(),this.hasValue){const{lastValue:e}=this;this.lastValue=null,this.hasValue=!1,this.destination.next(e)}}clearDebounce(){const e=this.debouncedSubscription;null!==e&&(this.remove(e),e.unsubscribe(),this.debouncedSubscription=null)}}function u(e){e.debouncedNext()}var b=i("/uUt"),d=i("8Y7J");const m=["dataTable"],h=["emailTemplate"],p=["phoneNumberTemplate"],f=["dateTimeTemplate"],g=["booleanTemplate"],T=["usernameTemplate"];let v=(()=>{class e{constructor(){this.search=new d.n,this.isLoading=!1,this.clientSide=!1,this.canFilter=!1,this.minimumSearchCharacters=3,this.page=1,this.defaultRowsPerPage=20}}return e.\u0275fac=function(t){return new(t||e)},e.\u0275dir=d.Jb({type:e,viewQuery:function(e,t){var i;1&e&&(d.Ec(m,!0),d.Ec(h,!0),d.Ec(p,!0),d.Ec(f,!0),d.Ec(g,!0),d.Ec(T,!0)),2&e&&(d.wc(i=d.hc())&&(t.dataTable=i.first),d.wc(i=d.hc())&&(t.emailTemplate=i.first),d.wc(i=d.hc())&&(t.phoneNumberTemplate=i.first),d.wc(i=d.hc())&&(t.dateTimeTemplate=i.first),d.wc(i=d.hc())&&(t.booleanTemplate=i.first),d.wc(i=d.hc())&&(t.usernameTemplate=i.first))},inputs:{rows:"rows",rowsPerPage:"rowsPerPage",columns:"columns",count:"count",defaultSortField:"defaultSortField",defaultSortDirection:"defaultSortDirection",isLoading:"isLoading",clientSide:"clientSide",canFilter:"canFilter"},outputs:{search:"search"}}),e})();var w,I=function(e){return e.Asc="Asc",e.Desc="Desc",e}({});!function(e){e.PAGE_SIZE="size",e.PAGE="page",e.SEARCH_TERM="query",e.SORT_FIELD="sort",e.SORT_DIRECTION="dir"}(w||(w={}));class U{constructor(e,t,i,o){this.page=e,this.pageSize=t,this.sortField=i,this.searchTerm=o}}var S=i("iInd"),y=i("s7LF"),P=i("w9WL"),x=i("SVse");const C=["filterInput"];function k(e,t){1&e&&(d.Ub(0,"i",15),d.Ic(1," block "),d.Tb())}function _(e,t){1&e&&(d.Ub(0,"i",16),d.Ic(1," vpn_key "),d.Tb())}function D(e,t){if(1&e&&(d.Ub(0,"span",12),d.Gc(1,k,2,0,"i",13),d.Gc(2,_,2,0,"i",14),d.Ic(3),d.Tb()),2&e){const e=d.ic(),t=e.row,i=e.value;d.Bb(1),d.oc("ngIf",t.blocked),d.Bb(1),d.oc("ngIf",t.accessFailedCount>0&&t.lockoutEnd&&!t.blocked),d.Bb(1),d.Kc(" ",i," ")}}function E(e,t){1&e&&(d.Ub(0,"span"),d.Ic(1," - "),d.Tb())}function R(e,t){if(1&e&&(d.Gc(0,D,4,3,"span",10),d.Gc(1,E,2,0,"span",11)),2&e){const e=t.value;d.oc("ngIf",e),d.Bb(1),d.oc("ngIf",!e)}}function O(e,t){1&e&&(d.Ub(0,"i",20),d.Ic(1," check "),d.Tb())}function A(e,t){if(1&e&&(d.Ub(0,"a",18),d.Gc(1,O,2,0,"i",19),d.Ic(2),d.Tb()),2&e){const e=d.ic(),t=e.value,i=e.row;d.oc("href","mailto:"+t,d.Bc),d.Bb(1),d.oc("ngIf",i.emailConfirmed),d.Bb(1),d.Kc(" ",t," ")}}function B(e,t){1&e&&(d.Ub(0,"span"),d.Ic(1," - "),d.Tb())}function F(e,t){if(1&e&&(d.Gc(0,A,3,3,"a",17),d.Gc(1,B,2,0,"span",11)),2&e){const e=t.value;d.oc("ngIf",e),d.Bb(1),d.oc("ngIf",!e)}}function G(e,t){1&e&&(d.Ub(0,"i",22),d.Ic(1," check "),d.Tb())}function M(e,t){if(1&e&&(d.Ub(0,"a",18),d.Gc(1,G,2,0,"i",21),d.Ic(2),d.Tb()),2&e){const e=d.ic(),t=e.value,i=e.row;d.oc("href","tel:"+t,d.Bc),d.Bb(1),d.oc("ngIf",i.phoneNumberConfirmed),d.Bb(1),d.Kc(" ",t," ")}}function L(e,t){1&e&&(d.Ub(0,"span"),d.Ic(1," - "),d.Tb())}function N(e,t){if(1&e&&(d.Gc(0,M,3,3,"a",17),d.Gc(1,L,2,0,"span",11)),2&e){const e=t.value;d.oc("ngIf",e),d.Bb(1),d.oc("ngIf",!e)}}function H(e,t){if(1&e&&(d.Ub(0,"span"),d.Ic(1),d.jc(2,"date"),d.Tb()),2&e){const e=d.ic().value;d.Bb(1),d.Jc(d.lc(2,1,e,"medium"))}}function j(e,t){1&e&&(d.Ub(0,"span"),d.Ic(1," - "),d.Tb())}function z(e,t){if(1&e&&(d.Gc(0,H,3,4,"span",11),d.Gc(1,j,2,0,"span",11)),2&e){const e=t.value;d.oc("ngIf",e),d.Bb(1),d.oc("ngIf",!e)}}function V(e,t){1&e&&(d.Ub(0,"i",25),d.Ic(1," check "),d.Tb())}function Q(e,t){1&e&&(d.Ub(0,"i",26),d.Ic(1," remove "),d.Tb())}function q(e,t){if(1&e&&(d.Gc(0,V,2,0,"i",23),d.Gc(1,Q,2,0,"i",24)),2&e){const e=t.value;d.oc("ngIf",!0===e),d.Bb(1),d.oc("ngIf",!1===e)}}let J=(()=>{class e extends v{constructor(e,t){super(),this.route=e,this.router=t}ngOnInit(){this.queryParamsSubscription=this.route.queryParams.subscribe(e=>{this.parseQueryParams(e),this.doSearch()}),Object(o.a)(this._filterInput.nativeElement,"keyup").pipe(Object(n.a)(e=>e.target.value),Object(r.a)(e=>e.length>=this.minimumSearchCharacters||0===e.length),function(e,t=s.a){return i=>i.lift(new a(e,t))}(1e3),Object(b.a)()).subscribe(e=>{this.setFilter()})}ngOnDestroy(){this.queryParamsSubscription&&this.queryParamsSubscription.unsubscribe()}setPage(e){this.page=e.offset+1,this.changeSearchLocation()}setSort(e){const t=e.sorts[e.sorts.length-1];this.sortField=t.prop,this.sortDirection="asc"===t.dir?I.Asc:I.Desc,this.changeSearchLocation()}setFilter(){0===this.searchTerm.length&&(this.searchTerm=void 0),this.changeSearchLocation()}parseQueryParams(e){this.page=+(e[w.PAGE]||1),this.dataTable.offset=this.page-1,this.rowsPerPage=+(e[w.PAGE_SIZE]||this.rowsPerPage||this.defaultRowsPerPage),this.sortField=e[w.SORT_FIELD]||this.defaultSortField||void 0,this.sortDirection=e[w.SORT_DIRECTION]||this.defaultSortDirection||void 0,this.sortField&&(this.dataTable.sorts.splice(0,this.dataTable.sorts.length),this.dataTable.sorts.push({prop:this.sortField,dir:this.sortDirection.toLowerCase()||"asc"})),this.searchTerm=e[w.SEARCH_TERM]||void 0}changeSearchLocation(){const e={};e[w.PAGE]=this.page,e[w.PAGE_SIZE]=this.rowsPerPage||this.defaultRowsPerPage,e[w.SORT_FIELD]=this.sortField||this.defaultSortField||void 0,e[w.SORT_DIRECTION]=this.sortDirection,e[w.SEARCH_TERM]=this.searchTerm,this.router.navigate([],{relativeTo:this.route,queryParams:e})}doSearch(){this.search.emit(new U(this.page,this.rowsPerPage,this.sortField?`${this.sortField}${this.sortDirection===I.Asc?"+":"-"}`:void 0,this.searchTerm||void 0))}isEmptyObject(e){return 0===Object.entries(e).length&&e.constructor===Object}}return e.\u0275fac=function(t){return new(t||e)(d.Ob(S.a),d.Ob(S.d))},e.\u0275cmp=d.Ib({type:e,selectors:[["app-list-view"]],viewQuery:function(e,t){var i;1&e&&d.Ec(C,!0),2&e&&d.wc(i=d.hc())&&(t._filterInput=i.first)},features:[d.yb],decls:15,vars:14,consts:[[1,"col-sm-12","col-md-4","m-auto",3,"hidden"],["type","text",1,"search","mt-0",3,"placeholder","ngModel","ngModelChange"],["filterInput",""],["trackByProp","id","reorderable","false",1,"material","expandable",3,"rows","columns","columnMode","count","rowHeight","headerHeight","footerHeight","limit","externalPaging","externalSorting","loadingIndicator","page","sort"],["dataTable",""],["usernameTemplate",""],["emailTemplate",""],["phoneNumberTemplate",""],["dateTimeTemplate",""],["booleanTemplate",""],["style","padding-right: 16px; position: relative;",4,"ngIf"],[4,"ngIf"],[2,"padding-right","16px","position","relative"],["class","material-icons font-14 checkmark","style","color: rgba(255, 81, 81, 0.61) !important;","title","Account is blocked",4,"ngIf"],["class","material-icons font-14 checkmark","style","color: rgba(255, 81, 81, 0.61) !important;","title","Account is locked",4,"ngIf"],["title","Account is blocked",1,"material-icons","font-14","checkmark",2,"color","rgba(255, 81, 81, 0.61) !important"],["title","Account is locked",1,"material-icons","font-14","checkmark",2,"color","rgba(255, 81, 81, 0.61) !important"],["style","padding-right: 16px; position: relative;",3,"href",4,"ngIf"],[2,"padding-right","16px","position","relative",3,"href"],["class","material-icons font-14 checkmark","style","color: #32CD32 !important;","title","Email is confirmed",4,"ngIf"],["title","Email is confirmed",1,"material-icons","font-14","checkmark",2,"color","#32CD32 !important"],["class","material-icons font-14 checkmark","style","color: #32CD32 !important;","title","Phone number is confirmed",4,"ngIf"],["title","Phone number is confirmed",1,"material-icons","font-14","checkmark",2,"color","#32CD32 !important"],["class","material-icons","style","color: #32CD32 !important;",4,"ngIf"],["class","material-icons","style","color: #CC0000 !important;",4,"ngIf"],[1,"material-icons",2,"color","#32CD32 !important"],[1,"material-icons",2,"color","#CC0000 !important"]],template:function(e,t){1&e&&(d.Ub(0,"div",0),d.Ub(1,"input",1,2),d.gc("ngModelChange",(function(e){return t.searchTerm=e})),d.Tb(),d.Tb(),d.Ub(3,"ngx-datatable",3,4),d.gc("page",(function(e){return t.setPage(e)}))("sort",(function(e){return t.setSort(e)})),d.Tb(),d.Gc(5,R,2,2,"ng-template",null,5,d.Hc),d.Gc(7,F,2,2,"ng-template",null,6,d.Hc),d.Gc(9,N,2,2,"ng-template",null,7,d.Hc),d.Gc(11,z,2,2,"ng-template",null,8,d.Hc),d.Gc(13,q,2,2,"ng-template",null,9,d.Hc)),2&e&&(d.oc("hidden",!t.canFilter),d.Bb(1),d.oc("placeholder","Type at least "+t.minimumSearchCharacters+" characters to filter results...")("ngModel",t.searchTerm),d.Bb(2),d.oc("rows",t.rows)("columns",t.columns)("columnMode","force")("count",t.count)("rowHeight",50)("headerHeight",50)("footerHeight",50)("limit",t.rowsPerPage)("externalPaging",!t.clientSide)("externalSorting",!t.clientSide)("loadingIndicator",t.isLoading))},directives:[y.b,y.m,y.p,P.a,x.m],pipes:[x.e],styles:[".search[_ngcontent-%COMP%]{padding:8px;margin:15px auto;font-size:14px;display:block;background:transparent;width:100%;border:none;border-bottom:1px solid #5985ee}"]}),e})()},u9xJ:function(e,t,i){"use strict";i.r(t),i.d(t,"ClaimTypesModule",(function(){return _}));var o=i("SVse"),n=i("s7LF"),r=i("hvj1"),c=i("PCNd"),s=i("iInd"),a=i("8Y7J"),l=i("onTr"),u=i("fQZA");const b=["rolesList"],d=["actionsTemplate"];function m(e,t){1&e&&(a.Ub(0,"a",11),a.Ic(1,"Edit"),a.Tb()),2&e&&a.oc("routerLink",t.value+"/edit")}let h=(()=>{class e{constructor(e){this.api=e,this.count=0,this.rows=[],this.columns=[]}ngOnInit(){this.columns=[{prop:"name",name:"Name",draggable:!1,canAutoResize:!0,sortable:!0,resizeable:!1},{prop:"description",name:"Description",draggable:!1,canAutoResize:!0,sortable:!1,resizeable:!1},{prop:"id",name:"Actions",draggable:!1,canAutoResize:!1,sortable:!1,resizeable:!1,cellTemplate:this.actionsTemplate,cellClass:"d-flex align-items-center"}]}getRoles(e){this.api.getRoles(e.page,e.pageSize,e.sortField,e.searchTerm).subscribe(e=>{this.count=e.count,this.rows=e.items})}}return e.\u0275fac=function(t){return new(t||e)(a.Ob(l.n))},e.\u0275cmp=a.Ib({type:e,selectors:[["app-roles"]],viewQuery:function(e,t){var i;1&e&&(a.Ec(b,!0),a.Ec(d,!0)),2&e&&(a.wc(i=a.hc())&&(t.rolesList=i.first),a.wc(i=a.hc())&&(t.actionsTemplate=i.first))},decls:15,vars:8,consts:[[1,"row"],[1,"col-sm-12"],[1,"page-title-box"],[1,"row","align-items-center"],[1,"col-md-8"],[1,"page-title","m-0"],[1,"text-muted","font-14"],[1,"col-12"],[3,"clientSide","columns","rows","count","rowsPerPage","defaultSortField","defaultSortDirection","search"],["rolesList",""],["actionsTemplate",""],["href","#","role","button",1,"btn","btn-success",3,"routerLink"]],template:function(e,t){1&e&&(a.Ub(0,"div",0),a.Ub(1,"div",1),a.Ub(2,"div",2),a.Ub(3,"div",3),a.Ub(4,"div",4),a.Ub(5,"h4",5),a.Ic(6),a.Tb(),a.Ub(7,"p",6),a.Ic(8," Get a list of all system roles. "),a.Tb(),a.Tb(),a.Tb(),a.Tb(),a.Tb(),a.Tb(),a.Ub(9,"div",0),a.Ub(10,"div",7),a.Ub(11,"app-list-view",8,9),a.gc("search",(function(e){return t.getRoles(e)})),a.Tb(),a.Tb(),a.Tb(),a.Gc(13,m,2,1,"ng-template",null,10,a.Hc)),2&e&&(a.Bb(6),a.Kc("Roles List (",t.count,")"),a.Bb(5),a.oc("clientSide",!1)("columns",t.columns)("rows",t.rows)("count",t.count)("rowsPerPage",15)("defaultSortField","name")("defaultSortDirection","Asc"))},directives:[u.a,s.g],encapsulation:2}),e})();var p=i("1uCz"),f=i("1+MP");const g=["validationSummary"];function T(e,t){1&e&&(a.Ub(0,"div",21),a.Ic(1," Field 'Name' is required. "),a.Tb())}const v=function(e){return{"was-validated":e}},w=function(e){return{invalid:e}};let I=(()=>{class e{constructor(e,t,i,o){this._api=e,this._router=t,this._route=i,this._toast=o,this.role=new l.j}save(){this._validationSummary.clear(),this._api.createRole(this.role).subscribe(e=>{this._toast.showSuccess(`Role '${e.name}' was created successfully.`),this._router.navigate(["../"],{relativeTo:this._route})},e=>{this.problemDetails=e})}}return e.\u0275fac=function(t){return new(t||e)(a.Ob(l.n),a.Ob(s.d),a.Ob(s.a),a.Ob(p.a))},e.\u0275cmp=a.Ib({type:e,selectors:[["app-role-add"]],viewQuery:function(e,t){var i;1&e&&a.Nc(g,!0),2&e&&a.wc(i=a.hc())&&(t._validationSummary=i.first)},decls:30,vars:10,consts:[[1,"row"],[1,"col-12"],[1,"card","m-b-30","m-t-30"],[1,"card-body"],[1,"mt-0","header-title"],[1,"text-muted","m-b-30","font-14"],["novalidate","",1,"needs-validation",3,"ngClass","ngSubmit"],["form","ngForm"],[3,"problemDetails"],["validationSummary",""],[1,"form-group","row"],["for","role-name",1,"col-sm-2","col-form-label"],[1,"col-sm-10"],["id","role-name","name","role-name","type","text","required","","maxlength","64",1,"form-control",3,"ngClass","ngModel","ngModelChange"],["name","ngModel"],["class","form-control-feedback text-danger",4,"ngIf"],["for","role-description",1,"col-sm-2","col-form-label"],["id","role-description","name","role-description","rows","3","maxlength","512",1,"form-control",3,"ngModel","ngModelChange"],["description","ngModel"],[1,"form-group"],["type","submit",1,"btn","btn-primary","waves-effect","waves-light","m-r-5"],[1,"form-control-feedback","text-danger"]],template:function(e,t){if(1&e){const e=a.Vb();a.Ub(0,"div",0),a.Ub(1,"div",1),a.Ub(2,"div",2),a.Ub(3,"div",3),a.Ub(4,"h4",4),a.Ic(5,"Add Role"),a.Tb(),a.Ub(6,"p",5),a.Ic(7," Add a custom role. "),a.Tb(),a.Ub(8,"form",6,7),a.gc("ngSubmit",(function(){return a.zc(e),a.xc(9).form.valid&&t.save()})),a.Pb(10,"app-validation-summary",8,9),a.Ub(12,"div",10),a.Ub(13,"label",11),a.Ic(14," Name (*) "),a.Tb(),a.Ub(15,"div",12),a.Ub(16,"input",13,14),a.gc("ngModelChange",(function(e){return t.role.name=e})),a.Tb(),a.Gc(18,T,2,0,"div",15),a.Tb(),a.Tb(),a.Ub(19,"div",10),a.Ub(20,"label",16),a.Ic(21," Description "),a.Tb(),a.Ub(22,"div",12),a.Ub(23,"textarea",17,18),a.gc("ngModelChange",(function(e){return t.role.description=e})),a.Ic(25,"                            "),a.Tb(),a.Tb(),a.Tb(),a.Ub(26,"div",19),a.Ub(27,"div"),a.Ub(28,"button",20),a.Ic(29," Save "),a.Tb(),a.Tb(),a.Tb(),a.Tb(),a.Tb(),a.Tb(),a.Tb(),a.Tb()}if(2&e){const e=a.xc(9),i=a.xc(17);a.Bb(8),a.oc("ngClass",a.sc(6,v,e.submitted)),a.Bb(2),a.oc("problemDetails",t.problemDetails),a.Bb(6),a.oc("ngClass",a.sc(8,w,e.submitted&&i.invalid))("ngModel",t.role.name),a.Bb(2),a.oc("ngIf",e.submitted&&i.invalid),a.Bb(5),a.oc("ngModel",t.role.description)}},directives:[n.y,n.n,n.o,o.k,f.a,n.b,n.u,n.j,n.m,n.p,o.m],encapsulation:2}),e})();const U=["deleteAlert"],S=function(e){return{"was-validated":e}};let y=(()=>{class e{constructor(e,t,i,o){this._api=e,this._router=t,this._route=i,this._toast=o,this.role=new l.o}ngOnInit(){this.role=this._route.snapshot.data.role}deletePrompt(){this._deleteAlert.fire()}delete(){this._api.deleteRole(this.role.id).subscribe(e=>{this._toast.showSuccess(`Role '${this.role.name}' was deleted successfully.`),this._router.navigate(["../../"],{relativeTo:this._route})})}update(){this._api.updateRole(this.role.id,{description:this.role.description}).subscribe(e=>{this._toast.showSuccess(`Role '${e.name}' was updated successfully.`)})}}return e.\u0275fac=function(t){return new(t||e)(a.Ob(l.n),a.Ob(s.d),a.Ob(s.a),a.Ob(p.a))},e.\u0275cmp=a.Ib({type:e,selectors:[["app-role-edit"]],viewQuery:function(e,t){var i;1&e&&a.Nc(U,!0),2&e&&a.wc(i=a.hc())&&(t._deleteAlert=i.first)},decls:34,vars:7,consts:[["title","Delete?","type","warning",3,"text","showCancelButton","confirm"],["deleteAlert",""],[1,"row"],[1,"col-12"],[1,"card","m-b-30","m-t-30"],[1,"card-body"],[1,"mt-0","header-title"],[1,"text-muted","m-b-30","font-14"],["novalidate","",1,"needs-validation",3,"ngClass","ngSubmit"],["form","ngForm"],[1,"form-group","row"],["for","role-name",1,"col-sm-2","col-form-label"],[1,"col-sm-10","col-form-label"],["id","role-name","name","role-name",1,"col-form-label"],["for","role-description",1,"col-sm-2","col-form-label"],[1,"col-sm-10"],["id","role-description","name","role-description","rows","3","maxlength","512",1,"form-control",3,"ngModel","ngModelChange"],[1,"form-group"],["type","submit",1,"btn","btn-primary","m-r-5"],[1,"material-icons","align-middle"],["type","button",1,"btn","btn-danger","m-r-5","float-right",3,"click"]],template:function(e,t){if(1&e){const e=a.Vb();a.Ub(0,"swal",0,1),a.gc("confirm",(function(){return t.delete()})),a.Tb(),a.Ub(2,"div",2),a.Ub(3,"div",3),a.Ub(4,"div",4),a.Ub(5,"div",5),a.Ub(6,"h4",6),a.Ic(7,"Edit Role"),a.Tb(),a.Ub(8,"p",7),a.Ic(9," Here you can modify some properties of a system role. "),a.Tb(),a.Ub(10,"form",8,9),a.gc("ngSubmit",(function(){return a.zc(e),a.xc(11).form.valid&&t.update()})),a.Ub(12,"div",10),a.Ub(13,"label",11),a.Ic(14," Name "),a.Tb(),a.Ub(15,"div",12),a.Ub(16,"span",13),a.Ic(17),a.Tb(),a.Tb(),a.Tb(),a.Ub(18,"div",10),a.Ub(19,"label",14),a.Ic(20," Description "),a.Tb(),a.Ub(21,"div",15),a.Ub(22,"textarea",16),a.gc("ngModelChange",(function(e){return t.role.description=e})),a.Ic(23,"                            "),a.Tb(),a.Tb(),a.Tb(),a.Ub(24,"div",17),a.Ub(25,"div"),a.Ub(26,"button",18),a.Ub(27,"i",19),a.Ic(28,"save"),a.Tb(),a.Ic(29," Save Changes "),a.Tb(),a.Ub(30,"button",20),a.gc("click",(function(){return a.zc(e),a.xc(1).fire()})),a.Ub(31,"i",19),a.Ic(32,"delete"),a.Tb(),a.Ic(33," Delete "),a.Tb(),a.Tb(),a.Tb(),a.Tb(),a.Tb(),a.Tb(),a.Tb(),a.Tb()}if(2&e){const e=a.xc(11);a.oc("text","Role '"+t.role.name+"' will be deleted permanently. This action cannot be reversed.")("showCancelButton",!0),a.Bb(10),a.oc("ngClass",a.sc(5,S,e.submitted)),a.Bb(7),a.Kc(" ",t.role.name," "),a.Bb(5),a.oc("ngModel",t.role.description)}},directives:[r.a,n.y,n.n,n.o,o.k,n.b,n.j,n.m,n.p],encapsulation:2}),e})();var P=i("LRne");let x=(()=>{class e{constructor(e){this.api=e}resolve(e,t){const i=e.params.id;return i?this.api.getRole(i):Object(P.a)(new l.o)}}return e.\u0275fac=function(t){return new(t||e)(a.cc(l.n))},e.\u0275prov=a.Kb({token:e,factory:e.\u0275fac}),e})();const C=[{path:"",component:h},{path:"add",component:I},{path:":id/edit",component:y,resolve:{role:x}}];let k=(()=>{class e{}return e.\u0275mod=a.Mb({type:e}),e.\u0275inj=a.Lb({factory:function(t){return new(t||e)},providers:[x],imports:[[s.h.forChild(C)],s.h]}),e})(),_=(()=>{class e{}return e.\u0275mod=a.Mb({type:e}),e.\u0275inj=a.Lb({factory:function(t){return new(t||e)},imports:[[o.c,n.i,k,c.a,r.c]]}),e})()}}]);