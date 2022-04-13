<template>
    <div v-cloak>

        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fas fa-fw fa-id-card mr-1"></i>Contacts
                </h1>
            </div>
            <div class="col-sm-auto">
                <div class="d-flex flex-row">
                    <div class="mr-1">
                        <router-link to="/contacts/add" class="btn btn-primary">
                            <i class="fas fa-plus"></i>
                        </router-link>
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
                    <div class="flex-grow-1">
                        <div class="input-group">
                            <input v-model="filter.query.criteria" @keyup.enter="search(1)" type="text" class="form-control" placeholder="Enter criteria..." aria-label="Enter criteria..." aria-describedby="button-addon2">
                            <div class="input-group-append">
                                <button @click="search(1)" class="btn btn-primary" type="button" id="button-addon2">
                                    <span class="fa fas fa-fw fa-search"></span>
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <b-collapse v-model="filter.visible">
            <!--<div class="card p-2 mt-2 bg-secondary">
                <div class="row">
                    <div class="col-6 col-sm-4 col-md-3">
                        <div class="form-group">
                            <label class="col-form-label">Patients</label>
                            <b-select v-model="filter.query.patientId" :options="lookups.patients" :value-field="`id`" :text-field="`name`">
                                <template v-slot:first>
                                    <option :value="null">All</option>
                                </template>
                            </b-select>
                        </div>
                    </div>
                    <div class="col-6 col-sm-4 col-md-3">
                        <div class="form-group">
                            <label class="col-form-label">Caregivers</label>
                            <b-form-select v-model="filter.query.caregiverId" :options="lookups.caregivers" :value-field="`id`" :text-field="`name`">
                                <template v-slot:first>
                                    <option :value="null">All</option>
                                </template>
                            </b-form-select>
                        </div>
                    </div>
                    <div class="col-6 col-sm-4 col-md-3">
                        <div class="form-group">
                            <label class="col-form-label ">Job Status</label>
                            <b-form-select v-model="filter.query.jobStatus" :options="lookups.jobStatuses" :value-field="`id`" :text-field="`name`" :class="`text-capitalize`">
                                <template v-slot:first>
                                    <option :value="`0`">All</option>
                                </template>

                            </b-form-select>
                        </div>
                    </div>

                    <div class="w-100 d-block d-sm-none d-md-block"></div>

                    <div class="col-6 col-sm-4 col-md-3">
                        <div class="form-group">
                            <label for="createdForm" class="col-form-label">Created From</label>

                            <b-form-datepicker id="dateStart"
                                               v-model="filter.query.dateStart"
                                               right
                                               :date-format-options="{ year: 'numeric', month: 'numeric', day: 'numeric' }"></b-form-datepicker>

                        </div>
                    </div>
                    <div class="col-6 col-sm-4 col-md-3">
                        <div class="form-group">
                            <label for="createdForm" class="col-form-label">Created To</label>
                            <b-form-datepicker id="dateEnd"
                                               v-model="filter.query.dateEnd"
                                               right
                                               :date-format-options="{ year: 'numeric', month: 'numeric', day: 'numeric' }"></b-form-datepicker>

                        </div>
                    </div>
                </div>
            </div>-->
        </b-collapse>

        <b-overlay :show="busy">
            <div class="mt-2 table-responsive shadow-sm">
                <table-list :header="{key: 'contactId', columns:[]}" :items="filter.items" :getRowNumber="getRowNumber" :setSelected="setSelected" :isSelected="isSelected" table-css="">
                    <template #header>
                        <th class="text-center">#</th>
                        <th>Name</th>
                        <th>Status</th>
                        <th>Contact #</th>
                        <th>Assigned To</th>
                        <th></th>
                    </template>
                    <template slot="table" slot-scope="row">
                        <td v-text="getRowNumber(row.index)" class="text-center"></td>
                        <td>
                            <router-link :to="{name:'contactsView', params:{id:row.item.contactId}}">
                                <span v-if="row.item.salutationText!=='Unknown'" class="mr-1">{{row.item.salutationText}}.</span>{{row.item.firstName}} {{row.item.middleName}} {{row.item.lastName}}
                            </router-link>
                            <p class="small">
                                {{row.item.email}}
                            </p>
                        </td>
                        <td>
                            <div>{{row.item.statusText}}</div>
                            <b-form-rating v-model="row.item.rating" readonly no-border size="sm" :inline="true" variant="success"></b-form-rating>
                        </td>

                        <td>
                            <p v-if="row.item.homePhone" class="small mb-1">
                                <span class="fas fa-fw fa-phone mr-1"></span>
                                <a :href="`tel:${row.item.homePhone}`">{{row.item.homePhone}}</a>
                            </p>
                            <p v-if="row.item.mobilePhone" class="small mb-1">
                                <span class="fas fa-fw fa-mobile mr-1"></span>
                                <a :href="`tel:${row.item.mobilePhone}`">{{row.item.mobilePhone}}</a>
                            </p>
                            <p v-if="row.item.businessPhone" class="small mb-1">
                                <span class="fas fa-fw fa-fax mr-1"></span>
                                <a :href="`tel:${row.item.businessPhone}`">{{row.item.businessPhone}}</a>
                            </p>
                        </td>
                        <td>
                            <span v-if="row.item.assignedTo">
                                {{row.item.assignedTo}}
                            </span>
                        </td>
                        <td>
                            <div>
                                <button @click="addTask(row.item)" class="btn btn-sm btn-outline-primary">
                                    Add Task
                                </button>
                                <button @click="assignContact(row.item)" class="btn btn-sm btn-outline-primary">
                                    Assign
                                </button>
                            </div>
                        </td>
                    </template>

                    <template slot="list" slot-scope="row">
                        <div>
                            <div class="form-group mb-0 row no-gutters">
                                <label class="col-3 col-form-label">Name</label>
                                <div class="col align-self-center">
                                    <router-link :to="{name:'contactsView', params:{id:row.item.contactId}}">
                                        <span v-if="row.item.salutationText!=='Unknown'" class="mr-1">{{row.item.salutationText}}.</span>{{row.item.firstName}} {{row.item.middleName}} {{row.item.lastName}}
                                    </router-link>
                                </div>
                            </div>
                            <div class="form-group mb-0 row no-gutters">
                                <label class="col-3 col-form-label">Status</label>
                                <div class="col align-self-center">
                                    <div>{{row.item.statusText}}</div>
                                    <b-form-rating  v-model="row.item.rating" readonly no-border size="sm" :inline="true" variant="success"></b-form-rating>
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
                                        <p v-if="row.item.homePhone" class="small mb-1">
                                            <span class="fas fa-fw fa-phone mr-1"></span>
                                            <a :href="`tel:${row.item.homePhone}`">{{row.item.homePhone}}</a>
                                        </p>
                                        <p v-if="row.item.mobilePhone" class="small mb-1">
                                            <span class="fas fa-fw fa-mobile mr-1"></span>
                                            <a :href="`tel:${row.item.mobilePhone}`">{{row.item.mobilePhone}}</a>
                                        </p>
                                        <p v-if="row.item.businessPhone" class="small mb-1">
                                            <span class="fas fa-fw fa-fax mr-1"></span>
                                            <a :href="`tel:${row.item.businessPhone}`">{{row.item.businessPhone}}</a>
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group mb-0 row no-gutters">
                                <label class="col-3 col-form-label">Assigned To</label>
                                <div class="col align-self-center">
                                    <span v-if="row.item.assignedTo">
                                        {{row.item.assignedTo}}
                                    </span>
                                </div>
                            </div>
                            <div class="form-group mb-0 row no-gutters">
                                <!--<label class="col-3 col-form-label"></label>-->
                                <div class="offset-3 col align-self-center">
                                    <button @click="addTask(row.item)" class="btn btn-sm btn-outline-primary">
                                        Add Task
                                    </button>
                                    <button @click="assignContact(row.item)" class="btn btn-sm btn-outline-primary">
                                        Assign
                                    </button>
                                </div>
                            </div>
                        </div>
                    </template>
                </table-list>





            </div>
        </b-overlay>


        <m-pagination :filter="filter" :search="search" :showPerPage="true" class="mt-2"></m-pagination>

        <modal-add-task ref="modalAddTask"></modal-add-task>
        <modal-assign-contact ref="modalAssignContact" @saved="search"></modal-assign-contact>
    </div>
</template>
<script>
    import paginatedMixin from '../../../_Core/Mixins/paginatedMixin';

    import modalAddTask from '../../Modals/Tasks/add-task.vue';
    import modalAssignContact from '../../Modals/Contacts/assign.vue';

    //import tableList from '../../../_Core/Components/table-list.vue';
    export default {
        mixins: [paginatedMixin],

        props: {
            uid: String,
            urlAdd: String,
            urlView: String,
        },
        components: {
            modalAddTask, modalAssignContact
        },
        data() {
            return {
                baseUrl: `/api/managers/contacts`,
                filter: {
                    cacheKey: `filter-${this.uid}/contacts`,
                    //query: {
                    //    orderStatus: 0,
                    //    dateStart: moment().startOf('week').format('YYYY-MM-DD'),
                    //    dateEnd: moment().endOf('week').format('YYYY-MM-DD')
                    //}
                },
            };
        },

        computed: {

        },

        async created() {
            const vm = this;
            const cache = JSON.parse(localStorage.getItem(vm.filter.cacheKey)) || {};

            vm.initializeFilter(cache);

            await vm.search();

        },

        async mounted() {
            const vm = this;

        },

        methods: {
            addTask(item) {
                const vm = this;

                const payload = {
                    contactId: item.contactId,
                    firstName: item.firstName,
                    middleName: item.middleName,
                    lastName: item.lastName,

                    statusText: item.statusText,
                    rating: item.rating,
                };

                vm.$refs.modalAddTask.open(payload);
            },

            async assignContact(item) {
                const vm = this;
                //const item = vm.item;
                const payload = {
                    contactId: item.contactId,
                    firstName: item.firstName,
                    middleName: item.middleName,
                    lastName: item.lastName,
                    statusText: item.statusText,
                    rating: item.rating,
                    token: item.token,
                    assignedToId: item.assignedToId,
                    assignedTo: item.assignedTo
                };

                vm.$refs.modalAssignContact.open(payload);
            }
        }
    }
</script>