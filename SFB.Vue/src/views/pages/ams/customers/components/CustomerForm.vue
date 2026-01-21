<template>
  <card-dialog v-model="showModal" :title="titleDlg" height="350" formValidate @accept="onAccept" @cancel="onCancel">
    <v-row class="pa-4 pb-0">
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Código</v-label>
          <v-text-field disabled v-model="model.CustomerId" required placeholder="Código" />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Nombres</v-label>
          <v-text-field v-model="model.Person.FirstName" :rules="[rRequired]" placeholder="Nombres" />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Apellidos</v-label>
          <v-text-field v-model="model.Person.LastName" placeholder="Apellidos" />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Fecha de nacimiento</v-label>
          <v-text-field v-model="model.Person.DateBirth" :rules="[rRequired]" type="date"
            placeholder="Fecha de nacimiento" />
        </div>
      </v-col>
      <v-col cols="12" sm="6" class="py-0">
        <div class="mb-6">
          <v-label>Dirección</v-label>
          <v-text-field v-model="model.Person.Address" :rules="[rRequired]" placeholder="Dirección" />
        </div>
      </v-col>
    </v-row>
  </card-dialog>
</template>

<script setup>
import { ref, inject } from 'vue'
const { customerServ, uiStore } = inject('services')
const { question } = inject('MsgDialog')

const showModal = ref(false)
const modeDlg = ref('')
const titleDlg = ref('')
const model = ref({
  CustomerId: 0,
  Person: {
    NroPerson: 0,
    FirstName: '',
    LastName: '',
    DateBirth: '',
    Address: ''
  }
})

const rRequired = v => (v !== null && v !== undefined && v !== '') || 'Campo requerido'
const fullName = person => `${person?.FirstName ?? ''} ${person?.LastName ?? ''}`.trim()

let _resolve = null

async function openForm(mode, item = null) {
  modeDlg.value = mode
  switch (mode) {
    case 'Insert':
      titleDlg.value = 'Nuevo Cliente'
      model.value = {
        CustomerId: 0,
        Person: {
          NroPerson: 0,
          FirstName: '',
          LastName: '',
          DateBirth: '',
          Address: ''
        }
      }
      showModal.value = true
      break
    case 'Update':
      titleDlg.value = 'Editar Cliente'
      model.value = {
        CustomerId: item?.CustomerId ?? 0,
        Person: {
          NroPerson: item?.Person?.NroPerson ?? 0,
          FirstName: item?.Person?.FirstName ?? '',
          LastName: item?.Person?.LastName ?? '',
          DateBirth: item?.Person?.DateBirth ?? '',
          Address: item?.Person?.Address ?? ''
        }
      }
      showModal.value = true
      break
    case 'Delete': {
      const confirmed = await question(
        'Eliminar Cliente',
        `Esta acción eliminará el Cliente (${fullName(item?.Person)}). ¿Desea continuar?`
      )

      if (!confirmed) return null

      const ok = await customerServ.remove(item.CustomerId)
      if (!ok) return
      const idx = customerServ.pageData.Data.findIndex(p => p.CustomerId === item.CustomerId)
      if (idx !== -1) {
        customerServ.pageData.Data.splice(idx, 1)
        customerServ.pageData.TotalCount--
        if (customerServ.pageData.Data.length === 0 && customerServ.pageData.TotalCount > 0) {
          await customerServ.loadPage()
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
      const newCustomer = await customerServ.create(model.value)
      uiStore.isLoadingBody = false
      if (!newCustomer) return
      model.value = newCustomer
      customerServ.pageData.Data.unshift(newCustomer)
      customerServ.pageData.TotalCount++
      break
    }
    case 'Update': {
      const updCustomer = await customerServ.update(model.value)
      uiStore.isLoadingBody = false
      if (!updCustomer) return
      const idx = customerServ.pageData.Data.findIndex(p => p.CustomerId === updCustomer.CustomerId)
      if (idx !== -1) customerServ.pageData.Data[idx] = updCustomer
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
