<template>
  <Transition name="dialog-fade">
    <div v-if="modelValue" class="full-cover">
      <v-card variant="outlined" elevation="0" class="modal-card d-flex flex-column bg-surface">
        <!-- Header -->
        <v-card-title class="d-flex align-center bg-containerBg">
          <span>{{ props.title }}</span>
          <v-spacer />
          <v-btn height="36" @click="onCancelClick" icon elevation="0">
            <component :is="CloseOutlined" :style="{ fontSize: '16px' }" />
          </v-btn>
        </v-card-title>
        <!-- Body -->
        <v-card-text class="modal-body" :class="classBody">
          <v-form ref="formRef">
            <slot></slot>
          </v-form>
        </v-card-text>
        <!-- footer -->
        <v-card-actions v-if="!hideActions" class="bg-containerBg">
          <v-spacer />
          <v-btn color="primary" variant="elevated" @click="onAcceptClick">Aceptar</v-btn>
          <v-btn color="lightprimary" variant="elevated" @click="onCancelClick">Cancelar</v-btn>
        </v-card-actions>
      </v-card>
    </div>
  </Transition>
</template>

<script setup>
import { ref, watch, onBeforeUnmount } from 'vue'
import { CloseOutlined } from '@ant-design/icons-vue';

const emit = defineEmits(['update:modelValue', 'accept', 'cancel'])

const props = defineProps({
  modelValue: { type: Boolean, default: false },
  title: { type: String, default: '' },
  formValidate: { type: Boolean, default: false },
  classBody: { type: String, default: '' },
  hideActions: { type: Boolean, default: false },
})

const isOpen = ref(props.modelValue)
const formRef = ref(null)

watch(() => props.modelValue, v => (isOpen.value = v))
watch(isOpen, v => {
  emit('update:modelValue', v)
  if (v) {
    document.body.classList.add('no-scroll-main')
  } else {
    document.body.classList.remove('no-scroll-main')
  }
})

onBeforeUnmount(() => {
  document.body.classList.remove('no-scroll-main')
})

function close() {
  isOpen.value = false
}

async function onAcceptClick() {

  if (props.formValidate) {
    const res = await formRef.value.validate()
    const valid = typeof res === 'boolean' ? res : res?.valid
    if (!valid) {
      return
    }
  }

  emit('accept')
}

function onCancelClick() {
  emit('cancel')
  close()
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

/* Transiciones */
.dialog-fade-enter-active,
.dialog-fade-leave-active {
  transition: opacity 0.3s ease;
}

.dialog-fade-enter-from,
.dialog-fade-leave-to {
  opacity: 0;
}

.dialog-fade-enter-active .modal-card,
.dialog-fade-leave-active .modal-card {
  transition: transform 0.3s ease;
}

.dialog-fade-enter-from .modal-card,
.dialog-fade-leave-to .modal-card {
  transform: scale(0.95);
}
</style>

<style>
/* Estilo global para evitar scroll cuando el diálogo está abierto */
.no-scroll-main main.v-main.page-wrapper {
  height: 100vh;
  overflow: hidden;
}
</style>
