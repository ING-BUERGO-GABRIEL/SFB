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

.body-loader__scrim {
  position: absolute;
  inset: 0;
  background: rgba(0, 0, 0, 0.118);
  pointer-events: all;
  cursor: default;
  z-index: 1; /* scrim debajo de la barra */
}

/* 🔹 Contenedor de la barra (equivalente a .page-loader) */
.body-loader__progress {
  position: absolute;
  top: 0;
  left: 0;
  height: 5px;
  z-index: 2;              /* por encima del scrim */
  pointer-events: none;
  animation: loading 2000ms ease-in-out;
  animation-iteration-count: infinite;
}

/* 🔹 La barra azul (equivalente a .bar) */
.body-loader__progress-bar {
  height: 100%;
  width: 100%;
  background-color: rgb(var(--v-theme-primary));
}

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
