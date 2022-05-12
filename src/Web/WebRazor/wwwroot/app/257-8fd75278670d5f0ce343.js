"use strict";(self.webpackChunkclient=self.webpackChunkclient||[]).push([[257],{69392:(e,t,r)=>{r.d(t,{Z:()=>M});r(38833),r(56394),r(62322),r(79006),r(11646);var n=r(68205),a={class:"row align-items-center"},s=(0,n._)("div",{class:"col-sm"},[(0,n._)("h1",{class:"h3 mb-sm-0"},[(0,n._)("i",{class:"fa-solid fa-fw fa-calendar-check me-1"}),(0,n.Uk)("Reservations ")])],-1),i={class:"col-sm-auto"},c={class:"d-flex flex-row"},o={key:0,class:""},l=["href"],u=[(0,n._)("i",{class:"fas fa-plus me-1"},null,-1),(0,n.Uk)("Add ")],d={class:"flex-grow-1 ms-1"},m={class:"input-group"},f=[(0,n._)("i",{class:"fa-solid fa-magnifying-glass"},null,-1)],v={class:"mt-2 table-responsive"},p=(0,n._)("th",{class:"text-center"},"#",-1),b=(0,n._)("th",null,"Branch",-1),h={key:0},w=(0,n._)("th",null,"Date",-1),g=(0,n._)("th",null,null,-1),_=["textContent"],y={key:0},k=["href"],x={key:0},R=["onClick"],q=[(0,n._)("i",{class:"fas fa-trash me-1"},null,-1),(0,n.Uk)("Delete ")],S={class:"row mb-3"},I=(0,n._)("label",{for:"subject",class:"col-sm-2 col-form-label"},"Branch",-1),D={class:"col-sm-10"},N={class:"form-control-plaintext"},C={class:"row mb-3"},z=(0,n._)("label",{for:"subject",class:"col-sm-2 col-form-label"},"Account",-1),Z={class:"col-sm-10"},$={class:"form-control-plaintext"},U={class:"row mb-3"},A=(0,n._)("label",{for:"subject",class:"col-sm-2 col-form-label"},"Date",-1),E={class:"col-sm-10"},O={class:"form-control-plaintext"},V=["href"],j={key:0,class:"row mb-3"},B={class:"col-sm-10 offset-sm-2"},F={class:"align-self-center"},K=["onClick"],P=[(0,n._)("i",{class:"fas fa-trash me-1"},null,-1),(0,n.Uk)("Delete ")];var H=r(54390);r(77588),r(92129),r(24655),r(46549),r(67441),r(84415);const J={mixins:[r(87124).Z],props:{uid:String,roleId:{type:String,required:!0},urlAdd:{type:String,required:!0},urlView:{type:String,required:!0}},components:{},data:function(){return{baseUrl:"/api/reservation",lookups:{},filter:{items:[],cacheKey:"filter-".concat(this.uid,"/notifications"),query:{}}}},computed:{},created:function(){var e=this;return(0,H.Z)(regeneratorRuntime.mark((function t(){var r,n,a;return regeneratorRuntime.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return n=(r=e).filter,new URLSearchParams(window.location.search),a=JSON.parse(localStorage.getItem(n.cacheKey))||{},"consumer"===r.roleId&&(r.baseUrl="/api/reservation/mine"),r.initializeFilter(a),t.next=8,r.search();case 8:case"end":return t.stop()}}),t)})))()},mounted:function(){var e=this;return(0,H.Z)(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:e;case 1:case"end":return t.stop()}}),t)})))()},methods:{getQuery:function(){var e=this.filter;if(!this.busy)return["?c=",encodeURIComponent(e.query.criteria),"&p=",e.query.pageIndex,"&s=",e.query.pageSize,"&sf=",e.query.sortField,"&so=",e.query.sortOrder].join("")},saveQuery:function(){var e=this.filter;localStorage.setItem(e.cacheKey,JSON.stringify({criteria:e.query.criteria,pageIndex:e.query.pageIndex,pageSize:e.query.pageSize,sortField:e.query.sortField,sortOrder:e.query.sortOrder,visible:e.visible}))},getTaskTdCss:function(e){switch(e.taskStatus){case 1:return"table-warning";case 2:return"table-info";case 3:return"table-success";default:return""}},deleteReservation:function(e){var t=this;return(0,H.Z)(regeneratorRuntime.mark((function r(){var n,a,s;return regeneratorRuntime.wrap((function(r){for(;;)switch(r.prev=r.next){case 0:if(n=t,a=prompt("Enter reason for deleting this reservation.")){r.next=4;break}return r.abrupt("return");case 4:if(!n.busy){r.next=6;break}return r.abrupt("return");case 6:return r.prev=6,n.busy=!0,r.next=10,n.$util.axios.delete("/api/reservation/".concat(e.reservationId,"/").concat(a)).then((function(e){n.$toast.warning("Delete Reservation","Reservation was deleted.")}));case 10:r.next=16;break;case 12:r.prev=12,r.t0=r.catch(6),s=n.$util.getErrorMessageAsHtml(r.t0),alert(s);case 16:return r.prev=16,n.busy=!1,r.next=20,n.search(1);case 20:return r.finish(16);case 21:case"end":return r.stop()}}),r,null,[[6,12,16,21]])})))()}}};const M=(0,r(46021).Z)(J,[["render",function(e,t,r,H,J,M){var Q=(0,n.up)("table-list"),T=(0,n.up)("m-pagination");return(0,n.wg)(),(0,n.iD)("div",null,[(0,n._)("div",a,[s,(0,n._)("div",i,[(0,n._)("div",c,["consumer"===r.roleId?((0,n.wg)(),(0,n.iD)("div",o,[(0,n._)("a",{href:r.urlAdd,class:"btn btn-primary"},u,8,l)])):(0,n.kq)("",!0),(0,n._)("div",d,[(0,n._)("div",m,[(0,n.wy)((0,n._)("input",{type:"text",class:"form-control",placeholder:"Enter criteria...","aria-label":"Enter criteria...","aria-describedby":"button-addon2","onUpdate:modelValue":t[0]||(t[0]=function(e){return J.filter.query.criteria=e}),onKeyup:t[1]||(t[1]=(0,n.D2)((function(t){return e.search(1)}),["enter"]))},null,544),[[n.nr,J.filter.query.criteria]]),(0,n._)("button",{class:"btn btn-primary",type:"button",id:"button-addon2",onClick:t[2]||(t[2]=function(t){return e.search(1)})},f)])])])])]),(0,n._)("div",v,[(0,n.Wm)(Q,{header:{key:"reservationId",columns:[]},items:J.filter.items,getRowNumber:e.getRowNumber,setSelected:e.setSelected,isSelected:e.isSelected,"table-css":""},{header:(0,n.w5)((function(){return[p,b,"administrator"===r.roleId?((0,n.wg)(),(0,n.iD)("th",h,"Account")):(0,n.kq)("",!0),w,g]})),"table-row":(0,n.w5)((function(t){return[(0,n._)("td",{textContent:(0,n.zw)(e.getRowNumber(t.index)),class:"text-center"},null,8,_),(0,n._)("td",null,(0,n.zw)(t.item.branchName),1),"administrator"===r.roleId?((0,n.wg)(),(0,n.iD)("td",y,(0,n.zw)(t.item.accountName),1)):(0,n.kq)("",!0),(0,n._)("td",null,[(0,n._)("a",{href:"".concat(r.urlView,"/").concat(t.item.reservationId)},(0,n.zw)(e.$moment(t.item.dateReservation).calendar()),9,k)]),(0,n._)("td",null,[t.item.expand?((0,n.wg)(),(0,n.iD)("div",x,[(0,n._)("button",{onClick:function(e){return M.deleteReservation(t.item)},class:"btn btn-warning"},q,8,R)])):(0,n.kq)("",!0)])]})),"table-list":(0,n.w5)((function(t){return[(0,n._)("div",null,[(0,n._)("div",S,[I,(0,n._)("div",D,[(0,n._)("div",N,(0,n.zw)(t.item.branchName),1)])]),(0,n._)("div",C,[z,(0,n._)("div",Z,[(0,n._)("div",$,(0,n.zw)(t.item.accountName),1)])]),(0,n._)("div",U,[A,(0,n._)("div",E,[(0,n._)("div",O,[(0,n._)("a",{href:"".concat(r.urlView,"/").concat(t.item.reservationId)},(0,n.zw)(e.$moment(t.item.dateReservation).calendar()),9,V)])])]),t.item.expand?((0,n.wg)(),(0,n.iD)("div",j,[(0,n._)("div",B,[(0,n._)("div",F,[(0,n._)("button",{onClick:function(e){return M.deleteReservation(t.item)},class:"btn btn-warning"},P,8,K)])])])):(0,n.kq)("",!0)])]})),_:1},8,["items","getRowNumber","setSelected","isSelected"])]),(0,n.Wm)(T,{filter:J.filter,search:e.search,showPerPage:!0,class:"mt-2"},null,8,["filter","search"])])}]])},58671:(e,t,r)=>{r.d(t,{Z:()=>C});var n=r(68205),a={class:"row align-items-center"},s=(0,n._)("div",{class:"col"},[(0,n._)("h1",{class:"h3 mb-sm-0"},[(0,n._)("i",{class:"fas fa-fw fa-calendar-check me-1"}),(0,n.Uk)("View Reservation ")])],-1),i={class:"col-auto"},c={class:"d-flex flex-row"},o=[(0,n._)("span",{class:"fas fa-fw fa-trash"},null,-1)],l=[(0,n._)("span",{class:"fas fa-fw fa-sync"},null,-1)],u=[(0,n._)("i",{class:"fas fa-fw fa-times"},null,-1)],d={class:"mt-2"},m={class:"card shadow-sm"},f=(0,n._)("div",{class:"card-header bg-success text-white"},null,-1),v={class:"card-body"},p={class:"row"},b={class:"col-sm"},h={class:"form-floating mb-3"},w={class:"form-control",id:"branchName",placeholder:"Branch"},g=(0,n._)("label",{for:"branchName"},"Branch",-1),_={key:0,class:"col-sm"},y={class:"form-floating mb-3"},k={class:"form-control",id:"accountName",placeholder:"Account"},x=(0,n._)("label",{for:"accountName"},"Account",-1),R={class:"col-sm"},q={class:"form-floating mb-3"},S={class:"form-control",id:"dateReservation",placeholder:"Date Reservation"},I=(0,n._)("label",{for:"dateReservation"},"Date Reservation",-1);var D=r(54390);r(11646),r(77588);const N={mixins:[r(15500).Z],props:{uid:{type:String,required:!0},id:{type:String,required:!0},roleId:{type:String,required:!0}},data:function(){return{item:{}}},computed:{},created:function(){var e=this;return(0,D.Z)(regeneratorRuntime.mark((function t(){return regeneratorRuntime.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:e;case 1:case"end":return t.stop()}}),t)})))()},mounted:function(){var e=this;return(0,D.Z)(regeneratorRuntime.mark((function t(){var r;return regeneratorRuntime.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:return r=e,t.next=3,r.get();case 3:case"end":return t.stop()}}),t)})))()},methods:{get:function(){var e=this;return(0,D.Z)(regeneratorRuntime.mark((function t(){var r;return regeneratorRuntime.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:if(!(r=e).busy){t.next=3;break}return t.abrupt("return");case 3:return t.prev=3,r.busy=!0,t.next=7,r.$util.axios.get("/api/reservation/".concat(r.id)).then((function(e){r.item=e.data}));case 7:t.next=12;break;case 9:t.prev=9,t.t0=t.catch(3),r.$util.handleError(t.t0);case 12:return t.prev=12,r.busy=!1,t.finish(12);case 15:case"end":return t.stop()}}),t,null,[[3,9,12,15]])})))()},remove:function(){var e=this;return(0,D.Z)(regeneratorRuntime.mark((function t(){var r,n,a;return regeneratorRuntime.wrap((function(t){for(;;)switch(t.prev=t.next){case 0:if(r=e,n=prompt("Enter reason for deleting this reservation.")){t.next=4;break}return t.abrupt("return");case 4:if(!r.busy){t.next=6;break}return t.abrupt("return");case 6:return t.prev=6,r.busy=!0,t.next=10,r.$util.axios.delete("/api/reservation/".concat(r.item.reservationId,"/").concat(n)).then((function(e){r.$toast.warning("Delete Reservation","Reservation was deleted.",{onClose:function(){return(0,D.Z)(regeneratorRuntime.mark((function e(){return regeneratorRuntime.wrap((function(e){for(;;)switch(e.prev=e.next){case 0:return e.next=2,r.close();case 2:case"end":return e.stop()}}),e)})))()}})}));case 10:t.next=16;break;case 12:t.prev=12,t.t0=t.catch(6),a=r.$util.getErrorMessageAsHtml(t.t0),alert(a);case 16:return t.prev=16,r.busy=!1,t.finish(16);case 19:case"end":return t.stop()}}),t,null,[[6,12,16,19]])})))()}}};const C=(0,r(46021).Z)(N,[["render",function(e,t,r,D,N,C){return(0,n.wg)(),(0,n.iD)("div",null,[(0,n._)("div",null,[(0,n._)("div",a,[s,(0,n._)("div",i,[(0,n._)("div",c,[(0,n._)("button",{onClick:t[0]||(t[0]=function(){return C.remove&&C.remove.apply(C,arguments)}),class:"ms-2 btn btn-warning"},o),(0,n._)("button",{onClick:t[1]||(t[1]=function(){return C.get&&C.get.apply(C,arguments)}),class:"ms-2 btn btn-primary"},l),(0,n._)("button",{onClick:t[2]||(t[2]=function(){return e.close&&e.close.apply(e,arguments)}),class:"ms-1 btn btn-secondary"},u)])])]),(0,n._)("div",d,[(0,n._)("div",m,[f,(0,n._)("div",v,[(0,n._)("div",null,[(0,n._)("div",p,[(0,n._)("div",b,[(0,n._)("div",h,[(0,n._)("div",w,(0,n.zw)(N.item.branchName),1),g])]),"administrator"===r.roleId?((0,n.wg)(),(0,n.iD)("div",_,[(0,n._)("div",y,[(0,n._)("div",k,(0,n.zw)(N.item.accountName),1),x])])):(0,n.kq)("",!0),(0,n._)("div",R,[(0,n._)("div",q,[(0,n._)("div",S,(0,n.zw)(e.$moment(N.item.dateReservation).calendar()),1),I])])])])])])])])])}]])}}]);