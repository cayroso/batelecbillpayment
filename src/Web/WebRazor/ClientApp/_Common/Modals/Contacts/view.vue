<style scoped>
    label {
        font-size: small;
        font-weight: bold;
        margin-bottom: 0;
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
                        View Contact
                    </h5>
                </div>
            </div>
        </template>
        <template v-slot:modal-footer>
            <a :href="`${baseViewUrl}/${contactId}`" class="btn btn-primary">
                View Details
            </a>
            <button @click="close" class="btn btn-secondary">
                Close
            </button>
        </template>
        <div>
            <div class="form-row">
                <div class="form-group col">
                    <label>Salutation</label>
                    <div class="form-control-plaintext">
                        {{item.salutationText}}
                    </div>
                </div>

                <div class="form-group col">
                    <label>First Name</label>
                    <div class="form-control-plaintext">
                        {{item.firstName}}
                    </div>
                </div>
                <div class="form-group col">
                    <label>Middle Name</label>
                    <div class="form-control-plaintext">
                        {{item.middleName}}
                    </div>
                </div>
                <div class="form-group col">
                    <label>Last Name</label>
                    <div class="form-control-plaintext">
                        {{item.lastName}}
                    </div>
                </div>
            </div>
            <hr class="d-md-none" />

            <div class="form-row">
                <div class="form-group col">
                    <label>Home Phone</label>
                    <div class="form-control-plaintext">
                        {{item.homePhone}}
                    </div>
                </div>
                <div class="form-group col">
                    <label>Mobile Phone</label>
                    <div class="form-control-plaintext">
                        {{item.mobilePhone}}
                    </div>
                </div>
                <div class="form-group col">
                    <label>Business Phone</label>
                    <div class="form-control-plaintext">
                        {{item.businessPhone}}
                    </div>

                </div>
                <div class="form-group col">
                    <label>Fax</label>
                    <div class="form-control-plaintext">
                        {{item.fax}}
                    </div>
                </div>
            </div>

            <hr class="d-md-none" />

            <div class="form-row">
                <div class="form-group col col-md-4">
                    <label>Email</label>
                    <div class="form-control-plaintext">
                        {{item.email}}
                    </div>
                </div>
                <div class="form-group col col-md-5">
                    <label>Website</label>
                    <div class="form-control-plaintext">
                        {{item.website}}
                    </div>
                </div>
            </div>

            <hr class="d-md-none" />

            <div class="form-row">
                <div class="form-group">
                    <label>Address</label>
                    <div class="form-control-plaintext"  style="white-space: pre-line;" >
                        {{item.address}}
                    </div>
                </div>
            </div>

        </div>
    </b-modal>
</template>
<script>
    export default {
        props: {
            baseViewUrl: {type:String, required: true},
        },
        data() {
            return {
                moment: moment,

                busy: false,
                contactId: null,
                item: {
                }
            }
        },
        methods: {

            async open(contactId) {
                const vm = this;

                if (vm.contactId !== contactId) {
                    vm.contactId = contactId;

                    await vm.get();
                }

                vm.$refs.modal.show();
            },

            close() {
                const vm = this;

                vm.$refs.modal.hide();
            },

            async get() {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    await vm.$util.axios.get(`/api/contacts/${vm.contactId}`)
                        .then(resp => {
                            vm.item = resp.data;

                            //vm.item.taskItems.forEach(ti => {
                            //    ti.dateCompleted = moment(ti.dateCompleted);
                            //});

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