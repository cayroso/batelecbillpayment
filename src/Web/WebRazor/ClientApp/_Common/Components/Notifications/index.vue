<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fa-solid fa-fw fa-bell me-1"></i>Notifications
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
            <table-list :header="{key: 'notificationId', columns:[]}" :items="filter.items" :getRowNumber="getRowNumber" :setSelected="setSelected" :isSelected="isSelected" table-css="">
                <template #header>
                    <th class="text-center">#</th>
                    <th>Subject</th>
                    <th>Date</th>
                    <th></th>
                </template>
                <template #table-row="row">
                    <td v-text="getRowNumber(row.index)" class="text-center"></td>
                    <td>
                        <a :href="`${urlView}/${row.item.notificationId}`" v-bind:class="!row.item.isRead ? 'fw-bold':''" >
                            {{row.item.subject}}
                        </a>
                    </td>
                    <td>
                        {{$moment(row.item.dateSent).calendar()}}
                    </td>

                    <td>
                        <div v-if="row.item.expand">
                            <button @click="deleteReservation(row.item)" class="btn btn-warning">
                                <i class="fas fa-trash me-1"></i>Delete
                            </button>
                        </div>
                    </td>
                </template>

                <template #table-list="row">
                    <div>
                        <div class="row mb-3">
                            <label for="subject" class="col-sm-2 col-form-label">Subject</label>
                            <div class="col-sm-10">
                                <div class="form-control-plaintext">
                                    <a :href="`${urlView}/${row.item.announcementId}`">
                                        {{row.item.subject}}
                                    </a>
                                </div>
                            </div>
                        </div>

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
            //urlAdd: { type: String, required: true },
            urlView: { type: String, required: true },
        },
        components: {
            //modalViewTask
        },
        data() {
            return {
                //moment: moment,

                baseUrl: `/api/notification/my-notifications`,
                lookups: {

                },
                filter: {
                    items: [],
                    cacheKey: `filter-${this.uid}/my-notifications`,
                    query: {
                        unreadOnly: false,
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

            filter.query.unreadOnly = urlParams.get('uro') || cache.unreadOnly || filter.query.unreadOnly;
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
                    '&uro=', filter.query.unreadOnly,
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
                    unreadOnly: filter.query.unreadOnly,
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

            async deleteNotification(item) {
                const vm = this;

                const reason = prompt('Enter reason for deleting this notification.');

                if (!reason)
                    return;

                if (vm.busy)
                    return;

                try {
                    vm.busy = true;

                    await vm.$util.axios.delete(`/api/reservation/${item.reservationId}/${reason}`)
                        .then(resp => {
                            alert('Reservation was deleted.');
                        });

                } catch (e) {
                    let errorHtml = vm.$util.getErrorMessageAsHtml(e);
                    alert(errorHtml);
                } finally {
                    vm.busy = false;
                    await vm.search(1);
                }
            }
        }
    }
</script>