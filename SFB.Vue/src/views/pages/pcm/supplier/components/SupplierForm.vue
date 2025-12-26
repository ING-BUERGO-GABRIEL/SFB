<template>
  <card-dialog v-model="showModal" :title="titleDlg" height="350" formValidate @accept="onAccept" @cancel="onCancel">
    <v-row class="pa-4 pb-0">
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Codigo</v-label>
          <v-text-field disabled v-model="model.SupplierId" required placeholder="Codigo" />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Nombre</v-label>
          <v-text-field v-model="model.Name" :rules="[rRequired]" required placeholder="Nombre" />
        </div>
      </v-col>
      <v-col cols="12" class="py-0">
        <div class="mb-6">
          <v-label>Dirección</v-label>
          <v-text-field v-model="model.Address" :rules="[rRequired]" placeholder="Dirección del proveedor" />
        </div>
      </v-col>
    </v-row>
  </card-dialog>
</template>

<script setup>
import { ref, inject } from 'vue'
const { supplierServ, uiStore } = inject('services')
const { question } = inject('MsgDialog')

const showModal = ref(false)
const modeDlg = ref('')
const titleDlg = ref('')
const model = ref({})

const rRequired = v => (v !== null && v !== undefined && v !== '') || 'Campo requerido'

let _resolve = null

async function openForm(mode, item = null) {
  modeDlg.value = mode

  switch (mode) {
    case 'Insert':
      titleDlg.value = 'Nuevo Proveedor'
      model.value = { SupplierId: 0 }
      showModal.value = true
      break
    case 'Update':
      titleDlg.value = 'Editar Proveedor'
      model.value = { ...item }
      showModal.value = true
      break
    case 'Delete': {
      const confirmed = await question(
        'Eliminar Proveedor',
        `Esta acción eliminará el Proveedor (${item.Name}). ¿Desea continuar?`
      )

      if (!confirmed) return null

      const ok = await supplierServ.remove(item.SupplierId)
      if (!ok) return
      const idx = supplierServ.pageData.Data.findIndex(p => p.SupplierId === item.SupplierId)
      if (idx !== -1) {
        supplierServ.pageData.Data.splice(idx, 1)
        supplierServ.pageData.TotalCount--
        if (supplierServ.pageData.Data.length === 0 && supplierServ.pageData.TotalCount > 0) {
          await supplierServ.loadPage()
        }
      }

      break
    }
  }

  return new Promise(resolve => {
    _resolve = resolve
  })
}

async function onAccept() {
  uiStore.isLoadingBody = true
  switch (modeDlg.value) {
    case 'Insert': {
      const newSupplier = await supplierServ.create(model.value)
      uiStore.isLoadingBody = false
      if (!newSupplier) return
      model.value = newSupplier
      supplierServ.pageData.Data.unshift(newSupplier)
      supplierServ.pageData.TotalCount++
      break
    }
    case 'Update': {
      const updSupplier = await supplierServ.update(model.value)
      uiStore.isLoadingBody = false
      if (!updSupplier) return
      const idx = supplierServ.pageData.Data.findIndex(p => p.SupplierId === updSupplier.SupplierId)
      if (idx !== -1) supplierServ.pageData.Data[idx] = updSupplier
      break
    }

  }
  uiStore.isLoadingBody = false
  _resolve(model.value)
  _resolve = null
  showModal.value = false
}

function onCancel() {
  _resolve(null)
  _resolve = null
  showModal.value = false
}

defineExpose({
  openForm
})
</script>
