<template>
  <dialog-body v-model="showModal" :title="titleDlg" formValidate @accept="onAccept" @cancel="onCancel">
    <template #body>
      <v-row class="mb-0" justify="center">
        <v-col cols="12" md="6">
          <parent-card :showHeader="true" title="Datos de Producto">
            <v-row class="pa-4">
              <v-col cols="12" sm="6" class="py-0">
                <div class="mb-6">
                  <v-text-field label="Codigo" base-color="red" disabled v-model="product.NroProduct" required />
                </div>
              </v-col>
              <v-col cols="12" sm="6" class="py-0">
                <div class="mb-6">
                  <v-text-field v-model="product.Name" label="Nombre" :rules="[rRequired]" required placeholder="Nombre"
                    :disabled="isReadOnly" />
                </div>
              </v-col>
              <v-col cols="12" sm="6" class="py-0">
                <div class="mb-6">
                  <v-text-field v-model="product.SerialNumber" label="Codigo de Barras" :disabled="isReadOnly">
                    <template #append-inner>
                      <BarcodeScanner @code-scan="product.SerialNumber = $event" />
                    </template>
                  </v-text-field>
                </div>
              </v-col>

              <!-- Campo Precio -->
              <v-col cols="12" sm="6" class="py-0">
                <div class="mb-6">
                  <v-text-field v-model="product.Price" label="Precio" :rules="[rRequired, rNonNegative]" type="number"
                    step="0.01" min="0" required placeholder="Ej: 15.50" :disabled="isReadOnly" />
                </div>
              </v-col>

              <v-col cols="12" sm="6" class="py-0">
                <div class="mb-6">
                  <v-select v-model="product.PresentCode" label="Presentación" :items="cmbPresent" :rules="[rRequired]"
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
                  <v-checkbox v-model="product.IsPurchases" label="Habilitado para Compras" color="primary" class="mt-2"
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
          <title-card title="Precios y Present." class-name="px-0 pb-0 rounded-md">
            <template #title-right>
              <v-btn color="primary" variant="tonal" size="small" @click="addProductPresent">
                Agregar presentacion
              </v-btn>
            </template>
            <ui-table :headers="headers" :items="product.ProductPresent" itemKey="NroProduct">
              <template #item.PresentCode="{ item }">
                <v-select v-model="item.PresentCode" :items="cmdProductPresent" item-title="Name" item-value="Code"
                  placeholder="Seleccionar presentación" :rules="[rRequired, rUniquePresent]" />
              </template>
              <template #item.QtyProduct="{ item }">
                <v-text-field v-model="item.QtyProduct" :rules="[rRequired, rNonNegative]" type="number" step="1"
                  min="0" required placeholder="00.00" />
              </template>
              <template #item.Price="{ item }">
                <v-text-field v-model="item.Price" :rules="[rRequired, rNonNegative]" type="number" step="1" min="0"
                  required placeholder="00.00" />
              </template>
              <template #item.SerialNumber="{ item }">
                <v-text-field v-model="item.SerialNumber" placeholder="Codigo de Barras">
                  <template #append-inner>
                    <BarcodeScanner @code-scan="item.SerialNumber = $event" />
                  </template>
                </v-text-field>
              </template>
              <template #item.Actions="{ item }">
                <v-btn icon variant="text" color="error" size="small" @click="deleteProductPresent(item)">
                  <ui-icon name="DeleteOutlined" size="18" />
                </v-btn>
              </template>
            </ui-table>
          </title-card>
        </v-col>
      </v-row>
    </template>
  </dialog-body>
</template>

<script setup>
import { ref, inject, computed } from 'vue'
const { productServ, uiStore } = inject('services')
import BarcodeScanner from './BarcodeScanner.vue'
const { question } = inject('MsgDialog')
import { message } from 'ant-design-vue'

const showModal = ref(false)
const modeDlg = ref('')
const titleDlg = ref('')
const product = ref({})

const rRequired = v => (v !== null && v !== undefined && v !== '') || 'Campo requerido'
const rNonNegative = v => (v !== '' && v != null && Number(v) > 0) || 'Debe ser > 0'
const rUniquePresent = value => {
  const productPresent = product.value?.ProductPresent ?? [];
  const count = productPresent.filter(item => item.PresentCode === value).length;
  return count <= 1 || 'Esta presentación ya ha sido agregada.';
}

const isReadOnly = computed(() => modeDlg.value === 'Delete')

const headers = [
  { title: 'PRESENTACION', key: 'PresentCode' },
  { title: 'CANTIDAD', key: 'QtyProduct', class: 'px-0' },
  { title: 'PRECIO', key: 'Price', class: 'px-0' },
  { title: 'COD.BARRAS', key: 'SerialNumber' },
  { title: 'ACT.', key: 'Actions' },
]

const metadata = ref({
  CmbPresent: [],
})

const cmdProductPresent = computed(() => {
  return metadata.value.CmbPresent.filter(p => p.Code !== product.value.PresentCode)
})


const cmbPresent = computed(() => {
  const presentCodes = (product.value?.ProductPresent || []).map(p => p.PresentCode);
  return (metadata.value?.CmbPresent || []).filter(p => !presentCodes.includes(p.Code));
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

const addProductPresent = () => {

  if (!product.value.PresentCode) {
    message.warning("Debe seleccionar una presentación")
    return
  }
  product.value.ProductPresent.push(productPresentDefault())
}

const deleteProductPresent = (item) => {
  const index = product.value.ProductPresent.indexOf(item)
  if (index === -1) return false

  product.value.ProductPresent.splice(index, 1)
  return true
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
      product.value = JSON.parse(JSON.stringify(item));
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
