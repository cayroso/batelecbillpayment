'use strict';


import * as Vue from 'vue';
import MomentVue from 'vue-moment';
Vue.use(MomentVue);

window.moment = require('moment');

Vue.filter('formatDate', function (value) {
    if (value) {
        return moment(String(value)).format('YYYY-MM-DD hh:mm:ss');
    }
});

Vue.filter('toMoment', function (value) {
    if (value) {
        return moment(String(value)).calendar();
    }
});

Vue.filter('toCurrency', function (value) {
    if (typeof value !== "number") {
        return value;
    }
    //var formatter = new Intl.NumberFormat('en-US', {
    //    style: 'currency',
    //    currency: 'USD',
    //    minimumFractionDigits: 2
    //});
    var formatter = new Intl.NumberFormat('en-PH', {
        style: 'currency',
        currency: 'PHP',
        minimumFractionDigits: 2
    });
    return formatter.format(value);
});
Vue.filter('prettyJson', function (value) {
    return JSON.stringify(value, null,1);
});
Vue.filter('textTruncate', function (text, length, suffix) {
    if (text.length > length) {
        return text.substring(0, length) + suffix;
    } else {
        return text;
    }
});
Vue.filter('formatBytes', function (bytes, decimals = 2) {
    if (bytes === 0) return '0 Bytes';

    const k = 1024;
    const dm = decimals < 0 ? 0 : decimals;
    const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

    const i = Math.floor(Math.log(bytes) / Math.log(k));

    return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
});

//import '../_Core/Plugins/bootstrap-vue';

//  global components
import Pagination from '../_Core/Components/pagination.vue';
import SortField from '../_Core/Components/sortfield.vue';
import TableList from '../_Core/Components/table-list.vue';
import GMapLocation from '../_Core/Components/gmap-location.vue';

Vue.component('m-pagination', Pagination);
Vue.component('sort-field', SortField);
Vue.component('table-list', TableList);
Vue.component('gmap-location', GMapLocation);