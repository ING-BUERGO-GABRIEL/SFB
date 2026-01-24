<template>
  <card-dialog v-model="showModal" :title="titleDlg" height="420" formValidate @accept="onAccept" @cancel="onCancel">
    <v-row class="pa-4 pb-0">
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Codigo</v-label>
          <v-text-field disabled v-model="model.CashBoxId" required placeholder="Codigo" />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Nombre</v-label>
          <v-text-field v-model="model.Name" :rules="[rRequired]" required placeholder="Nombre" />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Tipo</v-label>
          <v-select v-model="model.Type" :items="typeOptions" :rules="[rRequired]" required placeholder="Tipo" />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Moneda</v-label>
          <v-text-field v-model="model.CurrencyCode" :rules="[rRequired]" required placeholder="Moneda" />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Saldo actual</v-label>
          <v-text-field v-model.number="model.CurrentBalance" type="number" min="0" placeholder="0.00" />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Activa</v-label>
          <v-switch v-model="model.Active" color="primary" />
        </div>
      </v-col>
    </v-row>
  </card-dialog>
</template>

<script setup>
import { ref, inject } from 'vue'

const { cashBoxServ, uiStore } = inject('services')
const { question } = inject('MsgDialog')

const showModal = ref(false)
const modeDlg = ref('')
const titleDlg = ref('')
const model = ref({})

const typeOptions = ['POS', 'PCH']

const rRequired = v => (v !== null && v !== undefined && v !== '') || 'Campo requerido'

let _resolve = null

async function openForm(mode, item = null) {
  modeDlg.value = mode

  switch (mode) {
    case 'Insert':
      titleDlg.value = 'Nueva Caja'
      model.value = {
        CashBoxId: 0,
        Name: '',
        Type: 'POS',
        CurrencyCode: 'BOB',
        CurrentBalance: 0,
        Active: true
      }
      showModal.value = true
      break
    case 'Update':
      titleDlg.value = 'Editar Caja'
      model.value = { ...item }
      showModal.value = true
      break
    case 'Delete': {
      const confirmed = await question(
        'Eliminar Caja',
        `Esta acción eliminará la Caja (${item.Name}). ¿Desea continuar?`
      )

      if (!confirmed) return null

      const ok = await cashBoxServ.remove(item.CashBoxId)
      if (!ok) return
      const idx = cashBoxServ.pageData.Data.findIndex(p => p.CashBoxId === item.CashBoxId)
      if (idx !== -1) {
        cashBoxServ.pageData.Data.splice(idx, 1)
        cashBoxServ.pageData.TotalCount--
        if (cashBoxServ.pageData.Data.length === 0 && cashBoxServ.pageData.TotalCount > 0) {
          await cashBoxServ.loadPage()
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
  uiStore.isLoadingBody = true
  switch (modeDlg.value) {
    case 'Insert': {
      const newCashBox = await cashBoxServ.create(model.value)
      uiStore.isLoadingBody = false
      if (!newCashBox) return
      model.value = newCashBox
      cashBoxServ.pageData.Data.unshift(newCashBox)
      cashBoxServ.pageData.TotalCount++
      break
    }
    case 'Update': {
      const updCashBox = await cashBoxServ.update(model.value)
      uiStore.isLoadingBody = false
      if (!updCashBox) return
      const idx = cashBoxServ.pageData.Data.findIndex(p => p.CashBoxId === updCashBox.CashBoxId)
      if (idx !== -1) cashBoxServ.pageData.Data[idx] = updCashBox
      break
    }
  }
  uiStore.isLoadingBody = false
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
