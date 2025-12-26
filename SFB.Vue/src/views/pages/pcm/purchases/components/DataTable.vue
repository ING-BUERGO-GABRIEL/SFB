<!-- eslint-disable vue/valid-v-slot -->
<template>
  <title-card title="Compras" class-name="px-0 pb-0 rounded-md">
    <pag-table :headers="headers" :service="purchaseServ">
      <template #item.Status="{ value }">
        <v-chip variant="text" size="small" class="px-0">
          <v-avatar size="8" :color="value.Code === 'ACT'? 'success' : value.Code === 'PEN' ? 'warning' : 'error'" variant="flat" class="mr-2" />
          <p class="text-h6 mb-0">{{ value.Name }}</p>
        </v-chip>
      </template>
      <template #item.GrandTotal="{ value }">
        <span>{{ formatCurrency(value) }}</span>
      </template>
      <template #item.actions="{ item }">
        <v-btn icon variant="text" color="primary" @click="emit('edit', item)">
          <EditOutlined :style="{ fontSize: '18px' }" />
        </v-btn>
      </template>
    </pag-table>
  </title-card>
</template>

<script setup>
import { inject, ref } from 'vue'
import { EditOutlined } from '@ant-design/icons-vue'

const emit = defineEmits(['edit'])
const { purchaseServ } = inject('services')

const formatCurrency = (amount) => new Intl.NumberFormat('es-BO', { style: 'currency', currency: 'BOB' }).format(amount ?? 0)

const headers = ref([
  { title: 'ID TXN.', key: 'TxnId' },
  { title: 'PROVEEDOR', key: 'Supplier.Name' },
  { title: 'ALMACÉN', key: 'Warehouse.Name' },
  { title: 'TIPO', key: 'NameType' },
  { title: 'ESTADO', key: 'Status' },
  { title: 'TOTAL', key: 'GrandTotal', align: 'end' },
  { title: 'ACCIONES', key: 'actions', sortable: false, align: 'center' }
])
</script>
