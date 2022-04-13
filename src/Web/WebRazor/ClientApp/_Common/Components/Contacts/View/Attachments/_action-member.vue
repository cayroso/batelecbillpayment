<template>
    <div v-cloak>
        <button @click="$refs.modalAddNote.open(true, id,token)" class="btn btn-primary">
            <i class="fas fa-fw fa-plus"></i>Add Note
        </button>
        <button @click="$refs.modalAddFile.open(id, token)" class="btn btn-primary">
            <i class="fas fa-fw fa-plus"></i>Add File
        </button>


        <modal-add-note ref="modalAddNote" @saved="get"></modal-add-note>
        <modal-add-file ref="modalAddFile" @saved="get"></modal-add-file>
    </div>
</template>
<script>    

    import ModalAddFile from './Modals/add-file.vue';
    import ModalAddNote from './Modals/add-note.vue';

    export default {
        components: {
            ModalAddFile, ModalAddNote
        },
        props: {
            uid: {
                type: String, required: true
            },
            id: {
                type: String, required: true
            },
            token: {
                type: String, //required: true
            },
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
            }
        }
    }
</script>