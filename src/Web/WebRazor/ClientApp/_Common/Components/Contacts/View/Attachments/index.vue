<template>
    <b-overlay :show="busy" v-cloak>
        <div class="p-1 text-right">
            <template v-if="isAssigned">
                <action-member :uid="uid"
                               :id="id"
                               :roleId="roleId"
                               :token="token"
                               @get="get">
                </action-member>
            </template>
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
                    <template v-for="(att,index) in item.attachments">
                        <tr v-if="!att.isDeleted || isManager" v-bind:class="att.isDeleted? 'text-danger':''" class="border-top">
                            <td>
                                {{index+1}}
                            </td>
                            <td class="text-break p-1">
                                <div class="d-flex flex-row align-items-center">
                                    <i v-if="att.attachmentType===1" class="fas fa-fw fa-sticky-note"></i>
                                    <i v-else-if="att.attachmentType===2">
                                        <i v-if="att.contentType && att.contentType.startsWith('image/')" class="fas fa-fw fa-image"></i>
                                        <i v-else class="fas fa-fw fa-paperclip"></i>
                                    </i>
                                    {{att.title || att.fileName}}
                                </div>
                            </td>
                            <td>
                                <component-attachment-action :item="att" :readOnly="false" @get="get"></component-attachment-action>
                                <!--<div class="d-flex flex-row justify-content-end">
            <div v-if="att.attachmentType===1" @click="$refs.modalAddNote.open(false, att.contactAttachmentId, att.token, att.title, att.content)" class="ml-1">
                <button class="btn btn-sm btn-warning">
                    <i class="fas fa-fw fa-edit"></i>
                </button>
            </div>
            <div v-if="att.attachmentType===1 || (att.contentType && (att.contentType.startsWith('image/') || att.contentType.startsWith('video/')))" class="ml-1">
                <button v-b-toggle="`view-${att.contactAttachmentId}`" class="btn btn-sm btn-info">
                    <i class="fas fa-fw fa-eye"></i>
                </button>
            </div>
            <div v-if="att.attachmentType===2" class="ml-1">
                <a class="btn btn-sm btn-info" :href="att.url">
                    <i class="fas fa-fw fa-file-download"></i>
                </a>
            </div>
            <div class="ml-2">
                <button @click="deleteNote(att)" class="btn btn-sm btn-danger">
                    <i class="fas fa-fw fa-trash"></i>
                </button>
            </div>
    </div>-->
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" class="p-0 m-0 bg-secondary">
                                <b-collapse :id="`view-${att.contactAttachmentId}`" class="p-1">

                                    <div v-if="att.attachmentType===1">
                                        {{att.content}}
                                    </div>
                                    <div v-if="att.attachmentType===2">
                                        <div v-if="att.contentType && att.contentType.startsWith('image/')">
                                            <b-img-lazy :src="att.url" fluid center></b-img-lazy>
                                        </div>
                                        <div v-else-if="att.contentType && att.contentType.startsWith('video/')">
                                            <b-embed type="video"
                                                     aspect="16by9"
                                                     controls
                                                     allowfullscreen>
                                                <source :src="att.url" type="video/mp4">
                                            </b-embed>
                                            <!--<video width="400" controls>
                                                <source :src="att.url" type="video/mp4">-->
                                            <!--<source src="mov_bbb.ogg" type="video/ogg">-->
                                            <!--Your browser does not support HTML video.
                                            </video>-->
                                        </div>
                                        <div v-else>
                                            <object :data="`${att.url}`"
                                                    :type="att.contentType"
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
                                        <div>Created: {{att.dateCreated|moment('calendar')}}</div>
                                        <div v-if="att.dateCreated!==att.dateUpdated"> Updated: {{att.dateUpdated|moment('calendar')}}</div>
                                    </div>
                                </b-collapse>
                            </td>
                            <td class="m-0 p-0"></td>
                        </tr>
                    </template>
                </tbody>
            </table>
        </div>

        <modal-add-note ref="modalAddNote" @saved="get"></modal-add-note>
    </b-overlay>
</template>
<script>
    import ModalAddNote from './Modals/add-note.vue';
    //import modalAddFile from './Modals/add-file.vue';

    import ActionMember from './_action-member.vue';
    import ComponentAttachmentAction from '../../../Attachments/action.vue';

    export default {

        props: {
            uid: {
                type: String, required: true
            },
            roleId: {
                type: String, required: true
            },
            id: {
                type: String, required: true
            },
            token: {
                type: String, //required: true
            },
            item: {
                type: Object, required: true
            },
            busy: {
                type: Boolean, required: true
            }
        },
        components: {
            ActionMember,
            ModalAddNote,// modalAddFile
            ComponentAttachmentAction,
        },
        data() {
            return {

            };
        },

        computed: {
            isManager() {
                const vm = this;

                return vm.roleId === 'manager';
            },
            isMember() {
                const vm = this;

                return vm.roleId === 'member';
            },
            isAssigned() {
                const vm = this;
                const item = vm.item;

                if (vm.isMember && item.systemInformation.assignedToId == vm.uid)
                    return true;
                return false;
            },
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

            async deleteNote(item) {
                const vm = this;

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