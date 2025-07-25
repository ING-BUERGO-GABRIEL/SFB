<template>
  <header-bar>
    <search-field v-model="filtro" style="max-width:300px; width:100%" />
    <v-spacer />
    <v-btn color="primary">Crear Producto</v-btn>
  </header-bar>
</template>

<script setup>
import { ref } from 'vue';

const filtro = ref('');

</script>
