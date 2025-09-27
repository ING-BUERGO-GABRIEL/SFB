<template>
  <v-dialog v-model="confirmation.active" max-width="500" style="z-index:2500">
    <v-card>
      <v-card-title class="d-flex align-center mt-2">
        <component :is="icon.comp" class="mr-2 " :style="{ color: icon.color, fontSize: '22px' }" />
        {{ confirmation.title }}
      </v-card-title>

      <v-card-text v-html="confirmation.message"></v-card-text>

      <v-card-actions>
        <v-btn color="primary" variant="flat" @click="confirmation.resolve(true)">Aceptar</v-btn>
        <v-btn v-if="showCancel" @click="confirmation.resolve(false)">Cancelar</v-btn>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import { inject, computed } from 'vue'
import {
  QuestionCircleOutlined,
  CheckCircleOutlined,
  CloseCircleOutlined,
  InfoCircleOutlined,
} from '@ant-design/icons-vue'

export default {
  setup() {
    const { confirmation } = inject('MsgDialog')
   

    const icon = computed(() => {
      const t = confirmation.value?.type
      switch (t) {
        case 'question': return { comp: QuestionCircleOutlined,   color: '#1976d2' } // azul
        case 'confirm' : return { comp: CheckCircleOutlined, color: '#2e7d32' } // verde
        case 'error'   : return { comp: CloseCircleOutlined, color: '#d32f2f' } // rojo
        case 'info'    : return { comp: InfoCircleOutlined,  color: '#0288d1' } // cyan
        default        : return { comp: InfoCircleOutlined,  color: '#757575' } // gris
      }
    })

    const showCancel = computed(() =>
      ['confirm', 'question'].includes(confirmation.value?.type)
    )

    return { confirmation, icon, showCancel }
  },
}
</script>
