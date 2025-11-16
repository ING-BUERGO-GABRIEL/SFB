<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { useUIStore } from '@/stores/ui';

const uiStore = useUIStore();
const { isLoadingBody } = storeToRefs(uiStore);
</script>

<template>
  <transition name="body-loader-fade">
    <div v-if="isLoadingBody" class="body-loader" role="presentation">
      <div class="body-loader__scrim" />
      <div class="body-loader__progress" aria-hidden="true">
        <div class="body-loader__progress-bar" />
      </div>
    </div>
  </transition>
</template>

<style scoped>
.body-loader {
  position: absolute;
  inset: 0;
  z-index: 1000;
}

/* Fondo gris que bloquea la edición */
.body-loader__scrim {
  position: absolute;
  inset: 0;
  background: rgba(0, 0, 0, 0.118);
  pointer-events: all;
  cursor: default;
  z-index: 1;
}

/* 🔹 Contenedor de la barra: se comporta como .page-loader */
.body-loader__progress {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;          /* igual que .page-loader */
  height: 5px;
  z-index: 2;           /* por encima del scrim */
  opacity: 1;
  pointer-events: none;
  animation: loading 2000ms ease-in-out;
  animation-iteration-count: infinite;
  overflow: hidden;
}

/* 🔹 La barra azul: se comporta como .bar */
.body-loader__progress-bar {
  background-color: rgb(var(--v-theme-primary));
  height: 5PX;
  width: 100%;
}

/* misma animación que en LoaderWrapper */
@keyframes loading {
  0% {
    width: 0;
    left: 0;
  }
  50% {
    width: 100%;
    left: 0;
  }
  100% {
    width: 100%;
    left: 100%;
  }
}

.body-loader-fade-enter-active,
.body-loader-fade-leave-active {
  transition: opacity 150ms linear;
}

.body-loader-fade-enter-from,
.body-loader-fade-leave-to {
  opacity: 0;
}
</style>

