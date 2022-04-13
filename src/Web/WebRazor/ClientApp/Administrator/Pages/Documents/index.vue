<template>
    <div v-cloak>

        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fas fa-fw fa-archive mr-1"></i>Documents
                </h1>
            </div>
            <div class="col-sm-auto">
                <div class="d-flex flex-row">
                    <div class="mr-1">
                        <router-link to="/documents/add" class="btn btn-primary">
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
                <table-list :header="{key: 'documentId', columns:[]}" :items="filter.items" :getRowNumber="getRowNumber" :setSelected="setSelected" :isSelected="isSelected" table-css="">
                    <template #header>
                        <th class="text-center">#</th>
                        <th>Name</th>
                        <th>Filename</th>
                        <th>Size</th>
                        <th>Type</th>
                        <th>Uploaded By</th>
                        <th>Date</th>
                        <th></th>
                    </template>
                    <template slot="table" slot-scope="row">
                        <td v-text="getRowNumber(row.index)" class="text-center"></td>
                        <td class="text-break">
                            {{row.item.name}}
                            <!--<router-link :to="{name:'documentsView', params:{id:row.item.documentId}}">

                            </router-link>-->
                            <div class="small">
                                {{row.item.description}}
                            </div>
                        </td>
                        <td v-text="row.item.fileName" class="text-break"></td>
                        <td>
                            {{formatBytes(row.item.length)}}
                        </td>
                        <td v-text="row.item.contentType" class="text-break"></td>
                        <td v-text="row.item.uploadedBy" class="text-break"></td>
                        <td>
                            <div class="small">
                                Created: {{row.item.dateCreated|moment('calendar')}}
                            </div>
                            <div v-if="row.item.dateCreated!==row.item.dateUpdated" class="small">
                                Updated: {{row.item.dateUpdated|moment('calendar')}}
                            </div>
                        </td>
                        <td>
                            <a :href="row.item.url" class="btn btn-sm btn-primary">
                                <i class="fas fa-fw fa-file-download"></i>
                            </a>
                        </td>
                    </template>

                    <template slot="list" slot-scope="row">
                        <div>
                            <div class="form-group mb-0 row no-gutters">
                                <label class="col-4 col-form-label">Name</label>
                                <div class="col align-self-center">
                                    {{row.item.name}}
                                    <!--<router-link :to="{name:'documentsView', params:{id:row.item.documentId}}">

                                    </router-link>-->
                                    <div class="small mb-2">
                                        {{row.item.description}}
                                    </div>
                                </div>
                            </div>
                            <div class="form-group mb-0 row no-gutters">
                                <label class="col-4 col-form-label">Filename</label>
                                <div class="col align-self-center">
                                    <div class=" text-break">
                                        <a :href="row.item.url" class="btn btn-sm btn-outline-primary mr-1">
                                            <i class="fas fa-fw fa-file-download"></i>
                                        </a>
                                        {{row.item.fileName}}
                                        <div>
                                            {{formatBytes(row.item.length)}}
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group mb-0 row no-gutters">
                                <label class="col-4 col-form-label">Type</label>
                                <div class="col align-self-center">
                                    {{row.item.contentType}}
                                </div>
                            </div>
                            <div class="form-group mb-0 row no-gutters">
                                <label class="col-4 col-form-label">Uploaded By</label>
                                <div class="col align-self-center">
                                    {{row.item.uploadedBy}}
                                </div>
                            </div>
                        </div>
                    </template>
                </table-list>





            </div>
        </b-overlay>


        <m-pagination :filter="filter" :search="search" :showPerPage="true" class="mt-2"></m-pagination>
    </div>
</template>
<script>
    import paginatedMixin from '../../../_Core/Mixins/paginatedMixin';
    //import tableList from '../../../_Core/Components/table-list.vue';
    export default {
        mixins: [paginatedMixin],

        props: {
            uid: String,
            urlAdd: String,
            urlView: String,
        },
        components: {
            //tableList
        },
        data() {
            return {
                baseUrl: `/api/members/documents`,
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
            formatBytes(bytes, decimals = 2) {
                if (bytes === 0) return '0 Bytes';

                const k = 1024;
                const dm = decimals < 0 ? 0 : decimals;
                const sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

                const i = Math.floor(Math.log(bytes) / Math.log(k));

                return parseFloat((bytes / Math.pow(k, i)).toFixed(dm)) + ' ' + sizes[i];
            }
        }
    }
</script>