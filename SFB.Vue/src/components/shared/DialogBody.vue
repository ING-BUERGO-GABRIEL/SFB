<template>
  <div v-if="modelValue" class="full-cover" @click="closeModal">
    <div class="modal-card" @click.stop>
      <slot />
    </div>
  </div>
</template>

<script setup>
const { modelValue } = defineProps({
  modelValue: Boolean
})
const emit = defineEmits(['update:modelValue'])

function closeModal() {
  emit('update:modelValue', false)
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
