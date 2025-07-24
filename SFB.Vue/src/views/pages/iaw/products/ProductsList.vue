<template>
    <HeaderBar />
    <v-btn color="primary" @click="closeModal">Crear Producto2</v-btn>
    <v-row class="mb-0">
      <v-col cols="12" md="8">
        <DataTable />
      </v-col>
      <v-col cols="12" md="4">
        <AnalyticsReport />
      </v-col>
    </v-row>
<DialogBody v-model="showModal" >
  <h3>Holasasas</h3>
</DialogBody>
</template>

<script setup>
import { ref } from 'vue'
import DialogBody from '@/components/shared/DialogBody.vue'
import HeaderBar from './components/HeaderBar.vue'
import DataTable from './components/DataTable.vue'
import AnalyticsReport from './components/AnalyticsReport.vue'

const showModal = ref(false)

function closeModal() {
  showModal.value = true;
}

</script>


