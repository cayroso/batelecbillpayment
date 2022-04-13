<style>
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
                <!--<div class="col-auto mr-1">
                <a :href="urlAdd" class="btn btn-primary">
                    <span class="fas fa-fw fa-plus"></span>
                </a>
            </div>
            <div class="col-auto mr-1">
                <button @click="filter.visible = !filter.visible" class="btn btn-primary">
                    <span class="fas fa-fw fa-filter"></span>
                </button>
            </div>
            <div class="col">
                <div class="input-group">
                    <input v-model="filter.query.criteria" @keyup.enter="search(1)" type="text" class="form-control">
                    <div class="input-group-append">
                        <button @click="search(1)" class="btn btn-secondary" type="button" id="button-addon2">
                            <span class="fa fas fa-fw fa-search"></span>
                        </button>
                    </div>
                </div>
            </div>-->
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
                <m-information :item="item" @updated="get"></m-information>
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
                <m-work :item="item" @updated="get"></m-work>
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
                <m-attachments :id="item.contactId" :token="item.token" :items="item.attachments" @updated="get"></m-attachments>
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
                <m-tasks :item="item" @updated="get"></m-tasks>
            </b-collapse>
        </div>

        <div class="card mt-2">
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
                <m-system-information :item="item" @updated="get"></m-system-information>
            </b-collapse>
        </div>

        
    </div>
</template>
<script>
    import pageMixin from '../../../../_Core/Mixins/pageMixin';

    import mInformation from './_information.vue';
    import mWork from './_work.vue';
    import mNotes from './_notes.vue';
    import mAttachments from './_attachments.vue';
    import mTasks from './_tasks.vue';
    import mSystemInformation from './_system-information.vue';
    
    export default {
        mixins: [pageMixin],
        components: {
            mInformation,
            mWork,
            mNotes,
            mAttachments,
            mTasks,
            mSystemInformation
        },
        props: {
            uid: String,
            id: String,
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