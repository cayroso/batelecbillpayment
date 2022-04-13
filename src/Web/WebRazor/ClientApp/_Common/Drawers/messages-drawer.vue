<template>
    <b-sidebar id="messagesDrawer" title="Messages" right backdrop shadow>
        <div v-if="messages.length>0" class="d-block">
            <a v-for="msg in messages"
               :key="msg.chatId"
               v-on:click="viewChat(msg.chatId)"
               style="cursor:pointer"
               class="dropdown-item d-flex align-items-center">
                <b-avatar variant="info" :src="msg.urlPicture"></b-avatar>

                <div class="ml-1">
                    <div v-if="msg.lastMessageText">
                        {{msg.lastMessageText | textTruncate(20, '...')}}
                    </div>
                    <div class="small text-truncate ">{{msg.senderFirstLastName}} {{msg.lastDateSent|moment('calendar')}}</div>
                </div>
            </a>
        </div>
        <a class="dropdown-item text-center small text-gray-500" :href="page">Read More Messages</a>
    </b-sidebar>
</template>
<script>
    export default {
        props: {
            messages: Array,
            page: {
                type: String, required: true
            }
        },
        data() {
            return {
                //messages: []
            }
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
    };
</script>