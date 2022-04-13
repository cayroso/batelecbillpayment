<template>
    <b-modal ref="modal"
             :no-close-on-esc="false"
             :no-close-on-backdrop="true"
             scrollable>
        <template v-slot:modal-header>
            <div class="w-100">
                <div class="d-flex flex-row  align-items-center justify-content-between">
                    <h5 class="m-0">
                        Title here
                    </h5>
                </div>
            </div>
        </template>
        <template v-slot:modal-footer>
            
            <button @click="close" class="btn btn-secondary">
                Close
            </button>
        </template>
        <div>
           content here
        </div>
    </b-modal>
</template>
<script>
    export default {
        data() {
            return {
                moment: moment,

                busy: false,
                taskId: null,
                item: {
                    contact: {}
                }
            }
        },
        methods: {

            async open(taskId) {
                const vm = this;

                vm.taskId = taskId;

                await vm.get();

                vm.$refs.modal.show();
            },

            close() {
                const vm = this;

                vm.$refs.modal.hide();
            },

            async get() {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    await vm.$util.axios.get(`/api/members/tasks/${vm.taskId}`)
                        .then(resp => {
                            vm.item = resp.data;

                            //vm.item.taskItems.forEach(ti => {
                            //    ti.dateCompleted = moment(ti.dateCompleted);
                            //});

                        })
                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false;
                }
            },

        }
    }
</script>