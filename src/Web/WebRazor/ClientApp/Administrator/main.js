'use strict';

//import '../_Core/core';
import './main.css';

import * as bs from 'bootstrap';

import { createApp } from 'vue';
import App from './Pages/_Shared/app.vue';
import pluginCommon from '../_Core/Plugins/common';
import pluginToasts from '../_Core/Plugins/toasts';
import Notification from '../_Core/Components/Notifications/Notification.vue';

//import MomentVue from 'vue-moment';

//import VueObserveVisibility from 'vue-observe-visibility'
//Vue.use(VueObserveVisibility);

//import common from '../_Core/Plugins/common';
//Vue.use(common);

//import VueRouter from 'vue-router';
//Vue.use(VueRouter);

//import Router from './router';

//import '../_Core/Plugins/bootstrap-vue';

//  global components
import Pagination from '../_Core/Components/pagination.vue';
import SortField from '../_Core/Components/sortfield.vue';
import TableList from '../_Core/Components/table-list.vue';
//import GMapLocation from '../_Core/Components/gmap-location.vue';

//Vue.component('m-pagination', Pagination);
//Vue.component('sort-field', SortField);
//Vue.component('table-list', TableList);
//Vue.component('gmap-location', GMapLocation);

const app = createApp(App);

app.use(pluginCommon);
app.use(pluginToasts);

app.component('Notification', Notification);
app.component('m-pagination', Pagination);
app.component('sort-field', SortField);
app.component('table-list', TableList);

app.mount('#root');