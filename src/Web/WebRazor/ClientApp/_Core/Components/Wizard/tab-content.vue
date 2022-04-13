<template>
    <div v-show="isActive">
        <slot></slot>
    </div>
</template>

<script>
    export default {
        name: 'tab-content',
        props: {
            model: Object,
            selected: {
                default: false,
            },
            title: {
                type: String,
                required: true
            }
        },
        data() {
            return {
                isActive: false,
            }
        },
        created() {
            this.isActive = this.selected;
        }
    }
</script>