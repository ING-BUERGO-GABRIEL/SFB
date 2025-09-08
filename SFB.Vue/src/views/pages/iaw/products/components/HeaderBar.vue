<template>
  <header-bar>
    <search-field style="max-width:300px; width:100%" @search="onSearch" />
    <v-spacer />
    <v-btn color="primary" @click="emit('crear-producto', 'Insert')">Crear Producto</v-btn>
  </header-bar>
</template>

<script setup>
import { inject } from 'vue';
const { productServ } = inject('services')

const emit = defineEmits(['crear-producto']);

const onSearch = async (filtro) => {
  productServ.pageParams.pageNumber = 1
  productServ.pageParams.filter = filtro
  await productServ.loadPage()
}

</script>
