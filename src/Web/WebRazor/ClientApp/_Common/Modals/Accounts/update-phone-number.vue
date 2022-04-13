<template>
    <b-modal ref="modal"
             :no-close-on-esc="false"
             :no-close-on-backdrop="true"
             scrollable>
        <template v-slot:modal-header>
            <div class="w-100">
                <div class="d-flex flex-row  align-items-center justify-content-between">
                    <h5 class="m-0">
                        Update Phone Number
                    </h5>
                </div>
            </div>
        </template>
        <template v-slot:modal-footer>
            <button @click="save" class="btn btn-primary">
                Save
            </button>
            <button @click="close" class="btn btn-secondary">
                Close
            </button>
        </template>
        <div>
            <div class="form-group">
                <label>Phone Number</label>
                <input v-model="phoneNumber" type="tel" class="form-control" />
            </div>
        </div>
    </b-modal>
</template>
<script>
    export default {
        data() {
            return {
                moment: moment,

                busy: false,
                accountId: null,
                phoneNumber: null
            }
        },
        methods: {

            async open(item) {
                const vm = this;

                vm.accountId = item.userId;
                vm.phoneNumber = item.phoneNumber;

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
                    const payload = {
                        accountId: vm.accountId,
                        phoneNumber: vm.phoneNumber
                    };

                    await vm.$util.axios.post(`/api/accounts/updatePhoneNumber`, payload)
                        .then(resp => {
                            alert("Phone Number updated.");
                        });
                    vm.$emit('saved');

                    vm.close();
                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false;
                }
            },

        }
    }
</script>