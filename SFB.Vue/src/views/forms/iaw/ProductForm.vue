<template>
  <dialog-body v-model="showModal" title="Producto">
    <v-row class="mb-0">
      <v-col cols="12" md="8">
        <h3>Hola</h3>
      </v-col>
      <v-col cols="12" md="4">
        <h3>Hola</h3>
      </v-col>
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
