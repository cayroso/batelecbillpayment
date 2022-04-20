<template>
    <div v-cloak>
        <div v-cloak>
            <div class="row align-items-center">
                <div class="col">
                    <h1 class="h3 mb-sm-0">
                        <i class="fas fa-fw fa-calendar-check me-1"></i>View Reservation
                    </h1>
                </div>
                <div class="col-auto">
                    <div class="d-flex flex-row">
                        <button @click="remove" class="ms-2 btn btn-warning">
                            <span class="fas fa-fw fa-trash"></span>
                        </button>

                        <button @click="get" class="ms-2 btn btn-primary">
                            <span class="fas fa-fw fa-sync"></span>
                        </button>
                        <button @click="close" class="ms-1 btn btn-secondary">
                            <i class="fas fa-fw fa-times"></i>
                        </button>
                    </div>
                </div>
            </div>

            <div class="mt-2">
                <div class="card shadow-sm">
                    <div class="card-header bg-success text-white">
                        <!--Personal Information-->
                    </div>
                    <div class="card-body">
                        <div>
                            <div class="row">
                                <div class="col-sm">
                                    <div class="form-floating mb-3">
                                        <div class="form-control" id="branchName" placeholder="Branch">
                                            {{item.branchName}}
                                        </div>
                                        <label for="branchName">Branch</label>
                                    </div>
                                </div>
                                <div v-if="roleId==='administrator' " class="col-sm">
                                    <div class="form-floating mb-3">
                                        <div class="form-control" id="accountName" placeholder="Account">
                                            {{item.accountName}}
                                        </div>
                                        <label for="accountName">Account</label>
                                    </div>
                                </div>
                                <div class="col-sm">
                                    <div class="form-floating mb-3">
                                        <div class="form-control" id="dateReservation" placeholder="Date Reservation">
                                            {{$moment(item.dateReservation).calendar()}}
                                        </div>
                                        <label for="dateReservation">Date Reservation</label>
                                    </div>
                                </div>
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
            uid: { type: String, required: true },
            id: { type: String, required: true },
            roleId: { type: String, required: true },
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

                if (vm.busy)
                    return;

                try {
                    vm.busy = true;

                    await vm.$util.axios.get(`/api/reservation/${vm.id}`)
                        .then(resp => {

                            vm.item = resp.data;
                        });


                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false
                }
            },

            async remove() {
                const vm = this;

                const reason = prompt('Enter reason for deleting this reservation.');

                if (!reason)
                    return;

                if (vm.busy)
                    return;

                try {
                    vm.busy = true;

                    await vm.$util.axios.delete(`/api/reservation/${vm.item.reservationId}/${reason}`)
                        .then(resp => {
                            vm.$toast.warning('Delete Reservation', 'Reservation was deleted.', {
                                async onClose() {
                                    await vm.close();
                                }
                            });

                        });

                } catch (e) {
                    let errorHtml = vm.$util.getErrorMessageAsHtml(e);
                    alert(errorHtml);
                } finally {
                    vm.busy = false;

                }
            }
        }
    }
</script>