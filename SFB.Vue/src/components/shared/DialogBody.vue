<template>
  <div v-if="modelValue" class="full-cover" >
    <v-card variant="outlined" elevation="0" class="modal-card d-flex flex-column bg-surface">
      <!-- Header -->
      <v-card-title class="d-flex align-center bg-containerBg">
        <span>{{ props.title }}</span>
        <v-spacer />
        <v-avatar size="36" @click="close">
          <component :is="CloseOutlined" :style="{ fontSize: '16px' }" />
        </v-avatar>
      </v-card-title>
      <!-- Body -->
      <v-card-text class="modal-body">
        <slot></slot>
      </v-card-text>
      <!-- footer -->
      <v-card-actions class="bg-containerBg">
        <v-spacer/>
         <v-btn color="primary"  variant="elevated">Aceptar</v-btn>
          <v-btn color="lightprimary"  variant="elevated" @click="close">Cancelar</v-btn>
      </v-card-actions>
    </v-card>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'
import { CloseOutlined } from '@ant-design/icons-vue';
const props = defineProps({
  modelValue: { type: Boolean, default: false },
  title: { type: String, default: '' },
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
  display: flex;
  flex-direction: column;
  width: 100%;
  height: calc(100vh - 60px);
  margin: 0;
}

.modal-body {
  padding-top: 20px;
  flex: 1 1 auto;
  overflow-y: auto;
}
</style>
