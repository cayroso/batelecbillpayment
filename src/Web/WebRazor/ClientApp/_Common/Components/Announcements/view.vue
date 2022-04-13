<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fas fa-fw fa-bullhorn me-1"></i>View Billings
                </h1>
            </div>
            <div class="col-sm-auto">
                <div class="d-flex flex-row">
                    <template v-if="roleId==='administrator'">
                        <a :href="`${urlEdit}`" class="btn btn-warning">Edit</a>
                    </template>
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
                <div class="card-header bg-info text-white">
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
            roleId: {
                type: String, required: true
            },
            urlEdit: {
                type: String, required: true
            }
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

                    await vm.$util.axios.get(`/api/announcement/${vm.id}`)
                        .then(resp => {

                            vm.item = resp.data;
                        });

                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false
                }
            }
        }
    }
</script>