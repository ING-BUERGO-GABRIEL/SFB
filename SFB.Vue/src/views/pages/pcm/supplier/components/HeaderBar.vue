<template>
  <header-bar>
    <search-field style="max-width:300px; width:100%" @search="onSearch" />
    <v-spacer />
    <v-btn color="primary" @click="emit('create', 'Insert')">Crear Proveedor</v-btn>
  </header-bar>
</template>

<script setup>
import { inject } from 'vue';
const { supplierServ } = inject('services')

const emit = defineEmits(['create']);

const onSearch = async (filtro) => {
  supplierServ.pageParams.pageNumber = 1
  supplierServ.pageParams.filter = filtro
  await supplierServ.loadPage()
}

</script>
