<template>
  <dialog-body v-model="showModal" :title="titleDlg" @accept="onAccept" @cancel="onCancel">
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
                <v-text-field v-model="product.Name" required placeholder="Nombre" />
              </div>
            </v-col>
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Codigo de Barras</v-label>
                <v-text-field v-model="product.SerialNumber" :rules="lastRules" required
                  placeholder="Codigo de Barras" />
              </div>
            </v-col>

            <!-- Campo Precio -->
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Precio</v-label>
                <v-text-field v-model="product.Price" type="number" step="0.01" min="0" required
                  placeholder="Ej: 15.50" />
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
import { ref } from 'vue'

const showModal = ref(false)
const modeDlg = ref('')
const titleDlg = ref('')
const product = ref({})
//const prodResult = ref(null)

let _resolve = null

// function insert() {
//   // tu lógica de Insert
// }

// function update() {
//   // tu lógica de Update
// }

// function remove() {
//   // tu lógica de Delete
// }

async function openForm(mode, item = null) {
  modeDlg.value = mode

  switch (mode) {
    case "Insert":
      titleDlg.value = "Nuevo Producto"
      product.value = { NroProduct: 0 }
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

function onAccept() {
  _resolve({ ...product.value })
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
