
<script>
    'use strict';
    import appMixin from '../../../_Core/Mixins/appMixin';

    //import modalViewChat from '../../../_Core/Modals/Chats/view.vue';

    //import SystemBar from './system-bar.vue';
    import AppBar from './app-bar.vue';
    //import NavDrawer from './nav-drawer.vue';
    //import AppFooter from './footer.vue';
    //import BottomNav from './bottom-nav.vue';

    import Index from '../Index.vue';

    import AdministratorsAdd from '../Administrators/add.vue';
    import AdministratorsEdit from '../Administrators/edit.vue';
    import AdministratorsIndex from '../Administrators/index.vue';
    import AdministratorsView from '../Administrators/view.vue';

    import ConsumersAdd from '../Consumers/add.vue';
    import ConsumersIndex from '../Consumers/index.vue';
    import ConsumersView from '../Consumers/view.vue';

    import GcashWebhooksIndex from '../GcashWebhooks/index.vue';
    import GcashWebhooksView from '../GcashWebhooks/view.vue';

    import NotificationsIndex from '../Notifications/index.vue';
    import NotificationsView from '../Notifications/view.vue';
    
    export default {
        template: '#app',
        //mixins: [appMixin],
        components: {

            //modalViewChat,

            //SystemBar,
            AppBar,
            //NavDrawer,
            //AppFooter,
            //BottomNav,
            Index,

            AdministratorsAdd,
            AdministratorsEdit,
            AdministratorsIndex,
            AdministratorsView,

            ConsumersAdd,
            ConsumersIndex,
            ConsumersView,

            GcashWebhooksIndex,
            GcashWebhooksView,

            NotificationsIndex,
            NotificationsView,

        },
        props: {
            uid: String,
            appName: String,
            urlProfilePicture: String,
        },
        data() {
            return {
                menus: [
                    { to: '/', label: 'Home', icon: 'fas fa-fw fa-home' },
                    { to: '/contacts', label: 'Contacts', icon: 'fas fa-fw fa-id-card' },
                    { to: '/tasks', label: 'Tasks', icon: 'fas fa-fw fa-tasks' },
                    { to: '/documents', label: 'Documents', icon: 'fas fa-fw fa-archive' },
                    //{ to: '/teams', label: 'My Teams', icon: 'fas fa-fw fa-users' },
                ]
            }
        },
        async mounted() {
            //alert("app mounted")
            //this.$toast.success('Loaded', 'success <b>bold</bold> <a href="/administrator/billings/">Billings</a> <button>button</button>');
            //this.$toast.warning('Loaded', 'success');
            //this.$toast.info('Loaded', 'success');
            //this.$toast.error('Loaded', 'success');

            //let theme = localStorage.getItem('theme') || '';
            //if (theme) {
            //    //debugger;
            //    let style = document.createElement('link');
            //    style.type = "text/css";
            //    style.rel = "stylesheet";
            //    style.href = theme;// 'https://bootswatch.com/4/yeti/bootstrap.min.css';
            //    document.head.appendChild(style);
            //}
        },
        async created() {
            //const vm = this;
            //let theme = localStorage.getItem('theme') || '';
            //if (theme) {
            //    //debugger;
            //    let style = document.createElement('link');
            //    style.type = "text/css";
            //    style.rel = "stylesheet";
            //    style.href = theme;// 'https://bootswatch.com/4/yeti/bootstrap.min.css';
            //    document.head.appendChild(style);
            //}
        },
        methods: {
            //async getMembershipInfo() {
            //    const vm = this;
            //    try {
            //        await vm.$util.axios.get(`api/organizations/${vm.organizationId}/membership-info/${vm.uid}`).
            //            then(resp => {
            //                const data = resp.data;
            //                //vm.membership = data;
            //                if (data.status === 2) {
            //                    var isAdmin = data.roles.find(e => e.roleId === 'organizationadministrator') !== undefined;
            //                    data.isAdmin = isAdmin;
            //                    data.isMember = !isAdmin;
            //                    data.isAdminOrMember = data.isAdmin || data.isMember;
            //                }
            //                vm.$bus.$emit('event:membership', data);
            //            });
            //    } catch (e) {
            //    }
            //},
        }
    }
</script>
