<template>
    <div v-cloak>
        <div v-if="filter.items.length">
            <div class="d-flex flex-column-reverse flex-sm-row justify-content-center justify-content-sm-between align-items-center">
                <div v-if="showPerPage" class="small">
                    <div class="d-flex flex-row">
                        <span class="me-1 text-nowrap align-self-center">
                            <span class="me-1 font-weight-bold">{{filter.totalCount}}</span>records in {{filter.totalPages}} page(s)
                        </span>
                        <div class="d-flex flex-row align-items-center">
                            <select v-model="filter.query.pageSize" @change="changePagination1" class="form-select form-select-sm me-1">
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                                <option value="5">5</option>
                                <option value="10">10</option>
                                <option value="15">15</option>
                                <option value="25">25</option>
                                <option value="40">40</option>
                            </select>
                            <div class="text-nowrap">per page</div>
                        </div>
                    </div>
                </div>
                <div class="align-self-center">
                    <nav aria-label="Page navigation example">
                        <ul class="pagination">
                            <li class="page-item">
                                <a class="page-link" href="#" aria-label="Previous" @click.prevent="moveFirst">
                                    <span aria-hidden="true">&laquo;</span>
                                </a>
                            </li>
                            <li v-if="filter.pageIndex>1" class="page-item">
                                <a class="page-link" href="#" @click.prevent="movePrev">
                                    {{filter.pageIndex-1}}
                                </a>
                            </li>
                            <li class="page-item">
                                <a class="page-link" href.preventDefault="#">
                                    {{filter.pageIndex}}
                                </a>
                            </li>
                            <li v-if="filter.pageIndex<filter.totalPages" class="page-item">
                                <a class="page-link" href="#" @click.prevent="moveNext">
                                    {{filter.pageIndex+1}}
                                </a>
                            </li>
                            <li class="page-item">
                                <a class="page-link" href="#" aria-label="Next" @click.prevent="moveLast">
                                    <span aria-hidden="true">&raquo;</span>
                                </a>
                            </li>
                        </ul>
                    </nav>
                    <!--<b-pagination v-model="filter.query.pageIndex"
                                  :total-rows="filter.totalCount"
                                  :per-page="filter.query.pageSize"
                                  v-on:change="changePagination2"
                                  limit="3"
                                  class="mb-sm-0">
                    </b-pagination>-->
                </div>
            </div>

        </div>
        <div v-else>
            <div class="text-info font-weight-bold text-center">
                <div v-if="noRecords">
                    {{noRecords}}
                </div>
                <div v-else>No record(s) found.</div>
            </div>
        </div>
    </div>
</template>
<script>
    export default {
        props: {
            filter: Object,
            search: Function,
            noRecords: String,
            showPerPage: Boolean
        },
        data() {
            return {
            };
        },
        created() {
            const vm = this;
            ;
        },
        methods: {
            changePagination1() {
                const vm = this;
                //if (page !== vm.filter.pageIndex)
                vm.search(1);
            },
            changePagination2(page) {
                const vm = this;
                if (page !== vm.filter.pageIndex) {
                    this.search(page);
                }
            },
            moveFirst() {
                const filter = this.filter;

                if (filter.pageIndex !== 1) {
                    //debugger
                    this.search(1);
                }
            },
            movePrev() {
                const filter = this.filter;

                if (filter.pageIndex > 1) {
                    //debugger
                    this.search(filter.pageIndex - 1);
                }
            },
            moveNext() {
                const filter = this.filter;

                if (filter.pageIndex < filter.totalPages) {
                    //debugger
                    this.search(filter.pageIndex + 1);
                }
            },
            moveLast() {
                const filter = this.filter;

                if (filter.pageIndex !== filter.totalPages) {
                    //debugger
                    this.search(filter.totalPages);
                }
            }
        }
    }
</script>