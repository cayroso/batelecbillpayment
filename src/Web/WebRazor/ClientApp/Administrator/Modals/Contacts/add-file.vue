
<template>
    <b-modal ref="modal"
             :no-close-on-esc="false"
             :no-close-on-backdrop="true"
             scrollable>
        <template v-slot:modal-header>
            <div class="d-sm-flex align-items-center justify-content-between">
                <h1 class="h5 m-0">
                    <span class="text-capitalize">Add File</span>
                </h1>
            </div>
        </template>
        <template v-slot:modal-footer>
            <button @click="save" v-bind:disable="busy" class="btn btn-primary">
                Save
            </button>
            <button @click="close" class="btn btn-secondary">
                Close
            </button>
        </template>
        <b-overlay :show="busy">
            <div class="form-group">
                <label for="title">Link</label>
                <div>
                    <div v-bind:disabled="imageFileUrls===null || imageFileUrls.length<=0">
                        <div class="d-flex flex-row mb-2">
                            <div class="w-100">
                                <input v-model="imageLink" type="text" class="form-control" placeholder="Enter image link" />
                                <input v-model="imageName" type="text" class="form-control mt-1" placeholder="Enter image Name" />
                            </div>
                            <div v-if="imageLink" class="ml-2">
                                <button @click="imageLink=null" class="btn btn-danger">
                                    <span class="fas fa-fw fa-trash"></span>
                                </button>
                            </div>
                        </div>
                        <b-img-lazy v-if="imageLink" :src="imageLink" fluid center></b-img-lazy>


                    </div>
                </div>
            </div>
            <div class="text-center">
                Or
            </div>
            <div class="form-group">
                <label for="title">File</label>
                <div v-if="imageLink===null">
                    <div class="d-flex flex-row">
                        <b-form-file accept="*/*"
                                     v-model="imageFiles"
                                     @input="onImageFileChange"
                                     :state="imageFilesValid"
                                     placeholder="Choose a file or drop it here..."
                                     :capture="Boolean(0)"
                                     :multiple="Boolean(0)"
                                     drop-placeholder="Drop file here...">
                            <!--<template slot="file-name" slot-scope="{ files, names }">
                                {{files.length}} file(s) selected
                            </template>-->

                        </b-form-file>
                        <div v-if="imageFileUrls" class="ml-2">
                            <button @click="clearImageFile(0)" class="btn btn-danger">
                                <span class="fas fa-fw fa-trash"></span>
                            </button>
                        </div>
                    </div>
                    <div v-if="imageFileUrls" class="mt-2">
                        <div v-for="(imageFileUrl,index) in imageFileUrls">
                            <div>
                                <b-img-lazy v-if="imageFiles.type.startsWith('image/')" :src="imageFileUrl" fluid center></b-img-lazy>
                                <!--<div class="d-flex flex-row justify-content-between align-items-center mb-1">
                                    <div class="d-inline-block text-truncate">
                                        <span v-if="imageFiles[index]">{{imageFiles[index].name}}</span>
                                        <span v-else>{{imageFiles.name}}</span>
                                    </div>
                                    <div class="mt-2">
                                        <button @click="clearImageFile(index)" class="btn btn-sm btn-outline-danger align-middle">
                                            <span class="fas fa-fw fa-trash"></span>
                                        </button>
                                    </div>
                                </div>
                                <div class="text-right mt-2">
                                    <button @click="clearImageFile(index)" class="btn btn-outline-danger align-middle">
                                        <span class="fas fa-fw fa-trash"></span>
                                    </button>
                                </div>-->
                            </div>
                        </div>
                    </div>

                    <div v-if="imageLink">
                        {{imageLink}}
                    </div>

                </div>
            </div>


        </b-overlay>
    </b-modal>
</template>
<script>
    export default {
        data() {
            return {
                busy: false,
                contactId: null,
                token: null,
                imageFiles: null,
                imageFileUrls: null,
                imageName: null,
                imageLink: null,
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

            async open(id, token) {
                const vm = this;

                vm.busy = false;
                vm.contactId = id;
                vm.token = token;

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
                    vm.busy = true;

                    const obj = {
                        contactId: vm.contactId,
                        token: vm.token,
                        imageLink: vm.imageLink,
                        imageName: vm.imageName
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

                    await vm.$util.axios.post(`/api/members/contacts/add-file`, formData)
                        .then(resp => {
                            vm.$bvToast.toast('File added.', { title: 'Add File', variant: 'success', toaster: 'b-toaster-bottom-right' });
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