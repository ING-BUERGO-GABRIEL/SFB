<!-- eslint-disable vue/valid-v-slot -->
<template>
  <title-card title="Lista Cajas" class-name="px-0 pb-0 rounded-md">
    <pag-table :headers="headers" :service="cashBoxServ">
      <template #item.actions="{ item }">
        <v-btn icon variant="text" color="primary" @click="emit('edit', item)">
          <EditOutlined :style="{ fontSize: '18px' }" />
        </v-btn>
        <v-btn icon variant="text" color="error" @click="emit('delete', item)">
          <DeleteOutlined :style="{ fontSize: '18px' }" />
        </v-btn>
      </template>
    </pag-table>
  </title-card>
</template>

<script setup>
import { inject, ref } from 'vue'
import { EditOutlined, DeleteOutlined } from '@ant-design/icons-vue'

const emit = defineEmits(['edit', 'delete'])
const { cashBoxServ } = inject('services')

const headers = ref([
  { title: 'ID', key: 'CashBoxId' },
  { title: 'NOMBRE', key: 'Name' },
  { title: 'TIPO', key: 'Type' },
  { title: 'MONEDA', key: 'CurrencyCode' },
  { title: 'SALDO', key: 'CurrentBalance', align: 'end' },
  { title: 'ACTIVA', key: 'Active', align: 'center' },
  { title: 'ACCIONES', key: 'actions', sortable: false, align: 'center' }
])
</script>
