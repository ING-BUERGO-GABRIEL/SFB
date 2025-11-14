<template>
  <header-bar>
    <search-field style="max-width:300px; width:100%" @search="onSearch" />
    <v-spacer />
    <v-btn color="primary" @click="emit('create', 'Insert')">Crear Presentación</v-btn>
  </header-bar>
</template>

<script setup>
import { inject } from 'vue'

const emit = defineEmits(['create'])
const { presentationServ } = inject('services')

const onSearch = async (filtro) => {
  presentationServ.pageParams.pageNumber = 1
  presentationServ.pageParams.filter = filtro
  await presentationServ.loadPage()
}
</script>
