<template>
  <HeaderBar @create="() => refPostForm.openForm('Insert')" />
  <v-row class="mb-0">
    <v-col cols="12" md="8">
      <DataTable @edit="item => showModal('Update', item)" />
    </v-col>
    <v-col cols="12" md="4">
      <AnalyticsReport />
    </v-col>
  </v-row>
  <SalesTxnForm ref="refForm" />
  <SalesPostForm ref="refPostForm" />
</template>

<script setup>
import { ref } from 'vue'
import SalesTxnForm from './components/SalesTxnForm.vue'
import SalesPostForm from './components/SalesPostForm.vue'
import HeaderBar from './components/HeaderBar.vue'
import DataTable from './components/DataTable.vue'
import AnalyticsReport from './components/AnalyticsReport.vue'

const refForm = ref(null)
const refPostForm = ref(null)

async function showModal(mode, item = null) {
  await refForm.value.openForm(mode, item)
}


</script>
