<template>
    
    <div class="nav-scroller bg-body shadow-sm">
        <nav class="nav nav-underline" aria-label="Secondary navigation">
            <a class="nav-link active" aria-current="page" href="#">Dashboard</a>
            <a class="nav-link" href="#">
                Friends
                <span class="badge bg-light text-dark rounded-pill align-text-bottom">27</span>
            </a>
            <a class="nav-link" href="#">Explore</a>
            <a class="nav-link" href="#">Suggestions</a>
            <a class="nav-link" href="#">Link</a>
            <a class="nav-link" href="#">Link</a>
            <a class="nav-link" href="#">Link</a>
            <a class="nav-link" href="#">Link</a>
            <a class="nav-link" href="#">Link</a>
        </nav>
    </div>

    <main class="container">
        <div class="d-flex align-items-center p-3 my-3 text-white bg-purple rounded shadow-sm">
            <img class="me-3" src="/docs/{{< param docs_version >}}/assets/brand/bootstrap-logo-white.svg" alt="" width="48" height="38">
            <div class="lh-1">
                <h1 class="h6 mb-0 text-white lh-1">Bootstrap</h1>
                <small>Since 2011</small>
            </div>
        </div>

        <div class="my-3 p-3 bg-body rounded shadow-sm">
            <h6 class="border-bottom pb-2 mb-0">Recent updates</h6>
            <div class="d-flex text-muted pt-3">
                <p class="pb-3 mb-0 small lh-sm border-bottom">
                    <strong class="d-block text-gray-dark">@username</strong>
                    Some representative placeholder content, with some information about this user. Imagine this being some sort of status update, perhaps?
                </p>
            </div>
            <div class="d-flex text-muted pt-3">
                <p class="pb-3 mb-0 small lh-sm border-bottom">
                    <strong class="d-block text-gray-dark">@username</strong>
                    Some more representative placeholder content, related to this other user. Another status update, perhaps.
                </p>
            </div>
            <div class="d-flex text-muted pt-3">
                <p class="pb-3 mb-0 small lh-sm border-bottom">
                    <strong class="d-block text-gray-dark">@username</strong>
                    This user also gets some representative placeholder content. Maybe they did something interesting, and you really want to highlight this in the recent updates.
                </p>
            </div>
            <small class="d-block text-end mt-3">
                <a href="#">All updates</a>
            </small>
        </div>

        <div class="my-3 p-3 bg-body rounded shadow-sm">
            <h6 class="border-bottom pb-2 mb-0">Suggestions</h6>
            <div class="d-flex text-muted pt-3">
                <div class="pb-3 mb-0 small lh-sm border-bottom w-100">
                    <div class="d-flex justify-content-between">
                        <strong class="text-gray-dark">Full Name</strong>
                        <a href="#">Follow</a>
                    </div>
                    <span class="d-block">@username</span>
                </div>
            </div>
            <div class="d-flex text-muted pt-3">
                <div class="pb-3 mb-0 small lh-sm border-bottom w-100">
                    <div class="d-flex justify-content-between">
                        <strong class="text-gray-dark">Full Name</strong>
                        <a href="#">Follow</a>
                    </div>
                    <span class="d-block">@username</span>
                </div>
            </div>
            <div class="d-flex text-muted pt-3">
                <div class="pb-3 mb-0 small lh-sm border-bottom w-100">
                    <div class="d-flex justify-content-between">
                        <strong class="text-gray-dark">Full Name</strong>
                        <a href="#">Follow</a>
                    </div>
                    <span class="d-block">@username</span>
                </div>
            </div>
            <small class="d-block text-end mt-3">
                <a href="#">All suggestions</a>
            </small>
        </div>
    </main>

</template>

<script>
    import pageMixin from '../../_Core/Mixins/pageMixin';

    export default {
        //mixins: [pageMixin],

        props: {
            uid: String,
        },

        data() {
            return {
                message: null,
                text: null,
                item: {}
            };
        },

        computed: {

        },

        async created() {
            const vm = this;

        },

        async mounted() {
            const vm = this;
            //alert('index loaded')
            //await vm.get();
        },

        methods: {
            greet() {

                this.message = 'Hello ' + this.text;
            },
            //async clickMe() {
            //    alert('click me.')
            //},
            //async get() {
            //    const vm = this;

            //    try {
            //        await vm.$util.axios.get(`/api/managers/default/dashboard`)
            //            .then(resp => vm.item = resp.data);
            //    } catch (e) {
            //        vm.$util.handleError(e);
            //    }
            //}
        }
    }
</script>