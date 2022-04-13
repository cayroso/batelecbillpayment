<style></style>
<template>
    <div @click="click" class="d-flex flex-row justify-content-start align-content-center" :style="{'cursor': enabled? 'pointer': ''}">
        <slot></slot>
        <span class="align-self-center">
            <span v-if="showSort" v-bind:class="{'fa-sort text-gray-500': !currentSortField(), 'fa-sort-up': currentSortField() && sortUp(), 'fa-sort-down': currentSortField() && !sortUp()}"
                  class="fas fa-fw mr-1"></span>
        </span>
    </div>
</template>
<script>
    export default {
        props: {
            filter: Object,
            field: String,
            showSort: Boolean,
            enabled: Boolean
        },
        data() {
            return {
            };
        },
        mounted() {
        },
        methods: {
            currentSortField() {
                return this.filter.sortField === this.field;
            },
            sortUp() {
                return this.filter.sortOrder === 1;
            },
            click() {
                let vm = this;
                if (!vm.enabled)
                    return;
                let filter = vm.filter;
                if (filter.sortField === vm.field) {
                    if (filter.sortOrder === 0)
                        filter.sortField = null;
                    else
                        filter.sortOrder = filter.sortOrder === 0 ? 1 : 0;
                }
                else {
                    filter.sortField = vm.field;
                    filter.sortOrder = 1;
                }
                vm.$emit('search');
            }
        }
    }
</script>