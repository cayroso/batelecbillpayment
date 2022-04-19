'use strict';

import { createApp } from 'vue';
import NProgress from 'nprogress';
import axios from 'axios';
import moment from 'moment';
import eventBus from '../Components/EventBus/eventBus';

export default {
    install(app, options) {

        const instance = axios.create();

        instance.interceptors.request.use(config => {
            NProgress.start();
            return config;
        });
        instance.interceptors.response.use(response => {
            NProgress.done();
            return response;
        }, error => {
            NProgress.done();
            throw error;
        });

        app.config.globalProperties.$moment = moment;

        //app.filter('formatDate', function (value) {
        //    if (value) {
        //        return moment(String(value)).format('YYYY-MM-DD hh:mm:ss');
        //    }
        //});

        const handle400 = function (vm, error) {
            const response = error.response;
            const data = response.data;

            let message = '';

            if (data.errors) {
                const errors = data.errors;

                for (let key in errors) {

                    message += `${key}`;
                    for (let vals in errors[key]) {
                        var foo = errors[key][vals];
                        message += `${foo}`;
                    }
                    message += `<br/>`;
                }

                alert(`ERROR: ${message}`);                
            }
            else {
                alert(`UNHANDLED ERROR`);                
            }

        };

        const handle401 = function (vm) {

            alert('Error HTTP 401');

            //const url = `${window.location.pathname}${window.location.search}`;
            //const returnUrl = encodeURIComponent(url);
            //const redirect = `/identity/account/login/?returnUrl=${returnUrl}`;

            //// Use a shorter name for this.$createElement
            //const h = vm.$createElement;
            //// Create the message
            //const vNodesMsg = h(
            //    'div',
            //    [
            //        h('span', 'You are not authorized to access this page.'),
            //        h('br'),
            //        h('span', 'Or your session has expired.'),
            //        h('p'),
            //        h('a', { attrs: { href: redirect, } }, 'Click here to Login.')
            //    ]
            //);

            //vm.$bvToast.toast([vNodesMsg], {
            //    title: 'Login Required',// [vNodesTitle],
            //    solid: true,
            //    variant: 'danger',
            //    noAutoHide: true,
            //});
        };
        

        app.config.globalProperties.$util = {
            axios: instance,

            noop() {
                return arguments;
            },
            camelPad(str) {
                return str
                    // Look for long acronyms and filter out the last letter
                    .replace(/([A-Z]+)([A-Z][a-z])/g, ' $1 $2')
                    // Look for lower-case letters followed by upper-case letters
                    .replace(/([a-z\d])([A-Z])/g, '$1 $2')
                    // Look for lower-case letters followed by numbers
                    .replace(/([a-zA-Z])(\d)/g, '$1 $2')
                    .replace(/^./, function (str) { return str.toUpperCase(); })
                    // Remove any white space left around the word
                    .trim();
            },

            pushState(data, title, url) {
                window.history.pushState(data, title, url);
            },
            replaceState(data, title, url) {
                window.history.replaceState(data, title, url);
            },
            href(url) {
                window.location.href = url;
            },
            clone(instance) {
                return JSON.parse(JSON.stringify(instance));
            },

            handleError(error) {

                const response = error.response;
                let message = error.toString();

                if (response) {
                    switch (response.status) {
                        case 400:
                            handle400(this, error);
                            return;
                        case 401:
                            handle401(this);
                            return;
                        case 403:
                            message = `You are not authenticated`;
                            break;
                        case 404:
                            message = `Record or service [${response.config.url}] not found.`;
                            break;
                        case 500:
                            message = response.data.content || response.statusText || `Internal Server Error`;
                            break;
                        default:
                            message = `Status Code: ${response.status}`;
                            break;
                    }
                }
                alert(message);
                //this.$bvToast.toast(message, { title: 'Unhandled Error', variant: 'danger', noAutoHide: true });
            },

            getErrorMessageAsHtml(error) {

                let err = error;
                let resp = err.response;
                let errorMessage = '';

                if (!err.response)
                    return error.message;


                if (resp.status === 500) {
                    return resp.data.content;
                }


                if (resp.status === 401) {
                    const url = `${window.location.pathname}${window.location.search}`;
                    const returnUrl = encodeURIComponent(url);
                    const redirect = `/identity/account/login/?returnUrl=${returnUrl}`;
                    errorMessage = `You are not authorized or your session timed out. Please click <a href='${redirect}' class='btn btn-link'>here</a> to login.`;

                }
                else if (resp.status === 400) {
                    let errors = err.response.data.errors;

                    errorMessage = err.response.data.title;
                    errorMessage += '<ul>';

                    for (let k in errors) {
                        let key = errors[k];

                        for (let n in errors[k]) {
                            let val = key[n];
                            errorMessage += `<li>${val}</li>`;
                        }

                    }
                    errorMessage += '</ul>';

                }

                if (typeof resp.data === 'string') {
                    errorMessage = err.response.data;
                }

                return errorMessage;

            },

            setWithExpiry(key, value, ttl) {
                const now = new Date()

                // `item` is an object which contains the original value
                // as well as the time when it's supposed to expire
                const item = {
                    value: value,
                    expiry: now.getTime() + ttl
                }
                localStorage.setItem(key, JSON.stringify(item))
            },

            getWithExpiry(key) {
                const itemStr = localStorage.getItem(key)

                // if the item doesn't exist, return null
                if (!itemStr) {
                    return null
                }

                const item = JSON.parse(itemStr)
                const now = new Date()

                // compare the expiry time of the item with the current time
                if (now.getTime() > item.expiry) {
                    // If the item is expired, delete the item from storage
                    // and return null
                    localStorage.removeItem(key)
                    return null
                }
                return item.value
            }

        };

        app.config.globalProperties.$bus = eventBus;
    }
}