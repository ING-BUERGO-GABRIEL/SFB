<template>
  <header-bar>
    <search-field v-model="filtro" style="max-width:300px; width:100%" />
    <v-spacer />
    <v-btn color="primary" @click="$emit('crear-producto')">Crear Producto</v-btn>
  </header-bar>
</template>

<script setup>
import { ref } from 'vue';

const filtro = ref('');

const emit = defineEmits(['crear-producto']);

</script>
