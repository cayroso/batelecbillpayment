<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fa-solid fa-fw fa-calendar-check me-1"></i>Add Reservations
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
                                    <input v-model="item.dateReservation" type="date" class="form-control" id="dateReservation" placeholder="Date">
                                    <label for="dateReservation">Date</label>
                                    <div v-if="validations.has('dateReservation')" class="d-block invalid-feedback">
                                        {{validations.get('dateReservation')}}
                                    </div>
                                </div>

                                <div class="form-floating mb-3">
                                    <select v-model="item.branchId" class="form-control" id="branchId" placeholder="Branch">
                                        <option value="">- Select -</option>
                                        <option v-for="opt in lookups.branches" :value="opt.branchId">{{opt.name}}</option>
                                    </select>
                                    <label for="branchId">Branch</label>
                                    <div v-if="validations.has('branchId')" class="d-block invalid-feedback">
                                        {{validations.get('branchId')}}
                                    </div>
                                </div>

                                <div class="form-floating mb-3">
                                    <select v-model="timeSlot" class="form-control" id="timeSlot" placeholder="Time Slot">
                                        <option value="">- Select -</option>
                                        <option v-for="opt in lookups.timeSlots" :value="opt">{{opt}}</option>
                                    </select>
                                    <label for="timeSlot">Time SLot</label>
                                    <div v-if="validations.has('timeSlot')" class="d-block invalid-feedback">
                                        {{validations.get('timeSlot')}}
                                    </div>
                                </div>

                            </div>
                            <div v-if="errorHtml" class="col-sm-6">
                                <div class="alert alert-danger">
                                    <div v-html="errorHtml"></div>
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
                    branches: [],
                    timeSlots: [],
                },
                errorHtml: null,

                timeSlot: '',
                item: {
                    branchId: null,
                    dateReservation: this.$moment().format('YYYY-MM-DD'),
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

                if (!item.branchId) {
                    validations.set('branchId', 'Branch is required.');
                }

                if (!item.dateReservation) {
                    validations.set('dateReservation', 'Date is required.');
                }

                if (!vm.timeSlot) {
                    validations.set('timeSlot', 'Time SLot is required.');
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

                await vm.getBranches();

                vm.lookups.timeSlots = vm.getTimeStops('9:00', '16:00');
            },

            async getBranches() {
                const vm = this;

                try {
                    await vm.$util.axios.get(`/api/branch/lookup`)
                        .then(resp => {
                            vm.lookups.branches = resp.data;
                        });
                } catch (e) {
                    vm.$util.handleError(e);
                }
            },

            getTimeStops(start, end) {
                const vm = this;

                var startTime = vm.$moment(start, 'HH:mm');
                var endTime = vm.$moment(end, 'HH:mm');

                if (endTime.isBefore(startTime)) {
                    endTime.add(1, 'day');
                }

                var timeStops = [];

                while (startTime <= endTime) {
                    timeStops.push(new vm.$moment(startTime).format('hh:mm a'));
                    startTime.add(15, 'minutes');
                }
                return timeStops;
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

                    //var date = vm.$moment(payload.dateReservation, "MM-DD-YYYY");
                    var dateTime = vm.$moment.utc(`${payload.dateReservation} ${vm.timeSlot}`);                    
                    payload.dateReservation = dateTime;
                    
                    await vm.$util.axios.post(`/api/reservation`, payload)
                        .then(resp => {
                            vm.$toast.success('Add Reservation', 'Reservation was created.', {
                                async onClose () {
                                    await vm.close();
                                }
                            });

                            //setTimeout(() => {
                            //    //const url = `${vm.urlView}/${resp.data}`;
                            //    vm.close();
                            //    vm.$util.href(url);
                            //}, 3000);
                        });

                } catch (e) {
                    vm.errorHtml = vm.$util.getErrorMessageAsHtml(e);
                } finally {
                    vm.busy = false
                }
            }
        }
    }
</script>