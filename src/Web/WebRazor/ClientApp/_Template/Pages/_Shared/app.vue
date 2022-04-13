
<script>
    'use strict';
    import appMixin from '../../../_Core/Mixins/appMixin';
    //import SystemBar from './system-bar.vue';
    //import AppBar from './app-bar.vue';
    //import NavDrawer from './nav-drawer.vue';
    //import AppFooter from './footer.vue';
    //import BottomNav from './bottom-nav.vue';
    //import ReturnBottle from '../../../_Core/Modals/Bottles/return-bottle.vue';
    //import Index from '../index.vue';
    //import AccountsIndex from '../Accounts/index.vue';
    //import AdminsOrganizationsAdd from '../Admins/organizations-add.vue';
    //import AdminsOrganizationsEdit from '../Admins/organizations-edit.vue';
    //import AdminsOrganizationsIndex from '../Admins/organizations-index.vue';
    //import AdminsUsersIndex from '../Admins/users-index.vue';
    //import AdminsUsersView from '../Admins/users-view.vue';
    //import OrganizationsIndex from '../Organizations/index.vue';
    //import OrganizationsMembersIndex from '../Organizations/Members/index.vue';
    //import OrganizationsMembersView from '../Organizations/Members/view.vue';
    //import OrgAdminsMembersIndex from '../OrgAdmins/members-index.vue';
    //import OrgAdminsPostsIndex from '../OrgAdmins/posts-index.vue';
    //import OrganizationAccountsInquiriesIndex from '../Accounts/inquiries-index.vue';
    //import OrganizationAccountsPostsIndex from '../Accounts/posts-index.vue';
    //import OrganizationInquiriesAdd from '../Inquiries/add.vue';
    //import OrganizationInquiriesIndex from '../Inquiries/index.vue';
    //import OrganizationInquiriesView from '../Inquiries/view.vue';
    //import OrganizationMembersIndex from '../Members/index.vue';
    //import OrganizationMembersView from '../Members/view.vue';
    //import PostsAdd from '../Posts/add.vue';
    //import PostsIndex from '../Posts/index.vue';
    //import SearchIndex from '../Search/index.vue';
    export default {
        mixins: [appMixin],
        components: {
            //SystemBar, AppBar, NavDrawer, AppFooter, BottomNav,
            //ReturnBottle,
            //Index,
            //AccountsIndex,
            //AdminsOrganizationsAdd,
            //AdminsOrganizationsEdit,
            //AdminsOrganizationsIndex,
            //AdminsUsersIndex,
            //AdminsUsersView,
            //OrganizationsIndex,
            //OrganizationsMembersIndex,
            //OrganizationsMembersView,
            //OrgAdminsMembersIndex,
            //OrgAdminsPostsIndex,
            //PostsAdd,
            //PostsIndex,
            //SearchIndex,
            //OrganizationAccountsInquiriesIndex,
            //OrganizationAccountsPostsIndex,
            //OrganizationInquiriesAdd,
            //OrganizationInquiriesIndex,
            //OrganizationInquiriesView,
            //OrganizationMembersIndex,
            //OrganizationMembersView,
            //OrganizationPostsAdd,
            //OrganizationPostsIndex,
            //OrganizationPostsView,
            //ManagerCustomersAdd,
            //ManagerCustomersIndex,
            //ManagerCustomersView,
            //ManagerOrdersAdd,
            //ManagerOrdersIndex,
            //ManagerOrdersView,
            //ManagerProductsAdd, ManagerProductsIndex, ManagerProductsView,
            //ManagerPromotionsAdd, ManagerPromotionsIndex, ManagerPromotionsView,
            ////ManagerShippingSettingsAdd,
            //MemberOrganizationsIndex,
            ////ManagerShippingSettingsView,
            //ManagerDeliveryOptionsIndex,
            //ManagerSecurityUsersAdd,
            //ManagerSecurityUsersIndex,
            //ManagerSecurityUsersView,
            //ManagerTaskboardIndex
        },
        props: {
            organizationId: String,
            uid: String,
        },
        data() {
            return {
            }
        },
        async mounted() {
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
