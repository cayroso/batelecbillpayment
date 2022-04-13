<style scoped>
    /*.table-hover > tbody > tr:hover {
        background-color: papayawhip;
    }*/
    .b-left {
        border-left: 3px solid orange;
    }
</style>
<template>
    <div v-cloak>

        <div class="table-responsive m-0" v-bind:class="tableCss">
            <table class="table table-hover d-none d-md-table card">
                <thead>
                    <tr>
                        <slot name="header">
                            <th>#</th>
                            <th v-for="col in header.columns">{{col.title}}</th>
                        </slot>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(item,i) in items"
                        :key="`row1-${item[header.key]}`"
                        @click="onSelectedRow(item)">
                        <slot name="table-row" v-bind:item="item" v-bind:index="i">
                            <td>{{getRowNumber(i)}}</td>
                            <td v-for="col in header.columns">
                                <a v-if="col.href" :href="col.href(item[header.key])" class="font-weight-bold">
                                    {{item[col.field]}}
                                </a>
                                <span v-else>
                                    {{item[col.field]}}
                                </span>
                            </td>
                        </slot>
                    </tr>
                </tbody>
            </table>


        </div>
        <div class="d-block d-md-none">
            <div v-for="(item,i) in items"
                 :key="`row2-${item[header.key]}`"
                 @click="onSelectedRow(item)"
                 class="border">

                <div class="p-2" v-bind:class="{'b-left':isSelected(item)}">
                    <slot name="table-list" v-bind:item="item" v-bind:index="i">
                        <div v-for="col in header.columns" class="form-group row no-gutters">
                            <label class="col-5 col-form-label">{{col.title}}</label>
                            <div class="col align-self-center">
                                <a v-if="col.href" :href="col.href(item[header.key])" class="font-weight-bold">
                                    {{item[col.field]}}
                                </a>
                                <span v-else>
                                    {{item[col.field]}}
                                </span>
                            </div>
                        </div>
                    </slot>
                </div>
            </div>
        </div>
    </div>
</template>
<script>
    export default {
        props: {
            items: Array,
            header: Object,
            getRowNumber: Function,
            setSelected: Function,
            isSelected: Function,
            tableCss: String
        },
        data() {
            return {
            };
        },
        mounted() {
        },
        methods: {
            onSelectedRow(item) {
                const vm = this;

                vm.setSelected(item);

                vm.$emit('onSelectedRow', item);
            }
        }
    }
</script>