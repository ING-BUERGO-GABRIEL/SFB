<template>
  <HeaderBar>
    <Search v-model="filtro" style="max-width:300px; width:100%" />
    <v-spacer />
    <v-btn color="primary">Crear Producto</v-btn>
  </HeaderBar>

  <div class="pa-4">
      <v-btn small prepend-icon="mdi-cog" @click="overlay = true">
        Settings
      </v-btn>
    </div>
    <div v-if="overlay" class="full-cover" @click="overlay = false">
      <div class="modal-card" @click.stop>
        <h1>Hola</h1>
      </div>
    </div>
</template>

<script setup>
import { ref } from 'vue';
import HeaderBar from '@/components/shared/HeaderBar.vue';
import Search from '@/components/shared/Search.vue';

const filtro = ref('');
const overlay = ref(false)

</script>

<style scoped>

.full-cover {
  position: absolute;
  inset: 0;
  background: rgba(0, 0, 0, 0.118);
  z-index: 10;
}

.modal-card {
  background: white;
  padding: 1rem;
  border-radius: 4px;
  width: 100%;
  height: 100vh;
  margin-top: 0;
}
</style>
