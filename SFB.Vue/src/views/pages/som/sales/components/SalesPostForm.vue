<template>
  <dialog-body v-model="showModal" title="Vender" formValidate fullscreen hide-actions
    class-body="bg-containerBg pt-2 px-3" @accept="onAcceptClick">
    <template #body="{ onAccept }">
      <v-row class="fill-height ma-0 text-body-1 ">
        <!-- Left Column: Product Catalog -->
        <v-col cols="12" md="8" class="pa-0 pr-3 h-100">
          <!-- Header Section -->
          <header-bar class-card="pa-3" class="mb-2">
            <search-field @search="onSearch" />
            <v-divider vertical class="mx-2 my-1 hidden-sm-and-down"></v-divider>
            <v-menu>
              <template #activator="{ props }">
                <v-btn variant="text" v-bind="props" class="text-capitalize text-body-2" color="grey-darken-2">
                  <ui-icon name="TagOutlined" size="15" class="mr-2" /> Categorías
                  <ui-icon name="DownOutlined" size="10" class="ml-2" />
                </v-btn>
              </template>
            </v-menu>
            <v-spacer></v-spacer>
            <div class="d-flex align-center text-grey-darken-1 gap-4">
              <ui-icon name="ScanOutlined" size="20" style="cursor: pointer;" class="mx-2" title="Scan" />
              <ui-icon name="ThunderboltOutlined" size="20" style="cursor: pointer;" class="mx-2"
                title="Quick Actions" />
              <ui-icon name="UnorderedListOutlined" size="20" style="cursor: pointer;" class="mx-2" title="List View" />
            </div>
          </header-bar>

          <!-- Product Grid (Static Visual Representation) -->
          <v-row dense class="w-100" align-content="start" style="overflow-y: auto; height: calc(100% - 82px);">
            <v-col v-for="prod in products" :key="prod.NroProduct" cols="6" sm="4" md="4" lg="3">
              <v-card flat
                class="rounded-lg overflow-hidden border cursor-pointer h-100 position-relative transition-swing elevation-0 hover-card pa-3 d-flex flex-column"
                @click="onCatalogProductClick(prod)" style="max-height: 128px;">

                <div class="d-flex justify-space-between align-start mb-2">
                  <div class="font-weight-bold text-subtitle-2  mr-2">{{ prod.Name }}</div>
                  <v-chip size="x-small" :color="getStockByWarehouse(prod) > 0 ? 'success' : 'error'" label
                    class="font-weight-bold flex-shrink-0">
                    {{ getStockByWarehouse(prod) }} {{ prod.Presentation?.Code }}
                  </v-chip>
                </div>

                <div class="mt-auto d-flex justify-space-between align-end">
                  <div>
                    <div class="text-caption text-grey mb-1">{{ prod.Presentation?.Name }}</div>
                    <div class="text-h6 font-weight-bold text-grey-darken-3">
                      {{ formatCurrency(prod.Price) }}
                    </div>
                  </div>
                  <div class="text-caption text-grey font-weight-bold">
                    Stock: {{ prod.Stock }}
                  </div>
                </div>
              </v-card>
            </v-col>

            <!-- Add Custom Item Card Mock
            <v-col v-if="products.length" cols="6" sm="4" md="4" lg="3">
              <v-card flat
                class="rounded-lg border  h-100 d-flex align-center justify-center bg-teal-lighten-4 hover-card"
                style="min-height: 180px">
                <ui-icon name="PlusOutlined" size="40" color="white" />
              </v-card>
            </v-col> -->
          </v-row>
        </v-col>

        <!-- Right Column: Cart & Details -->
        <v-col cols="12" md="4" class="h-100 pa-0" style=" overflow-y: auto;">
          <v-card class="h-100 pa-4 bg-white d-flex flex-column" elevation="0" variant="outlined"
            style="background-color: white;">
            <!-- Header -->
            <div class="d-flex justify-end align-center" @click="showAdditionalData = !showAdditionalData">
              <span class="text-teal font-weight-medium text-body-2 text-decoration-underline mr-2"
                style="cursor: pointer;">Datos
                adicionales</span>
              <v-avatar color="grey-lighten-4" size="32" class="cursor-pointer">
                <ui-icon name="UserOutlined" size="15" class="text-teal" />
              </v-avatar>
            </div>

            <!-- Hidden/Compact Fields (Preserving logic) -->
            <v-expand-transition>
              <div v-show="showAdditionalData" class="mb-4">
                <div class="d-flex gap-2 mb-2">
                  <select-page v-model="model.CustomerId" :service="customerServ" :selected-label="customerLabel"
                    :rules="[rRequired]" label-key="Person.FirstName" value-key="CustomerId" placeholder="Cliente..."
                    variant="outlined" density="compact" hide-details menu-icon="">
                    <template #append-inner>
                      <ui-icon name="SearchOutlined" size="16" />
                    </template>
                  </select-page>
                  <v-btn variant="outlined" class="rounded-lg border-grey ml-1 px-0 " width="50" color="grey-darken-1"
                    @click="onAddCustomer" icon title="Nuevo Cliente">
                    <ui-icon name="UserAddOutlined" size="18" />
                  </v-btn>
                </div>

                <v-row dense>
                  <v-col cols="12">
                    <v-select v-model="model.WarehouseId" :items="metadata.CmbWarehouses" :rules="[rRequired]"
                      item-title="Name" item-value="WarehouseId" placeholder="Almacén" density="compact"
                      variant="outlined" hide-details />
                  </v-col>
                  <v-col cols="6">
                    <v-select v-model="model.Type" :items="metadata.CmbType" :rules="[rRequired]" item-title="Name"
                      item-value="Code" placeholder="Tipo" density="compact" variant="outlined" hide-details />
                  </v-col>
                  <v-col cols="6">
                    <v-select v-model="model.StatusCode" readonly :items="metadata.CmbStatus" item-title="Name"
                      item-value="Code" placeholder="Estado" density="compact" variant="outlined" hide-details />
                  </v-col>
                </v-row>
              </div>
            </v-expand-transition>

            <!-- Cart List -->
            <div class="flex-grow-1 overflow-y-auto pr-1">
              <div v-for="(detail, idx) in model.Details" :key="idx"
                class="d-flex align-start mb-4 animate__animated animate__fadeIn">
                <!-- Qty -->
                <div class="mr-3" style="width: 60px">
                  <v-text-field v-model.number="detail.QtyPresent" density="compact" variant="outlined"
                    hide-details="auto" :rules="[rQtyGreaterThanZero, (v) => rQtyStock(v, detail)]" type="number"
                    min="1" class="centered-input w-100 field-pl-2 field-pr-1" style="margin-top: 0px !important;"
                    @update:model-value="onQtyPresentChange(detail)">
                  </v-text-field>
                </div>

                <!-- Info -->
                <div class="flex-grow-1">
                  <!-- Product Selector / Name -->
                  <div class="font-weight-bold text-body-2">{{ detail._ProdName }}</div>

                  <!-- Unit Price -->
                  <div class="d-flex align-center text-caption text-grey">
                    <div style="width: 79px">
                      <v-select class="field-pa-0 field-font-12" v-model="detail.PresentCode"
                        :readonly="detail.DisablePresent" :items="detail.PresentItems" item-title="Presentation.Name"
                        item-value="Presentation.Code" variant="plain" density="compact" hide-details
                        @update:model-value="onQtyPresentChange(detail)" style="margin: 0px !important;">
                        <template #selection="{ item }">
                          {{ `${item.raw.Presentation.Code}-${item.raw.QtyProduct}` }}
                        </template>
                        <template #item="{ props, item }">
                          <v-list-item v-bind="props"
                            :title="item.raw.Presentation.Name + ' - ' + item.raw.QtyProduct" />
                        </template>
                      </v-select>
                    </div>
                    {{ formatCurrency(detail.UnitPrice) }}
                  </div>
                </div>

                <!-- Total & Delete -->
                <div class="d-flex flex-column align-end justify-space-between" style="min-height: 40px;">
                  <v-btn icon variant="text" size="x-small" color="grey-lighten-1" @click="removeDetail(idx)"
                    class="mb-1">
                    <ui-icon name="CloseOutlined" size="12" />
                  </v-btn>
                  <div class="font-weight-bold text-body-2">{{ formatCurrency(detail.TotalPrice) }}</div>
                </div>
              </div>
            </div>

            <!-- Footer Section -->
            <div class="mt-4 pt-4 border-t">
              <div class="d-flex justify-space-between mb-2 text-body-2 text-grey-darken-1">
                <span>{{ model.Details?.length ?? 0 }} items</span>
                <span>Subtotal: <strong>{{ formatCurrency(grandTotal) }}</strong></span>
              </div>
              <!-- Discount section removed as requested -->

              <div class="d-flex justify-space-between align-end mb-6">
                <span class="text-h6 font-weight-bold text-grey-darken-3">Total:</span>
                <span class="text-h5 font-weight-black text-grey-darken-4">{{ formatCurrency(grandTotal) }}</span>
              </div>

              <div class="d-flex gap-2">
                <v-btn variant="outlined" class="rounded-lg border-grey mr-1" height="50" width="50"
                  color="grey-darken-1" @click="onCancel">
                  <ui-icon name="DeleteOutlined" size="18" />
                </v-btn>
                <v-btn color="success"
                  class="flex-grow-1 rounded-lg text-white text-capitalize text-body-1 font-weight-bold" height="50"
                  flat @click="onAccept">
                  Pagar
                  <ui-icon name="ShoppingCartOutlined" class="ml-2" />
                </v-btn>
              </div>
            </div>
          </v-card>
        </v-col>
      </v-row>
      <customer-form ref="customerFormRef" />
      <payment-method ref="paymentFormRef" />
    </template>
  </dialog-body>
</template>

<script setup>
import { computed, ref, inject } from 'vue'
import { message } from 'ant-design-vue'
import CustomerForm from '@/views/pages/ams/customers/components/CustomerForm.vue'
import PaymentMethod from '@/views/pages/som/sales/components/PaymentMethod.vue'

const { salesServ, productServ, customerServ, uiStore } = inject('services')
const { question } = inject('MsgDialog')

const showModal = ref(false)
const showAdditionalData = ref(false)
const customerFormRef = ref(null)
const paymentFormRef = ref(null)
const model = ref({})

const metadata = ref({
  CmbCustomers: [],
  CmbWarehouses: [],
  CmbStatus: [],
  CmbType: []
})

const products = ref([])
// Removed staticProducts as we are using real data now

const rRequired = v => (v !== null && v !== undefined && v !== '') || 'Campo requerido'
const rQtyGreaterThanZero = v => v > 0 || 'Cantidad debe ser mayor a 0'
const rQtyStock = (val, detail) => {
  const stock = getStockByWarehouse(detail.Product)
  const pItem = detail.PresentItems.find(x => (x.Presentation?.Code || x.PresentCode) === detail.PresentCode)
  const qtyTotal = (val ?? 0) * (pItem?.QtyProduct ?? 1)
  if (qtyTotal > stock) {
    const max = Math.floor(stock / (pItem?.QtyProduct ?? 1))
    return `Stock insuficiente (Máx: ${max})`
  }
  return true
}

function getStockByWarehouse(prod) {
  if (!prod?.Stocks) return 0
  const stockInfo = prod.Stocks.find(s => s.WarehouseId === model.value.WarehouseId)
  return stockInfo ? stockInfo.QtyOnHand : 0
}

const customerLabel = computed(() => {
  if (!model.value.CustomerId) return null
  const customer = metadata.value.CmbCustomers.find(c => c.CustomerId === model.value.CustomerId) ||
    customerServ.pageData.Data.find(c => c.CustomerId === model.value.CustomerId)
  return customer ? customer.Person?.FirstName : 'Cliente'
})

const grandTotal = computed(() => (model.value.Details ?? []).reduce((acc, d) => acc + Number(d.TotalPrice ?? 0), 0))

let _resolve = null

async function openForm() {
  uiStore.isLoadingBody = true
  model.value = getDefaultModel()
  await loadMetadata()
  products.value = []
  uiStore.isLoadingBody = false
  showModal.value = true

  return new Promise(resolve => {
    _resolve = resolve
  })
}

async function onAcceptClick() {
  if (!model.value.Details?.length) {
    message.warning('El carrito debe tener al menos un item.')
    return
  }
  const paymentMethods = await paymentFormRef.value.openForm(grandTotal.value)
  if (!paymentMethods) return

  model.value.GrandTotal = grandTotal.value
  model.value.PaymentMethods = paymentMethods
  uiStore.isLoadingBody = true

  let txn = await salesServ.create(model.value)
  if (txn) salesServ.addItemPage(txn)

  uiStore.isLoadingBody = false
  if (!txn) return

  _resolve?.(txn)
  _resolve = null
  showModal.value = false
}

async function onCancel() {

  const confirmed = await question(
    'Cancelar Venta',
    `¿Desea cancelar la venta?`
  )

  if (!confirmed) return

  _resolve?.(null)
  _resolve = null
  showModal.value = false
}

async function onAddCustomer() {
  const newCust = await customerFormRef.value.openForm('Insert')
  if (newCust) {
    model.value.CustomerId = newCust.CustomerId
    // Asegurarse de que esté en la lista para que el label funcione
    if (!metadata.value.CmbCustomers.some(c => c.CustomerId === newCust.CustomerId)) {
      metadata.value.CmbCustomers.push(newCust)
    }
  }
}

async function loadMetadata() {
  const meta = await salesServ.getMetadata()
  if (meta) {
    metadata.value = meta
    model.value.WarehouseId = meta.DefaultWarehouseId
    model.value.CustomerId = meta.DefaultCustomerId
    customerServ.pageData.Data = meta.CmbCustomers || []
  }
}

function onCatalogProductClick(prod) {
  const exists = model.value.Details.some(d => d.NroProduct === prod.NroProduct || d.Product?.NroProduct === prod.NroProduct)

  if (exists) {
    message.warning('Este producto ya ha sido agregado a la lista.')
    return
  }

  const detail = createDetail()
  onProductPicked(detail, prod)
  model.value.Details.push(detail)
}

function removeDetail(idx) {
  if (model.value.Details.length > 1) {
    model.value.Details.splice(idx, 1)
  }
}

function getDefaultModel() {
  return {
    TxnId: 0,
    CustomerId: null,
    WarehouseId: null,
    Type: 'VEN',
    StatusCode: 'ACT',
    CurrencyCode: 'BOB',
    Reference: null,
    GrandTotal: 0,
    Details: [],
    PaymentMethods: []
  }
}

function createDetail() {
  return {
    DetailId: 0,
    SalesId: 0,
    NroProduct: null,
    QtyProduct: 0,
    QtyPresent: 1,
    PresentCode: null,
    PresentItems: [],
    DisablePresent: true,
    UnitPrice: 0,
    TotalPrice: 0,
    _ProdName: null
  }
}

function onProductPicked(detail, product) {
  console.log(product)
  detail._ProdName = product?.Name ?? detail._ProdName ?? null
  detail.Product = product
  detail.NroProduct = product?.NroProduct ?? null
  detail.UnitPrice = product?.Price ?? 0
  detail.PresentItems = [{ QtyProduct: 1, Presentation: { Name: product.Presentation.Name, Code: product.Presentation.Code }, Price: detail.UnitPrice }]
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
    detail.UnitPrice = pItem.Price
  }
  calcTotals(detail)
}

// function onCostChange(detail) {
//   calcTotals(detail)
// }

function calcTotals(detail) {
  detail.TotalPrice = Number(detail.UnitPrice ?? 0) * Number(detail.QtyPresent ?? 0)
}

// function selectedIds(exceptDetail = null) {
//   return new Set(
//     (model.value.Details ?? [])
//       .filter(d => d !== exceptDetail && d?.NroProduct != null)
//       .map(d => d.NroProduct)
//   )
// }

const formatCurrency = (amount) => new Intl.NumberFormat('es-BO', { style: 'currency', currency: 'BOB' }).format(amount ?? 0)

const onSearch = async (text) => {
  productServ.pageParams = {
    pageSize: 10,
    pageNumber: 1,
    filter: text,
    isSales: true
  }
  const data = await productServ.loadPage()
  if (data) {
    console.log(data.Data)
    products.value = data.Data || []
  }
}

defineExpose({
  openForm
})
</script>
