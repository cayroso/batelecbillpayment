'use strict';

export default {
    data() {
        return {
            notifications: [],
            messages: []
        }
    },
    async created() {
        const vm = this;

        //  event handlers
        vm.$bus.$on('event:notification-received', () => {
            vm.getUnreadNotifications();
        });

        vm.$bus.$on('event:notification-marked-as-read', (notificationId) => {
            let found = vm.notifications.some(p => p.notificationId === notificationId);

            if (found) {
                vm.getUnreadNotifications();
            }
        });

        vm.$bus.$on('event:notification-removed', (notificationId) => {
            let found = vm.notifications.some(p => p.notificationId === notificationId);
            if (found) {
                vm.getUnreadNotifications();
            }
        });

        vm.$bus.$on('event:chat-marked-as-read', vm.onChatMarkedAsRead);

        vm.$bus.$on('event:chat-message-received', vm.onChatMessageReceived);


    },
    async mounted() {
        const vm = this;

        const elNotif = vm.$refs.notifications;
        const elMessage = vm.$refs.messages;

        if (elNotif && elNotif.offsetParent) {
            //await vm.getUnreadNotifications();
            //debugger;
        }

        if (elMessage && elMessage.offsetParent) {
            await vm.getUnreadChats();
        }

        vm.showHiddenElements();
    },
    methods: {
        async onChatMarkedAsRead(chatId) {
            let vm = this;
            let found = vm.messages.find(p => p.chatId === chatId);

            if (found) {
                let index = vm.messages.indexOf(found);
                vm.messages.splice(index, 1);
            }
        },
        async onChatMessageReceived(chat) {
            let vm = this;

            let found = vm.messages.find(p => p.chatId === chat.chatId);

            if (!found) {
                var item = {
                    chatId: chat.chatId,
                    isRead: false,
                    lastDateSent: chat.dateSent,
                    lastMessageText: chat.content,
                    senderFirstLastName: `${chat.sender.firstName} ${chat.sender.lastName}`,
                    senderInitials: chat.sender.initials,
                    senderProfilePicture32: chat.sender.profilePicture32,
                    urlPicture: vm.$util.getProfilePictureUrl(chat.sender.profilePicture32, chat.sender.initials)
                };

                vm.messages.push(item);
            }
            else {
                found.lastMessageText = chat.content;
            }
        },
        async getUnreadNotifications() {

            const vm = this;

            try {
                await vm.$util.axios.get(`api/accounts/unread-notifications`)
                    .then(resp => {
                        vm.notifications = resp.data;
                    });
            } catch (e) {
                vm.$util.handleError(e);
            }
        },
        async getUnreadChats() {
            const vm = this;

            try {

                await vm.$util.axios.get(`api/accounts/unread-chats`)
                    .then(resp => {
                        vm.messages = resp.data;

                        for (let i in vm.messages) {
                            let item = vm.messages[i];

                            vm.$set(item, 'urlPicture', vm.$util.getProfilePictureUrl(item.senderProfilePicture32, item.senderInitials));
                        }
                    });
            } catch (e) {
                vm.$util.handleError(e);

            }
        },

        viewNotification(id) {
            this.$bus.$emit('event:open-notification', id);
        },
        viewChat(id) {
            this.$bus.$emit('event:open-chat', id);
        },
        showHiddenElements() {
            let vm = this;

            let elems = vm.$el.getElementsByClassName('initialHidden');

            for (let i = 0; i < elems.length; i++) {
                let el = elems[i];

                el.classList.remove('invisible');
            }

        }
    }
}
