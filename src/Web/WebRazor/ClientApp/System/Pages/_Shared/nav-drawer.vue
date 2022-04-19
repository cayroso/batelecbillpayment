<template>
    <b-sidebar id="navDrawer" :title="appName" backdrop shadow>
        <div class="list-group list-group-flush">
            <router-link to="/" class="list-group-item list-group-item-action">
                <span class="fas fa-fw fa-home mr-1"></span>Home
            </router-link>
            <router-link to="/contacts" class="list-group-item list-group-item-action">
                <span class="fas fa-fw fa-id-card mr-1"></span>Contacts
            </router-link>
            
            <router-link to="/tasks" class="list-group-item list-group-item-action">
                <span class="fas fa-fw fa-tasks mr-1"></span>Tasks
            </router-link>
            <router-link to="/documents" class="list-group-item list-group-item-action">
                <span class="fas fa-fw fa-archive mr-1"></span>Documents
            </router-link>
        </div>

    </b-sidebar>
</template>
<script>
    export default {
        props: {
            appName: String
        }

    };
</script>