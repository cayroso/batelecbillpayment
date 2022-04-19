'use strict';

import VueRouter from 'vue-router';

import index from './Pages/index.vue';

import accountsIndex from './Pages/Accounts/index.vue';

import contactsIndex from './Pages/Contacts/index.vue';
import contactsAdd from './Pages/Contacts/add.vue';
import contactsView from './Pages/Contacts/view.vue';

import documentsIndex from './Pages/Documents/index.vue';
import documentsAdd from './Pages/Documents/add.vue';
import documentsView from './Pages/Documents/view.vue';


import tasksIndex from './Pages/Tasks/index.vue';
//import tasksAdd from './Pages/Tasks/add.vue';
//import tasksView from './Pages/Tasks/View/index.vue';

const NotFound = {
    template: '<div>Not found</div>'
};

const routes = [
    { path: '/', name: "index", component: index },

    { path: '/accounts', name: "accounts", component: accountsIndex },

    { path: '/contacts', name: "contacts", component: contactsIndex },
    { path: '/contacts/add', name: "contactsAdd", component: contactsAdd },
    { path: '/contacts/view/:id', name: "contactsView", component: contactsView, props: true },

    { path: '/documents', name: "documents", component: documentsIndex },
    { path: '/documents/add', name: "documentsAdd", component: documentsAdd },
    { path: '/documents/view/:id', name: "documentsView", component: documentsView, props: true },

    { path: '/tasks', name: "tasks", component: tasksIndex },

    { path: '*', component: NotFound },
];

const router = new VueRouter({
    base:'/manager',
    mode: "history",
    routes: routes,
});

export default router;
