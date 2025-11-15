<script setup lang="ts">
import { storeToRefs } from 'pinia';
import { useUIStore } from '@/stores/ui';

const uiStore = useUIStore();
const { isLoadingBody } = storeToRefs(uiStore);
</script>

<template>
  <transition name="body-loader-fade">
    <div v-if="isLoadingBody" class="body-loader" role="status" aria-live="polite">
      <div class="body-loader__bar">
        <div class="body-loader__indicator" />
      </div>
    </div>
  </transition>
</template>

<style scoped>
.body-loader {
  position: absolute;
  inset: 0;
  z-index: 1000;
  display: flex;
  flex-direction: column;
  pointer-events: all;
}

.body-loader::after {
  content: '';
  flex: 1 1 auto;
  background-color: rgba(255, 255, 255, 0.65);
  backdrop-filter: blur(1px);
}

.body-loader__bar {
  position: relative;
  height: 4px;
  width: 100%;
  overflow: hidden;
  background-color: rgba(0, 0, 0, 0.08);
}

.body-loader__indicator {
  position: absolute;
  top: 0;
  left: -40%;
  height: 100%;
  width: 40%;
  background-color: rgb(var(--v-theme-primary));
  animation: body-loading 1.35s ease-in-out infinite;
}

@keyframes body-loading {
  0% {
    left: -40%;
    width: 40%;
  }
  50% {
    left: 30%;
    width: 40%;
  }
  100% {
    left: 100%;
    width: 40%;
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
