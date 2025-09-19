<template>
  <HeaderBar @crear-producto="showModal"/>
  <v-row class="mb-0">
    <v-col cols="12" md="8">
      <DataTable @edit-product="item => showModal('Update', item)"
                 @delete-product="item => showModal('Delete', item)"/>
    </v-col>
    <v-col cols="12" md="4">
      <AnalyticsReport />
    </v-col>
  </v-row>
  <ProductForm ref="productForm"/>
</template>

<script setup>
import { ref } from 'vue'
import ProductForm from '@/views/forms/iaw/ProductForm.vue'
import HeaderBar from './components/HeaderBar.vue'
import DataTable from './components/DataTable.vue'
import AnalyticsReport from './components/AnalyticsReport.vue'

const productForm = ref(null)

async function showModal(mode, item = null) {
  await productForm.value.openForm(mode, item)
}

</script>
