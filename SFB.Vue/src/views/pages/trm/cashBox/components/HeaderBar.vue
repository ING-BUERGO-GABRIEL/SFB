<template>
  <header-bar>
    <search-field style="max-width:300px; width:100%" @search="onSearch" />
    <v-spacer />
    <v-btn color="primary" @click="emit('create', 'Insert')">Crear Caja</v-btn>
  </header-bar>
</template>

<script setup>
import { inject } from 'vue'

const { cashBoxServ } = inject('services')

const emit = defineEmits(['create'])

const onSearch = async filtro => {
  cashBoxServ.pageParams.pageNumber = 1
  cashBoxServ.pageParams.filter = filtro
  await cashBoxServ.loadPage()
}
</script>
