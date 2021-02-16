function _inherits(e,t){if("function"!=typeof t&&null!==t)throw new TypeError("Super expression must either be null or a function");e.prototype=Object.create(t&&t.prototype,{constructor:{value:e,writable:!0,configurable:!0}}),t&&_setPrototypeOf(e,t)}function _setPrototypeOf(e,t){return(_setPrototypeOf=Object.setPrototypeOf||function(e,t){return e.__proto__=t,e})(e,t)}function _createSuper(e){var t=_isNativeReflectConstruct();return function(){var i,a=_getPrototypeOf(e);if(t){var n=_getPrototypeOf(this).constructor;i=Reflect.construct(a,arguments,n)}else i=a.apply(this,arguments);return _possibleConstructorReturn(this,i)}}function _possibleConstructorReturn(e,t){return!t||"object"!=typeof t&&"function"!=typeof t?_assertThisInitialized(e):t}function _assertThisInitialized(e){if(void 0===e)throw new ReferenceError("this hasn't been initialised - super() hasn't been called");return e}function _isNativeReflectConstruct(){if("undefined"==typeof Reflect||!Reflect.construct)return!1;if(Reflect.construct.sham)return!1;if("function"==typeof Proxy)return!0;try{return Date.prototype.toString.call(Reflect.construct(Date,[],(function(){}))),!0}catch(e){return!1}}function _getPrototypeOf(e){return(_getPrototypeOf=Object.setPrototypeOf?Object.getPrototypeOf:function(e){return e.__proto__||Object.getPrototypeOf(e)})(e)}function _classCallCheck(e,t){if(!(e instanceof t))throw new TypeError("Cannot call a class as a function")}function _defineProperties(e,t){for(var i=0;i<t.length;i++){var a=t[i];a.enumerable=a.enumerable||!1,a.configurable=!0,"value"in a&&(a.writable=!0),Object.defineProperty(e,a.key,a)}}function _createClass(e,t,i){return t&&_defineProperties(e.prototype,t),i&&_defineProperties(e,i),e}(window.webpackJsonp=window.webpackJsonp||[]).push([[7],{fQZA:function(e,t,i){"use strict";i.d(t,"a",(function(){return Z}));var a=i("xgIS"),n=i("lJxs"),c=i("pLZG"),o=i("7o/Q"),l=i("D0XW"),r=function(){function e(t,i){_classCallCheck(this,e),this.dueTime=t,this.scheduler=i}return _createClass(e,[{key:"call",value:function(e,t){return t.subscribe(new s(e,this.dueTime,this.scheduler))}}]),e}(),s=function(e){_inherits(i,e);var t=_createSuper(i);function i(e,a,n){var c;return _classCallCheck(this,i),(c=t.call(this,e)).dueTime=a,c.scheduler=n,c.debouncedSubscription=null,c.lastValue=null,c.hasValue=!1,c}return _createClass(i,[{key:"_next",value:function(e){this.clearDebounce(),this.lastValue=e,this.hasValue=!0,this.add(this.debouncedSubscription=this.scheduler.schedule(u,this.dueTime,this))}},{key:"_complete",value:function(){this.debouncedNext(),this.destination.complete()}},{key:"debouncedNext",value:function(){if(this.clearDebounce(),this.hasValue){var e=this.lastValue;this.lastValue=null,this.hasValue=!1,this.destination.next(e)}}},{key:"clearDebounce",value:function(){var e=this.debouncedSubscription;null!==e&&(this.remove(e),e.unsubscribe(),this.debouncedSubscription=null)}}]),i}(o.a);function u(e){e.debouncedNext()}var b,m,d=i("/uUt"),p=i("8Y7J"),f=["dataTable"],h=["emailTemplate"],g=["phoneNumberTemplate"],T=["dateTimeTemplate"],y=["booleanTemplate"],v=["usernameTemplate"],C=((b=function e(){_classCallCheck(this,e),this.search=new p.n,this.isLoading=!1,this.clientSide=!1,this.canFilter=!1,this.minimumSearchCharacters=3,this.page=1,this.defaultRowsPerPage=20}).\u0275fac=function(e){return new(e||b)},b.\u0275dir=p.Jb({type:b,viewQuery:function(e,t){var i;1&e&&(p.Ec(f,!0),p.Ec(h,!0),p.Ec(g,!0),p.Ec(T,!0),p.Ec(y,!0),p.Ec(v,!0)),2&e&&(p.wc(i=p.hc())&&(t.dataTable=i.first),p.wc(i=p.hc())&&(t.emailTemplate=i.first),p.wc(i=p.hc())&&(t.phoneNumberTemplate=i.first),p.wc(i=p.hc())&&(t.dateTimeTemplate=i.first),p.wc(i=p.hc())&&(t.booleanTemplate=i.first),p.wc(i=p.hc())&&(t.usernameTemplate=i.first))},inputs:{rows:"rows",rowsPerPage:"rowsPerPage",columns:"columns",count:"count",defaultSortField:"defaultSortField",defaultSortDirection:"defaultSortDirection",isLoading:"isLoading",clientSide:"clientSide",canFilter:"canFilter"},outputs:{search:"search"}}),b),U=function(e){return e.Asc="Asc",e.Desc="Desc",e}({});!function(e){e.PAGE_SIZE="size",e.PAGE="page",e.SEARCH_TERM="query",e.SORT_FIELD="sort",e.SORT_DIRECTION="dir"}(m||(m={}));var I=function e(t,i,a,n){_classCallCheck(this,e),this.page=t,this.pageSize=i,this.sortField=a,this.searchTerm=n},w=i("iInd"),_=i("s7LF"),k=i("w9WL"),S=i("SVse"),x=["filterInput"];function M(e,t){1&e&&(p.Ub(0,"i",15),p.Ic(1," block "),p.Tb())}function P(e,t){1&e&&(p.Ub(0,"i",16),p.Ic(1," vpn_key "),p.Tb())}function O(e,t){if(1&e&&(p.Ub(0,"span",12),p.Gc(1,M,2,0,"i",13),p.Gc(2,P,2,0,"i",14),p.Ic(3),p.Tb()),2&e){var i=p.ic(),a=i.row,n=i.value;p.Bb(1),p.oc("ngIf",a.blocked),p.Bb(1),p.oc("ngIf",a.accessFailedCount>0&&a.lockoutEnd&&!a.blocked),p.Bb(1),p.Kc(" ",n," ")}}function B(e,t){1&e&&(p.Ub(0,"span"),p.Ic(1," - "),p.Tb())}function E(e,t){if(1&e&&(p.Gc(0,O,4,3,"span",10),p.Gc(1,B,2,0,"span",11)),2&e){var i=t.value;p.oc("ngIf",i),p.Bb(1),p.oc("ngIf",!i)}}function D(e,t){1&e&&(p.Ub(0,"i",20),p.Ic(1," check "),p.Tb())}function R(e,t){if(1&e&&(p.Ub(0,"a",18),p.Gc(1,D,2,0,"i",19),p.Ic(2),p.Tb()),2&e){var i=p.ic(),a=i.value,n=i.row;p.oc("href","mailto:"+a,p.Bc),p.Bb(1),p.oc("ngIf",n.emailConfirmed),p.Bb(1),p.Kc(" ",a," ")}}function G(e,t){1&e&&(p.Ub(0,"span"),p.Ic(1," - "),p.Tb())}function F(e,t){if(1&e&&(p.Gc(0,R,3,3,"a",17),p.Gc(1,G,2,0,"span",11)),2&e){var i=t.value;p.oc("ngIf",i),p.Bb(1),p.oc("ngIf",!i)}}function A(e,t){1&e&&(p.Ub(0,"i",22),p.Ic(1," check "),p.Tb())}function N(e,t){if(1&e&&(p.Ub(0,"a",18),p.Gc(1,A,2,0,"i",21),p.Ic(2),p.Tb()),2&e){var i=p.ic(),a=i.value,n=i.row;p.oc("href","tel:"+a,p.Bc),p.Bb(1),p.oc("ngIf",n.phoneNumberConfirmed),p.Bb(1),p.Kc(" ",a," ")}}function L(e,t){1&e&&(p.Ub(0,"span"),p.Ic(1," - "),p.Tb())}function q(e,t){if(1&e&&(p.Gc(0,N,3,3,"a",17),p.Gc(1,L,2,0,"span",11)),2&e){var i=t.value;p.oc("ngIf",i),p.Bb(1),p.oc("ngIf",!i)}}function V(e,t){if(1&e&&(p.Ub(0,"span"),p.Ic(1),p.jc(2,"date"),p.Tb()),2&e){var i=p.ic().value;p.Bb(1),p.Jc(p.lc(2,1,i,"medium"))}}function j(e,t){1&e&&(p.Ub(0,"span"),p.Ic(1," - "),p.Tb())}function z(e,t){if(1&e&&(p.Gc(0,V,3,4,"span",11),p.Gc(1,j,2,0,"span",11)),2&e){var i=t.value;p.oc("ngIf",i),p.Bb(1),p.oc("ngIf",!i)}}function H(e,t){1&e&&(p.Ub(0,"i",25),p.Ic(1," check "),p.Tb())}function K(e,t){1&e&&(p.Ub(0,"i",26),p.Ic(1," remove "),p.Tb())}function Q(e,t){if(1&e&&(p.Gc(0,H,2,0,"i",23),p.Gc(1,K,2,0,"i",24)),2&e){var i=t.value;p.oc("ngIf",!0===i),p.Bb(1),p.oc("ngIf",!1===i)}}var J,Z=((J=function(e){_inherits(i,e);var t=_createSuper(i);function i(e,a){var n;return _classCallCheck(this,i),(n=t.call(this)).route=e,n.router=a,n}return _createClass(i,[{key:"ngOnInit",value:function(){var e=this;this.queryParamsSubscription=this.route.queryParams.subscribe((function(t){e.parseQueryParams(t),e.doSearch()})),Object(a.a)(this._filterInput.nativeElement,"keyup").pipe(Object(n.a)((function(e){return e.target.value})),Object(c.a)((function(t){return t.length>=e.minimumSearchCharacters||0===t.length})),function(e){var t=arguments.length>1&&void 0!==arguments[1]?arguments[1]:l.a;return function(i){return i.lift(new r(e,t))}}(1e3),Object(d.a)()).subscribe((function(t){e.setFilter()}))}},{key:"ngOnDestroy",value:function(){this.queryParamsSubscription&&this.queryParamsSubscription.unsubscribe()}},{key:"setPage",value:function(e){this.page=e.offset+1,this.changeSearchLocation()}},{key:"setSort",value:function(e){var t=e.sorts[e.sorts.length-1];this.sortField=t.prop,this.sortDirection="asc"===t.dir?U.Asc:U.Desc,this.changeSearchLocation()}},{key:"setFilter",value:function(){0===this.searchTerm.length&&(this.searchTerm=void 0),this.changeSearchLocation()}},{key:"parseQueryParams",value:function(e){this.page=+(e[m.PAGE]||1),this.dataTable.offset=this.page-1,this.rowsPerPage=+(e[m.PAGE_SIZE]||this.rowsPerPage||this.defaultRowsPerPage),this.sortField=e[m.SORT_FIELD]||this.defaultSortField||void 0,this.sortDirection=e[m.SORT_DIRECTION]||this.defaultSortDirection||void 0,this.sortField&&(this.dataTable.sorts.splice(0,this.dataTable.sorts.length),this.dataTable.sorts.push({prop:this.sortField,dir:this.sortDirection.toLowerCase()||"asc"})),this.searchTerm=e[m.SEARCH_TERM]||void 0}},{key:"changeSearchLocation",value:function(){var e={};e[m.PAGE]=this.page,e[m.PAGE_SIZE]=this.rowsPerPage||this.defaultRowsPerPage,e[m.SORT_FIELD]=this.sortField||this.defaultSortField||void 0,e[m.SORT_DIRECTION]=this.sortDirection,e[m.SEARCH_TERM]=this.searchTerm,this.router.navigate([],{relativeTo:this.route,queryParams:e})}},{key:"doSearch",value:function(){this.search.emit(new I(this.page,this.rowsPerPage,this.sortField?"".concat(this.sortField).concat(this.sortDirection===U.Asc?"+":"-"):void 0,this.searchTerm||void 0))}},{key:"isEmptyObject",value:function(e){return 0===Object.entries(e).length&&e.constructor===Object}}]),i}(C)).\u0275fac=function(e){return new(e||J)(p.Ob(w.a),p.Ob(w.d))},J.\u0275cmp=p.Ib({type:J,selectors:[["app-list-view"]],viewQuery:function(e,t){var i;1&e&&p.Ec(x,!0),2&e&&p.wc(i=p.hc())&&(t._filterInput=i.first)},features:[p.yb],decls:15,vars:14,consts:[[1,"col-sm-12","col-md-4","m-auto",3,"hidden"],["type","text",1,"search","mt-0",3,"placeholder","ngModel","ngModelChange"],["filterInput",""],["trackByProp","id","reorderable","false",1,"material","expandable",3,"rows","columns","columnMode","count","rowHeight","headerHeight","footerHeight","limit","externalPaging","externalSorting","loadingIndicator","page","sort"],["dataTable",""],["usernameTemplate",""],["emailTemplate",""],["phoneNumberTemplate",""],["dateTimeTemplate",""],["booleanTemplate",""],["style","padding-right: 16px; position: relative;",4,"ngIf"],[4,"ngIf"],[2,"padding-right","16px","position","relative"],["class","material-icons font-14 checkmark","style","color: rgba(255, 81, 81, 0.61) !important;","title","Account is blocked",4,"ngIf"],["class","material-icons font-14 checkmark","style","color: rgba(255, 81, 81, 0.61) !important;","title","Account is locked",4,"ngIf"],["title","Account is blocked",1,"material-icons","font-14","checkmark",2,"color","rgba(255, 81, 81, 0.61) !important"],["title","Account is locked",1,"material-icons","font-14","checkmark",2,"color","rgba(255, 81, 81, 0.61) !important"],["style","padding-right: 16px; position: relative;",3,"href",4,"ngIf"],[2,"padding-right","16px","position","relative",3,"href"],["class","material-icons font-14 checkmark","style","color: #32CD32 !important;","title","Email is confirmed",4,"ngIf"],["title","Email is confirmed",1,"material-icons","font-14","checkmark",2,"color","#32CD32 !important"],["class","material-icons font-14 checkmark","style","color: #32CD32 !important;","title","Phone number is confirmed",4,"ngIf"],["title","Phone number is confirmed",1,"material-icons","font-14","checkmark",2,"color","#32CD32 !important"],["class","material-icons","style","color: #32CD32 !important;",4,"ngIf"],["class","material-icons","style","color: #CC0000 !important;",4,"ngIf"],[1,"material-icons",2,"color","#32CD32 !important"],[1,"material-icons",2,"color","#CC0000 !important"]],template:function(e,t){1&e&&(p.Ub(0,"div",0),p.Ub(1,"input",1,2),p.gc("ngModelChange",(function(e){return t.searchTerm=e})),p.Tb(),p.Tb(),p.Ub(3,"ngx-datatable",3,4),p.gc("page",(function(e){return t.setPage(e)}))("sort",(function(e){return t.setSort(e)})),p.Tb(),p.Gc(5,E,2,2,"ng-template",null,5,p.Hc),p.Gc(7,F,2,2,"ng-template",null,6,p.Hc),p.Gc(9,q,2,2,"ng-template",null,7,p.Hc),p.Gc(11,z,2,2,"ng-template",null,8,p.Hc),p.Gc(13,Q,2,2,"ng-template",null,9,p.Hc)),2&e&&(p.oc("hidden",!t.canFilter),p.Bb(1),p.oc("placeholder","Type at least "+t.minimumSearchCharacters+" characters to filter results...")("ngModel",t.searchTerm),p.Bb(2),p.oc("rows",t.rows)("columns",t.columns)("columnMode","force")("count",t.count)("rowHeight",50)("headerHeight",50)("footerHeight",50)("limit",t.rowsPerPage)("externalPaging",!t.clientSide)("externalSorting",!t.clientSide)("loadingIndicator",t.isLoading))},directives:[_.b,_.m,_.p,k.a,S.m],pipes:[S.e],styles:[".search[_ngcontent-%COMP%]{padding:8px;margin:15px auto;font-size:14px;display:block;background:transparent;width:100%;border:none;border-bottom:1px solid #5985ee}"]}),J)},foSm:function(e,t,i){"use strict";i.r(t),i.d(t,"ClaimTypesModule",(function(){return Q}));var a=i("SVse"),n=i("s7LF"),c=i("hvj1"),o=i("PCNd"),l=i("iInd"),r=i("8Y7J"),s=i("onTr"),u=i("fQZA"),b=["claimTypesList"],m=["actionsTemplate"],d=["valueTypeTemplate"],p=["nameTemplate"];function f(e,t){if(1&e&&(r.Ub(0,"a",15),r.Ic(1," Edit "),r.Tb()),2&e){var i=r.ic().value;r.oc("routerLink",i+"/edit")}}function h(e,t){1&e&&(r.Ub(0,"span",16),r.Ic(1," system reserved "),r.Tb())}function g(e,t){if(1&e&&(r.Gc(0,f,2,1,"a",13),r.Gc(1,h,2,0,"span",14)),2&e){var i=t.row;r.oc("ngIf",!i.reserved),r.Bb(1),r.oc("ngIf",i.reserved)}}function T(e,t){if(1&e&&(r.Ub(0,"code",17),r.Ic(1),r.Tb()),2&e){var i=t.value;r.Bb(1),r.Kc(" ",i," ")}}function y(e,t){if(1&e&&(r.Ub(0,"span"),r.Ub(1,"i",19),r.Ic(2," info "),r.Tb(),r.Ic(3),r.Tb()),2&e){var i=r.ic().row;r.Bb(1),r.oc("title",i.description),r.Bb(2),r.Kc(" ",i.displayName||i.name," ")}}function v(e,t){if(1&e&&(r.Ub(0,"span"),r.Ic(1),r.Tb()),2&e){var i=r.ic().value;r.Bb(1),r.Jc(i)}}function C(e,t){if(1&e&&(r.Gc(0,y,4,2,"span",18),r.Gc(1,v,2,1,"span",18)),2&e){var i=t.row;r.oc("ngIf",i.description),r.Bb(1),r.oc("ngIf",!i.description)}}var U,I=((U=function(){function e(t){_classCallCheck(this,e),this._api=t,this.count=0,this.rows=[],this.columns=[]}return _createClass(e,[{key:"ngOnInit",value:function(){this.columns=[{prop:"name",name:"Name",draggable:!1,canAutoResize:!0,sortable:!0,resizeable:!1,cellTemplate:this._nameTemplate},{prop:"valueType",name:"Value Type",draggable:!1,canAutoResize:!0,sortable:!0,resizeable:!1,cellTemplate:this._valueTypeTemplate},{prop:"userEditable",name:"User Editable",draggable:!1,canAutoResize:!0,sortable:!0,resizeable:!1,cellTemplate:this._claimTypesList.booleanTemplate},{prop:"required",name:"Required",draggable:!1,canAutoResize:!0,sortable:!0,resizeable:!1,cellTemplate:this._claimTypesList.booleanTemplate},{prop:"id",name:"Actions",draggable:!1,canAutoResize:!1,sortable:!1,resizeable:!1,cellTemplate:this._actionsTemplate,cellClass:"d-flex align-items-center"}]}},{key:"getClaimTypes",value:function(e){var t=this;this._api.getClaimTypes(void 0,e.page,e.pageSize,e.sortField,e.searchTerm).subscribe((function(e){t.count=e.count,t.rows=e.items}))}}]),e}()).\u0275fac=function(e){return new(e||U)(r.Ob(s.n))},U.\u0275cmp=r.Ib({type:U,selectors:[["app-claim-types"]],viewQuery:function(e,t){var i;1&e&&(r.Ec(b,!0),r.Ec(m,!0),r.Ec(d,!0),r.Ec(p,!0)),2&e&&(r.wc(i=r.hc())&&(t._claimTypesList=i.first),r.wc(i=r.hc())&&(t._actionsTemplate=i.first),r.wc(i=r.hc())&&(t._valueTypeTemplate=i.first),r.wc(i=r.hc())&&(t._nameTemplate=i.first))},decls:19,vars:8,consts:[[1,"row"],[1,"col-sm-12"],[1,"page-title-box"],[1,"row","align-items-center"],[1,"col-md-8"],[1,"page-title","m-0"],[1,"text-muted","font-14"],[1,"col-12"],[3,"clientSide","columns","rows","count","rowsPerPage","defaultSortField","defaultSortDirection","search"],["claimTypesList",""],["actionsTemplate",""],["valueTypeTemplate",""],["nameTemplate",""],["class","btn btn-success","href","#","role","button",3,"routerLink",4,"ngIf"],["class","badge badge-warning",4,"ngIf"],["href","#","role","button",1,"btn","btn-success",3,"routerLink"],[1,"badge","badge-warning"],[1,"highlighter-rouge"],[4,"ngIf"],[1,"material-icons","align-middle",3,"title"]],template:function(e,t){1&e&&(r.Ub(0,"div",0),r.Ub(1,"div",1),r.Ub(2,"div",2),r.Ub(3,"div",3),r.Ub(4,"div",4),r.Ub(5,"h4",5),r.Ic(6),r.Tb(),r.Ub(7,"p",6),r.Ic(8," Get a list of all system claim types. "),r.Tb(),r.Tb(),r.Tb(),r.Tb(),r.Tb(),r.Tb(),r.Ub(9,"div",0),r.Ub(10,"div",7),r.Ub(11,"app-list-view",8,9),r.gc("search",(function(e){return t.getClaimTypes(e)})),r.Tb(),r.Tb(),r.Tb(),r.Gc(13,g,2,2,"ng-template",null,10,r.Hc),r.Gc(15,T,2,1,"ng-template",null,11,r.Hc),r.Gc(17,C,2,2,"ng-template",null,12,r.Hc)),2&e&&(r.Bb(6),r.Kc("Claim Types List (",t.count,")"),r.Bb(5),r.oc("clientSide",!1)("columns",t.columns)("rows",t.rows)("count",t.count)("rowsPerPage",15)("defaultSortField","name")("defaultSortDirection","Asc"))},directives:[u.a,a.m,l.g],encapsulation:2}),U),w=i("1uCz"),_=i("1+MP"),k=["validationSummary"];function S(e,t){1&e&&(r.Ub(0,"div",37),r.Ic(1," Field 'Name' is required. "),r.Tb())}function x(e,t){if(1&e&&(r.Ub(0,"option",38),r.Ic(1),r.Tb()),2&e){var i=t.$implicit;r.oc("value",i),r.Bb(1),r.Kc(" ",i," ")}}function M(e,t){1&e&&(r.Ub(0,"div",37),r.Ic(1," Field 'Type' is required. "),r.Tb())}var P,O=function(e){return{"was-validated":e}},B=function(e){return{invalid:e}},E=((P=function(){function e(t,i,a,n){_classCallCheck(this,e),this._api=t,this._router=i,this._route=a,this._toast=n,this.claimType=new s.g,this.valueTypes=[],this.errors=[],this.claimValueType=""}return _createClass(e,[{key:"ngOnInit",value:function(){for(var e in s.x)e&&this.valueTypes.push(e)}},{key:"save",value:function(){var e=this;this._validationSummary.clear(),this.claimType.valueType=this.claimValueType,this._api.createClaimType(this.claimType).subscribe((function(t){e._toast.showSuccess("Claim type '".concat(t.name,"' was created successfully.")),e._router.navigate(["../"],{relativeTo:e._route})}),(function(t){e.problemDetails=t}))}}]),e}()).\u0275fac=function(e){return new(e||P)(r.Ob(s.n),r.Ob(l.d),r.Ob(l.a),r.Ob(w.a))},P.\u0275cmp=r.Ib({type:P,selectors:[["app-claim-type-add"]],viewQuery:function(e,t){var i;1&e&&r.Nc(k,!0),2&e&&r.wc(i=r.hc())&&(t._validationSummary=i.first)},decls:64,vars:23,consts:[[1,"row"],[1,"col-12"],[1,"card","m-b-30","m-t-30"],[1,"card-body"],[1,"mt-0","header-title"],[1,"text-muted","m-b-30","font-14"],["novalidate","",1,"needs-validation",3,"ngClass","ngSubmit"],["form","ngForm"],[3,"problemDetails"],["validationSummary",""],[1,"form-group","row"],["for","claim-type-name",1,"col-sm-2","col-form-label"],[1,"col-sm-10"],["id","claim-type-name","name","claim-type-name","type","text","required","","maxlength","64",1,"form-control",3,"ngClass","ngModel","ngModelChange"],["name","ngModel"],["class","form-control-feedback text-danger",4,"ngIf"],["for","claim-type-display-name",1,"col-sm-2","col-form-label"],["id","claim-type-display-name","name","claim-type-display-name","type","text","maxlength","128",1,"form-control",3,"ngClass","ngModel","ngModelChange"],["displayName","ngModel"],["for","claim-type-description",1,"col-sm-2","col-form-label"],["id","claim-type-description","name","claim-type-description","rows","3","maxlength","1024",1,"form-control",3,"ngModel","ngModelChange"],["for","claim-type",1,"col-sm-2","col-form-label"],["id","claim-type","name","claim-type","required","",1,"form-control","custom-select",3,"ngClass","ngModel","ngModelChange"],["type","ngModel"],["value","","selected","","disabled",""],[3,"value",4,"ngFor","ngForOf"],["for","claim-type-rule",1,"col-sm-2","col-form-label"],["id","claim-type-rule","name","claim-type-rule","type","text","maxlength","512",1,"form-control",3,"ngModel","ngModelChange"],["for","claim-type-confirmed",1,"col-sm-2","col-form-label"],[1,"form-check","col-form-label","custom-control","custom-checkbox"],["id","claim-type-confirmed","name","claim-type-confirmed","type","checkbox",1,"form-check-input","custom-control-input",3,"ngModel","ngModelChange"],["for","claim-type-confirmed",1,"custom-control-label"],["for","claim-type-editable",1,"col-sm-2","col-form-label"],["id","claim-type-editable","name","claim-type-editable","type","checkbox",1,"form-check-input","custom-control-input",3,"ngModel","ngModelChange"],["for","claim-type-editable",1,"custom-control-label"],[1,"form-group"],["type","submit",1,"btn","btn-primary","waves-effect","waves-light","m-r-5"],[1,"form-control-feedback","text-danger"],[3,"value"]],template:function(e,t){if(1&e){var i=r.Vb();r.Ub(0,"div",0),r.Ub(1,"div",1),r.Ub(2,"div",2),r.Ub(3,"div",3),r.Ub(4,"h4",4),r.Ic(5,"Add Claim Type"),r.Tb(),r.Ub(6,"p",5),r.Ic(7," Add a custom claim type. "),r.Tb(),r.Ub(8,"form",6,7),r.gc("ngSubmit",(function(){return r.zc(i),r.xc(9).form.valid&&t.save()})),r.Pb(10,"app-validation-summary",8,9),r.Ub(12,"div",10),r.Ub(13,"label",11),r.Ic(14," Name (*) "),r.Tb(),r.Ub(15,"div",12),r.Ub(16,"input",13,14),r.gc("ngModelChange",(function(e){return t.claimType.name=e})),r.Tb(),r.Gc(18,S,2,0,"div",15),r.Tb(),r.Tb(),r.Ub(19,"div",10),r.Ub(20,"label",16),r.Ic(21," Display Name "),r.Tb(),r.Ub(22,"div",12),r.Ub(23,"input",17,18),r.gc("ngModelChange",(function(e){return t.claimType.displayName=e})),r.Tb(),r.Tb(),r.Tb(),r.Ub(25,"div",10),r.Ub(26,"label",19),r.Ic(27," Description "),r.Tb(),r.Ub(28,"div",12),r.Ub(29,"textarea",20),r.gc("ngModelChange",(function(e){return t.claimType.description=e})),r.Ic(30,"                            "),r.Tb(),r.Tb(),r.Tb(),r.Ub(31,"div",10),r.Ub(32,"label",21),r.Ic(33," Type (*) "),r.Tb(),r.Ub(34,"div",12),r.Ub(35,"select",22,23),r.gc("ngModelChange",(function(e){return t.claimValueType=e})),r.Ub(37,"option",24),r.Ic(38," Please select a suitable type "),r.Tb(),r.Gc(39,x,2,2,"option",25),r.Tb(),r.Gc(40,M,2,0,"div",15),r.Tb(),r.Tb(),r.Ub(41,"div",10),r.Ub(42,"label",26),r.Ic(43," Regex Rule "),r.Tb(),r.Ub(44,"div",12),r.Ub(45,"input",27),r.gc("ngModelChange",(function(e){return t.claimType.rule=e})),r.Tb(),r.Tb(),r.Tb(),r.Ub(46,"div",10),r.Ub(47,"label",28),r.Ic(48," Required Claim "),r.Tb(),r.Ub(49,"div",12),r.Ub(50,"div",29),r.Ub(51,"input",30),r.gc("ngModelChange",(function(e){return t.claimType.required=e})),r.Tb(),r.Pb(52,"label",31),r.Tb(),r.Tb(),r.Tb(),r.Ub(53,"div",10),r.Ub(54,"label",32),r.Ic(55," User Editable "),r.Tb(),r.Ub(56,"div",12),r.Ub(57,"div",29),r.Ub(58,"input",33),r.gc("ngModelChange",(function(e){return t.claimType.userEditable=e})),r.Tb(),r.Pb(59,"label",34),r.Tb(),r.Tb(),r.Tb(),r.Ub(60,"div",35),r.Ub(61,"div"),r.Ub(62,"button",36),r.Ic(63," Save "),r.Tb(),r.Tb(),r.Tb(),r.Tb(),r.Tb(),r.Tb(),r.Tb(),r.Tb()}if(2&e){var a=r.xc(9),n=r.xc(17),c=r.xc(24),o=r.xc(36);r.Bb(8),r.oc("ngClass",r.sc(15,O,a.submitted)),r.Bb(2),r.oc("problemDetails",t.problemDetails),r.Bb(6),r.oc("ngClass",r.sc(17,B,a.submitted&&n.invalid))("ngModel",t.claimType.name),r.Bb(2),r.oc("ngIf",a.submitted&&n.invalid),r.Bb(5),r.oc("ngClass",r.sc(19,B,a.submitted&&c.invalid))("ngModel",t.claimType.displayName),r.Bb(6),r.oc("ngModel",t.claimType.description),r.Bb(6),r.oc("ngClass",r.sc(21,B,a.submitted&&o.invalid))("ngModel",t.claimValueType),r.Bb(4),r.oc("ngForOf",t.valueTypes),r.Bb(1),r.oc("ngIf",a.submitted&&o.invalid),r.Bb(5),r.oc("ngModel",t.claimType.rule),r.Bb(6),r.oc("ngModel",t.claimType.required),r.Bb(7),r.oc("ngModel",t.claimType.userEditable)}},directives:[n.y,n.n,n.o,a.k,_.a,n.b,n.u,n.j,n.m,n.p,a.m,n.v,n.q,n.x,a.l,n.a],encapsulation:2}),P);function D(e,t){if(1&e&&(r.Ub(0,"option",38),r.Ic(1),r.Tb()),2&e){var i=t.$implicit;r.oc("value",i),r.Bb(1),r.Kc(" ",i," ")}}function R(e,t){1&e&&(r.Ub(0,"div",39),r.Ic(1," Field 'Type' is required. "),r.Tb())}var G,F,A,N,L=function(e){return{"was-validated":e}},q=function(e){return{invalid:e}},V=((G=function(){function e(t,i,a,n){_classCallCheck(this,e),this._api=t,this._router=i,this._route=a,this._toast=n,this.claimType=new s.c,this.valueTypes=[],this.claimValueType=""}return _createClass(e,[{key:"ngOnInit",value:function(){for(var e in this.claimType=this._route.snapshot.data.claimType,this.claimValueType=this.claimType.valueType,s.x)e&&this.valueTypes.push(e)}},{key:"delete",value:function(){var e=this;this._api.deleteClaimType(this.claimType.id).subscribe((function(t){e._toast.showSuccess("Role '".concat(e.claimType.name,"' was deleted successfully.")),e._router.navigate(["../../"],{relativeTo:e._route})}))}},{key:"update",value:function(){var e=this;this.claimType.valueType=this.claimValueType,this._api.updateClaimType(this.claimType.id,{description:this.claimType.description,required:this.claimType.required,rule:this.claimType.rule,userEditable:this.claimType.userEditable,valueType:this.claimValueType,displayName:this.claimType.displayName}).subscribe((function(t){e._toast.showSuccess("Claim type '".concat(t.name,"' was updated successfully."))}))}}]),e}()).\u0275fac=function(e){return new(e||G)(r.Ob(s.n),r.Ob(l.d),r.Ob(l.a),r.Ob(w.a))},G.\u0275cmp=r.Ib({type:G,selectors:[["app-claim-type-edit"]],decls:65,vars:20,consts:[["title","Delete?","type","warning",3,"text","showCancelButton","confirm"],["deleteAlert",""],[1,"row"],[1,"col-12"],[1,"card","m-b-30","m-t-30"],[1,"card-body"],[1,"mt-0","header-title"],[1,"text-muted","m-b-30","font-14"],["novalidate","",1,"needs-validation",3,"ngClass","ngSubmit"],["form","ngForm"],[1,"form-group","row"],["for","claim-type-name",1,"col-sm-2","col-form-label"],[1,"col-sm-10","col-form-label"],["id","claim-type-name","name","claim-type-name",1,"col-form-label"],["for","claim-type-display-name",1,"col-sm-2","col-form-label"],[1,"col-sm-10"],["id","claim-type-display-name","name","claim-type-display-name","type","text","maxlength","128",1,"form-control",3,"ngClass","ngModel","ngModelChange"],["displayName","ngModel"],["for","claim-type-description",1,"col-sm-2","col-form-label"],["id","claim-type-description","name","claim-type-description","rows","3","maxlength","1024",1,"form-control",3,"ngModel","ngModelChange"],["for","claim-type",1,"col-sm-2","col-form-label"],["id","claim-type","name","claim-type","required","",1,"form-control","custom-select",3,"ngClass","ngModel","ngModelChange"],["type","ngModel"],["value","","selected","","disabled",""],[3,"value",4,"ngFor","ngForOf"],["class","form-control-feedback text-danger",4,"ngIf"],["for","claim-type-rule",1,"col-sm-2","col-form-label"],["id","claim-type-rule","name","claim-type-rule","type","text","maxlength","512",1,"form-control",3,"ngModel","ngModelChange"],["for","claim-type-confirmed",1,"col-sm-2","col-form-label"],[1,"form-check","col-form-label","custom-control","custom-checkbox"],["id","claim-type-confirmed","name","claim-type-confirmed","type","checkbox",1,"form-check-input","custom-control-input",3,"ngModel","ngModelChange"],["for","claim-type-confirmed",1,"custom-control-label"],["for","claim-type-editable",1,"col-sm-2","col-form-label"],["id","claim-type-editable","name","claim-type-editable","type","checkbox",1,"form-check-input","custom-control-input",3,"ngModel","ngModelChange"],["for","claim-type-editable",1,"custom-control-label"],[1,"form-group"],["type","submit",1,"btn","btn-primary","m-r-5"],["type","button",1,"btn","btn-danger","m-r-5","float-right",3,"click"],[3,"value"],[1,"form-control-feedback","text-danger"]],template:function(e,t){if(1&e){var i=r.Vb();r.Ub(0,"swal",0,1),r.gc("confirm",(function(){return t.delete()})),r.Tb(),r.Ub(2,"div",2),r.Ub(3,"div",3),r.Ub(4,"div",4),r.Ub(5,"div",5),r.Ub(6,"h4",6),r.Ic(7,"Edit Claim Type"),r.Tb(),r.Ub(8,"p",7),r.Ic(9," Here you can modify some properties of a system claim type. "),r.Tb(),r.Ub(10,"form",8,9),r.gc("ngSubmit",(function(){return r.zc(i),r.xc(11).form.valid&&t.update()})),r.Ub(12,"div",10),r.Ub(13,"label",11),r.Ic(14," Name "),r.Tb(),r.Ub(15,"div",12),r.Ub(16,"span",13),r.Ic(17),r.Tb(),r.Tb(),r.Tb(),r.Ub(18,"div",10),r.Ub(19,"label",14),r.Ic(20," Display Name "),r.Tb(),r.Ub(21,"div",15),r.Ub(22,"input",16,17),r.gc("ngModelChange",(function(e){return t.claimType.displayName=e})),r.Tb(),r.Tb(),r.Tb(),r.Ub(24,"div",10),r.Ub(25,"label",18),r.Ic(26," Description "),r.Tb(),r.Ub(27,"div",15),r.Ub(28,"textarea",19),r.gc("ngModelChange",(function(e){return t.claimType.description=e})),r.Ic(29,"                            "),r.Tb(),r.Tb(),r.Tb(),r.Ub(30,"div",10),r.Ub(31,"label",20),r.Ic(32," Type (*) "),r.Tb(),r.Ub(33,"div",15),r.Ub(34,"select",21,22),r.gc("ngModelChange",(function(e){return t.claimValueType=e})),r.Ub(36,"option",23),r.Ic(37," Please select a suitable type "),r.Tb(),r.Gc(38,D,2,2,"option",24),r.Tb(),r.Gc(39,R,2,0,"div",25),r.Tb(),r.Tb(),r.Ub(40,"div",10),r.Ub(41,"label",26),r.Ic(42," Regex Rule "),r.Tb(),r.Ub(43,"div",15),r.Ub(44,"input",27),r.gc("ngModelChange",(function(e){return t.claimType.rule=e})),r.Tb(),r.Tb(),r.Tb(),r.Ub(45,"div",10),r.Ub(46,"label",28),r.Ic(47," Required Claim "),r.Tb(),r.Ub(48,"div",15),r.Ub(49,"div",29),r.Ub(50,"input",30),r.gc("ngModelChange",(function(e){return t.claimType.required=e})),r.Tb(),r.Pb(51,"label",31),r.Tb(),r.Tb(),r.Tb(),r.Ub(52,"div",10),r.Ub(53,"label",32),r.Ic(54," User Editable "),r.Tb(),r.Ub(55,"div",15),r.Ub(56,"div",29),r.Ub(57,"input",33),r.gc("ngModelChange",(function(e){return t.claimType.userEditable=e})),r.Tb(),r.Pb(58,"label",34),r.Tb(),r.Tb(),r.Tb(),r.Ub(59,"div",35),r.Ub(60,"div"),r.Ub(61,"button",36),r.Ic(62," Save Changes "),r.Tb(),r.Ub(63,"button",37),r.gc("click",(function(){return r.zc(i),r.xc(1).fire()})),r.Ic(64," Delete "),r.Tb(),r.Tb(),r.Tb(),r.Tb(),r.Tb(),r.Tb(),r.Tb(),r.Tb()}if(2&e){var a=r.xc(11),n=r.xc(23),c=r.xc(35);r.oc("text","Claim '"+t.claimType.name+"' will be deleted permanently. This action cannot be reversed.")("showCancelButton",!0),r.Bb(10),r.oc("ngClass",r.sc(14,L,a.submitted)),r.Bb(7),r.Kc(" ",t.claimType.name," "),r.Bb(5),r.oc("ngClass",r.sc(16,q,a.submitted&&n.invalid))("ngModel",t.claimType.displayName),r.Bb(6),r.oc("ngModel",t.claimType.description),r.Bb(6),r.oc("ngClass",r.sc(18,q,a.submitted&&c.invalid))("ngModel",t.claimValueType),r.Bb(4),r.oc("ngForOf",t.valueTypes),r.Bb(1),r.oc("ngIf",a.submitted&&c.invalid),r.Bb(5),r.oc("ngModel",t.claimType.rule),r.Bb(6),r.oc("ngModel",t.claimType.required),r.Bb(7),r.oc("ngModel",t.claimType.userEditable)}},directives:[c.a,n.y,n.n,n.o,a.k,n.b,n.j,n.m,n.p,n.v,n.u,n.q,n.x,a.l,a.m,n.a],encapsulation:2}),G),j=i("LRne"),z=((F=function(){function e(t){_classCallCheck(this,e),this._api=t}return _createClass(e,[{key:"resolve",value:function(e,t){var i=e.params.id;return i?this._api.getClaimType(i):Object(j.a)(new s.c)}}]),e}()).\u0275fac=function(e){return new(e||F)(r.cc(s.n))},F.\u0275prov=r.Kb({token:F,factory:F.\u0275fac}),F),H=[{path:"",component:I},{path:"add",component:E},{path:":id/edit",component:V,resolve:{claimType:z}}],K=((N=function e(){_classCallCheck(this,e)}).\u0275mod=r.Mb({type:N}),N.\u0275inj=r.Lb({factory:function(e){return new(e||N)},providers:[z],imports:[[l.h.forChild(H)],l.h]}),N),Q=((A=function e(){_classCallCheck(this,e)}).\u0275mod=r.Mb({type:A}),A.\u0275inj=r.Lb({factory:function(e){return new(e||A)},imports:[[a.c,n.i,K,o.a,c.c]]}),A)}}]);