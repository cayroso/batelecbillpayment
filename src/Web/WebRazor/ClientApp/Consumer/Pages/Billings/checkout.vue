<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fas fa-fw fa-money-bill me-1"></i>Checkout
                </h1>
            </div>
            <div class="col-sm-auto">
                <div class="d-flex flex-row">
                    <button @click="get" class="ms-2 btn btn-primary">
                        <i class="fas fa-fw fa-sync"></i>
                    </button>
                    <button @click="close" class="ms-1 btn btn-secondary">
                        <i class="fas fa-fw fa-times"></i>
                    </button>
                </div>
            </div>
        </div>

        <div class="card mt-2">
            <div class="card-body">
                <div class="row">
                    <div class="col-sm-6">
                        <div class="form-floating mb-3">
                            <div class="form-control" id="number">
                                {{item.number}}
                            </div>
                            <label for="number">number</label>
                        </div>
                        <div class="form-floating mb-3">
                            <div class="form-control" id="amount">
                                {{item.amount}}
                            </div>
                            <label for="amount">amount</label>
                        </div>
                        <div class="form-floating mb-3">
                            <div class="form-control" id="statusText">
                                {{item.statusText}}
                            </div>
                            <label for="statusText">Status</label>
                        </div>
                    </div>
                </div>
            </div>
            <div v-if="resource" class="card-footer">
                <a v-if="resource.attributes  && resource.attributes.status === 'pending'" :href="resource.attributes.redirect.checkout_Url">
                    Pay with GCash via Paymongo (<small>{{resource.attributes.status}}</small>)
                </a>
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
        },

        data() {
            return {
                item: {},
                resource: null
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

            if (!vm.item.gCashSourceResourceId) {

                await vm.createResource();
                await vm.get();
            }

            await vm.getResource();

            if (vm.resource.attributes.status !== 'pending'
                && vm.resource.attributes.status !== 'chargeable'
                && vm.resource.attributes.status !== 'paid') {
                debugger;
                await vm.createResource();
                await vm.get();
                await vm.getResource();
            }
        },

        methods: {
            async get() {
                const vm = this;

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
            async getResource() {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    vm.busy = true;

                    await vm.$util.axios.get(`/api/gcash/resource/${vm.item.gCashSourceResourceId}`)
                        .then(resp => {
                            vm.resource = resp.data.data;
                        });
                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false;
                }
            },
            async createResource() {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    vm.busy = true;

                    await vm.$util.axios.post(`/api/billing/${vm.id}/create-resource`)
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