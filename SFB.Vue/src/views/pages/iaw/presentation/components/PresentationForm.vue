<template>
  <card-dialog
    v-model="showModal"
    :title="titleDlg"
    height="300"
    formValidate
    @accept="onAccept"
    @cancel="onCancel"
  >
    <v-row class="pa-4 pb-0">
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Código</v-label>
          <v-text-field
            v-model="model.Code"
            :readonly="modeDlg === 'Update'"
            :rules="[rRequired, rMaxCode]"
            required
            placeholder="Código"
          />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Nombre</v-label>
          <v-text-field
            v-model="model.Name"
            :rules="[rRequired, rMaxName]"
            required
            placeholder="Nombre"
          />
        </div>
      </v-col>
    </v-row>
  </card-dialog>
</template>

<script setup>
import { inject, ref } from 'vue'

const { presentationServ } = inject('services')
const { question } = inject('MsgDialog')

const showModal = ref(false)
const modeDlg = ref('')
const titleDlg = ref('')
const model = ref({})

const rRequired = v => (v !== null && v !== undefined && v !== '') || 'Campo requerido'
const rMaxCode = v => (v ? v.length <= 3 : true) || 'Máximo 3 caracteres'
const rMaxName = v => (v ? v.length <= 30 : true) || 'Máximo 30 caracteres'

let _resolve = null

async function openForm(mode, item = null) {
  modeDlg.value = mode

  switch (mode) {
    case 'Insert':
      titleDlg.value = 'Nueva Presentación'
      model.value = { Code: '', Name: '' }
      showModal.value = true
      break
    case 'Update':
      titleDlg.value = 'Editar Presentación'
      model.value = { ...item }
      showModal.value = true
      break
    case 'Delete': {
      const confirmed = await question(
        'Eliminar Presentación',
        `Esta acción eliminará la presentación (${item.Name}). ¿Desea continuar?`
      )

      if (!confirmed) return null

      const ok = await presentationServ.remove(item.Code)
      if (!ok) return

      if (!Array.isArray(presentationServ.pageData.Data)) {
        presentationServ.pageData.Data = []
      }

      const idx = presentationServ.pageData.Data.findIndex(p => p.Code === item.Code)
      if (idx !== -1) {
        presentationServ.pageData.Data.splice(idx, 1)
        if (typeof presentationServ.pageData.TotalCount === 'number') {
          presentationServ.pageData.TotalCount--
        }
        if (presentationServ.pageData.Data.length === 0 && (presentationServ.pageData.TotalCount ?? 0) > 0) {
          await presentationServ.loadPage()
        }
      }

      break
    }
  }

  return new Promise(resolve => {
    _resolve = resolve
  })
}

async function onAccept() {
  switch (modeDlg.value) {
    case 'Insert': {
      const newPresentation = await presentationServ.create(model.value)
      if (!newPresentation) return
      model.value = newPresentation
      if (!Array.isArray(presentationServ.pageData.Data)) {
        presentationServ.pageData.Data = []
      }
      presentationServ.pageData.Data.unshift(newPresentation)
      if (typeof presentationServ.pageData.TotalCount === 'number') {
        presentationServ.pageData.TotalCount++
      }
      break
    }
    case 'Update': {
      const updated = await presentationServ.update(model.value)
      if (!updated) return
      if (!Array.isArray(presentationServ.pageData.Data)) {
        presentationServ.pageData.Data = []
      }
      const idx = presentationServ.pageData.Data.findIndex(p => p.Code === updated.Code)
      if (idx !== -1) presentationServ.pageData.Data[idx] = updated
      break
    }
  }

  _resolve(model.value)
  _resolve = null
  showModal.value = false
}

function onCancel() {
  _resolve(null)
  _resolve = null
  showModal.value = false
}

defineExpose({
  openForm
})
</script>
