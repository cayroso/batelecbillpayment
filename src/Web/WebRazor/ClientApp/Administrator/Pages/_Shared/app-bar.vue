<template>
    <nav class="navbar navbar-expand-lg fixed-top navbar-dark bg-success" aria-label="Main navigation">
        <div class="container-fluid">
            <a class="navbar-brand" href="#">Offcanvas navbar</a>
            <button class="navbar-toggler p-0 border-0" type="button" id="navbarSideCollapse" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="navbar-collapse offcanvas-collapse" id="navbarsExampleDefault">
                <ul class="navbar-nav me-auto mb-2 mb-lg-0">
                    <li class="nav-item">
                        <a class="nav-link" href="/administrator">
                            <i class="fas fa-dashboard me-2" aria-hidden="true"></i>Dashboard
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/administrator/billings">
                            <i class="fa-solid fa-money-bill me-2" aria-hidden="true"></i>Billings
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/administrator/announcements">
                            <i class="fa-solid fa-bullhorn me-2" aria-hidden="true"></i>Announcements
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/administrator/reservations">
                            <i class="fa-solid fa-calendar-check me-2" aria-hidden="true"></i>Reservations
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/administrator/consumers">
                            <i class="fa-solid fa-users me-2" aria-hidden="true"></i>Consumers
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/administrator/notifications">
                            <i class="fa-solid fa-bell me-2" aria-hidden="true"></i>Notifications
                        </a>
                    </li>

                    <li class="nav-item">
                        <a class="nav-link" href="/Account">
                            <i class="fas fa-user me-2" aria-hidden="true"></i>My Account
                        </a>
                    </li>

                    <!--<li class="nav-item dropdown">
        <a class="nav-link dropdown-toggle" href="#" id="dropdown01" data-bs-toggle="dropdown" aria-expanded="false">Settings</a>
        <ul class="dropdown-menu" aria-labelledby="dropdown01">
            <li><a class="dropdown-item" href="#">Action</a></li>
            <li><a class="dropdown-item" href="#">Another action</a></li>
            <li><a class="dropdown-item" href="#">Something else here</a></li>
        </ul>
    </li>-->
                </ul>
                <form class="d-flex">
                    <input class="form-control me-2" type="search" placeholder="Search" aria-label="Search">
                    <button class="btn btn-outline-success" type="submit">Search</button>
                </form>
            </div>
        </div>
    </nav>
</template>
<script>
    //import navbarMixin from '../../../_Core/Mixins/navbarMixin';

    //import navDrawer from './nav-drawer.vue';
    //import NotificationsDrawer from '../../../_Common/Drawers/notifications-drawer.vue';
    //import MessagesDrawer from '../../../_Common/Drawers/messages-drawer.vue';
    //import TeamsDrawer from '../../../_Common/Drawers/teams-drawer.vue';

    export default {
        //mixins: [navbarMixin],
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