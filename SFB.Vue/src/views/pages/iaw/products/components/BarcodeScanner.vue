<template>
  <v-dialog v-model="showModal" fullscreen hide-overlay transition="dialog-bottom-transition" class="scanner-dialog">
    <div class="scanner-wrapper">
      <video ref="videoRef" class="scanner-video" autoplay muted playsinline v-show="isCameraReady"></video>

      <div class="scanner-ui">
        <!-- Close Button -->
        <v-btn icon class="close-btn" @click="showModal = false" color="white" variant="text">
          <v-icon size="32">mdi-close</v-icon>
        </v-btn>

        <!-- Focus Box -->
        <div class="scan-area">
          <div class="corner top-left"></div>
          <div class="corner top-right"></div>
          <div class="corner bottom-left"></div>
          <div class="corner bottom-right"></div>
          <div class="anim-scan-line"></div>
        </div>

        <!-- Text -->
        <div class="mt-8 text-white text-h6 font-weight-regular text-center"
          style="z-index: 20; text-shadow: 0 1px 3px rgba(0,0,0,0.8);">
          Escanea el código de barras
        </div>
      </div>
    </div>
  </v-dialog>
</template>

<script setup>
import { ref, inject, computed, onBeforeUnmount, onMounted } from 'vue'
const { productServ, uiStore } = inject('services')
const { question } = inject('MsgDialog')
import { message } from 'ant-design-vue'

const showModal = ref(false)
const isCameraReady = ref(false)
const videoRef = ref(null)

async function openForm() {
  HybridWebView.SendRawMessageToDotNet("request-camera-permission");
  showModal.value = true
}

let stream = null

// Arranca la cámara cuando el componente monte
onMounted(async () => {

  try {
    stream = await navigator.mediaDevices.getUserMedia({
      video: { facingMode: 'environment' },
      audio: false,
    })
  } catch (err) {
    console.warn('Cámara trasera no encontrada, pidiendo la cámara por defecto', err)
    try {
      stream = await navigator.mediaDevices.getUserMedia({ video: true, audio: false })
    } catch (err2) {
      console.error('No se pudo acceder a ninguna cámara', err2)
      return
    }
  }
  videoRef.value.srcObject = stream
  videoRef.value.onloadedmetadata = () => {
    isCameraReady.value = true
  }

})

// Para la cámara al desmontar
onBeforeUnmount(() => {
  if (stream) {
    stream.getTracks().forEach(t => t.stop())
  }
})

defineExpose({
  openForm
})
</script>

<style scoped>
.scanner-wrapper {
  position: relative;
  width: 100%;
  height: 100%;
  background-color: #000;
  overflow: hidden;
}

.scanner-video {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  object-fit: cover;
}

.scanner-ui {
  position: absolute;
  inset: 0;
  z-index: 10;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}

.close-btn {
  position: absolute;
  top: 20px;
  right: 20px;
  z-index: 30;
}

.scan-area {
  position: relative;
  width: 280px;
  height: 180px;
  /* Dark overlay effect using box-shadow */
  box-shadow: 0 0 0 100vmax rgba(0, 0, 0, 0.6);
  border-radius: 12px;
}

.corner {
  position: absolute;
  width: 40px;
  height: 40px;
  border-color: white;
  border-style: solid;
  border-width: 0;
  pointer-events: none;
}

.top-left {
  top: -2px;
  left: -2px;
  border-top-width: 4px;
  border-left-width: 4px;
  border-top-left-radius: 16px;
}

.top-right {
  top: -2px;
  right: -2px;
  border-top-width: 4px;
  border-right-width: 4px;
  border-top-right-radius: 16px;
}

.bottom-left {
  bottom: -2px;
  left: -2px;
  border-bottom-width: 4px;
  border-left-width: 4px;
  border-bottom-left-radius: 16px;
}

.bottom-right {
  bottom: -2px;
  right: -2px;
  border-bottom-width: 4px;
  border-right-width: 4px;
  border-bottom-right-radius: 16px;
}

.anim-scan-line {
  position: absolute;
  width: 100%;
  height: 2px;
  background: #ff0000;
  top: 50%;
  left: 0;
  animation: scan 2.5s infinite linear;
  box-shadow: 0 0 4px rgba(255, 0, 0, 0.8);
}

@keyframes scan {
  0% {
    top: 5%;
    opacity: 0;
  }

  10% {
    opacity: 1;
  }

  90% {
    opacity: 1;
  }

  100% {
    top: 95%;
    opacity: 0;
  }
}
</style>
