<template>
    <div v-cloak>        
        <reservations-view :uid="uid"
                            :id="id"
                            :roleId="roleId"></reservations-view>
    </div>
</template>
<script>
    import reservationsView from '../../../_Common/Components/Reservations/view.vue';

    export default {
        //mixins: [pageMixin],

        props: {
            uid: { type: String, required: true },
            id: { type: String, required: true },
            roleId: { type: String, required: true },
        },
        components: {
            reservationsView
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

        }
    }
</script>