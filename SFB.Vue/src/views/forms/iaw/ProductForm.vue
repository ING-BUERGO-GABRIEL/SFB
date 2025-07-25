<template>
<dialog-body v-model="showModal" >
  <h3>Holasdsdas</h3>
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


