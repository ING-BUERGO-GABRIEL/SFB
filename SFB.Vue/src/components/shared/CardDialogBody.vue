<template>
  <div v-if="modelValue" class="full-cover">
    <div class="modal-card d-flex flex-column justify-center align-center">
      <v-card variant="outlined" elevation="0" class="bg-surface" :height="props.height" :width="props.width">

        <v-card-title class="d-flex align-center bg-containerBg">
          <span>{{ props.title }}</span>
          <v-spacer />
          <v-btn height="36" @click="onCancelClick" icon elevation="0">
            <component :is="CloseOutlined" :style="{ fontSize: '16px' }" />
          </v-btn>
        </v-card-title>
        <!-- Body -->
        <v-card-text class="modal-card-body">
          <v-form ref="formRef">
            <slot></slot>
          </v-form>
        </v-card-text>
        <!-- footer -->
        <v-card-actions class="bg-containerBg">
          <v-spacer />
          <v-btn color="primary" variant="elevated" @click="onAcceptClick">Aceptar</v-btn>
          <v-btn color="lightprimary" variant="elevated" @click="onCancelClick">Cancelar</v-btn>
        </v-card-actions>
      </v-card>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'
import { CloseOutlined } from '@ant-design/icons-vue';

const emit = defineEmits(['update:modelValue', 'accept', 'cancel'])

const props = defineProps({
  modelValue: { type: Boolean, default: false },
  title: { type: String, default: '' },
  formValidate: { type: Boolean, default: false },
  height: { type: String, default: '400' },
  width: { type: String, default: '500' },
})

const isOpen = ref(props.modelValue)
const formRef = ref(null)

watch(() => props.modelValue, v => (isOpen.value = v))
watch(isOpen, v => emit('update:modelValue', v))

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

.modal-card-body {
  height: calc(100% - 116px);
  padding-top: 20px;
  flex: 1 1 auto;
  overflow-y: auto;
}
</style>
