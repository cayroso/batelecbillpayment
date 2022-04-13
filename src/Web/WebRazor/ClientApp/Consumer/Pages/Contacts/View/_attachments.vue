<template>
    <div v-cloak>
        <div class="p-1 text-right">
            <button @click="$refs.modalAddNote.open(true, id,token)" class="btn btn-primary">
                <i class="fas fa-fw fa-plus"></i>Add Note
            </button>
            <button @click="$refs.modalAddFile.open(id, token)" class="btn btn-primary">
                <i class="fas fa-fw fa-plus"></i>Add File
            </button>
        </div>

        <div class="table-responsive mb-0">
            <table class="table table-borderless">
                <!--<thead>
                    <tr>
                        <th>#</th>
                        <th>Title</th>
                        <th class="mx-0 px-0"></th>
                    </tr>
                </thead>-->
                <tbody>
                    <template v-for="(item,index) in items">
                        <tr>
                            <td>{{index+1}}</td>
                            <td class="text-break">
                                <i v-if="item.attachmentType===1" class="fas fa-fw fa-sticky-note"></i>
                                <i v-else-if="item.attachmentType===2">
                                    <i v-if="item.contentType && item.contentType.startsWith('image/')" class="fas fa-fw fa-image"></i>
                                    <i v-else class="fas fa-fw fa-paperclip"></i>
                                </i>
                                {{item.title || item.fileName}}
                            </td>
                            <td>
                                <div class="text-right d-fkex flex-row">
                                    <button v-if="item.attachmentType===1" @click="$refs.modalAddNote.open(false, item.contactAttachmentId, item.token, item.title, item.content)" class="btn btn-sm btn-warning">
                                        <i class="fas fa-fw fa-edit"></i>
                                    </button>

                                    <span class="w-100 my-1 d-block d-sm-none"></span>

                                    <button v-if="item.attachmentType===1 || (item.contentType && item.contentType.startsWith('image/'))" v-b-toggle="`view-${item.contactAttachmentId}`" class="btn btn-sm btn-info">
                                        <i class="fas fa-fw fa-eye"></i>
                                    </button>

                                    <span class="w-100 my-1 d-block d-sm-none"></span>

                                    <a v-if="item.attachmentType===2" class="btn btn-sm btn-info" :href="item.url">
                                        <i class="fas fa-fw fa-file-download"></i>
                                    </a>

                                    <span class="w-100 my-2 d-block d-sm-none"></span>

                                    <button @click="deleteNote(item.contactAttachmentId, item.token)" class="btn btn-sm btn-danger ml-md-2">
                                        <i class="fas fa-fw fa-trash"></i>
                                    </button>
                                </div>

                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="p-0 m-0">
                                <b-collapse :id="`view-${item.contactAttachmentId}`" class="p-1">

                                    <div v-if="item.attachmentType===1">
                                        {{item.content}}
                                    </div>
                                    <div v-if="item.attachmentType===2">
                                        <div v-if="item.contentType && item.contentType.startsWith('image/')">
                                            <b-img-lazy :src="item.url" fluid center></b-img-lazy>
                                        </div>
                                        <div v-else>
                                            <object :data="`${item.url}`"
                                                    :type="item.contentType"
                                                    height="100%"
                                                    width="100%">
                                                <p>
                                                    Your browser does not support PDFs.
                                                    <a href="https://example.com/test.pdf">Download the PDF</a>.
                                                </p>
                                            </object>
                                        </div>
                                    </div>
                                    <div class="small text-right p-1">
                                        <div>Created: {{item.dateCreated|moment('calendar')}}</div>
                                        <div v-if="item.dateCreated!==item.dateUpdated"> Updated: {{item.dateUpdated|moment('calendar')}}</div>
                                    </div>
                                </b-collapse>
                            </td>
                            <td class="m-0 p-0"></td>
                        </tr>
                    </template>
                </tbody>
            </table>
        </div>

        <modal-add-note ref="modalAddNote" @saved="updated"></modal-add-note>
        <modal-add-file ref="modalAddFile" @saved="updated"></modal-add-file>
    </div>
</template>
<script>
    import modalAddNote from '../../../Modals/Contacts/add-note.vue';
    import modalAddFile from '../../../Modals/Contacts/add-file.vue';
    export default {

        props: {
            uid: String,
            id: String,
            token: String,
            items: Array
        },
        components: {
            modalAddNote, modalAddFile
        },
        data() {
            return {

            };
        },

        computed: {

        },

        async created() {
            const vm = this;

        },

        async mounted() {
            const vm = this;

        },

        methods: {
            updated() {
                const vm = this;
                vm.$emit('updated');
            },

            async deleteNote(id, token) {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    this.$bvModal.msgBoxConfirm('Are you sure you want to delete this note?')
                        .then(async value => {
                            if (value) {
                                await vm.$util.axios.delete(`/api/members/contacts/delete-attachment/${id}/${token}`)
                                    .then(resp => {
                                        vm.$bvToast.toast('Note deleted.', { title: 'Delete Note', variant: 'warning', toaster: 'b-toaster-bottom-right' });
                                    })

                                vm.updated();
                            }
                        })
                        .catch(err => {
                            vm.$util.handleError(err);
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