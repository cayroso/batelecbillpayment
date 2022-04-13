<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fas fa-fw fa-money-bill me-1"></i>Add Billings
                </h1>
            </div>
            <div class="col-sm-auto">
                <div class="d-flex flex-row">
                    <button v-bind:disabled="isDirty && !formIsValid" @click="save" class="btn btn-primary">
                        <span class="fas fa-fw fa-save"></span>
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
                            <div class="col-sm-6">

                                <div class="form-floating mb-3">
                                    <input v-model="item.subject" type="text" class="form-control" id="subject" placeholder="Subject">
                                    <label for="subject">Subject</label>
                                    <div v-if="validations.has('subject')" class="d-block invalid-feedback">
                                        {{validations.get('subject')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <textarea v-model="item.content" style="height: 100px" class="form-control" id="content" placeholder="Content"></textarea>
                                    <label for="content">Content</label>
                                    <div v-if="validations.has('content')" class="d-block invalid-feedback">
                                        {{validations.get('content')}}
                                    </div>
                                </div>

                                <div class="form-floating mb-3">
                                    <input v-model="item.datePost" type="date" class="form-control" id="datePost" placeholder="Post Date">
                                    <label for="datePost">Post Date</label>
                                    <div v-if="validations.has('datePost')" class="d-block invalid-feedback">
                                        {{validations.get('datePost')}}
                                    </div>
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
            uid: { type: String, required: true },
            urlView: { type: String, required: true },
        },

        data() {
            return {
                isDirty: false,
                validations: new Map(),
                lookups: {                    
                },
                item: {
                    subject: "",
                    content: "",                    
                    datePost: this.$moment().format('YYYY-MM-DD'),                    
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

                if (!item.subject) {
                    validations.set('subject', 'Subject is required.');
                }

                if (!item.content) {
                    validations.set('content', 'Content is required.');
                }
                
                if (!item.datePost) {
                    validations.set('datePost', 'Post Date is required.');
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
            const foo = vm.formIsValid;

        },

        methods: {
            getValidClass(field) {
                const vm = this;

                if (!vm.isDirty)
                    return '';

                if (vm.validations.has(field))
                    return 'is-invalid';
                return 'is-valid';
            },

            async save() {
                const vm = this;

                if (vm.busy)
                    return;

                if (!vm.formIsValid)
                    return;

                try {
                    vm.busy = true;

                    const payload = vm.$util.clone(vm.item);
                    //payload.salutation = payload.salutation === null ? 0 : payload.salutation;
                    //payload.dateOfInitialContact = moment(payload.dateOfInitialContact).utc();
                    //payload.annualRevenue = payload.annualRevenue === null ? 0 : payload.annualRevenue;
                    //payload.rating = payload.rating === null ? 0 : payload.rating;

                    await vm.$util.axios.post(`/api/announcement/add/`, payload)
                        .then(resp => {
                            alert('New Announcement Created.');
                            //vm.$bvToast.toast('Contact created.', { title: 'Add Contact', variant: 'success', toaster: 'b-toaster-bottom-right' });

                            setTimeout(() => {
                                const url = `${vm.urlView}/${resp.data}`;
                                vm.$util.href(url);
                                //vm.$router.push({ name: 'contactsView', params: { id: resp.data } });
                            }, 500);
                        });

                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false
                }
            }
        }
    }
</script>