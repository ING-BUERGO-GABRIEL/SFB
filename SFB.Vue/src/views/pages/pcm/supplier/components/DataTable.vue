<!-- eslint-disable vue/valid-v-slot -->
<template>
  <title-card title="Lista Proveedores" class-name="px-0 pb-0 rounded-md">
    <pag-table :headers="headers" :service="supplierServ">
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
const { supplierServ } = inject('services')

const headers = ref([
  { title: 'ID PROV.', key: 'SupplierId' },
  { title: 'NOMBRE PROV.', key: 'Name' },
  { title: 'DIRECCIÓN', key: 'Address' },
  { title: 'ACCIONES', key: 'actions', sortable: false, align: 'center' }
])
</script>
