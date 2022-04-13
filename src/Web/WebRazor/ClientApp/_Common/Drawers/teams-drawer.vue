<template>
    <b-sidebar id="teamsDrawer" title="My Teams" backdrop right shadow>
        <div v-if="teams" class="px-2" v-cloak>
            <ul class="list-unstyled">
                <li v-for="t in teams.items">
                    <i class="fas fa-fw fa-users mr-1"></i>
                    <a @click.prevent="$bus.$emit('event:send-team-message',t.teamId)" href="#">
                        {{t.name}}
                    </a>
                    <ul>
                        <li v-for="tm in t.members" v-if="tm.userId !== uid">
                            <!--<i class="fas fa-fw fa-user mr-1"></i>-->
                            <b-avatar :src="tm.urlProfilePicture" size="sm"></b-avatar>
                            <a @click.prevent="$bus.$emit('event:send-message',tm.userId)" href="#">
                                {{tm.firstName}} {{tm.lastName}}
                            </a>
                        </li>
                    </ul>
                </li>
            </ul>
        </div>
    </b-sidebar>
</template>
<script>
    export default {
        props: {
            uid: String
        },
        data() {
            return {
                teams: null
            }
        },
        async mounted() {
            const vm = this;

            await vm.get();
        },
        methods: {

            async get() {
                const vm = this;

                try {
                    const query = [
                        '?c=', //encodeURIComponent(filter.query.criteria),
                        '&p=', 1,//filter.query.pageIndex,
                        '&s=', 5,//filter.query.pageSize,
                        '&sf=', //filter.query.sortField,
                        '&so=', -1//filter.query.sortOrder
                    ].join('');

                    await vm.$util.axios.get(`/api/teams/my-teams/${query}`)
                        .then(resp => {
                            vm.teams = resp.data;
                        });
                } catch (e) {
                    vm.$util.handleError(e);
                }
            }
        }
    };
</script>