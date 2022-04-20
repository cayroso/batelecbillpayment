<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fa-solid fa-fw fa-user-shield me-1"></i>Administrators
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
            <table-list :header="{key: 'userId', columns:[]}" :items="filter.items" :getRowNumber="getRowNumber" :setSelected="setSelected" :isSelected="isSelected" table-css="">
                <template #header>
                    <th class="text-center">#</th>
                    <th>Name</th>
                    <th>Email</th>
                    <th>Phone</th>
                    <th>Is Locked</th>
                </template>
                <template #table-row="row">
                    <td v-text="getRowNumber(row.index)" class="text-center"></td>
                    <td>
                        <a :href="`${urlView}/${row.item.userId}`">
                            {{row.item.firstLastName}}
                        </a>
                    </td>
                    <td>
                        {{row.item.email}}
                    </td>
                    <td>
                        {{row.item.phoneNumber}}
                    </td>
                    <th>
                        <div v-if="row.item.expand">
                            <template v-if="row.item.isLocked">
                                <button @click="lockUnlockUser(row.item.userId, false)" class="btn btn-sm btn-outline-primary">
                                    <i class="fa-solid fa-unlock"></i>
                                    Unlock
                                </button>
                            </template>
                            <template v-else>
                                <button @click="lockUnlockUser(row.item.userId, true)" class="btn btn-sm btn-outline-warning">
                                    <i class="fa-solid fa-lock"></i>
                                    Lock
                                </button>
                            </template>
                        </div>
                        <div v-else>
                            <span v-if="row.item.isLocked" class="fa-solid fa-lock text-danger"></span>                      
                        </div>
                    </th>
                </template>

                <template #table-list="row">
                    <div>
                        <div class="row mb-3">
                            <label for="subject" class="col-sm-2 col-form-label">Name</label>
                            <div class="col-sm-10">
                                <div class="form-control-plaintext">
                                    <a :href="`${urlView}/${row.item.userId}`">
                                        {{row.item.firstLastName}}
                                    </a>
                                </div>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <label for="subject" class="col-sm-2 col-form-label">Email</label>
                            <div class="col-sm-10">
                                <div class="form-control-plaintext">
                                    {{row.item.email}}
                                </div>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <label for="subject" class="col-sm-2 col-form-label">Phone Number</label>
                            <div class="col-sm-10">
                                <div class="form-control-plaintext">
                                    {{row.item.phoneNumber}}
                                </div>
                            </div>
                        </div>
                        <div class="row mb-3">
                            <label for="subject" class="col-sm-2 col-form-label">Is Locked</label>
                            <div class="col-sm-10">
                                <div class="align-self-center">
                                    <div v-if="row.item.expand">
                                        <template v-if="row.item.isLocked">
                                            <button @click="lockUnlockUser(row.item.userId, false)" class="btn btn-sm btn-outline-primary">
                                                <i class="fa-solid fa-unlock"></i>
                                                Unlock
                                            </button>
                                        </template>
                                        <template v-else>
                                            <button @click="lockUnlockUser(row.item.userId, true)" class="btn btn-sm btn-outline-warning">
                                                <i class="fa-solid fa-lock"></i>
                                                Lock
                                            </button>
                                        </template>
                                    </div>
                                    <div v-else>
                                        <span v-if="row.item.isLocked" class="fa-solid fa-lock text-danger"></span>
                                    </div>
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
            urlAdd: { type: String, required: true },
            urlView: { type: String, required: true },
        },
        components: {
            //modalViewTask
        },
        data() {
            return {
                //moment: moment,

                baseUrl: `/api/account/administrators`,
                lookups: {

                },
                filter: {
                    items: [],
                    cacheKey: `filter-${this.uid}/account/administrators`,
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

            async lockUnlockUser(id, lock) {
                const vm = this;
                let answer = confirm('Are you sure you wnat to lock this administrator?');

                if (answer === true) {
                    try {
                        vm.busy = true;


                        let url = `/api/account/lock/`;
                        let payload = {
                            userId: id,
                            lockout: true
                        };
                        let title = 'Lock Administrator';
                        let message = 'Administrator successfully locked.'

                        if (!lock) {
                            title = 'Unlock Administrator';
                            message = 'Administrator successfully unlocked.';
                            payload = {
                                userId: id,
                                lockout: false
                            };
                        }

                        await vm.$util.axios.put(url, payload)
                            .then(resp => {
                                vm.$toast.success(title, message, {
                                    async onClose() {
                                        await vm.search(1);
                                    }
                                });
                            });

                    } catch (e) {
                        vm.$util.handleError(e);
                    } finally {
                        vm.busy = false
                    }
                }

            }
        }
    }
</script>