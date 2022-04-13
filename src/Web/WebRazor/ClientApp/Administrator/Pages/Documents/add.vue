<style scoped>
    label {
        font-size: small;
    }
</style>
<template>
    <div v-cloak>
        <div class="d-flex flex-column flex-sm-row justify-content-sm-between">
            <h1 class="h3 mb-sm-0">
                <i class="fas fa-fw fa-book mr-1"></i>Add Document
            </h1>
            <div class="text-right">
                <b-overlay :show="busy">
                    <button v-bind:disabled="isDirty && !formIsValid" @click="save" class="btn btn-primary">
                        <span class="fas fa-fw fa-save"></span>
                    </button>
                    <button @click="close" class="btn btn-secondary">
                        <span class="fas fa-fw fa-times-circle"></span>
                    </button>
                </b-overlay>
            </div>
        </div>
        <div class="mt-2">
            <b-overlay :show="busy">
                <div class="form-group">
                    <label for="name">Name</label>
                    <div>
                        <input v-model="name" type="text" id="name" class="form-control" v-bind:class="getValidClass('name')" />
                        <div v-if="validations.has('name')" class="invalid-feedback">
                            {{validations.get('name')}}
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <label for="description">Description</label>
                    <div>
                        <textarea v-model="description" type="text" id="description" class="form-control"></textarea>
                    </div>
                </div>

                <hr />

                <div class="form-group">
                    <label for="title">Document</label>
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
                        <div v-if="validations.has('imageFiles')" class="invalid-feedback d-block">
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
                </div>
            </b-overlay>
        </div>
    </div>
</template>
<script>
    import pageMixin from '../../../_Core/Mixins/pageMixin';

    export default {
        mixins: [pageMixin],

        data() {
            return {
                isDirty: false,
                validations: new Map(),
                imageFiles: null,
                imageFileUrls: null,
                name: null,
                description: null
            }
        },
        computed: {
            formIsValid() {
                const vm = this;

                //if (!vm.isDirty)
                //    return true;

                const item = vm.item;

                const validations = new Map();

                if (!vm.name) {
                    validations.set('name', 'Document name is required.');
                }
                
                if (!vm.imageFilesValid) {
                    validations.set('imageFiles', 'Document file is required.');                
                }

                vm.isDirty = true;
                vm.validations = validations;

                return validations.size == 0;
            },
            imageFilesValid() {
                const vm = this;

                if (Array.isArray(vm.imageFiles))
                    return vm.imageFiles.length > 0;
                else
                    return vm.imageFiles != null;
            },
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


            async save() {
                const vm = this;
                
                if (vm.busy)
                    return;

                if (!vm.formIsValid)
                    return;

                try {
                    vm.busy = true;

                    const obj = {
                        name: vm.name,
                        description: vm.description
                    };

                    const json = JSON.stringify(obj);
                    const blob = new Blob([json], {
                        type: 'application/json'
                    });

                    const formData = new FormData();

                    formData.append('info', blob);

                    if (Array.isArray(vm.imageFiles)) {
                        vm.imageFiles.forEach(file => {
                            formData.append('files', file);
                        });
                    }
                    else {
                        formData.append('files', vm.imageFiles);
                    }

                    await vm.$util.axios.post(`/api/members/documents`, formData)
                        .then(resp => {
                            vm.$bvToast.toast('Document added.', { title: 'Add Document', variant: 'success', toaster: 'b-toaster-bottom-right' });
                        });

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