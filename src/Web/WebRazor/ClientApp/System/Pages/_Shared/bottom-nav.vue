<template>
    <b-navbar fixed="bottom" type="dark" variant="dark" class="d-block d-sm-none">
        <b-navbar-nav align="center" :justified="true">
            <li class="nav-item">
                <router-link to="/" class="nav-link">
                    <span class="fas fa-fw fa-home mr-1"></span>
                    <span class="d-none d-md-inline">Home</span>
                </router-link>
            </li>
            <li class="nav-item">
                <router-link to="/contacts" class="nav-link">
                    <span class="fas fa-fw fa-id-card mr-1"></span>
                    <span class="d-none d-md-inline">Contacts</span>
                </router-link>
            </li>
            <li class="nav-item">
                <router-link to="/tasks" class="nav-link">
                    <span class="fas fa-fw fa-tasks mr-1"></span>
                    <span class="d-none d-md-inline">Tasks</span>
                </router-link>
            </li>
            <li class="nav-item">
                <router-link to="/documents" class="nav-link">
                    <span class="fas fa-fw fa-archive mr-1"></span>
                    <span class="d-none d-md-inline">Documents</span>
                </router-link>
            </li>
        </b-navbar-nav>
    </b-navbar>
</template>
<script>
    import pageMixin from '../../../_Core/Mixins/pageMixin';

    export default {
        mixins: [pageMixin],

        props: {
            uid: String,
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