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

        <div class="mt-2">
            <div class="card">
                <div class="card-header">
                    Step 1. Select Image File
                </div>
                <div class="card-body">
                    <input type="file" id="fileInput" @change="onImageFileChange" accept="image" placeholder="Upload Billings" />

                    <br />

                    <div v-if="imageFileUrls" class="mt-2">
                        <div v-for="(imageFileUrl,index) in imageFileUrls">
                            <div>
                                <img :src="imageFileUrl" class="img-fluid img-thumbnail" style="width:300px;">


                                <div class="mt-2">
                                    <button @click="clearImageFile(index)" class="btn btn-sm btn-outline-danger align-middle">
                                        <span class="fas fa-fw fa-trash"></span>
                                    </button>
                                </div>

                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div v-if="imageFiles !== null" class="mt-2">
            <div class="card">
                <div class="card-header">
                    Step 2. Enter Previous Reading
                </div>
                <div class="card-body">
                    <input v-model="previousReading" type="number" class="form-control" />
                </div>
            </div>

        </div>

        <div v-if="previousReading > 0" class="mt-2">

            <button @click="getPossibleValues" class="btn btn-primary">Read Meter</button>
        </div>

        <div v-if="previousReading > 0" class="table-responsive mt-3">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>Account Number</th>
                        <th>Customer</th>
                        <th>Meter Number</th>
                        <th>Mobile Number</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>
                            {{accountNumber}}
                        </td>
                        <td>
                            {{customer}}
                        </td>
                        <td>
                            {{meterNumber}}
                        </td>
                        <td>
                            {{phoneNumber}}
                        </td>
                    </tr>
                </tbody>
            </table>

            <table class="table table-bordered mt-2">
                <thead>
                    <tr>
                        <th>Previous Reading</th>
                        <th>Current Reading</th>
                        <th>kW Used * kWH</th>
                        <th>Amount</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(itm,index) in possibleValues" @click="setSelected(itm)">
                        <th v-if="index===0" :rowspan="possibleValues.length">
                            {{previousReading}}
                        </th>
                        <td :class="(selected==itm) ? 'text-primary':''">
                            <span v-if="selected===itm" class="fas fa-fw fa-check me-2"></span>{{itm.item}}
                        </td>
                        <th v-if="index===0" :rowspan="possibleValues.length">
                            <span v-if="selected">
                                {{(selected.item-previousReading)}} * {{kiloWattPerHour}}
                            </span>

                        </th>
                        <th v-if="index===0" :rowspan="possibleValues.length">
                            <span v-if="selected">
                                {{(selected.item-previousReading)*kiloWattPerHour}} php

                                <div>
                                    <button @click="uploadReading" class="btn btn-primary">Upload</button>
                                </div>
                            </span>
                        </th>
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
                kiloWattPerHour: 15,
                previousReading: 0,

                accountNumber: '',
                customer: '',
                meterNumber: '',
                phoneNumber: '',
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
                            var data = resp.data;
                            vm.accountNumber = data.accountNumber;
                            vm.meterNumber = data.meterNumber;
                            vm.phoneNumber = data.phoneNumber;
                            vm.customer = data.customer;
                            debugger;
                            const items = data.items.map((itm, i) =>
                            ({
                                item: itm,
                                selected: false
                            }));
                            vm.possibleValues = items;
                        });
                } catch (e) {
                    var err = vm.$util.getErrorMessageAsHtml(e);
                    alert(err);
                    //vm.$util.handleError(e);
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
                            //var data = resp.data
                            //vm.possibleValues = resp.data.items;
                        });
                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false
                }
            },

            async uploadReading() {
                const vm = this;
                if (vm.busy)
                    return;
                try {

                    await vm.$util.axios.post(`/api/reader/add-reading/${vm.selected.item}`)
                        .then(resp => {
                            alert('Meter Reading successfully uploaded.')
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
