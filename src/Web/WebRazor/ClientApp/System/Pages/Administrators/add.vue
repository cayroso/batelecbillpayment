<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fas fa-fw fa-user-shield me-1"></i>Add Administrator
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
                                    <input v-model="item.email" type="text" class="form-control" id="email" placeholder="Email">
                                    <label for="email">Email</label>
                                    <div v-if="validations.has('email')" class="d-block invalid-feedback">
                                        {{validations.get('email')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <input v-model="item.phoneNumber" type="tel" class="form-control" id="phoneNumber" placeholder="Phone Number">
                                    <label for="phoneNumber">Phone Number</label>
                                    <div v-if="validations.has('phoneNumber')" class="d-block invalid-feedback">
                                        {{validations.get('phoneNumber')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <input v-model="item.firstName" type="text" class="form-control" id="firstName" placeholder="First Name">
                                    <label for="firstName">First Name</label>
                                    <div v-if="validations.has('firstName')" class="d-block invalid-feedback">
                                        {{validations.get('firstName')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <input v-model="item.middleName" type="text" class="form-control" id="middleName" placeholder="Middle Name">
                                    <label for="middleName">Middle Name</label>
                                    <div v-if="validations.has('middleName')" class="d-block invalid-feedback">
                                        {{validations.get('middleName')}}
                                    </div>
                                </div>
                                <div class="form-floating mb-3">
                                    <input v-model="item.lastName" type="text" class="form-control" id="lastName" placeholder="Last Name">
                                    <label for="lastName">Last Name</label>
                                    <div v-if="validations.has('lastName')" class="d-block invalid-feedback">
                                        {{validations.get('lastName')}}
                                    </div>
                                </div>

                                <div class="form-floating mb-3">
                                    <input v-model="item.password" type="password" class="form-control" id="password" placeholder="Password">
                                    <label for="password">Password</label>
                                    <div v-if="validations.has('password')" class="d-block invalid-feedback">
                                        {{validations.get('password')}}
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
                    email: null,
                    phoneNumber: null,
                    firstName: null,
                    middleName: null,
                    lastName: null,
                    password: null,
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

                if (!item.email) {
                    validations.set('email', 'Email is required.');
                }

                if (!item.phoneNumber) {
                    validations.set('phoneNumber', 'Phone Number is required.');
                }

                if (!item.firstName) {
                    validations.set('firstName', 'First Name is required.');
                }

                if (!item.middleName) {
                    validations.set('middleName', 'Middle Name is required.');
                }

                if (!item.lastName) {
                    validations.set('lastName', 'Last Name is required.');
                }

                if (!item.password) {
                    validations.set('password', 'Password is required.');
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

                    await vm.$util.axios.post(`/api/account/administrator/add/`, payload)
                        .then(resp => {
                            vm.$toast.success('Add Administrator', 'New administrator created', {
                                async onClose() {
                                    const url = `${vm.urlView}/${resp.data}`;
                                    vm.$util.href(url);
                                }
                            })
                            //alert('New Announcement Created.');
                            ////vm.$bvToast.toast('Contact created.', { title: 'Add Contact', variant: 'success', toaster: 'b-toaster-bottom-right' });

                            //setTimeout(() => {
                            //    const url = `${vm.urlView}/${resp.data}`;
                            //    vm.$util.href(url);
                            //    //vm.$router.push({ name: 'contactsView', params: { id: resp.data } });
                            //}, 500);
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