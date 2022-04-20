<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col">
                <h1 class="h3 mb-sm-0">
                    <i class="fas fa-fw fa-bell me-1"></i>View Notification
                </h1>
            </div>
            <div class="col-auto">
                <div class="d-flex flex-row">
                    <!--<template v-if="roleId==='administrator'">
                        <a :href="`${urlEdit}`" class="btn btn-warning">Edit</a>
                    </template>-->
                    <button @click="deleteNotification" class="ms-2 btn btn-warning">
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
                <div class="card-header text-white" v-bind:class="`bg-${item.iconClass}`">
                    <!--Personal Information-->
                </div>
                <div class="card-body">
                    <div>
                        <div class="row">
                            <div class="col-sm-8">
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="subject" placeholder="Subject">
                                        <b>{{item.subject}}</b>
                                    </div>
                                    <label for="subject">Subject</label>
                                </div>
                            </div>
                            <div class="col-sm-4">
                                <div class="form-floating mb-3">
                                    <div class="form-control" id="dateCreated" placeholder="Post Date">
                                        {{$moment(item.dateCreated).calendar()}}
                                    </div>
                                    <label for="dateCreated">Post Date</label>
                                </div>
                            </div>
                        </div>

                        <div class="form-floating mb-3">
                            <div class="form-control h-25" id="content" placeholder="Content">
                                <div v-html="item.content"></div>
                            </div>
                            <label for="content">Content</label>
                        </div>

                        <div class="form-floating mb-3">
                            <div class="form-control h-25" id="content" placeholder="Link">
                                <a v-if="item.notificationEntityClassText === 'Announcement'" :href="`${urlViewAnnouncement}/${item.referenceId}`">
                                    View Announcement
                                </a>
                                <a v-else-if="item.notificationEntityClassText === 'Billing'" :href="`${urlViewBilling}/${item.referenceId}`">
                                    View Billing
                                </a>
                                <a v-else-if="item.notificationEntityClassText === 'Reservation'" :href="`${urlViewReservation}/${item.referenceId}`">
                                    View Reservation
                                </a>
                                <div v-else>
                                    No link provided
                                </div>
                            </div>
                            <label for="content">Link</label>
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
            urlViewAnnouncement: { type: String, required: true },
            urlViewBilling: { type: String, required: true },
            urlViewReservation: { type: String, required: true },
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

                    await vm.$util.axios.get(`/api/notification/${vm.id}`)
                        .then(resp => {

                            vm.item = resp.data;
                        });


                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false
                }
            },
            async deleteNotification() {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    vm.busy = true;

                    await vm.$util.axios.delete(`/api/notification/${vm.item.notificationId}`)
                        .then(resp => {
                            vm.$toast.info('Delete NOtification', 'Notification was deleted.', {
                                async onClose() {
                                    vm.close();
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