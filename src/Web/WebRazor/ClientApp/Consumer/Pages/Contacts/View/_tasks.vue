<template>
    <div v-cloak>
        <div class="p-1 d-flex flex-row justify-content-between">
            <div>
                Legend:
                <span class="badge badge-warning">Todo</span>
                <span class="badge badge-info">InProgress</span>
                <span class="badge badge-success">Complete</span>

            </div>
            <button @click="addTask" class="btn btn-primary">
                <i class="fas fa-fw fa-plus"></i>
            </button>
        </div>

        <div class="table-responsive">
            <table class="table table-bordered table-hover">
                <thead>
                    <tr>
                        <th class="text-center mx-sm-0 px-sm-0">#</th>
                        <th>Title</th>
                        <th>Type</th>

                    </tr>
                </thead>
                <tbody>
                    <template v-for="(task,index) in item.tasks">
                        <tr>
                            <td v-bind:rowspan="task.description? 3:2" class="text-center align-middle mx-sm-0 px-sm-0" v-bind:class="getTaskTdCss(task)">
                                {{index+1}}
                            </td>

                            <td>
                                <a @click.prevent="$refs.modalViewTask.open(task.taskId)" href="#">
                                    {{task.title}}
                                </a>
                            </td>
                            <td>
                                <p>{{task.taskTypeText}}</p>
                                <!--<div>
                                    Created: {{task.dateCreated|moment('calendar')}}
                                </div>
                                <div>
                                    Assigned: {{task.dateAssigned|moment('calendar')}}
                                </div>
                                <div v-if="moment(task.dateCompleted).isBefore()">
                                    Completed: {{task.dateCompleted|moment('calendar')}}
                                </div>-->

                            </td>
                        </tr>
                        <tr v-if="task.description">
                            <td colspan="2" class="small">
                                {{task.description}}
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="m-0 p-0">
                                <ol>
                                    <li v-for="ti in task.taskItems">
                                        <span v-bind:style="{'text-decoration': ti.isDone? 'line-through':''}">{{ti.title}}</span>
                                    </li>
                                </ol>

                            </td>
                        </tr>
                    </template>
                </tbody>
                
            </table>
        </div>

        <modal-add-task ref="modalAddTask" @saved="$emit('updated')"></modal-add-task>
        <modal-view-task ref="modalViewTask" @updated="$emit('updated')" @removed="$emit('updated')"></modal-view-task>
    </div>
</template>
<script>
    import modalAddTask from '../../../Modals/Tasks/add-task.vue';
    import modalViewTask from '../../../Modals/Tasks/view-task.vue';
    export default {

        props: {
            uid: String,
            item: Object
        },
        components: {
            modalAddTask, modalViewTask
        },
        data() {
            return {
                moment: moment
            };
        },

        computed: {

        },

        async created() {
            const vm = this;

        },

        async mounted() {
            const vm = this;

        },

        methods: {
            getTaskTrCss(task) {
                switch (task.taskStatus) {
                    case 1:
                        return 'table-warning';
                    case 2:
                        return 'table-info';
                    case 3:
                        return 'table-success';
                    default:
                        return '';
                }
            },
            getTaskTdCss(task) {

                switch (task.taskStatus) {
                    case 1:
                        return 'table-warning';
                    case 2:
                        return 'table-info';
                    case 3:
                        return 'table-success';
                    default:
                        return '';
                }
            },
            addTask() {
                const vm = this;
                const item = vm.item;

                const payload = {
                    contactId: item.contactId,
                    firstName: item.firstName,
                    middleName: item.middleName,
                    lastName: item.lastName,

                    statusText: item.systemInformation.statusText,
                    rating: item.rating,
                };

                vm.$refs.modalAddTask.open(payload);
            },

            viewTask(task) {
                const vm = this;
            }
        }
    }
</script>