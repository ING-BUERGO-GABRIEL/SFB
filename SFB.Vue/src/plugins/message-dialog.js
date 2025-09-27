// plugin/messagePlugin.js
import { ref } from 'vue';

export const messagePlugin = {
    install: (app) => {
        const confirmation = ref({
            active: false,
            title: '',
            message: '',
            type: '',
            resolve: null,
        });

        const openDialog = (type, title, message) => {
            return new Promise((resolve) => {
                confirmation.value.active = true;
                confirmation.value.type = type;
                confirmation.value.title = title;
                confirmation.value.message = message;
                confirmation.value.resolve = (value) => {
                    confirmation.value.active = false;
                    resolve(value);
                };
            });
        };

        // Atajos específicos
        const confirm = (title, message) => openDialog('confirm', title, message);
        const question = (title, message) => openDialog('question', title, message);
        const error = (title, message) => openDialog('error', title, message);
        const info = (title, message) => openDialog('info', title, message);

        app.provide('MsgDialog', {
            confirmation,
            confirm,
            question,
            error,
            info,
        });
    },
};
