<template>
  <header-bar>
    <search-field style="max-width:300px; width:100%" @search="onSearch" />
    <v-spacer />
    <v-btn color="primary" @click="emit('create', 'Insert')">Crear Cliente</v-btn>
  </header-bar>
</template>

<script setup>
import { inject } from 'vue'
const { customerServ } = inject('services')

const emit = defineEmits(['create'])

const onSearch = async (filtro) => {
  customerServ.pageParams.pageNumber = 1
  customerServ.pageParams.filter = filtro
  await customerServ.loadPage()
}
</script>
