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
            <button v-bind:disabled="busy" @click="resetItem" class="btn btn-info mr-auto">
                Reset
            </button>
            <button v-bind:disabled="busy || (isDirty && !formIsValid)" @click="save" class="btn btn-primary">
                Save
            </button>
            <button v-bind:disabled="busy" @click="close" class="btn btn-secondary">
                Close
            </button>
        </template>
        <div>
            <b-overlay :show="busy">
                <div class="form-row">
                    <div class="form-group col-md">
                        <label for="firstName">Contact</label>
                        <div>
                            {{item.firstName}} {{item.middleName}} {{item.lastName}}
                        </div>
                    </div>
                    <div class="form-group col-md-3">
                        <label for="firstName">Status</label>
                        <div>{{item.statusText}}</div>
                    </div>
                    <div class="form-group col-md-3">
                        <label for="firstName">Rating</label>
                        <div>
                            <b-form-rating inline no-border size="sm" v-model="item.rating" id="rating" readonly></b-form-rating>
                        </div>
                    </div>
                </div>


                <hr />
                <div class="form-row">
                    <div class="form-group col-md">
                        <label for="title">Task Type</label>
                        <div>
                            <b-form-select v-model="taskType" :options="lookups.taskTypes" @change="onSelectedTaskTypeChanged" v-bind:class="getValidClass('taskType')"></b-form-select>
                            <span v-if="validations.get('taskType')" class="text-danger">
                                {{validations.get('taskType')}}
                            </span>
                        </div>
                    </div>
                    <div class="form-group col-md">
                        <label for="title">Date to Complete</label>
                        <div>
                            <input v-model="dateToComplete" type="date" class="form-control" v-bind:class="getValidClass('dateToComplete')" />
                            <span v-if="validations.get('dateToComplete')" class="text-danger">
                                {{validations.get('dateToComplete')}}
                            </span>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label for="title">Title</label>
                    <div>
                        <input v-model="title" type="text" id="title" class="form-control" v-bind:class="getValidClass('title')" />
                        <span v-if="validations.get('title')" class="text-danger">
                            {{validations.get('title')}}
                        </span>
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
                    <div class="table-responsive" v-bind:class="getValidClass('taskItems')">
                        <table class="table table-sm table-hover">
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
                            </tbody>
                            <tfoot>
                                <tr>
                                    <td colspan="2">
                                        <input v-model="taskItemTitle" type="text" class="form-control" />
                                    </td>
                                    <td>
                                        <button @click="addTaskItem" class="btn btn-sm btn-outline-primary">
                                            <i class="fas fa-fw fa-save"></i>
                                        </button>
                                    </td>
                                </tr>
                            </tfoot>
                        </table>
                    </div>

                    <span v-if="validations.get('taskItems')" class="text-danger">
                        {{validations.get('taskItems')}}
                    </span>
                </div>
            </b-overlay>
        </div>
    </b-modal>
</template>
<script>
    export default {
        data() {
            return {
                isDirty: false,
                validations: new Map(),
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
                taskItemTitle: '',
                dateToComplete: moment().format('YYYY-MM-DD'),
            }
        },
        computed: {
            formIsValid() {
                const vm = this;

                const validations = new Map();

                if (!vm.taskType) {
                    validations.set('taskType', 'Task type is required.');
                }

                if (!vm.dateToComplete) {
                    validations.set('dateToComplete', 'Date to complete is required.');
                }

                if (!vm.title) {
                    validations.set('title', 'Title is required.');
                }

                if (vm.taskItems.length <= 0) {
                    validations.set('taskItems', 'At least 1 task item is required.');
                }

                vm.isDirty = true;
                vm.validations = validations;

                return validations.size == 0;
            }
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

            resetItem() {
                const vm = this;

                vm.taskType = null;
                vm.title = null;
                vm.description = null;
                vm.taskItems = [];
                vm.taskItemTitle = '';
                vm.dateToComplete = moment().format('YYYY-MM-DD'); //add(7, 'days').

                vm.isDirty = false;
                vm.validations.clear();
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

                if (!vm.formIsValid)
                    return;

                vm.busy = true;

                try {
                    const item = vm.item;

                    const obj = {
                        contactId: item.contactId,
                        taskType: vm.taskType,
                        title: vm.title,
                        description: vm.description,
                        dateToComplete: moment(vm.dateToComplete).utc(),
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
                    vm.busy = false;
                }
            },
        }
    }
</script>