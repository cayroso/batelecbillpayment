<style scoped>
    label {
        font-size: small;
        font-weight: bold;
    }
</style>
<template>
    <div v-cloak>
        Accounts

        <div class="card mb-3">
            <div class="row no-gutters">
                <div class="col-md-4 bg-light">
                    <!--<img src="..." class="card-img" alt="...">-->
                    <b-img-lazy v-if="urlProfilePicture" :src="urlProfilePicture" fluid center class="card-img"></b-img-lazy>
                    <div class="text-center mt-3">
                        <button @click="$refs.modalUploadProfilePicture.open(item)" class="btn btn-sm btn-primary">
                            <i class="fas fa-fw fa-upload"></i>
                        </button>
                    </div>
                </div>
                <div class="col-md-8">
                    <div class="card-body">
                        <div class="form-group">
                            <label>Email</label>
                            <div>
                                {{item.email}}
                            </div>
                        </div>
                        <div class="form-group">
                            <label>Phone Number</label>
                            <div>
                                {{item.phoneNumber}}
                                <button @click="$refs.modalUpdatePhoneNumber.open(item)" class="btn btn-sm btn-outline-primary">
                                    <i class="fas fa-fw fa-edit"></i>
                                </button>
                            </div>
                        </div>

                        <div class="form-group">
                            <label>Name</label>
                            <div>
                                <span>{{item.firstName}} {{item.middleName}} {{item.lastName}}</span>
                                <button @click="$refs.modalEdit.open(item)" class="btn btn-sm btn-outline-primary">
                                    <i class="fas fa-fw fa-edit"></i>
                                </button>
                            </div>
                        </div>

                        <div class="form-group">
                            <div>
                                <button @click="$refs.modalUpdatePhoneNumber.open()" class="btn btn-sm btn-info">
                                    Update Phone Number
                                </button>
                            </div>
                        </div>

                        <div class="form-group">
                            <div>
                                <button @click="$refs.modalChangeTheme.open()" class="btn btn-sm btn-info">
                                    Update Theme
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <modalChangePassword ref="modalChangePassword" :item="item"></modalChangePassword>
        <modalChangeTheme ref="modalChangeTheme" @saved="get"></modalChangeTheme>
        <modalEdit ref="modalEdit" @saved="get"></modalEdit>
        <modalUpdatePhoneNumber ref="modalUpdatePhoneNumber" @saved="get"></modalUpdatePhoneNumber>
        <modalUploadProfilePicture ref="modalUploadProfilePicture" @saved="get"></modalUploadProfilePicture>
    </div>
</template>
<script>
    import pageMixin from '../../../_Core/Mixins/pageMixin';

    import modalChangePassword from '../../Modals/Accounts/change-password.vue';
    import modalChangeTheme from '../../Modals/Accounts/change-theme.vue';
    import modalEdit from '../../Modals/Accounts/edit.vue';
    import modalUpdatePhoneNumber from '../../Modals/Accounts/update-phone-number.vue';
    import modalUploadProfilePicture from '../../Modals/Accounts/upload-profile-picture.vue';

    export default {
        mixins: [pageMixin],
        components: {
            modalChangePassword,
            modalChangeTheme,
            modalEdit,
            modalUpdatePhoneNumber,
            modalUploadProfilePicture
        },
        props: {
            uid: { type: String, required: true },
            urlProfilePicture: null,
        },

        data() {
            return {
                item: {
                    userId: '1234'
                }
            };
        },

        computed: {

        },

        async created() {
            const vm = this;

        },

        async mounted() {
            const vm = this;

            await vm.get();
        },

        methods: {
            async get() {
                const vm = this;

                try {
                    await vm.$util.axios.get(`/api/accounts/`)
                        .then(resp => {
                            vm.item = resp.data;
                        })
                } catch (e) {
                    vm.$util.handleError(e);
                }
            }
        }
    }
</script>