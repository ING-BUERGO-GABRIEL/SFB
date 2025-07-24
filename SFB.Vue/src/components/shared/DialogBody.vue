<template>
  <div v-if="modelValue" class="full-cover" @click="close">
    <div class="modal-card" @click.stop>
      <slot />
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'
const props = defineProps({
   modelValue: { type: Boolean, default: false },
})
const emit = defineEmits(['update:modelValue'])
const isOpen = ref(props.modelValue)
watch(() => props.modelValue, v => (isOpen.value = v))
watch(isOpen, v => emit('update:modelValue', v))

function close() {
    isOpen.value = false
}
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
