<template>
  <!-- 1) Contenedor padre que engloba cabecera, búsqueda, tabla y reporte -->
  <div class="content-wrapper">

    <!-- HeaderBar y botón de Settings -->
    <HeaderBar />
    <div class="pa-4">
      <v-btn small prepend-icon="mdi-cog" @click="overlay = true">
        Settings
      </v-btn>
    </div>

    <!-- Tu fila con DataTable y AnalyticsReport -->
    <v-row class="mb-0">
      <v-col cols="12" md="8">
        <DataTable />
      </v-col>
      <v-col cols="12" md="4">
        <AnalyticsReport />
      </v-col>
    </v-row>

    <!-- 2) Aquí metes el div azul que cubre TODO el .content-wrapper -->
    <div
      v-if="overlay"
      class="full-cover"
      @click="overlay = false"
    >
      <!-- contenido de tu “modal” -->
      <div class="modal-card" @click.stop>
        <h1>Hola</h1>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import HeaderBar from './components/HeaderBar.vue'
import DataTable from './components/DataTable.vue'
import AnalyticsReport from './components/AnalyticsReport.vue'

const overlay = ref(false)
</script>

<style scoped>
/* 1) El wrapper debe ser position:relative */
.content-wrapper {
  position: relative;
  /* opcional: si quieres que ocupe toda la altura restante
     podrías darle height: calc(100vh - alturaDelNavYHeader) */
}

/* 2) full-cover se estira al 100% de .content-wrapper */
.full-cover {
  position: absolute;
  inset: 0;               /* top:0; right:0; bottom:0; left:0 */
  background: rgba(0,0,255,0.2);
  z-index: 10;            /* para que quede encima */
}

/* opcional: un “modal-card” centrado dentro del full-cover */
.modal-card {
  background: white;
  padding: 1rem;
  border-radius: 4px;
  width: 100%;
  height: 100vh;
  margin-top: 0;
}
</style>
