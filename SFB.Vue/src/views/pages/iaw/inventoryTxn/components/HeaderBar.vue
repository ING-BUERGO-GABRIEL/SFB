<template>
  <header-bar>
    <search-field style="max-width:300px; width:100%" @search="onSearch" />
    <v-spacer />
    <v-btn color="primary" @click="emit('create', 'Insert')">Crear Transaccion</v-btn>
  </header-bar>
</template>

<script setup>
import { inject } from 'vue';
const { invTxnServ } = inject('services')

const emit = defineEmits(['create']);

const onSearch = async (filtro) => {
  invTxnServ.pageParams.pageNumber = 1
  invTxnServ.pageParams.filter = filtro
  await invTxnServ.loadPage()
}

</script>
