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

        <div class="row">

            <div class="col-md mb-2 mb-md-0">
                <div class="card card-primary">
                    <div class="card-header">
                        Announcements
                    </div>
                    <div class="card-body">
                        {{item.announcements}}
                    </div>
                </div>
            </div>
            <div class="col-md mb-2 mb-md-0">
                <div class="card card-primary">
                    <div class="card-header">
                        Notifications
                    </div>
                    <div class="card-body">
                        {{item.notifications}}
                    </div>
                </div>
            </div>
        </div>

        <br />
        <h5>Reservations</h5>
        <div class="row">
            <div class="col-md mb-2 mb-md-0">
                <div class="card card-primary">
                    <div class="card-header">
                        Today
                    </div>
                    <div class="card-body">
                        {{item.todayReservations}}
                    </div>
                </div>
            </div>
            <div class="col-md mb-2 mb-md-0">
                <div class="card card-primary">
                    <div class="card-header">
                        Tomorrow
                    </div>
                    <div class="card-body">
                        {{item.tomorrowReservations}}
                    </div>
                </div>
            </div>
            <div class="col-md mb-2 mb-md-0">
                <div class="card card-primary">
                    <div class="card-header">
                        This Week
                    </div>
                    <div class="card-body">
                        {{item.weekReservations}}
                    </div>
                </div>
            </div>
        </div>

        <br />
        <h5>Billing</h5>
        <div class="row">
            <div class="col-md mb-2 mb-md-0">
                <div class="card card-primary">
                    <div class="card-header">
                        Over Due
                    </div>
                    <div class="card-body">
                        {{item.pastDueDateBillings}}
                    </div>
                </div>
            </div>
            <div class="col-md mb-2 mb-md-0">
                <div class="card card-primary">
                    <div class="card-header">
                        Today
                    </div>
                    <div class="card-body">
                        {{item.todayDueDateBillings}}
                    </div>
                </div>
            </div>
            <div class="col-md mb-2 mb-md-0">
                <div class="card card-primary">
                    <div class="card-header">
                        Tomorrow
                    </div>
                    <div class="card-body">
                        {{item.tomorrowDueDateBillings}}
                    </div>
                </div>
            </div>
            <div class="col-md mb-2 mb-md-0">
                <div class="card card-primary">
                    <div class="card-header">
                        This Week
                    </div>
                    <div class="card-body">
                        {{item.weekDueDateBillings}}
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
            greet() {

                this.message = 'Hello ' + this.text;
            },
            //async clickMe() {
            //    alert('click me.')
            //},
            async get() {
                const vm = this;

                try {
                    await vm.$util.axios.get(`/api/dashboard/consumers`)
                        .then(resp => vm.item = resp.data);
                } catch (e) {
                    vm.$util.handleError(e);
                }
            }
        }
    }
</script>