<template>
  <dialog-body v-model="showModal" title="Producto">
    <v-row class="mb-0">
      <v-col cols="12" md="6">
        <parent-card :showHeader="false">
          <v-col cols="12" sm="6" class="py-0">
            <div class="mb-6">
              <v-label>First Name*</v-label>
              <v-text-field v-model="firstname" :rules="firstRules" hide-details="auto" required variant="outlined"
                class="mt-2" color="primary" placeholder="John"></v-text-field>
            </div>
          </v-col>
          <v-col cols="12" sm="6" class="py-0">
            <div class="mb-6">
              <v-label>Last Name*</v-label>
              <v-text-field v-model="lastname" :rules="lastRules" hide-details="auto" required variant="outlined"
                class="mt-2" color="primary" placeholder="Doe"></v-text-field>
            </div>
          </v-col>
        </parent-card>
      </v-col>
      <v-col cols="12" md="6">
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
