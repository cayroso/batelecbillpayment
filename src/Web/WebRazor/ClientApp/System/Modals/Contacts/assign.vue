
<template>
    <b-modal ref="modal"
             :no-close-on-esc="false"
             :no-close-on-backdrop="true"
             scrollable>
        <template v-slot:modal-header>
            <div class="d-sm-flex align-items-center justify-content-between">
                <h1 class="h5 m-0">
                    Assign Contact
                </h1>
            </div>
        </template>
        <template v-slot:modal-footer>
            <button @click="save(false)" class="btn btn-primary">
                Save
            </button>
            <button @click="close" class="btn btn-secondary">
                Close
            </button>
        </template>
        <div>
            <div class="form-row">
                <div class="form-group col-md">
                    <label for="firstName">Contact</label>
                    <div>
                        {{item.firstName}} {{item.middleName}} {{item.lastName}}
                    </div>
                </div>
                <div class="form-group col-md-3">
                    <label for="firstName">Status</label>
                    <div>{{item.statusText}}</div>
                </div>
                <div class="form-group col-md-3">
                    <label for="firstName">Rating</label>
                    <div>
                        <b-form-rating inline no-border size="sm" v-model="item.rating" id="rating" readonly></b-form-rating>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label for="firstName">Currently Assigned To</label>
                <div>
                    <span v-if="item.assignedTo">
                        {{item.assignedTo}}
                    </span>
                </div>
            </div>
            <div class="form-group">
                <label for="title">Member</label>
                <div>
                    <b-form-select v-model="memberId" :options="members" text-field="name" value-field="id" v-bind:class="getValidClass('memberId')"></b-form-select>
                    <span v-if="validations.get('memberId')" class="text-danger">
                        {{validations.get('memberId')}}
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
                moment: moment,
                isDirty: false,
                validations: new Map(),
                busy: false,

                item: {},
                memberId: null,
                members: []
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
            async getUsers() {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    await vm.$util.axios.get(`/api/users/lookup/`)
                        .then(resp => {
                            const users = resp.data;
                            const members = [];

                            users.forEach(u => {
                                const fnd = u.id === vm.item.assignedToId;

                                if (!fnd) {
                                    members.push(u);
                                }
                            });

                            vm.members = members;

                        })
                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false;
                }
            },
            reset() {
                const vm = this;

                vm.item = {};
                vm.memberId = null;
                vm.members = [];
            },
            async open(item) {
                const vm = this;

                vm.reset();

                vm.item = item;

                await vm.getUsers();

                vm.$refs.modal.show();
            },

            close() {
                const vm = this;

                vm.$refs.modal.hide();
            },

            async save() {
                const vm = this;

                try {
                    const item = vm.item;

                    await vm.$util.axios.put(`/api/managers/contacts/${item.contactId}/assign/${vm.memberId}/${item.token}`)
                        .then(resp => {
                            vm.$bvToast.toast('Member assigned.', { title: 'Assign Member', variant: 'success', toaster: 'b-toaster-bottom-right' });
                        });

                    vm.$emit('saved');

                    vm.close();

                } catch (e) {
                    vm.$util.handleError(e);
                }
            },
        }
    }
</script>