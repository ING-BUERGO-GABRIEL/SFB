<template>
  <HeaderBar>
    <Search v-model="filtro" style="max-width:300px; width:100%" />
    <v-spacer />
    <v-btn color="primary">Crear Producto</v-btn>
  </HeaderBar>
</template>

<script setup>
import { ref } from 'vue';
import HeaderBar from '@/components/shared/HeaderBar.vue';
import Search from '@/components/shared/Search.vue';

const filtro = ref('');

</script>
