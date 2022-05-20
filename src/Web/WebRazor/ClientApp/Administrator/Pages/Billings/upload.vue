<style scoped>
</style>
<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fas fa-fw fa-money-bill me-1"></i>Upload Billings
                </h1>
            </div>
            <div class="col-sm-auto">
                <div class="d-flex flex-row">
                    <a href="/staticfiles/billing-upload-template-with-sample.txt" target="_top" class="btn btn-primary">
                        <span class="fas fa-fw fa-file-download"></span>Template
                    </a>
                    <a href="/staticfiles/billing-upload-template.txt" class="btn btn-primary ms-1">
                        <span class="fas fa-fw fa-file-download"></span>Template with Sample
                    </a>

                    <button v-bind:disabled="isDirty && !formIsValid" @click="save" class="btn btn-primary ms-2">
                        <span class="fas fa-fw fa-save"></span>
                    </button>
                    <button @click="close" class="ms-1 btn btn-secondary">
                        <i class="fas fa-fw fa-times"></i>
                    </button>
                </div>
            </div>
        </div>

        <div class="mt-5">
            <input type="file" id="fileInput" @change="onImageFileChange" accept="image" placeholder="Upload Billings" />
        </div>
    </div>
</template>
<script>
    import pageMixin from '../../../_Core/Mixins/pageMixin';

    export default {
        mixins: [pageMixin],

        props: {
            uid: { type: String, required: true },
            urlView: { type: String, required: true },
        },

        data() {
            return {
                isDirty: false,
                validations: new Map(),
                imageFiles: null,
                imageFileUrls: null,
            };
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

        async created() {
            const vm = this;

        },

        async mounted() {
            const vm = this;

        },

        methods: {
            onImageFileChange(e) {
                
                const vm = this;

                var files = [];

                if (Array.isArray(e.currentTarget.files)) {
                    files = e.currentTarget.files;
                }
                else {
                    files = [e.currentTarget.files[0]];
                }

                vm.imageFileUrls = [];

                for (var i = 0; i < files.length; i++) {
                    const file = files[i];
                    if (file)
                        vm.imageFileUrls.push(URL.createObjectURL(file));
                }

                vm.imageFiles = files;
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

                try {
                    vm.busy = true;

                    const formData = new FormData();

                    if (Array.isArray(vm.imageFiles)) {
                        vm.imageFiles.forEach(file => {
                            formData.append('files', file);
                        });
                    }
                    else {
                        formData.append('files', vm.imageFiles);
                    }


                    await vm.$util.axios.post(`/api/billing/bulk-upload/`, formData)
                        .then(resp => {                            
                            alert('Billings uploaded successfully');
                            //vm.$bvToast.toast('Contact created.', { title: 'Add Contact', variant: 'success', toaster: 'b-toaster-bottom-right' });

                            //const url = `${vm.urlView}/${resp.data}`;

                            //vm.$toast.success('Add New Billing',
                            //    `Billing was created.
                            //    <br/>Click <a href="${url}">here</a> to view the record.`,
                            //    {
                            //        async onClose() {
                            //            vm.close();
                            //        }
                            //    })
                            //setTimeout(() => {
                            //    const url = `${vm.urlView}/${resp.data}`;
                            //    vm.$util.href(url);
                            //    //vm.$router.push({ name: 'contactsView', params: { id: resp.data } });
                            //}, 500);
                        });

                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false
                }
            }
        }
    }
</script>
