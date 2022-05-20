<template>
    <nav class="navbar navbar-expand-lg fixed-top navbar-dark bg-success" aria-label="Main navigation">
        <div class="container-fluid">
            <a class="navbar-brand" href="/">{{appName}}</a>
            <button class="navbar-toggler p-0 border-0" type="button" id="navbarSideCollapse" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="navbar-collapse offcanvas-collapse mb-2 mb-lg-0" id="navbarsExampleDefault">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <li class="nav-item">
                        <a class="nav-link" href="/consumer">
                            <i class="fas fa-dashboard me-2" aria-hidden="true"></i>Dashboard
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/consumer/billings">
                            <i class="fa-solid fa-money-bill me-2" aria-hidden="true"></i>Billings
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/consumer/announcements">
                            <i class="fa-solid fa-bullhorn me-2" aria-hidden="true"></i>Announcements
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/consumer/reservations">
                            <i class="fa-solid fa-calendar-check me-2" aria-hidden="true"></i>Reservations
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/consumer/notifications">
                            <i class="fa-solid fa-bell me-2" aria-hidden="true"></i>Notifications
                        </a>
                    </li>


                </ul>
                <ul class="navbar-nav ms-auto">
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" href="#" id="dropdown01" data-bs-toggle="dropdown" aria-expanded="false">
                            <i class="fas fa-user me-2" aria-hidden="true"></i>My Account
                        </a>
                        <ul class="dropdown-menu" aria-labelledby="dropdown01">
                            <!--<li><a class="dropdown-item" href="/account">Information</a></li>
                            <li><hr class="dropdown-divider"></li>-->
                            <!--<li><a class="dropdown-item" href="#">Another action</a></li>-->
                            <li><a class="dropdown-item" href="#" @click.prevent="logout">Logout</a></li>
                        </ul>
                    </li>
                </ul>
            </div>
        </div>
    </nav>
</template>
<script>
    import navbarMixin from '../../../_Core/Mixins/navbarMixin';

    //import navDrawer from './nav-drawer.vue';
    //import NotificationsDrawer from '../../../_Common/Drawers/notifications-drawer.vue';
    //import MessagesDrawer from '../../../_Common/Drawers/messages-drawer.vue';
    //import TeamsDrawer from '../../../_Common/Drawers/teams-drawer.vue';

    export default {
        mixins: [navbarMixin],
        props: {
            uid: String,
            appName: {
                type: String, required: true,
                default: 'LMS'
            },
            urlProfilePicture: String,
            menus: Array
        },
        components: {
            //navDrawer,
            //NotificationsDrawer,
            //MessagesDrawer,
            //TeamsDrawer
        },
        async mounted() {
            //alert('app-bar mounted.');

            document.querySelector('#navbarSideCollapse').addEventListener('click', function () {
                document.querySelector('.offcanvas-collapse').classList.toggle('open')
            })
        }
    };
</script>