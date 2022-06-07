<style scoped>
</style>
<template>
    <div v-cloak>
        <div class="row align-items-center">
            <div class="col-sm">
                <h1 class="h3 mb-sm-0">
                    <i class="fas fa-fw fa-upload me-1"></i>Scan Reader
                </h1>
            </div>
            <div class="col-sm-auto d-none">
                <div class="d-flex flex-row">
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
            <br />
            <br />
            <button @click="getPossibleValues">Read Meter</button>
        </div>

        <div class="table-responsive mt-3">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>Reading</th>
                        <th>kWH</th>
                        <th>Amount</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="itm in possibleValues" @click="setSelected(itm)">
                        <td>{{itm.item}}</td>
                        <td>
                            <template v-if="selected === itm">
                                x {{kiloWattPerHour}}
                            </template>
                        </td>
                        <td>
                            <template v-if="selected === itm">
                                {{selected.item * kiloWattPerHour}}
                            </template>
                        </td>
                    </tr>
                </tbody>
            </table>
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

                selected: null,
                possibleValues: [],
                kiloWattPerHour: 100
            };
        },

        computed: {
            formIsValid() {
                const vm = this;

                //if (!vm.isDirty)
                //    return true;

                const item = vm.item;

                const validations = new Map();

                //if (!vm.name) {
                //    validations.set('name', 'Image name is required.');
                //}

                //if (!vm.imageFilesValid) {
                //    validations.set('imageFiles', 'Image file is required.');
                //}

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

            async setSelected(item) {
                const vm = this;

                if (vm.selected == item)
                    item.selected = !item.selected;
                else {
                    if (vm.selected !== null)
                        vm.selected.selected = false;

                    vm.selected = item;
                    item.selected = true;
                }

            },

            async getPossibleValues() {
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


                    await vm.$util.axios.post(`/api/reader/upload-image/`, formData)
                        .then(resp => {

                            const items = resp.data.map((itm, i) =>
                            ({
                                item: itm,
                                selected: false
                            }));

                            vm.possibleValues = items;
                        });

                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false
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


                    await vm.$util.axios.post(`/api/reader/upload-image/`, formData)
                        .then(resp => {
                            debugger;
                            vm.possibleValues = resp.data;
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
