<template>
    <div v-cloak>
        <div class="p-1 text-right">
            <button v-if="viewMode" @click="edit" v-b-toggle.collapse-1 class="btn btn-warning">
                <i class="fas fa-fw fa-edit"></i>
            </button>
            <button v-if="editMode" @click="save" v-bind:disabled="isDirty && !formIsValid" class="btn btn-primary">
                <i class="fas fa-fw fa-save"></i>
            </button>
            <button v-if="editMode" @click="editMode=false" class="btn btn-secondary">
                <i class="fas fa-fw fa-times"></i>
            </button>
        </div>
        <div class="p-2">
            <b-collapse :visible="viewMode">
                <div class="form-row">
                    <div class="form-group col-md">
                        <label>Status</label>
                        <div class="form-control-plaintext">
                            {{item.systemInformation.statusText}}
                        </div>
                    </div>
                    <div class="form-group col-md">
                        <label for="rating">Rating</label>
                        <div>
                            <b-form-rating inline no-border v-model="item.systemInformation.rating" id="rating" readonly></b-form-rating>
                        </div>
                    </div>
                    <div class="form-group col-md">
                        <label>Referral Source</label>
                        <div class="form-control-plaintext">
                            {{item.systemInformation.referralSource}}
                        </div>
                    </div>
                    <div class="form-group col-md">
                        <label>Date Of Initial Contact</label>
                        <div class="form-control-plaintext">
                            {{item.systemInformation.dateOfInitialContact|moment('calendar')}}
                        </div>
                    </div>
                </div>

                <div class="form-row">
                    <div class="form-group col-md-4">
                        <label for="assignedTo">Assigned To</label>
                        <div class="form-control-plaintext">
                            {{item.systemInformation.assignedTo}}
                            <button @click="assignContact()" class="btn btn-sm btn-outline-primary">
                                Assign
                            </button>
                        </div>
                        
                    </div>

                    <div v-if="item.systemInformation.assignedTo!==item.systemInformation.createdBy" class="form-group col-md-4">
                        <label for="createdBy">Created By</label>
                        <div class="form-control-plaintext">
                            {{item.systemInformation.createdBy}}
                        </div>
                    </div>
                    <div class="form-group col-md-4">
                        <label for="dateCreated">Date Created</label>
                        <div class="form-control-plaintext">
                            {{item.systemInformation.dateCreated|moment('calendar')}}
                        </div>
                    </div>
                    <div v-if="item.systemInformation.dateCreated!==item.systemInformation.dateUpdated" class="form-group col-md-4">
                        <label for="dateUpdated">Date Updated</label>
                        <div class="form-control-plaintext">
                            {{item.systemInformation.dateUpdated|moment('calendar')}}
                        </div>
                    </div>
                </div>
            </b-collapse>

            <b-collapse :visible="editMode">
                <div class="form-row">
                    <div class="form-group col-md">
                        <label for="status">Status</label>
                        <b-form-select v-model="itemEdit.status" id="status" :options="lookups.status"></b-form-select>
                    </div>
                    <div class="form-group col-md">
                        <label for="ratingEdit">Rating</label>
                        <div>
                            <b-form-rating inline v-model="itemEdit.rating" id="ratingEdit"></b-form-rating>
                        </div>
                    </div>
                    <div class="form-group col-md">
                        <label for="referralSourceEdit">Referral Source</label>
                        <input v-model="itemEdit.referralSource" type="text" id="referralSourceEdit" class="form-control" />
                    </div>

                    <div class="form-group col-md">
                        <label for="dateOfInitialContactEdit">Date Of Initial Contact</label>
                        <input v-model="itemEdit.dateOfInitialContact" type="date" id="dateOfInitialContactEdit" class="form-control" />

                    </div>

                </div>


            </b-collapse>
        </div>

        <modal-assign-contact ref="modalAssignContact" @saved="$emit('updated')"></modal-assign-contact>
    </div>
</template>
<script>
    import modalAssignContact from '../../../Modals/Contacts/assign.vue';
    export default {
        components: {
            modalAssignContact
        },
        props: {
            uid: String,
            item: Object
        },

        data() {
            return {
                isDirty: false,
                validations: new Map(),
                lookups: {
                    status: [
                        { value: 1, text: 'Lead' },
                        { value: 2, text: 'Opportunity' },
                        { value: 3, text: 'Customer' },
                        { value: 4, text: 'Archive' },
                    ]
                },
                editMode: false,
                itemEdit: {
                    status: null,
                    referralSource: null,
                    dateOfInitialContact: null,
                    rating: null
                }
            };
        },

        computed: {
            viewMode() {
                return !this.editMode;
            },
            formIsValid() {
                const vm = this;

                //if (!vm.isDirty)
                //    return true;

                const item = vm.item;

                const validations = new Map();

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
        },

        methods: {
            edit() {
                const vm = this;
                const itemEdit = vm.itemEdit;
                const item = vm.item.systemInformation;

                itemEdit.status = item.status;
                itemEdit.referralSource = item.referralSource;
                itemEdit.dateOfInitialContact = moment(item.dateOfInitialContact).format('YYYY-MM-DD');
                itemEdit.rating = item.rating;

                vm.editMode = true;
            },
            async save() {
                const vm = this;

                try {
                    const payload = vm.$util.clone(vm.itemEdit);
                    payload.contactId = vm.item.contactId;
                    payload.token = vm.item.token;

                    await vm.$util.axios.put(`/api/members/contacts/edit-system-information`, payload)
                        .then(resp => {
                            vm.$bvToast.toast('System information updated.', { title: 'Update System Information', variant: 'success', toaster: 'b-toaster-bottom-right' });
                            vm.$emit('updated');
                        });

                    vm.editMode = false;
                } catch (e) {
                    vm.$util.handleError(e);
                }

            },

            async assignContact() {
                const vm = this;
                const item = vm.item;
                const payload = {
                    contactId: item.contactId,
                    firstName: item.firstName,
                    middleName: item.middleName,
                    lastName: item.lastName,
                    statusText: item.systemInformation.statusText,
                    rating: item.systemInformation.rating,
                    token: item.token,
                    assignedToId: item.systemInformation.assignedToId,
                    assignedTo: item.systemInformation.assignedTo
                };
                
                vm.$refs.modalAssignContact.open(payload);
            },
        }
    }

</script>