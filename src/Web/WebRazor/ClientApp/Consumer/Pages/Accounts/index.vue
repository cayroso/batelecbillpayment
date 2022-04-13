<template>
    <div v-cloak>
        <accounts-index></accounts-index>
        <!--Account Page
        <button @click="openModal" class="btn btn-primary">
            Change Theme
        </button>
        <modal-change-theme ref="modalChangeTheme"></modal-change-theme>-->
    </div>
</template>
<script>
    import accountsIndex from '../../../_Common/Components/Accounts/index.vue';

    export default {
        //mixins: [pageMixin],

        props: {
            uid: { type: String, required: true },
        },
        components: {
            accountsIndex
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
            //openModal() {
            //    const vm = this;
            //    vm.$refs.modalChangeTheme.open();
            //}
        }
    }
</script>