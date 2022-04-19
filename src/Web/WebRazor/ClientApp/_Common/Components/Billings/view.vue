<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fas fa-fw fa-money-bill me-1"></i>View Billings <sub>{{roleId}}</sub>
                </h1>
            </div>
            <div class="col-sm-auto">
                <div class="d-flex flex-row">
                    <template v-if="roleId===`consumer` && urlCheckout && item.status===1">
                        <a :href="`/consumer/billings/checkout/${item.billingId}`" class="btn btn-primary">
                            Checkout
                        </a>
                    </template>
                    <template v-if="roleId===`administrator`">
                        <a :href="urlEdit" class="ms-1 btn btn-warning">
                            Edit
                        </a>
                        <div class="dropdown ms-1">
                            <button class="btn btn-secondary dropdown-toggle" type="button" id="dropdownMenu2" data-bs-toggle="dropdown" aria-expanded="false">
                                Status: {{item.statusText}}
                            </button>
                            <ul class="dropdown-menu" aria-labelledby="dropdownMenu2">
                                <li>
                                    <button class="dropdown-item" type="button" @click="setStatus(0)">
                                        Pending
                                    </button>
                                </li>
                                <li>
                                    <button class="dropdown-item" type="button" @click="setStatus(1)">
                                        Unpaid
                                    </button>
                                </li>
                                <li>
                                    <button class="dropdown-item" type="button" @click="setStatus(2)">
                                        Review
                                    </button>
                                </li>
                                <li>
                                    <button class="dropdown-item" type="button" @click="setStatus(3)">
                                        Paid
                                    </button>
                                </li>
                            </ul>
                        </div>
                    </template>

                    <button @click="get" class="ms-2 btn btn-primary">
                        <i class="fas fa-fw fa-sync"></i>
                    </button>
                    <button @click="close" class="ms-1 btn btn-secondary">
                        <i class="fas fa-fw fa-times"></i>
                    </button>
                </div>
            </div>
        </div>

        <!--<div class="d-flex flex-row justify-content-between align-items-sm-center">
            <h1 class="h3 mb-sm-0">
                <i class="fas fa-fw fa-money-bill me-1"></i>
                View Billing
            </h1>

            <div class="text-right">
                <button @click="get" class="btn btn-primary">
                    <i class="fas fa-fw fa-sync"></i>
                </button>
                <button @click="close" class="ms-1 btn btn-secondary">
                    <i class="fas fa-fw fa-times"></i>
                </button>
            </div>
        </div>-->
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
                                    <div class="form-control" id="statusText">
                                        {{item.statusText}}
                                    </div>
                                    <label for="statusText">Status</label>
                                </div>
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="reader">
                                        {{item.reader}}
                                    </div>
                                    <label for="reader">Meter Read By</label>
                                </div>
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="accountName">
                                        {{item.accountName}}
                                    </div>
                                    <label for="accountId">Account</label>
                                </div>
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="amount">
                                        {{item.amount}}
                                    </div>
                                    <label for="amount">Amount</label>
                                </div>
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="number">
                                        {{item.number}}
                                    </div>
                                    <label for="number">Billing Number</label>
                                </div>
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="year">
                                        {{item.year}}
                                    </div>
                                    <label for="year">Year</label>
                                </div>
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="month">
                                        {{item.month}}
                                    </div>
                                    <label for="month">Month</label>

                                </div>

                            </div>


                            <div class="col-sm-6">
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="readingDate">
                                        {{$moment(item.readingDate).calendar()}}
                                    </div>
                                    <label for="readingDate">Reading Date</label>
                                </div>
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="dateDue">
                                        {{$moment(item.dateDue).calendar()}}
                                    </div>
                                    <label for="dateDue">Due Date</label>

                                </div>
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="dateStart">
                                        {{$moment(item.dateStart).calendar()}}
                                    </div>
                                    <label for="dateStart">Billing Start</label>
                                </div>
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="dateEnd">
                                        {{$moment(item.dateEnd).calendar()}}
                                    </div>
                                    <label for="dateEnd">Billing End</label>

                                </div>
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="presentReading">
                                        {{item.presentReading}}
                                    </div>
                                    <label for="presentReading">Present Reading</label>

                                </div>
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="previousReading">
                                        {{item.previousReading}}
                                    </div>
                                    <label for="previousReading">Previous Reading</label>

                                </div>

                                <div class="form-floating mb-3">
                                    <div class="form-control" id="multiplier">
                                        {{item.multiplier}}
                                    </div>
                                    <label for="multiplier">Multiplier</label>

                                </div>

                                <div class="form-floating mb-3">
                                    <div class="form-control" id="killoWattHourUsed">
                                        {{item.killoWattHourUsed}}
                                    </div>
                                    <label for="killoWattHourUsed">kWH Used</label>

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
            uid: {
                type: String, required: true
            },
            id: {
                type: String, required: true
            },
            roleId: {
                type: String, required: true
            },
            urlEdit: {
                type: String, required: true
            },
            urlCheckout: String,
        },

        data() {
            return {
                item: {}
            };
        },

        computed: {

        },

        async created() {
            const vm = this;

        },

        async mounted() {
            const vm = this;

            await vm.get();
        },

        methods: {
            async get() {
                const vm = this;
                //debugger
                if (vm.busy)
                    return;

                try {
                    vm.busy = true;

                    await vm.$util.axios.get(`/api/billing/${vm.id}`)
                        .then(resp => {
                            vm.item = resp.data;
                        });
                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false;
                }
            },

            async setStatus(status) {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    vm.busy = true;

                    await vm.$util.axios.put(`/api/billing/${vm.id}/status/${status}`)
                        .then(resp => {
                            //vm.item = resp.data;
                            vm.$toast.info('Change Status', 'Billing status was updated.', {
                                async onClose() {
                                    await vm.get();
                                }
                            })
                        });
                } catch (e) {
                    vm.$util.handleError(e);

                } finally {
                    vm.busy = false;

                }
            }
        }
    }
</script>