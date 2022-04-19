<template>
    
    <div v-cloak>
        <div>
            <h3>Last Reading</h3>
            <h3>Inquire Billing</h3>
            <h3>Annoucement</h3>
            <h3>Reservation</h3>
            <h3>Contact</h3>
        </div>
    </div>

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