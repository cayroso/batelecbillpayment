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
            <div class="d-sm-flex align-items-center justify-content-between">
                <h1 class="h5 m-0">
                    Add Task
                </h1>
            </div>
        </template>
        <template v-slot:modal-footer>
            <button @click="resetItem" class="btn btn-info">
                Reset
            </button>
            <button @click="save" class="btn btn-primary">
                Save
            </button>
            <button @click="close" class="btn btn-secondary">
                Close
            </button>
        </template>
        <div>
            <div class="form-group">
                <label for="firstName">Name</label>
                <div class="form-control" readonly>
                    {{item.firstName}} {{item.middleName}} {{item.lastName}}
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col-md">
                    <label for="firstName">Status</label>
                    <div class="form-control" readonly>
                        {{item.statusText}}
                    </div>
                </div>
                <div class="form-group col-md">
                    <label for="rating">Status</label>
                    <div>
                        <b-form-rating inline v-model="item.rating" id="ratingEdit" readonly></b-form-rating>
                    </div>
                </div>
            </div>
            <hr />
            <div class="form-group">
                <label for="title">Task Type</label>
                <div>
                    <b-form-select v-model="taskType" :options="lookups.taskTypes" @change="onSelectedTaskTypeChanged"></b-form-select>
                </div>
            </div>
            <div class="form-group">
                <label for="title">Title</label>
                <div>
                    <input v-model="title" type="text" id="title" class="form-control" />
                </div>
            </div>
            <div class="form-group">
                <label for="description">Description</label>
                <div>
                    <textarea v-model="description" rows="2" id="description" class="form-control" />
                </div>
            </div>

            <div class="form-group">
                <label for="description">Task Items</label>
                <div class="table-responsive">
                    <table class="table table-bordered">
                        <thead>
                            <tr>
                                <th>#</th>
                                <th>Title</th>
                                <th></th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr v-for="(taskItem,index) in taskItems">
                                <td>{{index+1}}</td>
                                <td>{{taskItem}}</td>
                                <td>
                                    <button @click="removeTaskItem(index)" class="btn btn-sm btn-outline-warning">
                                        <i class="fas fa-fw fa-trash"></i>
                                    </button>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <input v-model="taskItemTitle" type="text" class="form-control" />
                                </td>
                                <td>
                                    <button @click="addTaskItem" class="btn btn-sm btn-outline-primary">
                                        <i class="fas fa-fw fa-plus"></i>
                                    </button>
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
                busy: false,
                lookups: {
                    taskTypes: [
                        { value: 1, text: 'Follow-Up Email' },
                        { value: 2, text: 'Phone Call' },
                        { value: 3, text: 'Demo' },
                        { value: 4, text: 'Lunch Meeting' },
                        { value: 5, text: 'Meetup' },
                        { value: 6, text: 'Others' },
                    ]
                },
                item: {},

                taskType: null,
                title: '',
                description: '',
                taskItems: [],
                taskItemTitle: ''
            }
        },
        methods: {
            resetItem() {
                const vm = this;

                vm.title = null;
                vm.description = null;
                vm.taskItems = [];
                vm.taskItemTitle = '';
            },
            onSelectedTaskTypeChanged(index) {
                const vm = this;

                if (!vm.title) {
                    const taskType = vm.lookups.taskTypes.find(e => e.value === index);
                    vm.title = taskType.text;

                    if (vm.taskItems.length <= 0) {
                        vm.taskItems.push(taskType.text);
                    }
                }
            },

            removeTaskItem(index) {
                const vm = this;

                vm.taskItems.splice(index, 1);
            },

            addTaskItem() {
                const vm = this;

                if (vm.taskItemTitle) {
                    vm.taskItems.push(vm.taskItemTitle);
                    vm.taskItemTitle = '';
                }
            },

            async open(item) {
                const vm = this;

                vm.resetItem();

                vm.item = item;

                vm.$refs.modal.show();
            },

            close() {
                const vm = this;

                vm.$refs.modal.hide();
            },

            async save() {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    const item = vm.item;

                    const obj = {
                        contactId: item.contactId,
                        taskType: vm.taskType,
                        title: vm.title,
                        description: vm.description,
                        taskItems: vm.taskItems.map(e => {
                            return { title: e };
                        })
                    };

                    const json = JSON.stringify(obj);
                    const blob = new Blob([json], {
                        type: 'application/json'
                    });

                    const formData = new FormData();

                    formData.append('info', blob);

                    await vm.$util.axios.post(`/api/members/tasks`, formData)
                        .then(resp => {
                            vm.$bvToast.toast('Task added.', { title: 'Add Task', variant: 'success', toaster: 'b-toaster-bottom-right' });
                        });



                    vm.$emit('saved');
                    vm.close();

                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    busy = false;
                }
            },
        }
    }
</script>