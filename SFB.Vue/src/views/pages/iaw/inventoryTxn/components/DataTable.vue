<!-- eslint-disable vue/valid-v-slot -->
<template>
  <title-card title="Transacciones de Inventario" class-name="px-0 pb-0 rounded-md">
    <pag-table :headers="headers" :service="invTxnServ">
      <template #item.Status="{ value }">
        <v-chip variant="text" size="small" class="px-0">
          <v-avatar size="8" :color="value.Code === 'ACT'? 'success' : 'error'" variant="flat" class="mr-2" />
          <p class="text-h6 mb-0">{{ value.Name }}</p>
        </v-chip>
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

const emit = defineEmits(['edit', 'delete'])
const { invTxnServ } = inject('services')

const formatDate = (dateString) => {
  const date = new Date(dateString);
  const day = String(date.getDate()).padStart(2, '0');
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const year = date.getFullYear();
  return `${day}/${month}/${year}`;
};

const headers = ref([
  { title: 'ID TXN.', key: 'TxnId' },
  { title: 'MODULO ORIGEN', key: 'ModOrigin' },
  { title: 'TXN ORIGEN', key: 'TxnOrigin', align: 'end' },
  { title: 'TIPO', key: 'NameType' },
  { title: 'ALM. ORIGEN', key: 'WarehouseOrigin.Name' },
  { title: 'ALM. DESTINO', key: 'WarehouseDest.Name' },
  { title: 'ESTADO', key: 'Status' },
  {
    title: 'FECHA',
    key: 'DateReg',
    align: 'center',
    value: (item) => formatDate(item.DateReg)
  },
  { title: 'ACCIONES', key: 'actions', sortable: false, align: 'center' }
])
</script>
