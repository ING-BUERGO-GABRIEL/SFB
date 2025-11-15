<template>
  <dialog-body v-model="showModal" :title="titleDlg" formValidate @accept="onAccept" @cancel="onCancel">
    <v-row class="mb-0" justify="center">
      <v-col cols="12" md="6">
        <parent-card :showHeader="true" title="Datos de Producto">
          <v-row class="pa-4">
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Codigo</v-label>
                <v-text-field disabled v-model="product.NroProduct" required placeholder="Codigo" />
              </div>
            </v-col>
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Nombre</v-label>
                <v-text-field v-model="product.Name" :rules="[rRequired]" required placeholder="Nombre"
                  :disabled="isReadOnly" />
              </div>
            </v-col>
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Codigo de Barras</v-label>
                <v-text-field v-model="product.SerialNumber" placeholder="Codigo de Barras" :disabled="isReadOnly" />
              </div>
            </v-col>

            <!-- Campo Precio -->
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Precio</v-label>
                <v-text-field v-model="product.Price" :rules="[rRequired, rNonNegative]" type="number" step="0.01"
                  min="0" required placeholder="Ej: 15.50" :disabled="isReadOnly" />
              </div>
            </v-col>

            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Presentación</v-label>
                <v-select v-model="product.PresentCode" :items="metadata.CmbPresent" :rules="[rRequired]"
                  item-title="Name" item-value="Code" placeholder="Seleccionar presentación" />
              </div>
            </v-col>

            <v-col cols="12" sm="4" class="py-0">
              <div class="mb-6">
              </div>
            </v-col>

            <!-- Checkboxes Positivo / Negativo -->
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-checkbox v-model="product.IsPurchases" label="Habilitado para Copras" color="primary" class="mt-2"
                  :disabled="isReadOnly"></v-checkbox>
              </div>
            </v-col>
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-checkbox v-model="product.IsSales" label="Habilitado para Ventas" color="primary" class="mt-2"
                  :disabled="isReadOnly"></v-checkbox>
              </div>
            </v-col>
          </v-row>
        </parent-card>
      </v-col>
      <v-col cols="12" md="6">
        <title-card title="Presentaciones y precios" class-name="px-0 pb-0 rounded-md">
          <ui-table :headers="headers" :items="product.ProductPresent" itemKey="NroProduct">

          </ui-table>
        </title-card>
      </v-col>
    </v-row>
  </dialog-body>
</template>

<script setup>
import { ref, inject, computed } from 'vue'
const { productServ, uiStore } = inject('services')
const { question } = inject('MsgDialog')

const showModal = ref(false)
const modeDlg = ref('')
const titleDlg = ref('')
const product = ref({})

const rRequired = v => (v !== null && v !== undefined && v !== '') || 'Campo requerido'
const rNonNegative = v => (v !== '' && v != null && Number(v) > 0) || 'Debe ser > 0'

const isReadOnly = computed(() => modeDlg.value === 'Delete')

const headers = [
  { title: 'PRESENTACION', key: 'PresentCode' },
  { title: 'CANTIDAD', key: 'QtyProduct' },
  { title: 'PRECIO', key: 'Name' },
  { title: 'NRO. SERIE', key: 'SerialNumber' },
  { title: 'ACT.', key: 'Name' },
]

const metadata = ref({
  CmbPresent: [],
})

const productDefault = () => {
  return {
    NroProduct: 0,
    Name: null,
    SerialNumber: null,
    Price: null,
    PresentCode: null,
    IsPurchases: true,
    IsSales: true,
    ProductPresent: []
  }
}

const productPresentDefault = () => {
  return {
    ProductId: modeDlg.value === 'Insert' ? 0 : product.value.NroProduct,
    PresentCode: null,
    QtyProduct: null,
    Price: null,
    SerialNumber: null,
  }
}


let _resolve = null

async function openForm(mode, item = null) {
  modeDlg.value = mode

  switch (mode) {
    case 'Insert':
      uiStore.isLoadingBody = true
      await loadMetadata()
      titleDlg.value = 'Nuevo Producto'
      product.value = productDefault()
      showModal.value = true
      break
    case 'Update':
      uiStore.isLoadingBody = true
      await loadMetadata()
      titleDlg.value = 'Editar Producto'
      product.value = { ...item }
      showModal.value = true
      break
    case 'Delete': {
      const confirmed = await question(
        'Eliminar Producto',
        `Esta acción eliminará el producto (${item.Name}). ¿Desea continuar?`
      )

      if (!confirmed) return null

      const ok = await productServ.remove(item.NroProduct)
      if (!ok) return
      const idx = productServ.pageData.Data.findIndex(p => p.NroProduct === item.NroProduct)
      if (idx !== -1) {
        productServ.pageData.Data.splice(idx, 1)
        productServ.pageData.TotalCount--
        if (productServ.pageData.Data.length === 0 && productServ.pageData.TotalCount > 0) {
          await productServ.loadPage()
        }
      }

      break
    }
  }

  uiStore.isLoadingBody = false

  return new Promise(resolve => {
    _resolve = resolve
  })
}

async function loadMetadata() {
  const meta = await productServ.getMetadata()
  if (meta) {
    metadata.value = meta
  }
}

async function onAccept() {
  switch (modeDlg.value) {
    case 'Insert': {
      const newProd = await productServ.create(product.value)
      if (!newProd) return
      product.value = newProd
      productServ.pageData.Data.unshift(newProd)
      productServ.pageData.TotalCount++
      break
    }
    case 'Update': {
      const updProd = await productServ.update(product.value)
      if (!updProd) return
      product.value = updProd
      const idx = productServ.pageData.Data.findIndex(p => p.NroProduct === updProd.NroProduct)
      if (idx !== -1) productServ.pageData.Data[idx] = updProd
      break
    }

  }

  _resolve(product.value)
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
