<style scoped>
    label {
        font-size: small;
        font-weight: bold;
    }
</style>
<template>
    <b-overlay :show="busy" v-cloak>
        <div class="p-1 text-right">


            <template v-if="isAssigned">
                <action-member :uid="uid" 
                               :item="item" 
                               :roleId="roleId" 
                               :viewMode="viewMode" 
                               :editMode="editMode"
                               :canSave="isDirty || formIsValid"
                               @get="get" 
                               @close="close"
                               @edit="edit"
                               @save="save"
                               @cancel="editMode=false"
                               ></action-member>
            </template>
        </div>
        <div class="p-2">
            <b-collapse :visible="viewMode">
                <div class="form-row">
                    <div class="form-group col-md">
                        <label>Salutation</label>
                        <div class="form-control-plaintext">
                            {{item.salutationText}}
                        </div>
                    </div>

                    <div class="form-group col-md">
                        <label>First Name</label>
                        <div class="form-control-plaintext">
                            {{item.firstName}}
                        </div>
                    </div>
                    <div class="form-group col-md">
                        <label>Middle Name</label>
                        <div class="form-control-plaintext">
                            {{item.middleName}}
                        </div>
                    </div>
                    <div class="form-group col-md">
                        <label>Last Name</label>
                        <div class="form-control-plaintext">
                            {{item.lastName}}
                        </div>
                    </div>
                </div>

                <hr class="d-md-none" />

                <div class="form-row">
                    <div class="form-group col-md">
                        <label>Home Phone</label>
                        <div class="form-control-plaintext">
                            {{item.homePhone}}
                        </div>
                    </div>
                    <div class="form-group col-md">
                        <label>Mobile Phone</label>
                        <div class="form-control-plaintext">
                            {{item.mobilePhone}}
                        </div>
                    </div>
                    <div class="form-group col-md">
                        <label>Business Phone</label>
                        <div class="form-control-plaintext">
                            {{item.businessPhone}}
                        </div>

                    </div>
                    <div class="form-group col-md">
                        <label>Fax</label>
                        <div class="form-control-plaintext">
                            {{item.fax}}
                        </div>
                    </div>
                </div>

                <hr class="d-md-none" />

                <div class="form-row">
                    <div class="form-group col-md-4">
                        <label>Email</label>
                        <div class="form-control-plaintext">
                            {{item.email}}
                        </div>
                    </div>
                    <div class="form-group col-md-5">
                        <label>Website</label>
                        <div class="form-control-plaintext">
                            {{item.website}}
                        </div>
                    </div>
                </div>

                <hr class="d-md-none" />

                <div class="form-row">
                    <div class="form-group col-md">
                        <label>Address</label>
                        <div class="form-control-plaintext">
                            {{item.address}}
                        </div>
                    </div>
                </div>


            </b-collapse>

            <b-collapse :visible="editMode">

                <div class="form-row">
                    <div class="form-group col-md">
                        <label for="salutationEdit">Salutation</label>
                        <b-form-select v-model="itemEdit.salutation" id="salutationEdit" :options="lookups.salutations"></b-form-select>
                    </div>

                    <div class="form-group col-md">
                        <label for="firstNameEdit">First Name</label>
                        <input v-model="itemEdit.firstName" type="text" id="firstNameEdit" class="form-control" />
                    </div>
                    <div class="form-group col-md">
                        <label for="middleNameEdit">Middle Name</label>
                        <input v-model="itemEdit.middleName" type="text" id="middleNameEdit" class="form-control" />
                    </div>
                    <div class="form-group col-md">
                        <label for="lastNameEdit">Last Name</label>
                        <input v-model="itemEdit.lastName" type="text" id="lastNameEdit" class="form-control" />
                    </div>
                </div>

                <hr class="d-md-none" />

                <div class="form-row">
                    <div class="form-group col-md">
                        <label for="homePhoneEdit">Home Phone</label>
                        <input v-model="itemEdit.homePhone" type="tel" id="homePhoneEdit" class="form-control" />
                    </div>
                    <div class="form-group col-md">
                        <label for="mobilePhoneEdit">Mobile Phone</label>
                        <input v-model="itemEdit.mobilePhone" type="tel" id="mobilePhoneEdit" class="form-control" />
                    </div>
                    <div class="form-group col-md">
                        <label for="businessPhoneEdit">Business Phone</label>
                        <input v-model="itemEdit.businessPhone" type="tel" id="businessPhoneEdit" class="form-control" />

                    </div>
                    <div class="form-group col-md">
                        <label for="faxEdit">Fax</label>
                        <input v-model="itemEdit.fax" type="tel" id="faxEdit" class="form-control" />
                    </div>
                </div>

                <hr class="d-md-none" />

                <div class="form-row">
                    <div class="form-group col-md-4">
                        <label for="emailEdit">Email</label>
                        <input v-model="itemEdit.email" type="email" id="emailEdit" class="form-control" />
                    </div>
                    <div class="form-group col-md-5">
                        <label for="websiteEdit">Website</label>
                        <input v-model="itemEdit.website" type="text" id="websiteEdit" class="form-control" />
                    </div>
                </div>

                <hr class="d-md-none" />

                <div class="form-row">
                    <div class="form-group col-md">
                        <label for="addressEdit">Address</label>
                        <textarea v-model="itemEdit.address" rows="3" id="addressEdit" class="form-control"></textarea>
                    </div>
                </div>

            </b-collapse>


        </div>
    </b-overlay>
</template>
<script>
    import ActionMember from './_action-member.vue';

    export default {
        components: {
            ActionMember
        },
        props: {
            uid: {
                type: String, required: true
            },
            roleId: {
                type: String, required: true
            },
            item: {
                type: Object, required: true
            },
            busy: {
                type: Boolean, required: true
            }
        },

        data() {
            return {
                isDirty: false,
                validations: new Map(),
                lookups: {
                    salutations: [
                        { value: 1, text: 'Mr' },
                        { value: 2, text: 'Mrs' },
                        { value: 3, text: 'Ms' },
                    ]
                },
                editMode: false,
                itemEdit: {
                    salutation: null,
                    firstName: null,
                    middleName: null,
                    lastName: null,

                    homePhone: null,
                    mobilePhone: null,
                    businessPhone: null,
                    fax: null,

                    email: null,
                    website: null,
                    address: null,
                }
            };
        },

        computed: {
            isMember() {
                const vm = this;

                return vm.roleId === 'member';
            },
            isAssigned() {
                const vm = this;
                const item = vm.item;

                if (vm.isMember && item.systemInformation.assignedToId == vm.uid)
                    return true;
                return false;
            },
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
            get() {
                this.$emit('get');
            },
            close() {
                this.$emit('close');
            },
            edit() {
                const vm = this;
                const itemEdit = vm.itemEdit;
                const item = vm.item;

                itemEdit.salutation = item.salutation;
                itemEdit.firstName = item.firstName;
                itemEdit.middleName = item.middleName;
                itemEdit.lastName = item.lastName;

                itemEdit.homePhone = item.homePhone;
                itemEdit.mobilePhone = item.mobilePhone;
                itemEdit.businessPhone = item.businessPhone;
                itemEdit.fax = item.fax;

                itemEdit.email = item.email;
                itemEdit.website = item.website;
                itemEdit.address = item.address;


                vm.editMode = true;
            },
            async save() {
                const vm = this;

                try {
                    const payload = vm.$util.clone(vm.itemEdit);
                    payload.contactId = vm.item.contactId;
                    payload.token = vm.item.token;

                    await vm.$util.axios.put(`/api/members/contacts/edit-contact-information`, payload)
                        .then(resp => {
                            vm.$bvToast.toast('Contact information updated.', { title: 'Update Contact Information', variant: 'success', toaster: 'b-toaster-bottom-right' });
                            vm.$emit('updated');
                        });

                    vm.editMode = false;

                    vm.get();
                } catch (e) {
                    vm.$util.handleError(e);
                }

            }
        }
    }

</script>