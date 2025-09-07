<template>
  <title-card title="Lista Productos" class-name="px-0 pb-0 rounded-md">
    <pag-table :headers="headers" :service="productServ">
      <!-- ESTE slot ya funciona -->
      <template #item.IsSales="{ value }">
        <v-chip variant="text" size="small" class="px-0">
          <v-avatar
            size="8"
            :color="value ? 'success' : 'error'"
            variant="flat"
            class="mr-2"
          />
          <p class="text-h6 mb-0">{{ value ? 'Habilitado' : 'Deshabilitado' }}</p>
        </v-chip>
      </template>
    </pag-table>
  </title-card>
</template>

<script setup>
import { inject, ref } from 'vue'
const { productServ } = inject('services')

const headers = ref([
  { title: 'ID PROD.', key: 'NroProduct' },
  { title: 'NOMBRE PRO.', key: 'Name' },
  { title: 'TOTAL STOCK', key: 'TotalStock', align: 'end' },
  { title: 'VENTAS', key: 'IsSales' },
  { title: 'PRECIO Bs.', key: 'Price', align: 'end' }
])
</script>
