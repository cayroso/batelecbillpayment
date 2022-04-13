<style scoped>
    label {
        font-size: small;
    }
</style>
<template>
    <div v-cloak>
        <div class="d-flex flex-column flex-sm-row justify-content-sm-between">
            <h1 class="h3 mb-sm-0">
                <i class="fas fa-fw fa-user-tie mr-1"></i><span v-if="id">Edit Contact</span><span v-else>Add Contact</span>
            </h1>
            <div class="text-right">
                <button v-if="id" @click="get" class="btn btn-primary">
                    <span class="fas fa-fw fa-sync"></span>
                </button>
                <button :disabled="isDirty && !formIsValid" @click="save" class="btn btn-primary">
                    <span class="fas fa-fw fa-save"></span>
                </button>
                <button @click="close" class="btn btn-secondary">
                    <span class="fas fa-fw fa-times-circle"></span>
                </button>
            </div>
        </div>
        <div class="mt-2">
            <div class="card shadow-sm">
                <div class="card-header">
                    Information
                </div>
                <div class="card-body">

                    <div class="form-row">
                        <div class="form-group col-md-auto">
                            <label>Salutation</label>
                            <div>
                                <b-form-select v-model="item.salutation" :options="lookups.salutations"></b-form-select>
                            </div>
                        </div>

                        <div class="form-group col-md">
                            <label for="firstName">First Name</label>
                            <div>
                                <input v-model="item.firstName" type="text" id="firstName" class="form-control" v-bind:class="getValidClass('firstName')" />
                                <div v-if="validations.has('firstName')" class="invalid-feedback">
                                    {{validations.get('firstName')}}
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-md">
                            <label>Middle Name</label>
                            <div>
                                <input v-model="item.middleName" type="text" class="form-control" v-bind:class="getValidClass('middleName')" />
                                <div v-if="validations.has('middleName')" class="invalid-feedback">
                                    {{validations.get('middleName')}}
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-md">
                            <label>Last Name</label>
                            <div>
                                <input v-model="item.lastName" type="text" class="form-control" v-bind:class="getValidClass('lastName')" />
                                <div v-if="validations.has('lastName')" class="invalid-feedback">
                                    {{validations.get('lastName')}}
                                </div>
                            </div>
                        </div>
                    </div>

                    <hr class="d-md-none" />

                    <div class="form-row">
                        <div class="form-group col-md">
                            <label>Job Title</label>
                            <div>
                                <input v-model="item.title" type="text" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group col-md">
                            <label>Company</label>
                            <div>
                                <input v-model="item.company" type="text" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group col-md">
                            <label>Industry</label>
                            <div>
                                <input v-model="item.industry" type="text" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group col-md">
                            <label>Annual Revenue</label>
                            <div>
                                <input v-model="item.annualRevenue" type="number" min="0" class="form-control" />
                            </div>
                        </div>
                    </div>

                    <hr class="d-md-none" />

                    <div class="form-row">
                        <div class="form-group col-md">
                            <label>Home Phone</label>
                            <div>
                                <input v-model="item.homePhone" type="tel" class="form-control" v-bind:class="getValidClass('homePhone')" />
                                <div v-if="validations.has('homePhone')" class="invalid-feedback">
                                    {{validations.get('homePhone')}}
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-md">
                            <label>Mobile Phone</label>
                            <div>
                                <input v-model="item.mobilePhone" type="tel" class="form-control" v-bind:class="getValidClass('mobilePhone')" />
                                <div v-if="validations.has('mobilePhone')" class="invalid-feedback">
                                    {{validations.get('mobilePhone')}}
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-md">
                            <label>Business Phone</label>
                            <div>
                                <input v-model="item.businessPhone" type="tel" class="form-control" v-bind:class="getValidClass('businessPhone')" />
                                <div v-if="validations.has('businessPhone')" class="invalid-feedback">
                                    {{validations.get('businessPhone')}}
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-md">
                            <label>Fax</label>
                            <div>
                                <input v-model="item.fax" type="tel" class="form-control" v-bind:class="getValidClass('fax')" />
                                <div v-if="validations.has('fax')" class="invalid-feedback">
                                    {{validations.get('fax')}}
                                </div>
                            </div>
                        </div>
                    </div>

                    <hr class="d-md-none" />

                    <div class="form-row">
                        <div class="form-group col-md-4">
                            <label>Email</label>
                            <div>
                                <input v-model="item.email" type="email" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group col-md-5">
                            <label>Website</label>
                            <div>
                                <input v-model="item.website" type="text" class="form-control" />
                            </div>
                        </div>
                    </div>

                    <hr class="d-md-none" />

                    <div class="form-row">
                        <div class="form-group col-md-3">
                            <label>Referral Source</label>
                            <div>
                                <input v-model="item.referralSource" type="text" class="form-control" v-bind:class="{'is-invalid':validations.get('referralSource')}" />
                                <span v-if="validations.get('referralSource')" class="text-danger">
                                    {{validations.get('referralSource')}}
                                </span>
                            </div>
                        </div>

                        <div class="form-group col-md-3">
                            <label>Date Of Initial Contact</label>
                            <div>
                                <input v-model="item.dateOfInitialContact" type="date" class="form-control" v-bind:class="getValidClass('dateOfInitialContact')" />
                                <div v-if="validations.has('dateOfInitialContact')" class="invalid-feedback">
                                    {{validations.get('dateOfInitialContact')}}
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-md">
                            <label>Rating</label>
                            <div>
                                <b-form-rating inline v-model="item.rating"></b-form-rating>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="mt-2 card shadow-sm">
                <div class="card-header">
                    Location
                </div>
                <div class="card-body">
                    <div class="form-row">
                        <div class="form-group col">
                            <label>Address</label>
                            <div>
                                <textarea v-model="item.address" type="text" class="form-control" v-bind:class="{'is-invalid':validations.get('address')}"></textarea>
                                <span v-if="validations.get('address')" class="text-danger">
                                    {{validations.get('address')}}
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="form-row d-none">
                        <div class="form-group col">
                            <label>Map</label>
                            <div style="height:400px;">
                                <!--<gmap-location map-name="add-contact"
                                               :fixed="false"
                                               :show-location="true"
                                               :cx="item.geoX" :cy="item.geoY"
                                               :draggable="true"
                                               @onMapReady="onMapReady"
                                               @onAddress="onAddress">
                                </gmap-location>-->
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    import pageMixin from '../../../_Core/Mixins/pageMixin';

    export default {
        mixins: [pageMixin],

        props: {
            uid: String,
            id: String,
            urlView: String
        },

        data() {
            return {
                isDirty: false,
                validations: new Map(),
                lookups: {
                    salutations: [
                        { value: 1, text: 'Mr' },
                        { value: 2, text: 'Mrs' },
                        { value: 3, text: 'Ms' },
                    ]
                },
                item: {

                    salutation: null,
                    firstName: null,
                    middleName: null,
                    lastName: null,

                    title: null,
                    company: null,
                    industry: null,
                    annualRevenue: null,

                    homePhone: null,
                    mobilePhone: null,
                    businessPhone: null,
                    fax: null,
                    email: null,
                    website: null,

                    referralSource: null,
                    dateOfInitialContact: moment().format('YYYY-MM-DD'),
                    rating: null,

                    address: null,
                    geoX: 0,
                    geoY: 0,
                }
            };
        },

        computed: {
            formIsValid() {
                const vm = this;

                //if (!vm.isDirty)
                //    return true;

                const item = vm.item;

                const validations = new Map();

                //if (!item.salutation) {
                //    validations.set('salutation', 'Salutation is required.');
                //}

                const missingName = !item.firstName && !item.middleName && !item.lastName;

                if (missingName) {
                    validations.set('firstName', 'First name is required.');
                    validations.set('middleName', 'Middle name is required.');
                    validations.set('lastName', 'Last name is required.');
                }

                if (Number.isNaN(item.annualRevenue)) {
                    validations.set('annualRevenue', 'Annual Revenue is required.');
                }

                const missingContactNumber = !item.homePhone && !item.mobilePhone && !item.businessPhone && !item.fax;

                if (missingContactNumber) {
                    validations.set('homePhone', 'Home Phone is required.');
                    validations.set('mobilePhone', 'Mobile Phone is required.');
                    validations.set('businessPhone', 'Business Phone is required.');
                    validations.set('fax', 'Fax is required.');
                }

                //if (!(item.rating >= 1 && item.rating <= 5)) {
                //    validations.set('rating', 'Rating can only have values from 1 to 5.');
                //}

                if (!item.dateOfInitialContact) {
                    validations.set('dateOfInitialContact', 'Date of initial contact is required.');
                }

                vm.isDirty = true;
                vm.validations = validations;

                return validations.size == 0;
            },
        },

        async created() {
            const vm = this;

        },

        async mounted() {
            const vm = this;
            const foo = vm.formIsValid;
        },

        methods: {
            getValidClass(field) {
                const vm = this;

                if (!vm.isDirty)
                    return '';

                if (vm.validations.has(field))
                    return 'is-invalid';
                return 'is-valid';
            },

            async get() {
                const vm = this;

                try {
                    await vm.$util.axios.get(`/api/owner/customers/${vm.id}`)
                        .then(resp => {
                            vm.item = resp.data;
                            //vm.$emit('event:center-position', { x: vm.item.geoX, y: vm.item.geoY });
                        });
                } catch (e) {
                    vm.$util.handleError(e);
                }
            },
            async onMapReady() {
                const vm = this;

                //if (vm.id) {
                //    await vm.get();
                //}
            },

            onAddress(address, location) {
                const vm = this;

                vm.item.address = address.formatted_address;
                vm.item.geoX = location.lat;
                vm.item.geoY = location.lng;
            },



            async save() {
                const vm = this;

                if (vm.busy)
                    return;

                if (!vm.formIsValid)
                    return;

                try {
                    vm.busy = true;

                    const payload = vm.$util.clone(vm.item);
                    payload.salutation = payload.salutation === null ? 0 : payload.salutation;
                    payload.dateOfInitialContact = moment(payload.dateOfInitialContact).utc();
                    payload.annualRevenue = payload.annualRevenue === null ? 0 : payload.annualRevenue;
                    payload.rating = payload.rating === null ? 0 : payload.rating;
                    
                    await vm.$util.axios.post(`/api/members/contacts/`, payload)
                        .then(resp => {
                            vm.$bvToast.toast('Contact created.', { title: 'Add Contact', variant: 'success', toaster: 'b-toaster-bottom-right' });

                            setTimeout(() => {
                                //const url = `${vm.urlView}${resp.data}`;
                                //vm.$util.href(url);
                                vm.$router.push({ name: 'contactsView', params: { id: resp.data } });
                            }, 500);
                        });

                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false
                }
            }
        }
    }
</script>