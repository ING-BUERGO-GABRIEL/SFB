<template>
  <header-bar>
    <search-field style="max-width:300px; width:100%" @search="onSearch" />
    <v-spacer />
    <v-btn color="primary" @click="barcodeScannerRef.openForm()">Escanear</v-btn>
    <v-btn color="primary" @click="emit('crear-producto', 'Insert')">Crear Producto</v-btn>
    <BarcodeScanner ref="barcodeScannerRef" />
  </header-bar>
</template>

<script setup>
import { inject, ref } from 'vue';
const { productServ } = inject('services')
import BarcodeScanner from './BarcodeScanner.vue'
const barcodeScannerRef = ref(null)

const emit = defineEmits(['crear-producto']);

const onSearch = async (filtro) => {
  productServ.pageParams.pageNumber = 1
  productServ.pageParams.filter = filtro
  await productServ.loadPage()
}

</script>
