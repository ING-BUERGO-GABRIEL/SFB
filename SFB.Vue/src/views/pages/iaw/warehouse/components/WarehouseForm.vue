<template>
  <card-dialog v-model="showModal" :title="titleDlg" height="350"  formValidate @accept="onAccept" @cancel="onCancel">
    <v-row class="pa-4 pb-0">
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Codigo</v-label>
          <v-text-field disabled v-model="model.WarehouseId" required placeholder="Codigo" />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Nombre</v-label>
          <v-text-field v-model="model.Name" :rules="[rRequired]" required placeholder="Nombre"
             />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Localizacion</v-label>
          <v-text-field v-model="model.Location" placeholder="Lugar del almacen" />
        </div>
      </v-col>
    </v-row>
  </card-dialog>
</template>

<script setup>
import { ref, inject } from 'vue'
const { warehouseServ } = inject('services')
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
      titleDlg.value = 'Nuevo Almacen'
      model.value = { WarehouseId: 0 }
      showModal.value = true
      break
    case 'Update':
      titleDlg.value = 'Editar Almacen'
      model.value = { ...item }
      showModal.value = true
      break
    case 'Delete': {
      const confirmed = await question(
        'Eliminar Almacen',
        `Esta acción eliminará el Almacen (${item.Name}). ¿Desea continuar?`
      )

      if (!confirmed) return null

      const ok = await warehouseServ.remove(item.WarehouseId)
      if (!ok) return
      const idx = warehouseServ.pageData.Data.findIndex(p => p.WarehouseId === item.WarehouseId)
      if (idx !== -1) {
        warehouseServ.pageData.Data.splice(idx, 1)
        warehouseServ.pageData.TotalCount--
        if (warehouseServ.pageData.Data.length === 0 && warehouseServ.pageData.TotalCount > 0) {
          await warehouseServ.loadPage()
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
  switch (modeDlg.value) {
    case 'Insert': {
      const newProd = await warehouseServ.create(model.value)
      if (!newProd) return
      model.value = newProd
      warehouseServ.pageData.Data.unshift(newProd)
      warehouseServ.pageData.TotalCount++
      break
    }
    case 'Update': {
      const updWare = await warehouseServ.update(model.value)
      if (!updWare) return
      const idx = warehouseServ.pageData.Data.findIndex(p => p.WarehouseId === updWare.WarehouseId)
      if (idx !== -1) warehouseServ.pageData.Data[idx] = updWare
      break
    }

  }

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
