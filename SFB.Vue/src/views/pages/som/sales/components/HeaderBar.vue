<template>
  <header-bar>
    <search-field style="max-width:300px; width:100%" @search="onSearch" />
    <v-spacer />
    <v-btn color="primary" @click="emit('create', 'Insert')">Registrar venta</v-btn>
  </header-bar>
</template>

<script setup>
import { inject } from 'vue'
const { salesServ } = inject('services')

const emit = defineEmits(['create'])

const onSearch = async (filtro) => {
  salesServ.pageParams.pageNumber = 1
  salesServ.pageParams.filter = filtro
  await salesServ.loadPage()
}
</script>
