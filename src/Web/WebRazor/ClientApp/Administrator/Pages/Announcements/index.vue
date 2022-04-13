<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fa-solid fa-fw fa-bullhorn me-1"></i>Announcements
                </h1>
            </div>
            <div class="col-sm-auto">
                <div class="d-flex flex-row">
                    <div class="">
                        <a :href="urlAdd" class="btn btn-primary">
                            <i class="fas fa-plus me-1"></i>Add
                        </a>
                    </div>

                    <!--<div v-if="filter.visible" class="mr-1">
                    <button @click="resetDates" class="btn btn-primary">
                        <i class="fas fa-sync mr-1"></i>
                    </button>
                </div>
                <div class="mr-1">
                    <button @click="filter.visible = !filter.visible" class="btn btn-secondary">
                        <span class="fa fas fa-fw fa-filter"></span>
                    </button>
                </div>-->
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
            <table-list :header="{key: 'announcementId', columns:[]}" :items="filter.items" :getRowNumber="getRowNumber" :setSelected="setSelected" :isSelected="isSelected" table-css="">
                <template #header>
                    <th class="text-center">#</th>
                    <th>Post Date</th>
                    <th>Subject</th>                    
                </template>
                <template #table-row="row">
                    <td v-text="getRowNumber(row.index)" class="text-center"></td>
                    <td>
                        <a :href="`${urlView}/${row.item.announcementId}`">
                            {{row.item.subject}}
                        </a>
                    </td>
                    <td>
                        {{$moment(row.item.dateCreated).calendar()}}
                    </td>                    
                </template>

                <template #table-list="row">
                    <div>
                        <div class="form-group mb-0 row no-gutters">
                            <label class="col-3 col-form-label">Name</label>
                            <div class="col align-self-center">
                                <a href="#" @click.prevent="$refs.modalViewContact.open(row.item.contactId)">
                                    {{row.item.number}}
                                </a>
                            </div>
                        </div>
                        <div class="form-group mb-0 row no-gutters">
                            <label class="col-3 col-form-label">Status</label>
                            <div class="col align-self-center">
                                <div>{{row.item.statusText}}</div>
                            </div>
                        </div>
                        <div class="form-group mb-0 row no-gutters">
                            <label class="col-3 col-form-label">Email</label>
                            <div class="col align-self-center">
                                {{row.item.email}}
                            </div>
                        </div>
                        <div class="form-group mb-0 row no-gutters">
                            <label class="col-3 col-form-label">Contact #</label>
                            <div class="col">
                                <div class="form-control-plaintext">

                                </div>
                            </div>
                        </div>
                        <div class="form-group mb-0 row no-gutters">
                            <!--<label class="col-3 col-form-label"></label>-->
                            <div class="offset-3 col align-self-center">
                                <button @click="addTask(row.item)" class="btn btn-sm btn-outline-primary">Add Task</button>
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
            urlAdd: { type: String, required: true },
            urlView: { type: String, required: true },
        },
        components: {
            //modalViewTask
        },
        data() {
            return {
                //moment: moment,

                baseUrl: `/api/announcement`,
                lookups: {
                    
                },
                filter: {
                    items: [],
                    cacheKey: `filter-${this.uid}/announcements`,
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