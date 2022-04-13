<template>
    <page-view :id="id" :uid="uid" viewed="manager"></page-view>
</template>
<script>
    import pageView from '../../../_Common/Components/Contacts/View/index.vue';
    export default {
        components: {
            pageView
        },
        props: {
            uid: String,
            id: String,
        },
    }
</script>