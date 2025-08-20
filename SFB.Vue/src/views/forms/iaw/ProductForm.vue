<template>
  <dialog-body v-model="showModal" title="Producto">
    <v-row class="mb-0" justify="center">
      <v-col cols="12" md="6">
        <parent-card :showHeader="true" title="Datos de Producto">
          <v-row class="pa-4">
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Codigo</v-label>
                <v-text-field v-model="firstname" :rules="firstRules" required placeholder="Codigo"/>
              </div>
            </v-col>
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Nombre</v-label>
                <v-text-field v-model="lastname" :rules="lastRules" required placeholder="Nombre"/>
              </div>
            </v-col>
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Codigo de Barras</v-label>
                <v-text-field v-model="lastname" :rules="lastRules" required
                  placeholder="Codigo de Barras"/>
              </div>
            </v-col>

            <!-- Campo Precio -->
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-label>Precio</v-label>
                <v-text-field v-model="price" type="number" step="0.01" min="0" required
                  placeholder="Ej: 15.50"/>
              </div>
            </v-col>
            <!-- Checkboxes Positivo / Negativo -->
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-checkbox v-model="isPositive" label="Habilitado para Copras" color="primary"
                  class="mt-2"></v-checkbox>
              </div>
            </v-col>
            <v-col cols="12" sm="6" class="py-0">
              <div class="mb-6">
                <v-checkbox v-model="isPositive" label="Habilitado para Ventas" color="primary"
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
function insert() {
  // tu lógica de Insert
}

function update() {
  // tu lógica de Update
}

function remove() {
  // tu lógica de Delete
}

// expongo los tres métodos al consumidor del componente
defineExpose({
  insert,
  update,
  delete: remove
})
</script>
