<template>
    <div v-cloak>
        <notifications-index :uid="uid"
                             :urlView="urlView"></notifications-index>
    </div>
</template>
<script>
    import notificationsIndex from '../../../_Common/Components/Notifications/index.vue';
    //import modalViewTask from '../../Modals/Tasks/view-task.vue';
    export default {

        props: {
            uid: String,
            urlView: { type: String, required: true },
        },
        components: {
            notificationsIndex
        },
        data() {
            return {

            };
        },

        computed: {

        },

        async created() {

        },

        async mounted() {
            const vm = this;

        },

        methods: {

        }
    }
</script>