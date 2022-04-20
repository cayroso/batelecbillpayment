<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fas fa-fw fa-money-bill me-1"></i>Billings
                </h1>
            </div>
            <div class="col-sm-auto">
                <div class="d-flex flex-row">                    
                    <div class="flex-grow-1 ms-1">
                        <div class="input-group">
                            <input type="text" class="form-control" placeholder="Enter criteria..." aria-label="Enter criteria..." aria-describedby="button-addon2" v-model="filter.query.criteria" @keyup.enter="search(1)">
                            <button class="btn btn-primary" type="button" id="button-addon2" @click="search(1)">
                                <i class="fa-solid fa-magnifying-glass"></i>
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="mt-2 table-responsive">
        <table-list :header="{key: 'billingId', columns:[]}" :items="filter.items" :getRowNumber="getRowNumber" :setSelected="setSelected" :isSelected="isSelected" table-css="">
            <template #header>
                <th class="text-center">#</th>
                <th>Billing Number</th>
                <th>Status</th>
                <th>Due Date</th>
                <th>Amount</th>
            </template>
            <template #table-row="row">
                <td v-text="getRowNumber(row.index)" class="text-center"></td>
                <td>
                    <a :href="`${urlView}/${row.item.billingId}`">
                        {{row.item.number}}
                    </a>
                </td>
                <td>
                    {{row.item.statusText}}
                </td>
                <td>
                    {{$moment(row.item.dateDue).format('YYYY-MM-DD hh:mm:ss')}}
                </td>
                <td>
                    {{row.item.amount}}
                </td>
            </template>

            <template #table-list="row">
                <div>
                    <div class="row mb-3">
                        <label for="subject" class="col-sm-2 col-form-label">Billing Number</label>
                        <div class="col-sm-10">
                            <div class="form-control-plaintext">
                                <a :href="`/administrator/billings/view/${row.item.billingId}`">
                                    {{row.item.number}}
                                </a>
                            </div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <label for="subject" class="col-sm-2 col-form-label">Status</label>
                        <div class="col-sm-10">
                            <div class="form-control-plaintext">
                                {{row.item.statusText}}
                            </div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <label for="subject" class="col-sm-2 col-form-label">Due Date</label>
                        <div class="col-sm-10">
                            <div class="form-control-plaintext">
                                {{$moment(row.item.dateDue).format('YYYY-MM-DD hh:mm:ss')}}
                            </div>
                        </div>
                    </div>
                    <div class="row mb-3">
                        <label for="subject" class="col-sm-2 col-form-label">Amount</label>
                        <div class="col-sm-10">
                            <div class="form-control-plaintext">
                                {{row.item.amount}}
                            </div>
                        </div>
                    </div>

                </div>
            </template>
        </table-list>

        </div>

        <m-pagination :filter="filter" :search="search" :showPerPage="true" class="mt-2"></m-pagination>
    </div>
</template>
<script>
    import paginatedMixin from '../../../_Core/Mixins/paginatedMixin';
    //import modalViewTask from '../../Modals/Tasks/view-task.vue';
    export default {
        mixins: [paginatedMixin],

        props: {
            uid: String,
            urlAdd: String,
            urlView: String,
        },
        components: {
            //modalViewTask
        },
        data() {
            return {
                //moment: moment,

                baseUrl: `/api/billing/my-billings`,
                lookups: {
                    taskStatuses: [
                        { id: 0, name: 'All' },
                        { id: 1, name: 'Todo' },
                        { id: 2, name: 'In Progress' },
                        { id: 3, name: 'Done' },
                    ],
                    taskTypes: [
                        { id: 0, name: 'All' },
                        { id: 1, name: 'Follow-Up Email' },
                        { id: 2, name: 'Phone Call' },
                        { id: 3, name: 'Demo' },
                        { id: 4, name: 'Lunch Meeting' },
                        { id: 5, name: 'Meetup' },
                        { id: 6, name: 'Others' },
                    ]
                },
                filter: {
                    items:[],
                    cacheKey: `filter-${this.uid}/billings`,
                    query: {
                        //taskStatus: 0,
                        //taskType: 0,
                        //    dateStart: moment().startOf('week').format('YYYY-MM-DD'),
                        //    dateEnd: moment().endOf('week').format('YYYY-MM-DD')
                    }
                },
            };
        },

        computed: {

        },

        async created() {
            const vm = this;
            const filter = vm.filter;

            const urlParams = new URLSearchParams(window.location.search);
            const cache = JSON.parse(localStorage.getItem(filter.cacheKey)) || {};

            //filter.query.taskType = urlParams.get('tt') || cache.taskType || filter.query.taskType;
            //filter.query.taskStatus = urlParams.get('ts') || cache.taskStatus || filter.query.taskStatus;

            vm.initializeFilter(cache);

            await vm.search();

        },

        async mounted() {
            const vm = this;

        },

        methods: {
            getQuery() {

                const vm = this;
                const filter = vm.filter;

                if (vm.busy)
                    return;

                const query = [
                    '?c=', encodeURIComponent(filter.query.criteria),
                    '&p=', filter.query.pageIndex,
                    '&s=', filter.query.pageSize,
                    '&sf=', filter.query.sortField,
                    '&so=', filter.query.sortOrder,
                    //'&tt=', filter.query.taskType,
                    //'&ts=', filter.query.taskStatus,
                ].join('');

                return query;
            },

            saveQuery() {
                const vm = this;
                const filter = vm.filter;

                localStorage.setItem(filter.cacheKey, JSON.stringify({
                    criteria: filter.query.criteria,
                    pageIndex: filter.query.pageIndex,
                    pageSize: filter.query.pageSize,
                    sortField: filter.query.sortField,
                    sortOrder: filter.query.sortOrder,
                    visible: filter.visible,
                    //taskType: filter.query.taskType,
                    //taskStatus: filter.query.taskStatus,
                }));
            },

            getTaskTdCss(task) {

                switch (task.taskStatus) {
                    case 1:
                        return 'table-warning';
                    case 2:
                        return 'table-info';
                    case 3:
                        return 'table-success';
                    default:
                        return '';
                }
            },
        }
    }
</script>