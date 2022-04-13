<style scoped>
    label {
        font-size: small;
        font-weight: bold;
    }
</style>
<template>
    <b-modal ref="modal"
             :no-close-on-esc="false"
             :no-close-on-backdrop="true"
             scrollable>
        <template v-slot:modal-header>
            <div class="w-100">
                <div class="d-flex flex-row  align-items-center justify-content-between">
                    <h5 class="m-0">
                        View Task
                    </h5>
                    <h6 class="m-0">
                        {{item.taskTypeText}} - {{item.taskStatusText}}
                    </h6>
                </div>
            </div>
        </template>
        <template v-slot:modal-footer>
            <button v-if="!item.isDeleted" @click="deleteTask" class="btn btn-danger">
                Delete
            </button>

            <button @click="close" class="btn btn-secondary">
                Close
            </button>
        </template>
        <div>
            <div class="form-row">
                <div class="form-group col-md">
                    <label for="firstName">Contact</label>
                    <div>
                        <a :href="`/member/contacts/view/${item.contact.contactId}`">
                            {{item.contact.firstName}} {{item.contact.middleName}} {{item.contact.lastName}}
                        </a>
                        <div>
                            <b-form-rating inline no-border class="shadow-none" size="sm" v-model="item.contact.rating" id="rating" readonly></b-form-rating>
                        </div>
                    </div>
                </div>
                <div class="form-group col-md">
                    <label for="firstName">Status</label>
                    <div>{{item.contact.statusText}}</div>
                </div>
                <!--<div class="form-group col-md-3">
                    <label for="firstName">Rating</label>
                    <div>
                        <b-form-rating inline no-border size="sm" v-model="item.contact.rating" id="rating" readonly></b-form-rating>
                    </div>
                </div>-->
            </div>

            <div class="form-row">
                <div class="form-group col-md order-1 order-md-0">
                    <label for="title">Title</label>
                    <div>
                        {{item.title}}
                    </div>
                </div>
                <div class="form-group col order-0 order-md-1">

                    <label for="dateActualCompleted">Dates</label>
                    <div>
                        <div class="small">Assigned: {{item.dateAssigned | moment('calendar')}}</div>
                        <div class="small">Deadline: {{item.dateCompleted | moment('calendar')}}</div>
                    </div>
                </div>
            </div>

            <div v-if="item.description" class="form-group">
                <label for="description">Description</label>
                <div class="bg-secondary p-1 rounded">
                    {{item.description}}
                </div>
            </div>



            <div class="form-group">
                <label for="description">Task Items</label>
                <div class="table-responsive mb-0">
                    <table class="table table-sm border">
                        <thead>
                            <tr>
                                <td></td>
                                <td>Title</td>
                                <td>Date Completed</td>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(taskItem,index) in item.taskItems" v-bind:class="{'table-success': taskItem.isDone}">
                                <td>
                                    <div class="custom-control custom-switch" >
                                        <input v-bind:disabled="item.isDeleted"
                                               v-model="taskItem.isDone" @change="updateTaskItem(taskItem)" 
                                               type="checkbox" 
                                               class="custom-control-input" 
                                               :id="`customSwitch-${taskItem.taskItemId}`">
                                        <label class="custom-control-label" 
                                               :for="`customSwitch-${taskItem.taskItemId}`"></label>
                                    </div>
                                </td>
                                <td>
                                    <label :for="`customSwitch-${taskItem.taskItemId}`" class="font-weight-normal">
                                        {{taskItem.title}}
                                    </label>                                    
                                </td>
                                <td>
                                    <div v-if="taskItem.isDone">
                                        {{taskItem.dateCompleted|moment('calendar')}}
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
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

            async updateTaskItem(taskItem) {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    const item = vm.item;

                    await vm.$util.axios.put(`/api/members/tasks/update-task-item/${item.taskId}/${taskItem.taskItemId}/${taskItem.isDone}`)
                        .then(async resp => {
                            vm.$bvToast.toast('Task Item updated.', { title: 'Update Task Item', variant: 'success', toaster: 'b-toaster-bottom-right' });

                            vm.$emit('updated');
                            vm.get();
                        });
                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false;
                }
            },

            deleteTask() {
                const vm = this;

                if (vm.busy)
                    return;

                vm.busy = true;

                try {
                    this.$bvModal.msgBoxConfirm('Are you sure you want to delete this task?')
                        .then(async value => {
                            if (value) {
                                const item = vm.item;

                                const api = `/api/members/tasks/${item.taskId}/${item.token}`;

                                await vm.$util.axios.delete(api)
                                    .then(resp => {
                                        vm.$emit('removed');
                                        vm.close();
                                    });
                            }
                        })
                        .catch(err => {
                            vm.$util.handleError(err);
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