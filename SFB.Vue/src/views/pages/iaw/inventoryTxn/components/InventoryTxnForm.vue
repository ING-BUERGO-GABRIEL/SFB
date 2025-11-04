<template>
  <card-dialog v-model="showModal" :extraButton="updReadOnly" textExtraButton="Anular Txn" :disabledAccept="updReadOnly" :title="titleDlg" height="640" width="900" formValidate @accept="onAccept"
    @cancel="onCancel" @btnExtra="onAnular">
    <v-row class="pa-4 pb-0" dense>
      <v-col cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Código</v-label>
          <v-text-field v-model="model.TxnId" disabled placeholder="Código" />
        </div>
      </v-col>

      <v-col cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Tipo de Transacción</v-label>
          <v-select v-model="model.Type" :readonly="updReadOnly" :items="metadata.CmbType" :rules="[rRequired]" item-title="Name"
            item-value="Code" placeholder="Seleccionar tipo" />
        </div>
      </v-col>

      <v-col cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Estado</v-label>
          <v-select v-model="model.StatusCode" readonly :items="metadata.CmbStatus" item-title="Name" item-value="Code"
            placeholder="Seleccionar estado" />
        </div>
      </v-col>

      <v-col cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Almacén Origen</v-label>
          <v-select v-model="model.WarehouseOriginId" :readonly="updReadOnly" :items="metadata.CmbWerehouses" :rules="originRules"
            :disabled="originDisabled" item-title="Name" item-value="WarehouseId" 
            placeholder="Seleccionar almacén" />
        </div>
      </v-col>

      <v-col cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Almacén Destino</v-label>
          <v-select v-model="model.WarehouseDestId" :readonly="updReadOnly" :items="metadata.CmbWerehouses" :rules="destRules"
            :disabled="destDisabled" item-title="Name" item-value="WarehouseId" 
            placeholder="Seleccionar almacén" />
        </div>
      </v-col>

      <v-col v-if="model?.ModOrigin != 'IAW'" cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Nro. Transacción Origen</v-label>
          <v-text-field v-model.number="model.TxnOrigin" type="number" disabled min="0" />
        </div>
      </v-col>
    </v-row>

    <v-divider class="mx-4" />

    <div class="px-4 py-3">
      <div class="d-flex align-center justify-space-between mb-3">
        <h4 class="text-subtitle-2 mb-0">Detalle de Productos</h4>

        <v-btn color="primary" variant="tonal" size="small" @click="addDetail" :readonly="updReadOnly">
          Agregar producto
        </v-btn>
      </div>

      <v-table density="comfortable" class="inventory-details-table">
        <thead>
          <tr>
            <th class="text-left px-0">Producto</th>
            <th class="text-left " style="width: 50px">Cantidad</th>
            <th class="text-center px-0" style="width: 50px">Acc.</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(detail, idx) in model.InvDetails" :key="idx">
            <td class="px-0">
              <select-page v-model="detail.NroProduct" :readonly="updReadOnly" :service="productServ" :taken-ids="[...selectedIds(detail)]"
                :selected-label="detail._ProdName" :rules="[rRequired]" placeholder="Seleccionar producto"
                @picked="p => { detail._ProdName = p?.Name ?? detail._ProdName ?? null }" />
            </td>
            <td class="pr-0">
              <v-text-field v-model.number="detail.QtyProduct" :readonly="updReadOnly" :rules="qtyRules" type="number" min="0" step="1"
                variant="filled" placeholder="Cant." />
            </td>
            <td class="text-center px-0">
              <v-btn icon variant="text" :readonly="updReadOnly" color="error" size="small" :disabled="model.InvDetails.length === 1"
                @click="removeDetail(idx)">
                <DeleteOutlined :style="{ fontSize: '15px' }" />
              </v-btn>
            </td>
          </tr>
        </tbody>
      </v-table>
    </div>
  </card-dialog>
</template>

<script setup>
import { computed, ref, inject, watch } from 'vue'
import { DeleteOutlined } from '@ant-design/icons-vue'
const { invTxnServ, productServ } = inject('services')
const { question } = inject('MsgDialog')

const showModal = ref(false)
const modeDlg = ref('')
const titleDlg = ref('')
const model = ref({})

const metadata = ref({
  CmbType: [],
  CmbWerehouses: [],
  CmbStatus: []
})


const rRequired = v => (v !== null && v !== undefined && v !== '') || 'Campo requerido'
const qtyRules = [
  rRequired,
  v => (v !== null && v !== undefined && Number(v) > 0) || 'Debe ser mayor a 0'
]

const originDisabled = computed(() => !['SAL', 'TRA'].includes(model.value?.Type))
const destDisabled = computed(() => !['ING', 'TRA', 'INI'].includes(model.value?.Type))

const updReadOnly = computed(() => modeDlg.value === 'Update')

const originRules = computed(() => (originDisabled.value ? [] : [rRequired]))
const destRules = computed(() => (destDisabled.value ? [] : [rRequired]))

watch(
  () => model.value?.Type,
  () => {
    if (originDisabled.value) {
      model.value.WarehouseOriginId = null
    }
    if (destDisabled.value) {
      model.value.WarehouseDestId = null
    }
  }
)

let _resolve = null

async function openForm(mode, item = null) {
  modeDlg.value = mode
  productServ.loadTable = false
  switch (mode) {
    case 'Insert':
      await loadMetadata()
      titleDlg.value = 'Nueva transacción'
      model.value = getDefaultModel()
      break
    case 'Update': {
      await loadMetadata()
      titleDlg.value = `Editar transacción`
      const resp = await invTxnServ.getById(item.TxnId)
      if (!resp) return null
      model.value = resp
      break
    }
    default:
      return Promise.resolve(null)
  }

  showModal.value = true

  return new Promise(resolve => {
    _resolve = resolve
  })
}

async function onAccept() {

  let txn = null


  switch (modeDlg.value) {
    case 'Insert':
      txn = await invTxnServ.create(model.value)
      if (!txn) return
      invTxnServ.addItemPage(txn)
      break
    case 'Update':
      txn = await invTxnServ.update(model.value)
      if (!txn) return
      invTxnServ.updItemPage(txn)
      break
  }

  _resolve?.(txn)
  _resolve = null
  showModal.value = false
}

async function onAnular() {

  const confirmed = await question(
        'Anular Transacción',
        `¿Esta seguro de Anular la Transaccion ?`
      )
  console.log(confirmed)

  // const res = await invTxnServ.anular(model.value.TxnId)
  // if (res) {
  //   invTxnServ.updItemPage(res)
  //   _resolve?.(res)
  //   _resolve = null
  //   showModal.value = false
  // }
}

function onCancel() {
  _resolve?.(null)
  _resolve = null
  showModal.value = false
}

async function loadMetadata() {
  const meta = await invTxnServ.getMetadata()
  if (meta) {
    metadata.value = meta
  }
}

function addDetail() {
  model.value.InvDetails.push(createDetail())
}

function removeDetail(idx) {
  if (model.value.InvDetails.length > 1) {
    model.value.InvDetails.splice(idx, 1)
  }
}


function getDefaultModel() {
  return {
    TxnId: 0,
    TxnOrigin: null,
    ModOrigin: 'IAW',
    Type: null,
    WarehouseOriginId: null,
    WarehouseDestId: null,
    StatusCode: 'ACT',
    Delete: false,
    InvDetails: [createDetail()]
  }
}

function createDetail() {
  return {
    DetailId: 0,
    TxnId: 0,
    NroProduct: null,
    QtyProduct: null,
    _ProdName: null,
  }
}

// IDs ya seleccionados (excluye la fila actual)
function selectedIds(exceptDetail = null) {
  return new Set(
    (model.value.InvDetails ?? [])
      .filter(d => d !== exceptDetail && d?.NroProduct != null)
      .map(d => d.NroProduct)
  )
}


defineExpose({
  openForm
})
</script>

<style scoped>
.inventory-details-table :deep(.v-field__input) {
  min-height: 38px;
}
</style>
