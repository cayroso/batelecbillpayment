<template>
    <div v-cloak>
        <b-modal ref="modal"
                 size="lg"
                 :no-close-on-esc="true"
                 :no-close-on-backdrop="true"
                 @shown="scrollToBottom"
                 header-class="p-1"
                 body-class="body-css bg-light p-2"
                 footer-class="footer-css p-0"
                 scrollable>
            <template v-slot:modal-header>
                <div class="flex-fill">

                    <div class="d-flex align-items-center justify-content-between pb-1 border-bottom">
                        <h4 class="mb-0">
                            <span class="fas fa-fw fa-envelope mr-1"></span>View Message
                        </h4>
                        <!--<div>
                            <button @click="leave()" class="btn btn-primary">
                                <span class="fas fa-fw fa-sign-out-alt"></span>
                                <span class="ml-1 d-none d-sm-inline">
                                    Leave
                                </span>
                            </button>
                        </div>-->
                    </div>
                    <div class="d-flex flex-row">
                        <div v-for="user in item.receivers"
                             :key="user.userId"
                             class="d-flex flex-row justify-content-start">
                            <div class="m-1">
                                <b-avatar v-if="user.profilePicture32" variant="dark" :src="user.urlProfilePicture" :text="user.initials" size="sm"></b-avatar>
                                <b-avatar v-else variant="dark" :text="user.initials" size="sm"></b-avatar>
                            </div>
                            <div class="flex-grow-1 align-self-center">
                                <span v-bind:class="{'text-danger': user.isRemoved}">{{user.owner}}</span>
                            </div>
                        </div>
                    </div>
                </div>
            </template>
            <template v-slot:modal-footer>
                <div class="w-100">
                    <div class="">
                        <textarea rows="3" ref="content" v-model="content" class="form-control d-block d-sm-none d-md-block" placeholder="Enter your message here..."></textarea>
                        <textarea rows="2" ref="content" v-model="content" class="form-control d-none d-sm-block d-md-none" placeholder="Enter your message here..."></textarea>
                    </div>
                    <div class="mt-1 text-right">

                        <button v-bind:disabled="!content" v-on:click="send()" class="btn btn-primary">
                            <span class="fas fa-fw fa-paper-plane"></span>
                            <span class="ml-1 d-none d-sm-inline">
                                Send
                            </span>
                        </button>
                        <button @click="close_internal()" class="btn btn-secondary">
                            <span class="fas fa-fw fa-times"></span>
                            <span class="ml-1 d-none d-sm-inline">
                                Close
                            </span>
                        </button>

                    </div>
                </div>

            </template>

            <template v-slot:default>
                <div>
                    <div v-for="msg in messages" :key="`chat-${msg.chatMessageId}`">
                        <div class="my-1">
                            <div class="d-block">
                                <div class="row no-gutters">
                                    <div v-if="msg.chatMessageType!==0" class="col-auto" v-bind:class="{'order-last' : msg.isOwner}">
                                        <div class="mx-1">
                                            <!--<b-avatar variant="info" :src="msg.urlPicture"></b-avatar>-->
                                            <!--<b-avatar variant="dark" :text="msg.sender.initials"></b-avatar>-->
                                            <b-avatar v-if="msg.sender.profilePicture32" variant="dark" :src="msg.sender.urlProfilePicture" :text="msg.sender.initials"></b-avatar>
                                            <b-avatar v-else variant="dark" :text="msg.sender.initials"></b-avatar>
                                        </div>
                                    </div>
                                    <div class="col">
                                        <div class="card p-2 overflow-auto" v-bind:class="{'text-center text-warning': msg.chatMessageType===0}">
                                            <span v-text="msg.content" style="white-space:pre-wrap;"></span>
                                            <small class="text-right text-muted text-nowrap mt-1">
                                                {{msg.dateSent|moment('calendar')}}
                                            </small>
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div ref="bottom"></div>
                </div>
            </template>

        </b-modal>
    </div>
</template>
<script>
    import * as signalR from '@microsoft/signalr';
    import pageMixin from '../../Mixins/pageMixin';

    export default {
        mixins: [pageMixin],
        props: {
            uid: String
        },
        data() {
            return {
                moment: moment,

                chatHub: null,
                content: null,
                chatId: null,
                item: null,
                messages: []
            }
        },
        methods: {
            async onChatMessageReceived(resp) {
                let vm = this;
                
                if (vm.item.chatId !== resp.chatId) {
                    return;
                }

                vm.item.hasPendingMessage = true;
                vm.processMessage(resp);
                vm.messages.push(resp);
            },

            reset() {
                let vm = this;
                vm.chatHub = null;
                vm.content = null;
                vm.chatId = null;
                vm.item = {};
                vm.messages = [];
            },

            async openChatHub() {
                let vm = this;

                vm.chatHub = new signalR.HubConnectionBuilder()
                    .withUrl('/chatHub')
                    .build();

                let start = vm.chatHub.start();

                await start.then(function () {

                    vm.chatHub.on('chatMessageReceivedFromGroup', vm.onChatMessageReceived);

                    vm.chatHub.invoke('joinChat', vm.chatId)
                        .then(resp => {
                            //debugger;
                        })
                        .catch((err) => {
                            vm.$util.handleError(err);
                        });
                });

                await start.catch(function (err) {
                    vm.$util.handleError(err);
                });
            },

            async closeChatHub() {
                let vm = this;

                if (vm.chatHub !== null && vm.chatHub !== undefined) {
                    //  disconnect to chat group
                    await vm.chatHub.invoke('leaveChat', vm.chatId).then(
                        function (resp) {
                            vm.chatHub.stop().then(function (resp) {
                                //debugger;
                            }, function (err) {
                                vm.$util.handleError(err);
                            });
                        }, (err) => {
                            vm.$util.handleError(err);
                        });
                }
            },

            async open(chatId) {
                const vm = this;

                vm.reset();

                vm.chatId = chatId;

                try {
                    await vm.get();
                    await vm.openChatHub();

                    vm.$refs.modal.show();

                } catch (e) {
                    vm.$util.handleError(e);
                }


            },

            async close_internal() {
                const vm = this;

                vm.$bus.$emit('event:close-chat');
            },
            async close() {
                const vm = this;
                
                await vm.closeChatHub();
                await vm.markChatAsRead();

                vm.$refs.modal.hide();
            },
            async get() {
                const vm = this;

                try {
                    await vm.$util.axios.get(`/api/chat/${vm.chatId}`)
                        .then(async resp => {
                            vm.item = resp.data;
                            
                            for (let i in vm.item.receivers) {
                                let item = vm.item.receivers[i];

                                item.isOwner = item.userId === vm.uid;

                                item.owner = item.isOwner ? 'You' : `${item.firstName} ${item.lastName}`;
                                item.urlProfilePicture = vm.$util.getProfilePictureUrl(item.profilePicture32, item.initials);
                            }

                            await vm.getMessages();
                        });

                } catch (e) {
                    vm.$util.handleError(e);
                }

            },
            async getMessages() {
                const vm = this;
                const query = `?criteria=&pageIndex=1&pageSize=99`;

                try {
                    await vm.$util.axios.get(`/api/chat/${vm.chatId}/messages/${query}`)
                        .then(resp => {

                            vm.messages = resp.data.items;
                            for (let i in vm.messages) {
                                let item = vm.messages[i];
                                vm.processMessage(item);
                            }
                        });
                } catch (e) {
                    vm.$util.handleError(e);
                }

            },
            processMessage(item) {
                const vm = this;

                item.isOwner = item.sender.userId === vm.uid;
                item.owner = item.isOwner ? 'You' : `${item.sender.firstName} ${item.sender.lastName}`;

                //  get profile picture
                const receiver = vm.item.receivers.find(p => p.userId == item.sender.userId);
                
                if (receiver) {
                    item.sender.urlProfilePicture = vm.$util.getProfilePictureUrl(receiver.profilePicture32, item.sender.initials);
                }

            },
            async send() {
                const vm = this;

                const payload = {
                    chatId: vm.chatId,
                    content: vm.content
                };

                try {
                    await vm.$util.axios.post(`/api/chat/message`, payload);
                } catch (e) {
                    vm.$util.handleError(e);
                }

                vm.$bus.$emit('event:chat-sent');
                vm.content = null;
                //vm.focusContent();
                vm.scrollToBottom();
            },
            async leave() {
                const vm = this;

                try {
                    await vm.$util.axios.post(`/api/chat/${vm.chatId}/remove`)
                        .then(resp => {
                            vm.close();
                        })
                } catch (e) {
                    vm.$util.handleError(e)
                }

            },
            async markChatAsRead() {
                const vm = this;

                if (vm.item.hasPendingMessage) {
                    try {
                        await vm.$util.axios.post(`/api/chat/${vm.chatId}/markAsRead`);
                        vm.$bus.$emit('event:chat-marked-as-read', vm.chatId);
                    } catch (e) {
                        vm.$util.handleError(e);
                    }
                }
            },
            scrollToBottom() {
                const vm = this;
                var element = vm.$refs.bottom;

                element.scrollIntoView(true);

            },
            focusContent() {
                this.$refs.content.focus();
            }
        }
    }
</script>