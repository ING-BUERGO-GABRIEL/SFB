<template>
  <card-dialog
    v-model="showModal"
    :title="titleDlg"
    height="640"
    width="900"
    formValidate
    @accept="onAccept"
    @cancel="onCancel"
  >
    <v-row class="pa-4 pb-0" dense>
      <v-col cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Código</v-label>
          <v-text-field
            v-model="model.TxnId"
            disabled
            placeholder="Código"
          />
        </div>
      </v-col>

      <v-col cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Tipo de Transacción</v-label>
          <v-select
            v-model="model.Type"
            :items="metadata.CmbType"
            :rules="[rRequired]"
            item-title="Name"
            item-value="Type"            
            placeholder="Seleccionar tipo"
          />
        </div>
      </v-col>

      <v-col cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Estado</v-label>
          <v-select
            v-model="model.StatusCode"
            :items="metadata.CmbStatus"
            item-title="Name"
            item-value="Code"            
            placeholder="Seleccionar estado"
          />
        </div>
      </v-col>

      <v-col cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Almacén Origen</v-label>
          <v-select
            v-model="model.WarehouseOriginId"
            :items="metadata.CmbWerehouses"
            :rules="originRules"
            :disabled="originDisabled"
            item-title="Name"
            item-value="WarehouseId"
            clearable
            
            placeholder="Seleccionar almacén"
          />
        </div>
      </v-col>

      <v-col cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Almacén Destino</v-label>
          <v-select
            v-model="model.WarehouseDestId"
            :items="metadata.CmbWerehouses"
            :rules="destRules"
            :disabled="destDisabled"
            item-title="Name"
            item-value="WarehouseId"
            clearable            
            placeholder="Seleccionar almacén"
          />
        </div>
      </v-col>

      <v-col v-if="model?.ModOrigin != 'IAW'" cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Nro. Transacción Origen</v-label>
          <v-text-field
            v-model.number="model.TxnOrigin"
            type="number"
            disabled
            min="0"
          />
        </div>
      </v-col>
    </v-row>

    <v-divider class="mx-4" />

    <div class="px-4 py-3">
      <div class="d-flex align-center justify-space-between mb-3">
        <h4 class="text-subtitle-2 mb-0">Detalle de Productos</h4>
        
        <v-btn color="primary" variant="tonal" size="small" @click="addDetail">
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
              <select-page
                v-model="detail.NroProduct"
                :service="productServ"
                :taken-ids="[...selectedIds(detail)]"   
                :selected-label="detail._ProdName"    
                :rules="[rRequired]"
                placeholder="Seleccionar producto"
                @picked="p => { detail._ProdName = p?.Name ?? detail._ProdName ?? null }"
              />
            </td>

            <td class="pr-0">
              <v-text-field
                v-model.number="detail.QtyProduct"
                :rules="qtyRules"
                type="number"
                min="0"
                step="1"     
                variant="filled"          
                placeholder="Cant."
              />
            </td>
            <td class="text-center px-0" >
              <v-btn
                icon
                variant="text"
                color="error"
                size="small"
                :disabled="model.InvDetails.length === 1"
                @click="removeDetail(idx)"
              >
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
import { message } from 'ant-design-vue'
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

const originRules = computed(() => (originDisabled.value ? [] : [rRequired]))
const destRules = computed(() => (destDisabled.value ? [] : [rRequired]))

const originDisabled = computed(() => !['SAL', 'TRA'].includes(model.value?.Type))
const destDisabled = computed(() => !['ING', 'TRA'].includes(model.value?.Type))



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
      titleDlg.value = 'Nueva transacción de inventario'
      model.value = getDefaultModel()
      showModal.value = true
      break
    case 'Update':
      await loadMetadata()
      titleDlg.value = `Editar transacción #${item?.TxnId ?? ''}`
      model.value = normalizeModel(item)
      showModal.value = true
      break
    case 'Delete': {
      const confirmed = await question(
        'Anular transacción',
        `Esta acción marcará como anulada la transacción #${item.TxnId}. ¿Desea continuar?`
      )

      if (!confirmed) return null

      const cancelled = await invTxnServ.update({
        ...item,
        StatusCode: 'ANU'
      })
      if (!cancelled) return null

      const idx = invTxnServ.pageData?.Data?.findIndex(p => p.TxnId === cancelled.TxnId) ?? -1
      if (idx !== -1) {
        invTxnServ.pageData.Data[idx] = cancelled
      }

      return cancelled
    }
  }

  return new Promise(resolve => {
    _resolve = resolve
  })
}

async function onAccept() {
  if (!validateDetails()) {
    return
  }

  const payload = {
    ...model.value,
    InvDetails: model.value.InvDetails.map(detail => ({
      ...detail,
      QtyProduct: Number(detail.QtyProduct)
    }))
  }

  let txn = null

  switch (modeDlg.value) {
    case 'Insert':
      txn = await invTxnServ.create(payload)
      if (!txn) return
      model.value = normalizeModel(txn)
      if (invTxnServ.pageData?.Data) {
        invTxnServ.pageData.Data.unshift(txn)
        invTxnServ.pageData.TotalCount = (invTxnServ.pageData.TotalCount ?? 0) + 1
      }
      break
    case 'Update':
      txn = await invTxnServ.update(payload)
      if (!txn) return
      if (invTxnServ.pageData?.Data) {
        const idx = invTxnServ.pageData.Data.findIndex(p => p.TxnId === txn.TxnId)
        if (idx !== -1) invTxnServ.pageData.Data[idx] = txn
      }
      break
  }

  _resolve?.(txn ?? payload)
  _resolve = null
  showModal.value = false
}

function onCancel() {
  _resolve?.(null)
  _resolve = null
  showModal.value = false
}

async function loadMetadata() {
  const meta = await invTxnServ.getMetadata()
  if(meta){
    metadata.value =  meta
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

function validateDetails() {
  if (!model.value.InvDetails?.length) {
    message.warning('Debe registrar al menos un producto para la transacción.')
    return false
  }

  const hasInvalid = model.value.InvDetails.some(detail => !detail.NroProduct || !(Number(detail.QtyProduct) > 0))
  if (hasInvalid) {
    message.warning('Complete los productos y cantidades antes de guardar.')
    return false
  }

  return true
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

function normalizeModel(item) {
  if (!item) {
    return getDefaultModel()
  }

  return {
    TxnId: item.TxnId ?? 0,
    TxnOrigin: item.TxnOrigin ?? null,
    ModOrigin: item.ModOrigin ?? 'IAW',
    Type: item.Type ?? '',
    WarehouseOriginId: item.WarehouseOriginId ?? null,
    WarehouseDestId: item.WarehouseDestId ?? null,
    StatusCode: item.StatusCode ?? 'ACT',
    Delete: item.Delete ?? false,
    InvDetails: Array.isArray(item.InvDetails) && item.InvDetails.length
      ? item.InvDetails.map(d => ({
        DetailId: d.DetailId ?? 0,
        TxnId: d.TxnId ?? item.TxnId ?? 0,
        NroProduct: d.NroProduct ?? null,
        QtyProduct: d.QtyProduct ?? 0
      }))
      : [createDetail()]
  }
}

function createDetail() {
  return {
    DetailId: 0,
    TxnId: 0,
    NroProduct: null,
    QtyProduct: null
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
