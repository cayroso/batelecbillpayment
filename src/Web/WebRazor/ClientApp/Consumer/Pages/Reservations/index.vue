<template>
    <div v-cloak>
        <reservations-index :uid="uid"
                            :urlView="urlView"
                            :urlAdd="urlAdd"
                            :roleId="roleId"></reservations-index>
    </div>
</template>
<script>
    import reservationsIndex from '../../../_Common/Components/Reservations/index.vue';

    export default {
        props: {
            uid: { type: String, required: true },
            roleId: { type: String, required: true },
            urlView: { type: String, required: true },
            urlAdd: { type: String, required: true },
        },
        components: {
            reservationsIndex
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