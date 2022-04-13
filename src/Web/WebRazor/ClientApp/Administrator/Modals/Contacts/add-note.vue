
<template>
    <b-modal ref="modal"
             :no-close-on-esc="false"
             :no-close-on-backdrop="true"
             scrollable>
        <template v-slot:modal-header>
            <div class="d-sm-flex align-items-center justify-content-between">
                <h1 class="h5 m-0">
                    <span v-if="isAdd">
                        Add Note
                    </span>
                    <span v-else>
                        Edit Note
                    </span>
                </h1>
            </div>
        </template>
        <template v-slot:modal-footer>
            <button @click="save(false)" class="btn btn-primary">
                Save
            </button>
            <button v-if="isAdd" @click="save(true)" class="btn btn-primary">
                Save and Add
            </button>
            <button @click="close" class="btn btn-secondary">
                Close
            </button>
        </template>
        <div>
            <div class="form-row1">

                <div class="form-group">
                    <label for="title">Title</label>
                    <div>
                        <input v-model="title" type="text" id="title" class="form-control" />
                    </div>
                </div>
                <div class="form-group">
                    <label for="content">Content</label>
                    <div>
                        <textarea v-model="content" rows="4" id="content" class="form-control"></textarea>
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
                isAdd: null,
                id: null,
                token: null,
                title: null,
                content: null,
            }
        },
        methods: {
            
            async open(isAdd, id, token, title, content) {
                const vm = this;

                vm.isAdd = isAdd;
                vm.id = id;
                vm.token = token;
                vm.title = title;
                vm.content = content;

                vm.$refs.modal.show();
            },

            close() {
                const vm = this;

                vm.$refs.modal.hide();
            },

            async save(saveAndAdd) {
                const vm = this;

                try {
                    const payload = {
                        //contactId: vm.contactId,
                        token: vm.token,
                        title: vm.title,
                        content: vm.content,
                    };

                    if (vm.isAdd) {
                        payload.contactId = vm.id;

                        await vm.$util.axios.post(`/api/members/contacts/add-note`, payload)
                            .then(resp => {
                                vm.$bvToast.toast('Note added.', { title: 'Add Note', variant: 'success', toaster: 'b-toaster-bottom-right' });                                
                            });
                    }
                    else {
                        payload.contactAttachmentId = vm.id;

                        await vm.$util.axios.put(`/api/members/contacts/edit-note`, payload)
                            .then(resp => {
                                vm.$bvToast.toast('Note updated.', { title: 'Edit Note', variant: 'success', toaster: 'b-toaster-bottom-right' });
                                vm.token = resp.data;
                            });
                    }

                    vm.$emit('saved');

                    if (saveAndAdd) {
                        vm.title = '';
                        vm.content = '';
                    }
                    else {
                        vm.close();
                    }
                } catch (e) {
                    vm.$util.handleError(e);
                }
            },
        }
    }
</script>