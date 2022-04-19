<template>
    <div v-cloak>
        <notifications-view :uid="uid"
                            :id="id"></notifications-view>
    </div>
</template>
<script>
    import notificationsView from '../../../_Common/Components/Notifications/view.vue';

    export default {
        
        props: {
            uid: { type: String, required: true },
            id: { type: String, required: true },
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