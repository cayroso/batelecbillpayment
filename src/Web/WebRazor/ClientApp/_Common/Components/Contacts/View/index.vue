<style scoped>
    label {
        font-size: small;
        font-weight: bold;
    }
</style>
<template>
    <div v-cloak>
        <div class="d-flex flex-row justify-content-between align-items-sm-center">
            <h1 class="h3 mb-sm-0">
                <span class="fas fa-fw fa-id-card mr-1"></span>View Contact
            </h1>

            <div class="text-right">
                <button @click="get" class="btn btn-primary">
                    <i class="fas fa-fw fa-sync"></i>
                </button>
                <button @click="close" class="btn btn-secondary">
                    <i class="fas fa-fw fa-times"></i>
                </button>
            </div>
        </div>


        <div class="card mt-2">
            <div @click="toggle('contactInformation')" class="card-header d-flex flex-row justify-content-between align-items-center">
                <h5 class="mb-0 align-self-start">
                    <span class="fas fa-fw fa-info-circle mr-1 d-none"></span>Information
                </h5>
                <div>
                    <span>
                        <span v-if="toggles.contactInformation" class="fas fa-fw fa-angle-up"></span>
                        <span v-else class="fas fa-fw fa-angle-down"></span>
                    </span>
                </div>
            </div>
            <b-collapse v-model="toggles.contactInformation">
                <page-information :roleId="roleId" :uid="uid" :item="item" :busy="busy" @get="get"></page-information>
            </b-collapse>
        </div>

        <div class="card mt-2">
            <div @click="toggle('workInformation')" class="card-header d-flex flex-row justify-content-between align-items-center">
                <h5 class="mb-0 align-self-start">
                    <span class="fas fa-fw fa-info-circle mr-1 d-none"></span>Work
                </h5>
                <div>
                    <span>
                        <span v-if="toggles.workInformation" class="fas fa-fw fa-angle-up"></span>
                        <span v-else class="fas fa-fw fa-angle-down"></span>
                    </span>
                </div>
            </div>
            <b-collapse v-model="toggles.workInformation">
                <page-work :roleId="roleId" :uid="uid" :item="item" :busy="busy" @get="get"></page-work>
            </b-collapse>
        </div>

        <div class="card mt-2">
            <div @click="toggle('attachments')" class="card-header d-flex flex-row justify-content-between align-items-center">
                <h5 class="mb-0 align-self-start">
                    <span class="fas fa-fw fa-sticky-note mr-1 d-none"></span>Attachments
                </h5>
                <div>
                    <span>
                        <span v-if="toggles.attachments" class="fas fa-fw fa-angle-up"></span>
                        <span v-else class="fas fa-fw fa-angle-down"></span>
                    </span>
                </div>
            </div>
            <b-collapse v-model="toggles.attachments">
                <page-attachments :roleId="roleId" :uid="uid" :id="id" :token="item.token" :item="item" :busy="busy" @get="get"></page-attachments>
            </b-collapse>
        </div>

        <div class="card mt-2">
            <div @click="toggle('tasks')" class="card-header d-flex flex-row justify-content-between align-items-center">
                <h5 class="mb-0 align-self-start">
                    <span class="fas fa-fw fa-tasks mr-1 d-none"></span>Tasks
                </h5>
                <div>
                    <span>
                        <span v-if="toggles.tasks" class="fas fa-fw fa-angle-up"></span>
                        <span v-else class="fas fa-fw fa-angle-down"></span>
                    </span>
                </div>
            </div>
            <b-collapse v-model="toggles.tasks">
                <page-tasks :item="item" :busy="busy" @updated="get"></page-tasks>
            </b-collapse>
        </div>

        <!--<div class="card mt-2">
        <div @click="toggle('systemInformation')" class="card-header d-flex flex-row justify-content-between align-items-center">
            <h5 class="mb-0 align-self-start">
                <span class="fas fa-fw fa-money-bill mr-1 d-none"></span>System
            </h5>
            <div>
                <span>
                    <span v-if="toggles.systemInformation" class="fas fa-fw fa-angle-up"></span>
                    <span v-else class="fas fa-fw fa-angle-down"></span>
                </span>
            </div>
        </div>
        <b-collapse v-model="toggles.systemInformation">
            <m-system-information :item="item" :busy="busy" @updated="get"></m-system-information>
        </b-collapse>
    </div>-->
    </div>
</template>
<script>
    import pageMixin from '../../../../_Core/Mixins/pageMixin';

    import PageInformation from './Information/index.vue';
    import PageWork from './WorkInformation/index.vue';
    import PageAttachments from './Attachments/index.vue';
    import PageTasks from './Tasks/index.vue';
    //import PageSystemInformation from './_system-information.vue';

    export default {
        mixins: [pageMixin],
        components: {
            PageInformation,
            PageWork,
            PageAttachments,
            PageTasks,
            //mSystemInformation
        },
        props: {
            uid: {
                type: String, required: true
            },
            id: {
                type: String, required: true
            },
            roleId: {
                type: String, required: true
            }
        },

        data() {
            return {
                togglesKey: `view-contact/${this.uid}/toggles`,
                toggles: {
                    contactInformation: false,
                    workInformation: false,
                    systemInformation: false,
                    notes: false,
                    attachments: false,
                    tasks: false,
                },

                item: {
                    systemInformation: {},
                    notes: [],
                    attachments: [],
                    tasks: []
                },
            };
        },

        computed: {

        },

        async created() {
            const vm = this;

            const toggles = JSON.parse(localStorage.getItem(vm.togglesKey)) || null;

            if (toggles)
                vm.toggles = toggles;

            await vm.get();
        },

        async mounted() {
            const vm = this;

            //await vm.get();
        },

        methods: {
            async get() {
                const vm = this;
                //debugger
                if (vm.busy)
                    return;

                try {
                    vm.busy = true;

                    await vm.$util.axios.get(`/api/members/contacts/${vm.id}`)
                        .then(resp => {
                            vm.item = resp.data;
                        });
                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false;
                }
            },
        }
    }
</script>