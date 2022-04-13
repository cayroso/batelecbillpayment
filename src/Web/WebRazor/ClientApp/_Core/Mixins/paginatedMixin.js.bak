'use strict';

import pageMixin from './pageMixin';

export default {
    props: {
    },
    mixins: [pageMixin],
    data() {
        return {
            filter: {
                query: {
                    criteria: '',
                    pageIndex: 1,
                    pageSize: 10,
                    sortOrder: -1,
                    sortField: '',
                },
                items: [],
                selected: null,
                visible: false,
            }
        };
    },
    async created() {
        const vm = this;

        //debugger;
    },
    methods: {
        initializeFilter(cache) {
            const filter = this.filter;
            const urlParams = new URLSearchParams(window.location.search);

            filter.query.criteria = urlParams.get('c') || cache.criteria || filter.query.criteria;
            filter.query.pageIndex = parseInt(urlParams.get('p'), 10) || cache.pageIndex || filter.query.pageIndex;
            filter.query.pageSize = parseInt(urlParams.get('s'), 10) || cache.pageSize || filter.query.pageSize;
            filter.query.sortField = urlParams.get('sf') || cache.sortField || filter.query.sortField;
            filter.query.sortOrder = parseInt(urlParams.get('so'), 10) || cache.sortOrder || filter.query.sortOrder;
            filter.visible = cache.visible || filter.visible;

        },
        getQuery() {

            const vm = this;
            const filter = vm.filter;

            if (vm.busy)
                return;

            const query = [
                '?c=', encodeURIComponent(filter.query.criteria),
                '&p=', filter.query.pageIndex,
                '&s=', filter.query.pageSize,
                '&sf=', filter.query.sortField,
                '&so=', filter.query.sortOrder
            ].join('');

            return query;
        },

        saveQuery() {
            const vm = this;
            const filter = vm.filter;

            localStorage.setItem(filter.cacheKey, JSON.stringify({
                criteria: filter.query.criteria,
                pageIndex: filter.query.pageIndex,
                pageSize: filter.query.pageSize,
                sortField: filter.query.sortField,
                sortOrder: filter.query.sortOrder,
                visible: filter.visible
            }));
        },

        async _search() {
            const vm = this;

            if (vm.busy)
                return;

            const query = vm.getQuery();

            vm.busy = true;
            await vm.search_internal(vm.baseUrl, query, vm.title,
                () => {
                    vm.saveQuery();
                }, () => {
                    localStorage.removeItem(vm.filter.cacheKey);
                }, () => {
                    vm.busy = false;
                });

        },

        async search(index) {
            const vm = this;

            if (index > 0) {
                vm.filter.query.pageIndex = index;
            }

            await vm._search();
        },

        async search_internal(url, query, state, successAction, failAction, finalAction) {
            const vm = this;
            let filter = vm.filter;

            try {
                filter.selected = null;

                await vm.$util.axios(`${url}/${query}`)
                    .then(resp => {
                        filter = Object.assign(filter, resp.data);
                        let url = location.pathname;

                        if (url.endsWith('/'))
                            url += query;
                        else
                            url += '/' + query;

                        vm.$util.replaceState(query, state, url);

                        filter.items.forEach(item => {
                            vm.$set(item, 'expand', false);
                        });

                        successAction();
                    });
            } catch (e) {
                failAction();
                vm.$util.handleError(e);
            } finally {
                finalAction();

                vm.scrollIntoView();
            }
        },

        getRowNumber(index) {
            const filter = this.filter;
            const rowNum = ((filter.pageIndex - 1) * filter.pageSize) + (index + 1);
            return rowNum;
        },

        setSelected(item) {
            const filter = this.filter;

            if (filter.selected && item && filter.selected === item) {
                //vm.selectedItem.expand = false;
                //vm.selectedItem = null;
            }
            else {
                if (filter.selected)
                    filter.selected.expand = false;
                filter.selected = item;
                filter.selected.expand = true;
            }
        },

        isSelected(item) {
            const filter = this.filter;

            if (item === null)
                return false;

            if (filter.selected === null)
                return false;

            return item === filter.selected;
        }
    }
}