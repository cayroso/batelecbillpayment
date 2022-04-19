
import Toasts from "../Components/Toasts/Toasts.vue";
import eventBus from '../Components/EventBus/eventBus';

export default {
    install(app, options) {
        
        // attach these methods with Vue instance
        app.config.globalProperties.$toast = {
            /**
             * Send the event on channel (toast-message) with a given payload.
             *
             * @param {string} message
             * @param {string} type
             */
            send(title, message, type, options) {                
                eventBus.$emit("toast-message", {
                    title: title,
                    message: message,
                    type: type,
                    options: options
                });
            },

            /**
             * Send a success message event.
             *
             * @param {string} message
             */
            success(title, message, options = {}) {
                this.send(title, message, "success", options);
            },

            /**
             * Send a warning message event.
             *
             * @param {string} message
             */
            warning(title, message, options = {}) {
                this.send(title, message, "warning", options);
            },

            /**
             * Send an info message event.
             *
             * @param {string} message
             */
            info(title, message, options = {}) {
                this.send(title, message, "info", options);
            },

            /**
             * Send an error message event.
             *
             * @param {string} message
             */
            error(title, message, options = {}) {
                this.send(title, message, "danger", options);
            }
        };

        // register the component
        app.component("Toasts", Toasts);
    }
};