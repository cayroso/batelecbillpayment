<template>

    <div v-cloak>

        <div class="row">
            <div class="col">
                <h3>Dashboard</h3>
            </div>
            <div class="col-auto">
                <button class="btn btn-secondary" @click="get">
                    <i class="oi oi-reload"></i>
                    Refresh
                </button>
            </div>
        </div>
        
        <h5>Accounts</h5>
        <div class="row">
            <div class="col-md mb-2 mb-md-0">
                <div class="card card-primary">
                    <div class="card-header">
                        Administrators
                    </div>
                    <div class="card-body">
                        {{item.adminCount}}
                    </div>
                </div>
            </div>
            <div class="col-md mb-2 mb-md-0">
                <div class="card card-primary">
                    <div class="card-header">
                        Consumers
                    </div>
                    <div class="card-body">
                        {{item.consumerCount}}
                    </div>
                </div>
            </div>
            <div class="col-md mb-2 mb-md-0">
                <div class="card card-primary">
                    <div class="card-header">
                        Locked
                    </div>
                    <div class="card-body">
                        {{item.lockedCount}}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
    import pageMixin from '../../_Core/Mixins/pageMixin';

    export default {
        mixins: [pageMixin],

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
            await vm.get();
        },

        methods: {            
            async get() {
                const vm = this;

                try {
                    await vm.$util.axios.get(`/api/dashboard/systems`)
                        .then(resp => vm.item = resp.data);
                } catch (e) {
                    vm.$util.handleError(e);
                }
            }
        }
    }
</script>