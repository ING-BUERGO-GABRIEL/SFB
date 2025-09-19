import { ref } from 'vue';

// Plugin para el confirmation
export const messagePlugin = {
    install: (app) => {

        const confirmation = ref({
            active: false,
            title: '',
            message: '',
            resolve: null,
        });

        const ask = (title, message) => {
            return new Promise((resolve) => {
                confirmation.value.active = true;
                confirmation.value.title = title;
                confirmation.value.message = message;
                confirmation.value.resolve = (value) => {
                    confirmation.value.active = false;
                    resolve(value);
                };
            });
        };

        app.provide('MsgDialog', {
            confirmation,
            ask
        });
    },
};