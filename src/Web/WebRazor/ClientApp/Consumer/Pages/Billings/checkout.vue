<template>
    <div v-cloak>
        {{item}}

        <a :href="item.gCashCheckoutUrl">
            Pay with GCash via Paymongo
        </a>
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
            debugger
            if (!vm.item.gCashSourceResourceId) {
                await vm.createResource();

                await vm.get();

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