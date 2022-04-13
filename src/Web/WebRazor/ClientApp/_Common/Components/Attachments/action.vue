<template>
    <div v-cloak>
        <div class="d-flex flex-row justify-content-end">
            <div v-if="!readOnly">
                <div v-if="item.attachmentType===1" @click="$refs.modalAddNote.open(false,item.contactAttachmentId,item.token,item.title,item.content)" class="ml-1">
                    <button class="btn btn-sm btn-warning">
                        <i class="fas fa-fw fa-edit"></i>
                    </button>
                </div>
            </div>


            <div v-if="item.attachmentType===1 || (item.contentType && (item.contentType.startsWith('image/') ||item.contentType.startsWith('video/')))" class="ml-1">
                <button v-b-toggle="`view-${item.contactAttachmentId}`" class="btn btn-sm btn-info">
                    <i class="fas fa-fw fa-eye"></i>
                </button>
            </div>

            <div v-if="item.attachmentType===2" class="ml-1">
                <a class="btn btn-sm btn-info" :href="item.url">
                    <i class="fas fa-fw fa-file-download"></i>
                </a>
            </div>
            <div v-if="!readOnly">
                <div class="ml-2">
                    <button @click="deleteNote()" class="btn btn-sm btn-danger">
                        <i class="fas fa-fw fa-trash"></i>
                    </button>
                </div>
            </div>
        </div>

        <modal-add-note ref="modalAddNote" @saved="get"></modal-add-note>
    </div>
</template>
<script>
    import ModalAddNote from '../../Components/Contacts/View/Attachments/Modals/add-note.vue';

    export default {
        mixins: [],

        props: {
            uid: String,
            item: { type: Object, required: true },
            readOnly: { type: Boolean, required: false, default: true }
        },
        components: {
            ModalAddNote
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

            get() {
                this.$emit('get');
            },

            async deleteNote() {
                const vm = this;
                const item = vm.item;

                if (vm.busy)
                    return;

                try {

                    let msg = 'Are you sure you want to archive this attachment?'

                    if (item.isDeleted)
                        msg = 'Are you sure you want to permanently delete this attachment?';

                    this.$bvModal.msgBoxConfirm(msg)
                        .then(async value => {
                            if (value) {

                                await vm.$util.axios.delete(`/api/members/contacts/delete-attachment/${item.contactAttachmentId}/${item.token}/${item.isDeleted}`)
                                    .then(resp => {
                                        vm.$bvToast.toast('Note deleted.', { title: 'Delete Note', variant: 'warning', toaster: 'b-toaster-bottom-right' });
                                    })

                                vm.get();
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