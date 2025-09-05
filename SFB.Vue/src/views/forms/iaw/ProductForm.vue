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
                <v-text-field v-model="product.Name" :rules="[rRequired]" required placeholder="Nombre" />
              </div>
            </v-col>
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Codigo de Barras</v-label>
                <v-text-field v-model="product.SerialNumber"
                  placeholder="Codigo de Barras" />
              </div>
            </v-col>

            <!-- Campo Precio -->
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Precio</v-label>
                <v-text-field v-model="product.Price" :rules="[rRequired, rNonNegative]" type="number" step="0.01"
                  min="0" required placeholder="Ej: 15.50" />
              </div>
            </v-col>
            <!-- Checkboxes Positivo / Negativo -->
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-checkbox v-model="product.IsPurchases" label="Habilitado para Copras" color="primary"
                  class="mt-2"></v-checkbox>
              </div>
            </v-col>
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-checkbox v-model="product.IsSales" label="Habilitado para Ventas" color="primary"
                  class="mt-2"></v-checkbox>
              </div>
            </v-col>
          </v-row>
        </parent-card>
      </v-col>
      <!-- <v-col cols="12" md="6">
        <h3>Hola</h3>
      </v-col> -->
    </v-row>
  </dialog-body>
</template>

<script setup>
import { ref, inject } from 'vue'
const { productServ } = inject('services')

const showModal = ref(false)
const modeDlg = ref('')
const titleDlg = ref('')
const product = ref({})

const rRequired = v => (v !== null && v !== undefined && v !== '') || 'Campo requerido'
const rNonNegative = v => (v !== '' && v != null && Number(v) > 0) || 'Debe ser > 0'

let _resolve = null

async function openForm(mode, item = null) {
  modeDlg.value = mode

  switch (mode) {
    case "Insert":
      titleDlg.value = "Nuevo Producto"
      product.value = { NroProduct: 0, IsPurchases: true, IsSales: true }
      showModal.value = true
      break;
    case "Update":
      titleDlg.value = "Editar Producto"
      product.value = item
      showModal.value = true
      break;
    case "Delete":

      showModal.value = true
      break;
  }

  return new Promise((resolve) => {
    _resolve = resolve
  })
}

async function onAccept() {
  switch (modeDlg.value) {
    case "Insert": {
      const newProd = await productServ.create(product.value)
      if (!newProd) return;  
      product.value = newProd;
      break;
    } case "Update": {

      break;
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

// expongo los tres métodos al consumidor del componente
defineExpose({
  openForm
})
</script>
