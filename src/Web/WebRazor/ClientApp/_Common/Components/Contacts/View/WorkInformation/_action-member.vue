<template>
    <div v-cloak>
        <button v-if="viewMode" @click="edit" v-b-toggle.collapse-1 class="btn btn-warning">
            <i class="fas fa-fw fa-edit"></i>
        </button>
        <button v-if="editMode" @click="save" v-bind:disabled="!canSave" class="btn btn-primary">
            <i class="fas fa-fw fa-save"></i>
        </button>
        <button v-if="editMode" @click="cancel" class="btn btn-secondary">
            <i class="fas fa-fw fa-times"></i>
        </button>
    </div>
</template>
<script>
    
    export default {
        
        props: {
            uid: {
                type: String, required: true
            },
            item: {
                type: Object, required: true
            },
            viewMode: {
                type: Boolean, required: true
            },
            editMode: {
                type: Boolean, required: true
            },
            canSave: {
                type: Boolean, required: true
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
            },
            close() {
                this.$emit('close');
            },

            edit() {
                this.$emit('edit');

            },
            save() {
                this.$emit('save');
            },
            cancel() {
                this.$emit('cancel');
            }
        }
    }
</script>