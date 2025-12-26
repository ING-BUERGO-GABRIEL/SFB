<template>
  <header-bar>
    <search-field style="max-width:300px; width:100%" @search="onSearch" />
    <v-spacer />
    <v-btn color="primary" @click="emit('create', 'Insert')">Registrar compra</v-btn>
  </header-bar>
</template>

<script setup>
import { inject } from 'vue'
const { purchaseServ } = inject('services')

const emit = defineEmits(['create'])

const onSearch = async (filtro) => {
  purchaseServ.pageParams.pageNumber = 1
  purchaseServ.pageParams.filter = filtro
  await purchaseServ.loadPage()
}
</script>
