<template>
    <b-modal ref="modal"
             :no-close-on-esc="false"
             :no-close-on-backdrop="true"
             scrollable>
        <template v-slot:modal-header>
            <div class="w-100">
                <div class="d-flex flex-row  align-items-center justify-content-between">
                    <h5 class="m-0">
                        Upload Profile Picture
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
            <div class="d-flex flex-row">
                <b-form-file accept="*/*"
                             v-model="imageFiles"
                             @input="onImageFileChange"
                             :state="imageFilesValid"
                             placeholder="Choose a file or drop it here..."
                             :capture="Boolean(0)"
                             :multiple="Boolean(0)"
                             drop-placeholder="Drop file here...">
                </b-form-file>


                <div v-if="imageFileUrls" class="ml-2">
                    <button @click="clearImageFile(0)" class="btn btn-danger">
                        <span class="fas fa-fw fa-trash"></span>
                    </button>
                </div>
            </div>
            <div v-if="validations.get('imageFiles')" class="text-danger">
                {{validations.get('imageFiles')}}
            </div>
            <div v-if="imageFileUrls" class="mt-2">
                <div v-for="(imageFileUrl,index) in imageFileUrls">
                    <div>
                        <b-img-lazy v-if="imageFiles.type.startsWith('image/')" :src="imageFileUrl" fluid center></b-img-lazy>

                    </div>
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

                imageFiles: null,
                imageFileUrls: null,

                item: {}
            }
        },

        computed: {
            imageFilesValid() {
                const vm = this;

                if (Array.isArray(vm.imageFiles))
                    return vm.imageFiles.length > 0;
                else
                    return vm.imageFiles != null;
            },

            formIsValid() {
                const vm = this;

                const validations = new Map();

                if (!vm.imageFilesValid) {
                    validations.set('imageFiles', 'File is required.');
                }

                vm.isDirty = true;
                vm.validations = validations;

                return validations.size == 0;
            }
        },

        methods: {
            onImageFileChange(e) {
                const vm = this;

                var files = [];

                if (Array.isArray(e)) {
                    files = e;
                }
                else {
                    files = [e];
                }

                vm.imageFileUrls = [];

                for (var i = 0; i < files.length; i++) {
                    const file = files[i];
                    if (file)
                        vm.imageFileUrls.push(URL.createObjectURL(file));
                }
            },

            clearImageFile(index) {
                const vm = this;

                if (Array.isArray(vm.imageFiles)) {
                    vm.imageFiles.splice(index, 1);
                    vm.imageFileUrls.splice(index, 1);
                }
                else {
                    vm.imageFiles = null;
                    vm.imageFileUrls = null;
                }
            },

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
                vm.imageFiles = null;
                vm.imageFileUrls = null;
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

                if (!vm.formIsValid)
                    return;

                try {
                    const formData = new FormData();

                    if (Array.isArray(vm.imageFiles)) {
                        vm.imageFiles.forEach(file => {
                            formData.append('files', file);
                        });
                    }
                    else {
                        formData.append('files', vm.imageFiles);
                    }

                    await vm.$util.axios.post(`/api/accounts/profile-picture/`, formData)
                        .then(resp => {
                            vm.$bvToast.toast('Profile picture updated.', { title: 'Update Profile Picture', variant: 'success', toaster: 'b-toaster-bottom-right' });                            
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