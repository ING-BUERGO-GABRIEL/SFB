<template>
  <ui-icon name="ScanOutlined" @click="openScan" />
  <v-dialog v-model="showModal" fullscreen hide-overlay transition="dialog-bottom-transition" class="scanner-dialog">
    <div class="scanner-wrapper">
      <video ref="videoRef" class="scanner-video" autoplay muted playsinline v-show="isCameraReady"></video>

      <div class="scanner-ui">
        <!-- Close Button -->
        <v-btn icon class="close-btn" @click="showModal = false" color="white" variant="text">
          <ui-icon name="CloseOutlined" size="18" color="white" />
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
import { ref, onBeforeUnmount, watch, nextTick } from 'vue'
import config from '@/config'
import { message } from 'ant-design-vue'
const emit = defineEmits(['code-scan'])
const showModal = ref(false)
const isCameraReady = ref(false)
const videoRef = ref(null)
let stream = null

async function startCamera() {
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
  if (videoRef.value) {
    videoRef.value.srcObject = stream
    videoRef.value.onloadedmetadata = () => {
      isCameraReady.value = true
      startScanning()
    }
  }
}

async function startScanning() {
  if (!('BarcodeDetector' in window)) {
    console.warn('BarcodeDetector no soportado en este navegador.')
    return
  }

  const barcodeDetector = new BarcodeDetector({
    formats: ['qr_code', 'ean_13', 'code_128', 'ean_8', 'upc_a', 'upc_e']
  })

  const scanLoop = async () => {
    if (!showModal.value || !isCameraReady.value) return

    try {
      const barcodes = await barcodeDetector.detect(videoRef.value)
      if (barcodes.length > 0) {
        emit('code-scan', barcodes[0].rawValue)
        showModal.value = false
      }
    } catch {
      // Ignorar errores de detección vacía o frames corruptos
    }

    requestAnimationFrame(scanLoop)
  }

  scanLoop()
}

// requestCameraPermissionFromNative: pide permiso a la app nativa y devuelve Promise<boolean>
function requestCameraPermissionFromNative(timeout = 10000) {
  return new Promise((resolve) => {
    let settled = false;
    const cleanup = () => {
      try { delete window.__onPermissionResult; } catch { window.__onPermissionResult = null; }
    };

    const finish = (granted) => {
      if (settled) return;
      settled = true;
      clearTimeout(timer);
      cleanup();
      resolve(Boolean(granted));
    };

    // Timeout por si no llega respuesta
    const timer = setTimeout(() => {
      console.warn('requestCameraPermissionFromNative: timeout');
      finish(false);
    }, timeout);

    // Callback que la app nativa invocará: window.__onPermissionResult(granted)
    window.__onPermissionResult = function (granted) {
      console.log('requestCameraPermissionFromNative: callback received', granted);
      finish(granted);
    };

    // Intenta enviar el mensaje al WebView nativo; si no está disponible, reintenta unas veces
    const sendMessage = () => {
      if (window.HybridWebView && typeof window.HybridWebView.SendRawMessageToDotNet === 'function') {
        try {
          window.HybridWebView.SendRawMessageToDotNet('request-camera-permission');
          console.log('requestCameraPermissionFromNative: message sent');
        } catch (err) {
          console.error('requestCameraPermissionFromNative: send error', err);
          finish(false);
        }
        return;
      }

      // Reintentos cortos si todavía no está cargado
      let attempts = 0;
      const maxAttempts = 6;
      const iv = setInterval(() => {
        attempts++;
        if (window.HybridWebView && typeof window.HybridWebView.SendRawMessageToDotNet === 'function') {
          clearInterval(iv);
          try {
            window.HybridWebView.SendRawMessageToDotNet('request-camera-permission');
            console.log('requestCameraPermissionFromNative: message sent after retry');
          } catch (err) {
            console.error('requestCameraPermissionFromNative: send error', err);
            finish(false);
          }
        } else if (attempts >= maxAttempts) {
          clearInterval(iv);
          console.warn('requestCameraPermissionFromNative: HybridWebView not available');
          finish(false);
        }
      }, 100);
    };

    // Pequeña espera para asegurar que la callback esté registrada antes de enviar
    setTimeout(sendMessage, 30);
  });
}

// Uso desde tu función openForm (ejemplo con Vue + showModal.value)
async function openScan() {
  if (config.platform === 'maui') {
    console.log('Abriendo formulario en maui');
    try {
      const granted = await requestCameraPermissionFromNative(/* opcional timeout ms */);
      if (granted) {
        showModal.value = true;
      } else {
        console.warn('Permiso de cámara no concedido por la app nativa.');
      }
    } catch (e) {
      console.error('openForm error:', e);
      showModal.value = true; // fallback web
    }
  } else {
    showModal.value = true; // web
  }
}

function stopCamera() {
  if (stream) {
    stream.getTracks().forEach(t => t.stop())
    stream = null
  }
  isCameraReady.value = false
}

watch(showModal, async (val) => {
  if (val) {
    await nextTick()
    startCamera()
  } else {
    stopCamera()
  }
})

// Para la cámara al desmontar
onBeforeUnmount(() => {
  stopCamera()
})

defineExpose({
  openScan
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
