<template>
    <div v-cloak>
        <div class="d-flex flex-row justify-content-between align-items-sm-center">
            <h1 class="h3 mb-sm-0">
                <i class="fas fa-fw fa-money-bill me-1"></i>
                View Billing
            </h1>

            <div class="text-right">
                <button @click="get" class="btn btn-primary">
                    <i class="fas fa-fw fa-sync"></i>
                </button>
                <button @click="close" class="ms-1 btn btn-secondary">
                    <i class="fas fa-fw fa-times"></i>
                </button>
            </div>
        </div>
        <div class="mt-2">
            {{item}}
        </div>
    </div>
</template>
<script>
    import pageMixin from '../../../_Core/Mixins/pageMixin';

    export default {
        mixins: [pageMixin],

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
                //debugger
                if (vm.busy)
                    return;

                try {
                    vm.busy = true;

                    await vm.$util.axios.get(`/api/billing/${vm.id}`)
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