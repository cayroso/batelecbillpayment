<style scoped>
    label {
        font-size: small;
        font-weight: bold;
    }
</style>
<template>
    <b-modal ref="modal"
             :no-close-on-esc="false"
             :no-close-on-backdrop="true"
             scrollable>
        <template v-slot:modal-header>
            <div class="w-100">
                <div class="d-flex flex-row  align-items-center justify-content-between">
                    <h5 class="m-0">
                        Edit Account
                    </h5>
                </div>
            </div>
        </template>
        <template v-slot:modal-footer>
            <button v-bind:disabled="busy || (isDirty && !formIsValid)" @click="save" class="btn btn-primary">
                Save
            </button>
            <button @click="close" class="btn btn-secondary">
                Close
            </button>
        </template>
        <div>
            <div class="form-group">
                <label for="firstName">First Name</label>
                <div>
                    <input v-model="item.firstName" type="text" id="firstName" class="form-control" v-bind:class="getValidClass('firstName')" />
                    <span v-if="validations.get('firstName')" class="text-danger">
                        {{validations.get('firstName')}}
                    </span>
                </div>
            </div>
            <div class="form-group">
                <label for="middleName">Middle Name</label>
                <div>
                    <input v-model="item.middleName" type="text" id="middleName" class="form-control" v-bind:class="getValidClass('middleName')" />
                    <span v-if="validations.get('middleName')" class="text-danger">
                        {{validations.get('middleName')}}
                    </span>
                </div>
            </div>
            <div class="form-group">
                <label for="lastName">Last Name</label>
                <div>
                    <input v-model="item.lastName" type="text" id="lastName" class="form-control" v-bind:class="getValidClass('lastName')" />
                    <span v-if="validations.get('lastName')" class="text-danger">
                        {{validations.get('lastName')}}
                    </span>
                </div>
            </div>
        </div>
    </b-modal>
</template>
<script>
    export default {
        data() {
            return {
                isDirty: false,
                validations: new Map(),
                busy: false,
                item: {}
            }
        },

        computed: {
            formIsValid() {
                const vm = this;

                const validations = new Map();

                if (!vm.taskType) {
                    validations.set('taskType', 'Task type is required.');
                }

                if (!vm.dateToComplete) {
                    validations.set('dateToComplete', 'Date to complete is required.');
                }

                if (!vm.title) {
                    validations.set('title', 'Title is required.');
                }

                if (vm.taskItems.length <= 0) {
                    validations.set('taskItems', 'At least 1 task item is required.');
                }

                vm.isDirty = true;
                vm.validations = validations;

                return validations.size == 0;
            }
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

            resetItem() {
                const vm = this;

                vm.item = {};

                vm.isDirty = false;
                vm.validations.clear();
            },

            async open(item) {
                const vm = this;

                vm.resetItem();

                vm.item = vm.$util.clone(item);

                vm.$refs.modal.show();
            },

            close() {
                const vm = this;

                vm.$refs.modal.hide();
            },

            async save() {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    const item = vm.item;
                    const payload = {
                        firstName: item.firstName,
                        middleName: item.middleName,
                        lastName: item.lastName,
                    };

                    await vm.$util.axios.put(`/api/accounts/`, payload)
                        .then(resp => {
                            vm.$bvToast.toast('Information updated.', { title: 'Update Information', variant: 'success', toaster: 'b-toaster-bottom-right' });

                            vm.$emit('saved');
                            vm.close();
                        })
                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false;
                }
            },

        }
    }
</script>