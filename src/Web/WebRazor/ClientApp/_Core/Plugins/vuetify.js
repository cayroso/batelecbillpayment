// src/plugins/vuetify.js
import 'vuetify/dist/vuetify.min.css'
import Vue from 'vue'
import Vuetify
, {
    VApp,
    VContent,
    VContainer,

    VAppBar,
    VAppBarNavIcon,
    VToolbarTitle,
    VTextField,
    VBtn,
    VSpacer,
    VIcon,
    VAvatar,
    VImg,

    VNavigationDrawer,
    VList,
    VRow,
    VCol,
    VSubheader,
    VListGroup,
    VListItemContent,
    VListItemTitle,
    VListItem,
    VListItemAction,

}
    from 'vuetify/lib';


Vue.use(Vuetify, {
    components: {
        VApp,
        VContent,
        VContainer,

        VAppBar,
        VAppBarNavIcon,
        VToolbarTitle,
        VTextField,
        VBtn,
        VSpacer,
        VIcon,
        VAvatar,
        VImg,

        VNavigationDrawer,
        VList,
        VRow,
        VCol,
        VSubheader,
        VListGroup,
        VListItemContent,
        VListItemTitle,
        VListItem,
        VListItemAction,
    }
})

const opts = {}

export default new Vuetify(opts);