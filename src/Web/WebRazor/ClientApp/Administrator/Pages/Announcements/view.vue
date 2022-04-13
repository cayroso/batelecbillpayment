<template>
    <announcements-view :uid="uid"
                        :id="id"
                        :roleId="roleId"
                        :urlEdit="urlEdit"></announcements-view>
</template>
<script>

    import announcementsView from '../../../_Common/Components/Announcements/view.vue';

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
            },
            urlEdit: {
                type: String, required: true
            }
        },
        components: {
            announcementsView
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