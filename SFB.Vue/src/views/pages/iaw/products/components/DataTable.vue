<!-- eslint-disable vue/valid-v-slot -->
<template>
  <title-card title="Lista Productos" class-name="px-0 pb-0 rounded-md">
    <pag-table :headers="headers" :service="productServ">
      <!-- Slot personalizado para campo de ventas -->
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

      <!-- Acciones de edición y eliminación -->
      <template #item.actions="{ item }">
        <v-btn icon variant="text" color="primary" @click="emit('edit-product', item)">
          <EditOutlined :style="{ fontSize: '18px' }" />
        </v-btn>
        <v-btn icon variant="text" color="error" @click="emit('delete-product', item)">
          <DeleteOutlined :style="{ fontSize: '18px' }" />
        </v-btn>
      </template>
    </pag-table>
  </title-card>
</template>

<script setup>
import { inject, ref } from 'vue'
import { EditOutlined, DeleteOutlined } from '@ant-design/icons-vue'

const emit = defineEmits(['edit-product', 'delete-product'])
const { productServ } = inject('services')

const headers = ref([
  { title: 'ID PROD.', key: 'NroProduct' },
  { title: 'NOMBRE PRO.', key: 'Name' },
  { title: 'TOTAL STOCK', key: 'TotalStock', align: 'end' },
  { title: 'VENTAS', key: 'IsSales' },
  { title: 'PRECIO Bs.', key: 'Price', align: 'end' },
  { title: 'ACCIONES', key: 'actions', sortable: false, align: 'center' }
])
</script>
