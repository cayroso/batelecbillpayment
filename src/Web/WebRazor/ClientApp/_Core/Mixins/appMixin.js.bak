
'use strict';

import * as signalR from '@microsoft/signalr';

export default {
    components: {
    },
    data() {
        return {
            enums: {
                orderStatus: {
                    requested: 1,
                    placed: 2,
                    accepted: 3,
                    processed: 4,
                    delivered: 5,
                    completed: 6,
                    cancelled: 7
                },
                salutations: {
                    mr: 1,
                    mrs: 2,
                    ms: 3
                }
            },

            messageIds: []
        };
    },
    async created() {
        const vm = this;


        //vm.$bus.$on('event:open-notification', vm.onOpenNotification);
        //vm.$bus.$on('event:close-notification', vm.onCloseNotification);



        //vm.$bus.$on('event:quick-view-account', vm.onQuickViewAccount);

        //vm.$bus.$on('event:quick-view-qb-transaction', vm.onQuickViewQbTransaction);

        //await Promise.all([vm.connectOrderHub()]);
    },
    async mounted() {
        const vm = this;

        if (vm.uid) {
            //await vm.connectNotificationHub();

            await vm.connectChatHub();

            vm.$bus.$on('event:open-chat', vm.onOpenChat);
            vm.$bus.$on('event:close-chat', vm.onCloseChat);
            vm.$bus.$on('event:send-message', vm.onSendMessage);
            vm.$bus.$on('event:send-team-message', vm.onSendTeamMessage);
        }
    },
    methods: {
        scrollIntoView() {
            const topElem = document.getElementById('top');
            if (topElem) {
                topElem.scrollIntoView({ behavior: 'smooth' });
                //debugger
            }
        },

        showHiddenElements() {
            let vm = this;

            if (!vm.$el.getElementsByClassName)
                return;

            let elems = vm.$el.getElementsByClassName('initialHidden');

            for (let i = 0; i < elems.length; i++) {
                let el = elems[i];

                el.classList.remove('invisible');
                el.classList.remove('d-none');
            }

        },
        close() {
            //const vm = this;

            //const href = atob(vm.rurl) || vm.durl;
            //vm.$util.href(href);
            const referrer = document.referrer;

            if (referrer && referrer.includes('/Identity/Account/Login')) {
                history.go(-2);
            }
            else {
                history.back();
            }
        },

        async connectNotificationHub() {
            const vm = this;

            const notificationHub = new signalR.HubConnectionBuilder()
                .withUrl('/notificationHub')
                //.configureLogging(signalR.LogLevel.Debug)
                .withAutomaticReconnect()
                .build();

            notificationHub.on('received', function (resp) {

                const message = resp.content || "-no reason specified-";

                const h = vm.$createElement;
                // Create the message
                let foo = [
                    h('span', message),
                    h('p'),
                ];

                if (resp.refLink) {
                    //debugger
                    foo.push(
                        h('a', { attrs: { href: resp.refLink } }, 'Click here to View.')
                    )
                }
                //debugger
                const vNodesMsg = h(
                    'div', foo
                );

                var route = window.location;
                var contains = route.pathname.includes(resp.itemId);

                if (!contains || (contains && resp.itemEvent !== 'Post:Comment')) {
                    vm.$bvToast.toast([vNodesMsg], {
                        title: `${resp.subject}`,
                        variant: 'info',
                        solid: true
                    });
                }

                //  emit event
                vm.$bus.$emit('event:notification-received', resp);
            });

            notificationHub.on('jobUpdated', function (resp) {
                vm.$bus.$emit('event:job-updated', resp);
            });

            await notificationHub.start().then(function () {
                //debugger;
            }).catch(function (err) {
                //debugger;
                //vm.$util.handleError(err);
            });
        },


        async connectChatHub() {
            const vm = this;

            const chatHub = new signalR.HubConnectionBuilder()
                .withUrl('/chatHub')
                //.configureLogging(signalR.LogLevel.Debug)
                .withAutomaticReconnect()
                .build();

            chatHub.on('chatMessageReceived', function (resp) {
                const uid = vm.uid;

                if (uid !== resp.sender.userId && (vm.currentOpenChatId === null || vm.currentOpenChatId !== resp.chatId)) {
                    const sender = resp.sender;

                    const h = vm.$createElement;
                    const vNodesMsg = h(
                        'div',
                        [
                            h('span', `You received a message from ${sender.firstName} ${sender.lastName}`),
                            h('br'),
                            h('br'),
                            h('a', {
                                attrs: {
                                    style: 'cursor:pointer'
                                }, on: {
                                    click: () => vm.$bus.sendMessage(sender.userId)
                                }
                            }, 'Click here to open.')
                        ]
                    );
                    vm.$bvToast.toast([vNodesMsg], {
                        title: `New Message`,
                        solid: true,
                        variant: 'info',
                        //noAutoHide: true,
                    });
                }

                vm.$bus.$emit('event:chat-message-received', resp);
            });

            await chatHub.start().then(function () {
                //debugger;
            }).catch(function (error) {
                alert(error);
            });
        },

        async onSendMessage(userId) {
            const vm = this;

            if (vm.uid === userId) {
                vm.$bvToast.toast('Are you sure?', { title: 'Talking to yourself?', variant: 'warning' });
                return;
            }

            if (!userId) {
                vm.$bvToast.toast('Account was not found.', { title: 'Account Not Found', variant: 'warning' });
                return;
            }

            try {
                await vm.$util.axios.post(`api/chat/add/${userId}`)
                    .then(async resp => {
                        await vm.onOpenChat(resp.data);
                    });
            } catch (e) {
                vm.$util.handleError(e);
            }
        },

        async onSendTeamMessage(teamId) {
            const vm = this;

            try {
                await vm.onOpenChat(teamId);
            } catch (e) {
                vm.$util.handleError(e);
            }
        },

        async onOpenChat(chatId) {
            const vm = this;

            vm.currentOpenChatId = chatId;
            vm.$refs.modalViewChat.open(chatId);
        },

        async onCloseChat() {
            const vm = this;

            vm.currentOpenChatId = null;
            vm.$refs.modalViewChat.close();
        },
    }
}