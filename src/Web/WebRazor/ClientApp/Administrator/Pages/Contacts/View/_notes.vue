<template>
    <div v-cloak>
        <div class="p-1 text-right">
            <button @click="$refs.modalAddNote.open(true, id,token)" class="btn btn-primary">
                <i class="fas fa-fw fa-plus"></i>
            </button>
        </div>




        <div class="table-responsive mb-0">
            <table class="table table-bordered">
                <thead>
                    <tr>
                        <th>#</th>
                        <th>Title</th>
                        
                        <th>Date</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <template  v-for="(note,index) in items">
                        <tr :class="{'table-info': index%2===0}">
                            <td rowspan="2">{{index+1}}</td>
                            <td> {{note.title}}</td>
                            <td class="small">
                                <div>Created: {{note.dateCreated|moment('calendar')}}</div>
                                <div>Updated: {{note.dateUpdated|moment('calendar')}}</div>

                            </td>
                            <td>
                                <button @click="$refs.modalAddNote.open(false, note.contactNoteId, note.token, note.title, note.content)" class="btn btn-sm btn-outline-primary">
                                    <i class="fas fa-fw fa-edit"></i>
                                </button>
                                <button @click="deleteNote(note.contactNoteId,note.token)" class="btn btn-sm btn-outline-danger">
                                    <i class="fas fa-fw fa-trash"></i>
                                </button>
                            </td>
                        </tr>
                        <tr :class="{'table-info': index%2===0}">
                            <td colspan="3">
                                {{note.content}}
                            </td>
                        </tr>
                    </template>
                </tbody>
            </table>
        </div>
        <modal-add-note ref="modalAddNote" @noteAdded="updated"></modal-add-note>
    </div>
</template>
<script>
    import modalAddNote from '../../../Modals/Contacts/add-note.vue';

    export default {

        props: {
            uid: String,
            id: String,
            token: String,
            items: Array
        },
        components: {
            modalAddNote
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

            async deleteNote(contactNoteId, token) {
                const vm = this;

                if (vm.busy)
                    return;

                try {
                    this.$bvModal.msgBoxConfirm('Are you sure you want to delete this note?')
                        .then(async value => {
                            if (value) {
                                await vm.$util.axios.delete(`/api/members/contacts/delete-note/${contactNoteId}/${token}`)
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
            }

        }
    }
</script>