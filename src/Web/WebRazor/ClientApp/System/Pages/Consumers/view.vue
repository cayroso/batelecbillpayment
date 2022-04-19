<template>
    <billings-view :uid="uid"
                   :id="id"
                   :roleId="roleId"></billings-view>
</template>
<script>

    import billingsView from '../../../_Common/Components/Billings/view.vue';

    export default {

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
        components: {
            billingsView
        },
        data() {
            return {

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
            //async get() {
            //    const vm = this;
            //    //debugger
            //    if (vm.busy)
            //        return;

            //    try {
            //        vm.busy = true;

            //        await vm.$util.axios.get(`/api/billing/${vm.id}`)
            //            .then(resp => {
            //                vm.item = resp.data;
            //            });
            //    } catch (e) {
            //        vm.$util.handleError(e);
            //    } finally {
            //        vm.busy = false;
            //    }
            //},
        }
    }
</script>