<template>
  <card-dialog v-model="showModal" :extraButton="updReadOnly && model.StatusCode === 'ACT'" textExtraButton="Anular"
    :disabledAccept="updReadOnly" :title="titleDlg" height="100%" width="1000" formValidate @accept="onAccept"
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
          <v-label>Proveedor</v-label>
          <v-select v-model="model.SupplierId" :readonly="updReadOnly" :items="metadata.CmbSuppliers"
            :rules="[rRequired]" item-title="Name" item-value="SupplierId" placeholder="Seleccionar proveedor" />
        </div>
      </v-col>
      <v-col cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Almacén destino</v-label>
          <v-select v-model="model.WarehouseId" :readonly="updReadOnly" :items="metadata.CmbWarehouses"
            :rules="[rRequired]" item-title="Name" item-value="WarehouseId" placeholder="Seleccionar almacén" />
        </div>
      </v-col>
      <v-col cols="12" sm="4" class="py-0">
        <div class="mb-6">
          <v-label>Tipo de compra</v-label>
          <v-select v-model="model.Type" :readonly="updReadOnly" :items="metadata.CmbType" :rules="[rRequired]"
            item-title="Name" item-value="Code" placeholder="Seleccionar tipo" />
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
          <v-label>Referencia</v-label>
          <v-text-field v-model="model.Reference" :readonly="updReadOnly" placeholder="Factura o referencia" />
        </div>
      </v-col>
    </v-row>

    <v-divider class="mx-4" />

    <div class="px-0 py-3">
      <div class="d-flex align-center justify-space-between mb-3">
        <h4 class="text-subtitle-2 mb-0">Detalle de productos</h4>

        <v-btn color="primary" variant="tonal" size="small" :disabled="updReadOnly" @click="addDetail">
          Agregar producto
        </v-btn>
      </div>

      <v-table density="comfortable" class="inventory-details-table">
        <thead>
          <tr>
            <th class="text-left px-0">Producto</th>
            <th class="text-left" style="width: 100px">Present.</th>
            <th class="text-left" style="width: 70px">Cantidad</th>
            <th class="text-left" style="width: 90px">Costo</th>
            <th class="text-left" style="width: 110px">Total</th>
            <th v-if="!updReadOnly" class="text-center px-0" style="width: 50px">Acc.</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(detail, idx) in model.Details" :key="idx">
            <td class="px-0">
              <select-page v-model="detail.NroProduct" :readonly="updReadOnly" :service="productServ"
                :taken-ids="[...selectedIds(detail)]" :selected-label="detail._ProdName" :rules="[rRequired]"
                placeholder="Seleccionar producto" @picked="p => onProductPicked(detail, p)" />
            </td>
            <td class="pr-0">
              <v-select class="pr-0" v-model="detail.PresentCode" :readonly="updReadOnly" :items="detail.PresentItems"
                :disabled="detail.DisablePresent" item-title="Presentation.Name" item-value="Presentation.Code"
                placeholder="Pre" @update:model-value="onQtyPresentChange(detail)">
                <template #selection="{ item }">
                  {{ item.raw.Presentation.Code }}
                </template>
                <template #item="{ props, item }">
                  <v-list-item v-bind="props" :title="item.raw.Presentation.Name + ' - ' + item.raw.QtyProduct" />
                </template>
              </v-select>
            </td>
            <td class="pr-0">
              <v-text-field v-model.number="detail.QtyPresent" :readonly="updReadOnly" :rules="qtyRules" type="number"
                min="0" step="1" variant="filled" placeholder="Cant."
                @update:model-value="onQtyPresentChange(detail)" />
            </td>
            <td class="pr-0">
              <v-text-field v-model.number="detail.UnitCost" :readonly="updReadOnly" :rules="costRules" type="number"
                min="0" step="0.01" variant="filled" placeholder="Costo" @update:model-value="onCostChange(detail)" />
            </td>
            <td class="pr-0">
              <v-text-field v-model.number="detail.TotalCost" readonly type="number" variant="filled"
                placeholder="Total" />
            </td>
            <td v-if="!updReadOnly" class="text-center px-0">
              <v-btn icon variant="text" color="error" size="small" :disabled="model.Details.length === 1"
                @click="removeDetail(idx)">
                <DeleteOutlined :style="{ fontSize: '15px' }" />
              </v-btn>
            </td>
          </tr>
        </tbody>
      </v-table>

      <div class="d-flex justify-end mt-4">
        <div style="min-width: 240px">
          <div class="d-flex justify-space-between align-center mb-2">
            <span class="text-subtitle-2">Total general:</span>
            <strong class="text-h6">{{ formatCurrency(grandTotal) }}</strong>
          </div>
        </div>
      </div>
    </div>
  </card-dialog>
</template>

<script setup>
import { computed, ref, inject } from 'vue'
import { DeleteOutlined } from '@ant-design/icons-vue'

const { purchaseServ, productServ, uiStore } = inject('services')
const { question } = inject('MsgDialog')

const showModal = ref(false)
const modeDlg = ref('')
const titleDlg = ref('')
const model = ref({})

const metadata = ref({
  CmbSuppliers: [],
  CmbWarehouses: [],
  CmbStatus: [],
  CmbType: []
})

const rRequired = v => (v !== null && v !== undefined && v !== '') || 'Campo requerido'
const qtyRules = [
  rRequired,
  v => (v !== null && v !== undefined && Number(v) > 0) || 'Debe ser mayor a 0'
]
const costRules = [
  rRequired,
  v => (v !== null && v !== undefined && Number(v) >= 0) || 'Debe ser mayor o igual a 0'
]

const updReadOnly = computed(() => modeDlg.value === 'Update')

const grandTotal = computed(() => (model.value.Details ?? []).reduce((acc, d) => acc + Number(d.TotalCost ?? 0), 0))

let _resolve = null

async function openForm(mode, item = null) {
  modeDlg.value = mode
  switch (mode) {
    case 'Insert':
      uiStore.isLoadingBody = true
      await loadMetadata()
      titleDlg.value = 'Nueva compra'
      model.value = getDefaultModel()
      break
    case 'Update':
      uiStore.isLoadingBody = true
      await loadMetadata()
      titleDlg.value = 'Editar compra'
      if (!item?.TxnId) return null
      const resp = await purchaseServ.getById(item.TxnId)
      if (!resp) return null
      resp.Details?.forEach(d => {
        d.DisablePresent = !(d.PresentItems?.length > 1)
      })
      model.value = resp
      break
    default:
      return Promise.resolve(null)
  }

  uiStore.isLoadingBody = false
  showModal.value = true

  return new Promise(resolve => {
    _resolve = resolve
  })
}

async function onAccept() {
  model.value.GrandTotal = grandTotal.value
  uiStore.isLoadingBody = true

  let txn = null
  if (modeDlg.value === 'Insert') {
    txn = await purchaseServ.create(model.value)
    if (txn) purchaseServ.addItemPage(txn)
  } else if (modeDlg.value === 'Update') {
    txn = await purchaseServ.update(model.value)
    if (txn) purchaseServ.updItemPage(txn)
  }

  uiStore.isLoadingBody = false
  if (!txn) return

  _resolve?.(txn)
  _resolve = null
  showModal.value = false
}

async function onAnular() {
  const confirmed = await question('Anular compra', '¿Está seguro de anular la compra?')
  if (!confirmed) return null

  const res = await purchaseServ.anular(model.value.TxnId)
  if (res) {
    purchaseServ.updItemPage(res)
    _resolve?.(res)
    _resolve = null
    showModal.value = false
  }
}

function onCancel() {
  _resolve?.(null)
  _resolve = null
  showModal.value = false
}

async function loadMetadata() {
  const meta = await purchaseServ.getMetadata()
  if (meta) {
    metadata.value = meta
  }
}

function addDetail() {
  model.value.Details.push(createDetail())
}

function removeDetail(idx) {
  if (model.value.Details.length > 1) {
    model.value.Details.splice(idx, 1)
  }
}

function getDefaultModel() {
  return {
    TxnId: 0,
    SupplierId: null,
    WarehouseId: null,
    Type: null,
    StatusCode: 'ACT',
    CurrencyCode: 'BOB',
    Reference: null,
    GrandTotal: 0,
    Details: [createDetail()]
  }
}

function createDetail() {
  return {
    DetailId: 0,
    PurchaseId: 0,
    NroProduct: null,
    QtyProduct: 0,
    QtyPresent: 1,
    PresentCode: null,
    PresentItems: [],
    DisablePresent: true,
    UnitCost: 0,
    TotalCost: 0,
    _ProdName: null
  }
}

function onProductPicked(detail, product) {
  detail._ProdName = product?.Name ?? detail._ProdName ?? null
  detail.PresentItems = [{ QtyProduct: 1, Presentation: { Name: product.Presentation.Name, Code: product.Presentation.Code } }]
  detail.PresentCode = product?.PresentCode
  detail.QtyPresent = 1
  detail.QtyProduct = product?.QtyProduct ?? 1
  if (product?.ProductPresent?.length) {
    detail.DisablePresent = false
    detail.PresentItems.push(...product.ProductPresent)
  } else {
    detail.DisablePresent = true
  }
  calcTotals(detail)
}

function onQtyPresentChange(detail) {
  const pItem = detail.PresentItems.find(x => x.Presentation.Code === detail.PresentCode)
  if (pItem) {
    detail.QtyProduct = (detail.QtyPresent ?? 0) * pItem.QtyProduct
  }
  calcTotals(detail)
}

function onCostChange(detail) {
  calcTotals(detail)
}

function calcTotals(detail) {
  detail.TotalCost = Number(detail.UnitCost ?? 0) * Number(detail.QtyPresent ?? 0)
}

function selectedIds(exceptDetail = null) {
  return new Set(
    (model.value.Details ?? [])
      .filter(d => d !== exceptDetail && d?.NroProduct != null)
      .map(d => d.NroProduct)
  )
}

const formatCurrency = (amount) => new Intl.NumberFormat('es-BO', { style: 'currency', currency: 'BOB' }).format(amount ?? 0)

defineExpose({
  openForm
})
</script>

<style scoped>
.inventory-details-table :deep(.v-field__input) {
  min-height: 38px;
}
</style>
