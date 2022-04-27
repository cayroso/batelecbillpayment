<style scoped>
</style>
<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fas fa-fw fa-money-bill me-1"></i>Add Billings
                </h1>
            </div>
            <div class="col-sm-auto">
                <div class="d-flex flex-row">
                    <button v-bind:disabled="isDirty && !formIsValid" @click="save" class="btn btn-primary">
                        <span class="fas fa-fw fa-save"></span>
                    </button>
                    <button @click="close" class="ms-1 btn btn-secondary">
                        <i class="fas fa-fw fa-times"></i>
                    </button>
                </div>
            </div>
        </div>

        <div class="mt-2">
            <div class="card shadow-sm">
                <div class="card-header bg-info text-white">
                    <!--Personal Information-->
                </div>
                <div class="card-body">
                    <form class="needs-validation" novalidate>
                        <div class="row">
                            <div class="col-sm-6">
                                <div class="form-floating mb-3">
                                    <select v-model="item.accountId" class="form-select" id="accountId">
                                        <option value="">- Select Account -</option>
                                        <option v-for="opt in lookups.accounts" :value="opt.accountId">{{opt.name}}</option>
                                    </select>
                                    <label for="accountId">Account</label>
                                    <div v-if="validations.has('accountId')" class="d-block invalid-feedback">
                                        {{validations.get('accountId')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <input v-model="item.amount" type="number" min="0" max="99999" class="form-control" id="amount" placeholder="Amount">
                                    <label for="amount">Amount</label>
                                    <div v-if="validations.has('amount')" class="d-block invalid-feedback">
                                        {{validations.get('amount')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <input v-model="item.number" type="text" class="form-control" id="number" placeholder="Billing Number">
                                    <label for="number">Billing Number</label>
                                    <div v-if="validations.has('number')" class="d-block invalid-feedback">
                                        {{validations.get('number')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <select v-model="item.year" class="form-select" id="year">
                                        <option value="">- Select Year -</option>
                                        <option value="2021">2021</option>
                                        <option value="2022">2022</option>
                                        <option value="2023">2023</option>
                                        <option value="2024">2024</option>
                                    </select>
                                    <label for="year">Year</label>
                                    <div v-if="validations.has('year')" class="d-block invalid-feedback">
                                        {{validations.get('year')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <select v-model="item.month" class="form-select" id="month">
                                        <option value="">- Select Month -</option>
                                        <option value="January">January</option>
                                        <option value="February">February</option>
                                        <option value="March">March</option>
                                        <option value="April">April</option>
                                        <option value="May">May</option>
                                        <option value="June">June</option>
                                        <option value="July">July</option>
                                        <option value="August">August</option>
                                        <option value="September">September</option>
                                        <option value="October">October</option>
                                        <option value="November">November</option>
                                        <option value="December">December</option>
                                    </select>
                                    <label for="month">Month</label>
                                    <div v-if="validations.has('month')" class="d-block invalid-feedback">
                                        {{validations.get('month')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <input v-model="item.readingDate" type="date" class="form-control" id="readingDate" placeholder="Reading Date">
                                    <label for="readingDate">Reading Date</label>
                                    <div v-if="validations.has('readingDate')" class="d-block invalid-feedback">
                                        {{validations.get('readingDate')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <input v-model="item.dateDue" type="date" class="form-control" id="dateDue" placeholder="Due Date">
                                    <label for="dateDue">Due Date</label>
                                    <div v-if="validations.has('dateDue')" class="d-block invalid-feedback">
                                        {{validations.get('dateDue')}}
                                    </div>
                                </div>
                            </div>


                            <div class="col-sm-6">
                                <div class="form-floating mb-3">
                                    <input v-model="item.dateStart" type="date" class="form-control" id="dateStart" placeholder="Billing Start">
                                    <label for="dateStart">Billing Start</label>
                                    <div v-if="validations.has('dateStart')" class="d-block invalid-feedback">
                                        {{validations.get('dateStart')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <input v-model="item.dateEnd" type="date" class="form-control" id="dateEnd" placeholder="Billing End">
                                    <label for="dateEnd">Billing End</label>
                                    <div v-if="validations.has('dateEnd')" class="d-block invalid-feedback">
                                        {{validations.get('dateEnd')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <input v-model="item.presentReading" type="number" min="0" max="99999" class="form-control" id="presentReading" placeholder="Present Reading">
                                    <label for="presentReading">Present Reading</label>
                                    <div v-if="validations.has('presentReading')" class="d-block invalid-feedback">
                                        {{validations.get('presentReading')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <input v-model="item.previousReading" type="number" min="0" max="99999" class="form-control" id="previousReading" placeholder="Previous Reading">
                                    <label for="previousReading">Previous Reading</label>
                                    <div v-if="validations.has('previousReading')" class="d-block invalid-feedback">
                                        {{validations.get('previousReading')}}
                                    </div>
                                </div>

                                <div class="form-floating mb-3">
                                    <input v-model="item.multiplier" type="number" min="0" max="99999" class="form-control" id="multiplier" placeholder="Multiplier">
                                    <label for="multiplier">Multiplier</label>
                                    <div v-if="validations.has('multiplier')" class="d-block invalid-feedback">
                                        {{validations.get('multiplier')}}
                                    </div>
                                </div>

                                <div class="form-floating mb-3">
                                    <input v-model="item.killoWattHourUsed" type="number" min="0" max="99999" class="form-control" id="killoWattHourUsed" placeholder="kWH Used">
                                    <label for="killoWattHourUsed">kWH Used</label>
                                    <div v-if="validations.has('killoWattHourUsed')" class="d-block invalid-feedback">
                                        {{validations.get('killoWattHourUsed')}}
                                    </div>
                                </div>

                                <div class="form-floating mb-3">
                                    <input v-model="item.reader" type="text" class="form-control" id="reader" placeholder="Meter Read By">
                                    <label for="reader">Meter Read By</label>
                                    <div v-if="validations.has('reader')" class="d-block invalid-feedback">
                                        {{validations.get('reader')}}
                                    </div>
                                </div>
                            </div>

                        </div>
                    </form>



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
            uid: { type: String, required: true },
            urlView: { type: String, required: true },
        },

        data() {
            return {
                isDirty: false,
                validations: new Map(),
                lookups: {
                    accounts: [],
                    salutations: [
                        { value: 1, text: 'Mr' },
                        { value: 2, text: 'Mrs' },
                        { value: 3, text: 'Ms' },
                    ]
                },
                item: {

                    accountId: "",
                    amount: 1,
                    number: "",
                    month: "",
                    year: "",

                    readingDate: this.$moment().format('YYYY-MM-DD'),
                    dateStart: this.$moment().format('YYYY-MM-DD'),
                    dateEnd: this.$moment().format('YYYY-MM-DD'),

                    presentReading: null,
                    previousReading: null,
                    multiplier: null,
                    killoWattHourUsed: null,
                    dateDue: null,
                    reader: null,

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

                if (!item.accountId) {
                    validations.set('accountId', 'Account is required.');
                }

                if (!item.amount) {
                    validations.set('amount', 'Billing Amount is required.');
                }
                if (item.amount < 0) {
                    validations.set('amount', 'Billing Amount is incorrect.');
                }

                if (!item.number) {
                    validations.set('number', 'Billing Number is required.');
                }

                if (!item.month) {
                    validations.set('month', 'Month is required.');
                }
                if (!item.year) {
                    validations.set('year', 'Year is required.');
                }

                if (!item.readingDate) {
                    validations.set('readingDate', 'Date Reading is required.');
                }

                if (!item.dateDue) {
                    validations.set('dateDue', 'Date Due is required.');
                }

                if (!item.presentReading) {
                    validations.set('presentReading', 'Present reading is invalid.');
                }

                if (!item.previousReading) {
                    validations.set('previousReading', 'Previous reading is invalid.');
                }

                if (!item.multiplier) {
                    validations.set('multiplier', 'Multiplier is invalid.');
                }

                if (!item.killoWattHourUsed) {
                    validations.set('killoWattHourUsed', 'kWH Used is invalid.');
                }

                if (!item.reader) {
                    validations.set('reader', 'Reader is required.');
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

            await vm.getLookups();
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

            async getLookups() {
                const vm = this;

                try {
                    await vm.$util.axios.get(`/api/account/lookup`)
                        .then(resp => {
                            vm.lookups.accounts = resp.data;
                            //vm.$emit('event:center-position', { x: vm.item.geoX, y: vm.item.geoY });
                        });
                } catch (e) {
                    vm.$util.handleError(e);
                }
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
                    //payload.salutation = payload.salutation === null ? 0 : payload.salutation;
                    //payload.dateOfInitialContact = moment(payload.dateOfInitialContact).utc();
                    //payload.annualRevenue = payload.annualRevenue === null ? 0 : payload.annualRevenue;
                    //payload.rating = payload.rating === null ? 0 : payload.rating;

                    await vm.$util.axios.post(`/api/billing/add-billing/`, payload)
                        .then(resp => {
                            //alert('New Billing Created.');
                            //vm.$bvToast.toast('Contact created.', { title: 'Add Contact', variant: 'success', toaster: 'b-toaster-bottom-right' });

                            const url = `${vm.urlView}/${resp.data}`;

                            vm.$toast.success('Add New Billing',
                                `Billing was created. 
                                <br/>Click <a href="${url}">here</a> to view the record.`,
                                {
                                    async onClose() {
                                        vm.close();
                                    }
                                })
                            //setTimeout(() => {
                            //    const url = `${vm.urlView}/${resp.data}`;
                            //    vm.$util.href(url);
                            //    //vm.$router.push({ name: 'contactsView', params: { id: resp.data } });
                            //}, 500);
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