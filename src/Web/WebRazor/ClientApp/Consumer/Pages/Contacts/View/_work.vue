<style scoped>
    label {
        font-size: small;
    }
</style>
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
                        <label>Job Title</label>
                        <div class="form-control-plaintext">
                            {{item.title}}
                        </div>
                    </div>
                    <div class="form-group col-md">
                        <label >Company</label>
                        <div class="form-control-plaintext">
                            {{item.company}}
                        </div>
                    </div>
                    <div class="form-group col-md">
                        <label>Industry</label>
                        <div class="form-control-plaintext">
                            {{item.industry}}
                        </div>
                    </div>
                    <div class="form-group col-md">
                        <label>Annual Revenue</label>
                        <div class="form-control-plaintext">
                            {{item.annualRevenue|toCurrency}}
                        </div>
                    </div>
                </div>


            </b-collapse>

            <b-collapse :visible="editMode">
                <div class="form-row">
                    <div class="form-group col-md">
                        <label for="titleEdit">Job Title</label>
                        <input v-model="itemEdit.title" type="text" id="titleEdit" class="form-control" />
                    </div>
                    <div class="form-group col-md">
                        <label for="companyEdit">Company</label>
                        <input v-model="itemEdit.company" type="text" id="companyEdit" class="form-control" />
                    </div>
                    <div class="form-group col-md">
                        <label for="industryEdit">Industry</label>
                        <input v-model="itemEdit.industry" type="text" id="industryEdit" class="form-control" />
                    </div>
                    <div class="form-group col-md">
                        <label for="annualRevenueEdit">Annual Revenue</label>
                        <input v-model="itemEdit.annualRevenue" type="number" min="0" id="annualRevenueEdit" class="form-control" />
                    </div>
                </div>
            </b-collapse>
        </div>
    </div>
</template>
<script>
    export default {

        props: {
            uid: String,
            item: Object
        },

        data() {
            return {
                isDirty: false,
                validations: new Map(),
                editMode: false,
                itemEdit: {
                    title: null,
                    company: null,
                    industry: null,
                    annualRevenue: null
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
                const item = vm.item;

                itemEdit.title = item.title;
                itemEdit.company = item.company;
                itemEdit.industry = item.industry;
                itemEdit.annualRevenue = item.annualRevenue;


                vm.editMode = true;
            },
            async save() {
                const vm = this;

                try {
                    const payload = vm.$util.clone(vm.itemEdit);
                    payload.contactId = vm.item.contactId;
                    payload.token = vm.item.token;

                    await vm.$util.axios.put(`/api/members/contacts/edit-work-information`, payload)
                        .then(resp => {
                            vm.$bvToast.toast('Work information updated.', { title: 'Update Work Information', variant: 'success', toaster: 'b-toaster-bottom-right' });
                            vm.$emit('updated');
                        });

                    vm.editMode = false;
                } catch (e) {
                    vm.$util.handleError(e);
                }

            }
        }
    }
</script>