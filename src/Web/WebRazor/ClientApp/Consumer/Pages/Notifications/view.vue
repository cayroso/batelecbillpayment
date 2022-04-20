<template>
    <div v-cloak>
        <notifications-view :uid="uid"
                            :id="id"
                            :roleId="roleId"
                            :urlViewAnnouncement="urlViewAnnouncement"
                            :urlViewBilling="urlViewBilling"
                            :urlViewReservation="urlViewReservation"></notifications-view>
    </div>
</template>
<script>
    import notificationsView from '../../../_Common/Components/Notifications/view.vue';

    export default {
        //mixins: [pageMixin],

        props: {
            uid: { type: String, required: true },
            id: { type: String, required: true },
            roleId: { type: String, required: true },
            urlViewAnnouncement: { type: String, required: true },
            urlViewBilling: { type: String, required: true },
            urlViewReservation: { type: String, required: true },
        },
        components: {
            notificationsView
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