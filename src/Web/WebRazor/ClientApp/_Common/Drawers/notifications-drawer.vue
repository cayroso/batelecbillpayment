<template>
    <b-sidebar id="notificationsDrawer" title="Notifications" backdrop right shadow>
        <div v-if="notifications" v-cloak>
            <a v-for="item in notifications" v-on:click="viewNotification(item)" style="cursor:pointer" class="dropdown-item d-flex align-items-center">
                <div class="mr-3">
                    <i v-bind:class="item.iconClass"></i>
                </div>
                <div>
                    <div class="small text-gray-500">{{item.dateSent|moment('calendar')}}</div>
                    <span class="font-weight-bold">{{item.subject}}</span>
                </div>
            </a>
        </div>
        <a class="dropdown-item text-center small" :href="page">Show All Motifications</a>

        <b-modal ref="modal"
                 :no-close-on-esc="false"
                 :no-close-on-backdrop="true"
                 scrollable>
            <template v-slot:modal-header>
                <div class="w-100">
                    <div class="d-flex flex-row  align-items-center justify-content-between">
                        <h5 class="m-0">
                            <i v-bind:class="item.iconClass"></i> {{item.subject}}
                        </h5>
                        <div class="small">
                            {{item.dateSent|moment('calendar')}}
                        </div>
                        {{item}}
                    </div>
                </div>
            </template>
            <template v-slot:modal-footer>
                <button v-if="canView" @click="view" class="btn btn-primary mr-auto">
                    <i class="fas fa-fw fa-search mr-1"></i>View
                </button>
                <button @click="markAsRead" class="btn btn-info">
                    <i class="fas fa-fw fa-check-circle mr-1"></i>Mark As Read
                </button>
                <button @click="close" class="btn btn-secondary">
                    <i class="fas fa-fw fa-times-circle mr-1"></i>Close
                </button>
            </template>
            <div>
                <div v-if="item.content" v-html="item.content"></div>
            </div>
        </b-modal>
    </b-sidebar>
</template>
<script>
    export default {
        props: {
            notifications: Array,
            //urlViewJob: { type: String, required: true },
            page: { type: String, required: true },
            roleId: { type: String, required: true }
        },
        data() {
            return {
                item: {}
            }
        },
        async mounted() {
            const vm = this;

            //await vm.getUnreadNotifications();
        },
        computed: {
            canView() {
                const vm = this;
                const item = vm.item;

                if (item.notificationEntityClass === 2)
                    return true;

                return false;

            }
        },
        methods: {
            view() {
                const vm = this;
                
                switch (vm.item.notificationEntityClass) {
                    case 2: // job
                        vm.$util.href(`/${vm.roleId}/jobs/view/${vm.item.referenceId}`);
                        break;
                }
                //vm.$util.href(`${vm.urlViewJob}${vm.item.referenceId}`);
                //vm.$util.href(`${vm.urlViewJob}${vm.item.referenceId}`);
                //vm.$router.push({ name: vm.urlView, params: { id: vm.item.referenceId } });

                vm.close();
            },

            async getUnreadNotifications() {

                const vm = this;

                try {
                    await vm.$util.axios.get(`/api/notifications/unread/?criteria=&pageIndex=1&pageSize=20`)
                        .then(resp => {
                            vm.notifications = resp.data;
                        });
                } catch (e) {
                    vm.$util.handleError(e);
                }
            },

            viewNotification(item) {
                //this.$bus.$emit('event:open-notification', id);
                const vm = this;

                vm.item = item;
                vm.$refs.modal.show();
            },
            close() {
                const vm = this;

                vm.item = {};
                vm.$refs.modal.hide();
            },
            async markAsRead() {
                const vm = this;
                const item = vm.item;

                try {
                    await vm.$util.axios.post(`/api/notifications/${item.notificationId}/markAsRead`);

                    vm.close();
                    //await vm.getUnreadNotifications();
                    vm.$bus.$emit('event:notification-received');

                } catch (e) {
                    vm.$util.handleError(e);
                }
            },
            showHiddenElements() {
                let vm = this;

                let elems = vm.$el.getElementsByClassName('initialHidden');

                for (let i = 0; i < elems.length; i++) {
                    let el = elems[i];

                    el.classList.remove('invisible');
                }

            }
        }
    };
</script>