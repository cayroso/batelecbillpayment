<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fa-solid fa-fw fa-calendar-check me-1"></i>View Gcash WebHook
                </h1>
            </div>
            <div class="col-sm-auto">
                <div class="d-flex flex-row">
                    <button v-bind:disabled="isDirty && !formIsValid" @click="save" class="btn btn-primary">
                        <span class="fas fa-fw fa-save"></span>
                    </button>

                    <button @click="get" class="ms-2 btn btn-secondary">
                        <i class="fas fa-fw fa-sync"></i>
                    </button>
                    <button @click="close" class="ms-1 btn btn-secondary">
                        <i class="fas fa-fw fa-times"></i>
                    </button>
                </div>
            </div>
        </div>

        <div class="mt-2">
            <div class="card shadow-sm">
                <div class="card-header bg-info text-white">
                    <!--Personal Information-->                    
                </div>
                <div class="card-body">
                    <form class="needs-validation" novalidate>
                        <div class="row">
                            <div class="col-sm-12">

                                <div class="form-floating mb-3">
                                    <input v-model="item.url" type="text" class="form-control" id="url" placeholder="Url">
                                    <label for="url">Url</label>
                                    <div v-if="validations.has('url')" class="d-block invalid-feedback">
                                        {{validations.get('url')}}
                                    </div>
                                </div>

                                <div class="form-check" v-for="evt in lookups.events">
                                    <input class="form-check-input" type="checkbox" value="" :id="evt.id" v-model="evt.selected">
                                    <label class="form-check-label" :for="evt.id">
                                        {{evt.id}}
                                    </label>
                                </div>
                                <div v-if="validations.has('events')" class="d-block invalid-feedback">
                                    {{validations.get('events')}}
                                </div>

                            </div>
                            <div v-if="errorHtml" class="col-sm-6">
                                <div class="alert alert-danger">
                                    <div v-html="errorHtml"></div>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</template>
<script>

    import pageMixin from '../../../_Core/Mixins/pageMixin';

    export default {
        mixins: [pageMixin],
        props: {
            uid: {
                type: String, required: true
            },
            id: {
                type: String, required: true
            },
            roleId: {
                type: String, required: true
            }
        },
        components: {
            //billingsView
        },
        data() {
            return {
                isDirty: false,
                validations: new Map(),
                errorHtml: null,

                originalUrl: '',
                item: {
                    url: '',
                    events: []
                },
                lookups: {
                    events: [
                        {
                            id: 'source.chargeable', selected: false
                        },
                        {
                            id: 'payment.paid', selected: false
                        },
                        {
                            id: 'payment.failed', selected: false
                        },
                        {
                            id: 'payment.refunded', selected: false
                        },
                        {
                            id: 'payment.refund.updated', selected: false
                        },
                    ]
                }
            };
        },

        computed: {
            formIsValid() {
                const vm = this;

                //if (!vm.isDirty)
                //    return true;

                const item = vm.item;

                const validations = new Map();

                if (!item.url) {
                    validations.set('url', 'Url is required.');
                }

                if (item.url === vm.originalUrl) {
                    validations.set('url', `Url must be updated.`);
                }

                if (vm.lookups.events.every(e=> !e.selected)) {
                    validations.set('events', 'Must have at least 1 event selected.');
                }

                vm.isDirty = true;
                vm.validations = validations;

                return validations.size == 0;
            },
        },

        async created() {
            const vm = this;

        },

        async mounted() {
            const vm = this;

            await vm.get();
        },

        methods: {
            async get() {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    vm.busy = true;

                    await vm.$util.axios.get(`/api/gcash/webhooks/${vm.id}`)
                        .then(resp => {
                            let data = resp.data;

                            let events = data.events.split(',');

                            vm.lookups.events.forEach(e => {                                
                                e.selected = events.includes(e.id);
                            });

                            vm.item = data;
                            vm.originalUrl = data.url;

                            vm.isDirty = false;
                        });
                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false;
                }
            },

            async save() {
                const vm = this;

                if (vm.busy)
                    return;

                if (!vm.formIsValid)
                    return;

                try {
                    vm.busy = true;

                    const payload = {
                        id: vm.item.id,
                        url: vm.item.url,
                    };

                    let events = vm.lookups.events.filter(e => e.selected);

                    payload.events = events.map(e => e.id);

                    await vm.$util.axios.put(`/api/gcash/webhooks`, payload)
                        .then(resp => {
                            vm.$toast.success('Edit Webhook', 'Webhook was update.', {
                                async onClose() {
                                    await vm.get();
                                }
                            });
                        });

                } catch (e) {
                    vm.errorHtml = vm.$util.getErrorMessageAsHtml(e);
                } finally {
                    vm.busy = false
                }
            }
        }
    }
</script>