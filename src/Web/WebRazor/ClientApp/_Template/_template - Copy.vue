<style></style>
<template>
    <div v-cloak>
        <div class="row align-items-center pb-2 border-bottom">
            <div class="col-sm">
                <h1 class="h3 mb-0">
                    <i class="fas fa-fw fa-tag mr-1"></i>Add Product
                </h1>
            </div>
            <div class="col-sm-auto text-right">
                <div class="btn-group" role="group">
                    <button v-bind:disabled="!formIsValid" id="btnGroupSave" type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <span class="fas fa-fw fa-save mr-1"></span>Save
                    </button>
                    <div class="dropdown-menu" aria-labelledby="btnGroupSave">
                        <a class="dropdown-item" href="#" @click.prevent="save(true)">Save and Add</a>
                        <a class="dropdown-item" href="#" @click.prevent="save(false)">Save and View</a>
                        <a class="dropdown-item" href="#" @click.prevent="save()">Save and Close</a>
                    </div>
                </div>
                <button @click="close" class="btn btn-secondary">
                    <span class="fas fa-fw fa-times-circle"></span>
                </button>
            </div>
        </div>



        <b-overlay :show="busy">
            <div class="mt-2">
                <div class="form-group row">
                    <label class="col-3 text-sm-right col-form-label">Image</label>
                    <div class="col col-md-6">
                        <div v-if="imageFileUrls===null || imageFileUrls.length<=0">
                            <input v-model="item.imageLink" type="text" class="form-control mb-2" placeholder="image url" />
                            <b-img-lazy v-if="item.imageLink" :src="item.imageLink" class="img-fluid img-thumbnail" style="width:128px;"></b-img-lazy>

                            <button v-if="item.imageLink" @click="item.imageLink=null" class="btn btn-sm btn-outline-danger align-middle">
                                <span class="fas fa-fw fa-trash"></span>
                            </button>
                        </div>

                        <div class="my-2"></div>

                        <div v-if="item.imageLink===null">
                            <b-form-file accept="image/*"
                                         v-model="imageFiles"
                                         @input="onImageFileChange"
                                         :state="imageFilesValid"
                                         placeholder="Choose a image or drop it here..."
                                         :capture="Boolean(0)"
                                         :multiple="Boolean(0)"
                                         drop-placeholder="Drop image here...">
                                <template slot="file-name" slot-scope="{ files, names }">
                                    {{files.length}} file(s) selected
                                </template>

                            </b-form-file>
                            <div v-if="imageFileUrls" class="mt-2">
                                <div v-for="(imageFileUrl,index) in imageFileUrls">
                                    <div>
                                        <b-img-lazy :src="imageFileUrl" class="img-fluid img-thumbnail" style="width:300px;"></b-img-lazy>
                                        <div class="d-flex flex-row justify-content-between align-items-center mb-1">
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
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-3 text-sm-right col-form-label">Item Group</label>
                    <div class="col col-md-6">
                        <b-form-select v-model="item.itemId" :options="filteredItems" v-bind:class="{'is-invalid':validations.get('itemId')}">
                        </b-form-select>
                        <span v-if="validations.has('itemId')" class="text-danger">
                            {{validations.get('itemId')}}
                        </span>
                    </div>
                </div>

                <div v-if="item.itemId===null" class="form-group row">
                    <label class="col-3 text-sm-right col-form-label">Item Group Name</label>
                    <div class="col col-md-6">
                        <input type="text" v-model="item.name" class="form-control" v-bind:class="{'is-invalid':validations.get('name')}" />
                        <span v-if="validations.has('name')" class="text-danger">
                            {{validations.get('name')}}
                        </span>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-3 text-sm-right col-form-label">Product Name</label>
                    <div class="col col-md-6">
                        <input type="text" v-model="item.variant" class="form-control" v-bind:class="{'is-invalid':validations.get('variant')}" />
                        <span v-if="validations.has('variant')" class="text-danger">
                            {{validations.get('variant')}}
                        </span>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-3 text-sm-right col-form-label">Description</label>
                    <div class="col col-md-6">
                        <textarea v-model="item.description" class="form-control"></textarea>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-3 text-sm-right col-form-label">Multi Pack</label>
                    <div class="col col-md-6">
                        <input type="number" v-model="item.quantity" class="form-control" v-bind:class="{'is-invalid':validations.get('quantity')}" />
                        <span v-if="validations.has('quantity')" class="text-danger">
                            {{validations.get('quantity')}}
                        </span>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-3 text-sm-right col-form-label">Price</label>
                    <div class="col col-md-6">
                        <input type="number" v-model="item.price" class="form-control" v-bind:class="{'is-invalid':validations.get('price')}" />
                        <span v-if="validations.has('price')" class="text-danger">
                            {{validations.get('price')}}
                        </span>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-3 text-sm-right col-form-label">Cost</label>
                    <div class="col col-md-6">
                        <input type="number" v-model="item.cogs" class="form-control" v-bind:class="{'is-invalid':validations.get('cogs')}" />
                        <span v-if="validations.has('cogs')" class="text-danger">
                            {{validations.get('cogs')}}
                        </span>
                    </div>
                </div>

                <div class="form-group row">
                    <label class="col-3 text-sm-right col-form-label">Loyalty Points</label>
                    <div class="col col-md-6">
                        <input type="number" v-model="item.loyaltyPoints" class="form-control" v-bind:class="{'is-invalid':validations.get('loyaltyPoints')}" />
                        <span v-if="validations.has('loyaltyPoints')" class="text-danger">
                            {{validations.get('loyaltyPoints')}}
                        </span>
                    </div>
                </div>

            </div>
        </b-overlay>
    </div>
</template>
<script>
    import pageMixin from '../../../_Core/Mixins/pageMixin';

    export default {
        mixins: [
            pageMixin,
        ],
        props: {
            uid: { type: String, required: true },
            routeView: { type: String, required: true },
        },
        components: {

        },
        data() {
            return {
                isDirty: false,
                validations: new Map(),

                imageFiles: null,
                imageFileUrls: null,
                item: {
                    imageLink: null,
                    itemId: null,
                    name: null,
                    variant: null,
                    description: null,
                    quantity: 1,
                    loyaltyPoints: 0,
                    price: 0,
                    cogs: 0,
                },

                items: []
            };
        },
        computed: {
            imageFilesValid() {
                const vm = this;

                if (Array.isArray(vm.imageFiles))
                    return vm.imageFiles.length > 0;
                else
                    return vm.imageFiles != null;
            },

            filteredItems() {
                const vm = this;

                var items = vm.items.map(e => {
                    const item = {
                        value: e.itemGroupId,
                        text: e.name
                    };

                    return item;
                });

                items.unshift({
                    value: null,
                    text: 'New'
                });

                return items;
            },

            formIsValid() {
                const vm = this;

                if (!vm.isDirty)
                    return true;

                const item = vm.item;
                const validations = new Map();

                if (item.itemId == null && !item.name)
                    validations.set('name', 'Name is required.');

                if (!item.variant)
                    validations.set('variant', 'Variant is required.');

                if (isNaN(item.quantity) || item.quantity <= 0)
                    validations.set('quantity', 'Quantity is required.');

                if (isNaN(item.price) || item.price <= 0)
                    validations.set('price', 'Price is required.');

                vm.validations = validations;

                return vm.validations.size == 0;
            },
        },
        async created() {
            const vm = this;

            await Promise.all([vm.getItems()]);
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
            async getItems() {
                const vm = this;

                try {
                    await vm.$util.axios.get(`/api/products/item-groups`)
                        .then(resp => vm.items = resp.data);

                } catch (e) {
                    vm.$util.handleError(e);
                }
            },
            async save(flag) {
                const vm = this;
                const item = vm.item;
                if (vm.busy)
                    return;

                vm.isDirty = true;

                if (!vm.formIsValid)
                    return;

                try {
                    vm.busy = true;

                    const obj = item;

                    const json = JSON.stringify(obj);
                    const blob = new Blob([json], {
                        type: 'application/json'
                    });

                    const formData = new FormData();

                    formData.append('product', blob);

                    if (Array.isArray(vm.imageFiles)) {
                        vm.imageFiles.forEach(file => {
                            formData.append('images', file);
                        });
                    }
                    else {
                        formData.append('images', vm.imageFiles);
                    }

                    await vm.$util.axios.post(`/api/products`, formData, {
                        headers: {
                            'Content-Type': 'multipart/form-data'
                        }
                    }).then(resp => {
                        vm.$bvToast.toast('Product created.', { title: 'Create Product', variant: 'success', toaster: 'b-toaster-bottom-right' });

                        setTimeout(() => {
                            if (flag) {
                                location.reload();
                            }
                            else if (flag == false) {
                                //const url = `${vm.urlView}${resp.data}`;
                                //vm.$util.href(url);
                                const rp = {
                                    name: vm.routeView, params: { id: resp.data }
                                };

                                vm.$router.push(rp);
                            }
                            else {
                                vm.close();
                            }

                        }, 300);
                    });

                } catch (e) {
                    vm.$util.handleError(e);
                } finally {
                    vm.busy = false;
                }
            },

        }
    }
</script>