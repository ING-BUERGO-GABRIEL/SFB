<template>
    <v-dialog v-model="confirmation.active" max-width="500" style="z-index:2500">
        <v-card>
            <v-card-title>{{ confirmation.title }}</v-card-title>
            <v-card-text v-html="confirmation.message"></v-card-text>
            <v-card-actions>
                <v-btn color="primary" variant="flat" @click="confirmation.resolve(true)">Confirmar</v-btn>
                <v-btn @click="confirmation.resolve(false)">Cancelar</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
    import { inject } from 'vue';

    export default {
        setup: () => {
            const { confirmation } = inject('MsgDialog');
            return { confirmation };
        }
    };
</script>

