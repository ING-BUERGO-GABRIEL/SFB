<template>
  <dialog-body v-model="showModal" title="Vender" fullscreen hide-actions class-body="bg-containerBg">
    <v-row class="fill-height ma-0 text-body-1 ">
      <!-- Left Column: Product Catalog -->
      <v-col cols="12" md="8" class="pa-0">
        <!-- Header Section -->
        <header-bar class-card="pa-3">
          <v-text-field density="compact" variant="plain" placeholder="Nombre o código" hide-details
            class="flex-grow-1 mr-4">
            <template #append-inner>
              <SearchOutlined class="text-grey" />
            </template>
          </v-text-field>
          <v-divider vertical class="mx-2 my-1"></v-divider>
          <v-menu>
            <template #activator="{ props }">
              <v-btn variant="text" v-bind="props" class="text-capitalize text-body-2" color="grey-darken-2">
                <TagOutlined class="mr-2" /> Categorías
                <DownOutlined class="ml-2" style="font-size: 10px;" />
              </v-btn>
            </template>
          </v-menu>
          <v-spacer></v-spacer>
          <div class="d-flex align-center text-grey-darken-1 gap-4">
            <ScanOutlined style="font-size: 20px; cursor: pointer;" class="mx-2" title="Scan" />
            <ThunderboltOutlined style="font-size: 20px; cursor: pointer;" class="mx-2" title="Quick Actions" />
            <UnorderedListOutlined style="font-size: 20px; cursor: pointer;" class="mx-2" title="List View" />
          </div>
        </header-bar>

        <!-- Product Grid (Static Visual Representation) -->
        <v-row dense>
          <v-col v-for="prod in staticProducts" :key="prod.id" cols="12" sm="6" md="4" lg="3">
            <v-card flat
              class="rounded-lg overflow-hidden border cursor-pointer h-100 position-relative transition-swing elevation-0 hover-card"
              @click="onStaticProductValues(prod)">
              <!-- Status Dots -->
              <div v-if="prod.status === 'low'" class="position-absolute top-0 left-0 ma-2 z-index-1"
                style="width: 8px; height: 8px; background: red; border-radius: 50%;"></div>
              <div v-if="prod.status === 'med'" class="position-absolute top-0 left-0 ma-2 z-index-1"
                style="width: 8px; height: 8px; background: orange; border-radius: 50%;"></div>

              <!-- Qty Badge -->
              <v-chip v-if="prod.qty" color="success" size="x-small" label
                class="position-absolute top-0 right-0 ma-0 rounded-bs-lg rounded-tr-lg"
                style="height: 24px; min-width: 24px; justify-content: center; z-index: 1;">
                {{ prod.qty }}
              </v-chip>

              <div class="d-flex flex-column align-center justify-center bg-grey-lighten-5 pt-4 pb-2">
                <v-img :src="prod.image" height="100" width="100" contain class="mb-2"></v-img>
              </div>

              <div class="pa-3 bg-blue-grey-darken-4 text-white">
                <div class="font-weight-bold text-subtitle-2 mb-0 text-truncate">{{ prod.name }}</div>
                <div class="d-flex align-center">
                  <div class="text-caption font-weight-bold">${{ prod.price }}</div>
                  <div v-if="prod.oldPrice" class="text-caption text-decoration-line-through opacity-50 ml-2">${{
                    prod.oldPrice
                    }}</div>
                </div>
              </div>
            </v-card>
          </v-col>

          <!-- Add Custom Item Card Mock -->
          <v-col cols="12" sm="6" md="4" lg="3">
            <v-card flat
              class="rounded-lg border cursor-pointer h-100 d-flex align-center justify-center bg-teal-lighten-4 hover-card"
              style="min-height: 180px" @click="addDetail">
              <PlusOutlined style="font-size: 40px; color: white;" />
            </v-card>
          </v-col>
        </v-row>
      </v-col>

      <!-- Right Column: Cart & Details -->
      <v-col cols="12" md="4" class="bg-white d-flex flex-column pa-5 border-s shadow-left"
        style="height: 100vh; overflow-y: auto;">
        <!-- Header -->
        <div class="d-flex justify-end align-center mb-6">
          <span class="text-teal font-weight-medium text-body-2 mr-2" style="cursor: pointer;">Seleccionar
            cliente</span>
          <v-avatar color="grey-lighten-4" size="32" class="cursor-pointer">
            <UserOutlined class="text-teal" />
          </v-avatar>
        </div>

        <!-- Hidden/Compact Fields (Preserving logic) -->
        <div class="mb-4">
          <v-select v-model="model.CustomerId" :items="metadata.CmbCustomers" :rules="[rRequired]"
            item-title="Person.FirstName" item-value="CustomerId" placeholder="Cliente..." variant="outlined"
            density="compact" hide-details class="mb-2" />

          <v-row dense>
            <v-col cols="12">
              <v-select v-model="model.WarehouseId" :items="metadata.CmbWarehouses" :rules="[rRequired]"
                item-title="Name" item-value="WarehouseId" placeholder="Almacén" density="compact" variant="outlined"
                hide-details />
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

        <!-- Cart List -->
        <div class="flex-grow-1 overflow-y-auto pr-1">
          <div v-for="(detail, idx) in model.Details" :key="idx"
            class="d-flex align-start mb-4 animate__animated animate__fadeIn">
            <!-- Qty -->
            <div class="mr-3 pt-2">
              <v-text-field v-model.number="detail.QtyPresent" density="compact" variant="outlined" hide-details
                type="number" min="1" class="centered-input rounded-lg" style="width: 50px"
                @update:model-value="onQtyPresentChange(detail)"></v-text-field>
            </div>

            <!-- Info -->
            <div class="flex-grow-1">
              <!-- Product Selector / Name -->
              <div v-if="detail._ProdName" class="font-weight-bold text-body-2 mb-1">{{ detail._ProdName }}</div>
              <div v-else class="mb-1">
                <select-page v-model="detail.NroProduct" :service="productServ" :taken-ids="[...selectedIds(detail)]"
                  :selected-label="detail._ProdName" :rules="[rRequired]" placeholder="Buscar producto..."
                  @picked="p => onProductPicked(detail, p)" variant="plain" density="compact" hide-details />
              </div>

              <!-- Unit Price -->
              <div class="text-caption text-grey">
                ${{ detail.UnitPrice }}
              </div>
            </div>

            <!-- Total & Delete -->
            <div class="d-flex flex-column align-end justify-space-between" style="min-height: 40px;">
              <v-btn icon variant="text" size="x-small" color="grey-lighten-1" @click="removeDetail(idx)" class="mb-1">
                <CloseOutlined style="font-size: 12px" />
              </v-btn>
              <div class="font-weight-bold text-body-2">${{ detail.TotalPrice }}</div>
            </div>
          </div>
        </div>

        <!-- Footer Section -->
        <div class="mt-4 pt-4 border-t">
          <div class="d-flex justify-space-between mb-2 text-body-2 text-grey-darken-1">
            <span>{{ model.Details?.length ?? 0 }} items</span>
            <span>Subtotal: <strong>{{ formatCurrency(grandTotal) }}</strong></span>
          </div>
          <div class="d-flex justify-space-between mb-4 text-caption text-error bg-red-lighten-5 px-2 py-1 rounded">
            <span class="font-weight-medium">Editar descuento</span>
            <span>x (0%) -$0.00</span>
          </div>

          <div class="d-flex justify-space-between align-end mb-6">
            <span class="text-h6 font-weight-bold text-grey-darken-3">Total:</span>
            <span class="text-h5 font-weight-black text-grey-darken-4">{{ formatCurrency(grandTotal) }}</span>
          </div>

          <div class="d-flex gap-2">
            <v-btn variant="outlined" class="rounded-lg border-grey" height="50" width="50" color="grey-darken-1"
              @click="onCancel">
              <DeleteOutlined style="font-size: 18px;" />
            </v-btn>
            <v-btn color="success"
              class="flex-grow-1 rounded-lg text-white text-capitalize text-body-1 font-weight-bold" height="50" flat
              @click="onAccept">
              Ir al pago
              <RightOutlined class="ml-2" />
            </v-btn>
          </div>
        </div>
      </v-col>
    </v-row>
  </dialog-body>
</template>

<script setup>
import { computed, ref, inject } from 'vue'
import {
  DeleteOutlined,
  TagOutlined,
  DownOutlined,
  ScanOutlined,
  ThunderboltOutlined,
  UnorderedListOutlined,
  PlusOutlined,
  UserOutlined,
  CloseOutlined,
  RightOutlined,
  SearchOutlined
} from '@ant-design/icons-vue'

const { salesServ, productServ, uiStore } = inject('services')
const { question } = inject('MsgDialog')

const showModal = ref(false)
const titleDlg = ref('')
const model = ref({})

const metadata = ref({
  CmbCustomers: [],
  CmbWarehouses: [],
  CmbStatus: [],
  CmbType: []
})

// Static Data for Visual Mockup
const staticProducts = ref([
  { id: 1, name: 'Acquaplay', price: 11.11, qty: 1, image: 'https://cdn.vuetifyjs.com/images/cards/cooking.png', status: 'med' },
  { id: 2, name: 'Beeper', price: 123.45, qty: 0, image: 'https://cdn.vuetifyjs.com/images/cards/sunshine.jpg' },
  { id: 3, name: 'Boombox', price: 665.54, qty: 1, image: 'https://cdn.vuetifyjs.com/images/cards/docks.jpg', status: 'med' },
  { id: 4, name: 'Dollface', price: 33.22, qty: 1, image: 'https://cdn.vuetifyjs.com/images/cards/road.jpg' },
  { id: 5, name: 'Gametoy', price: 334.45, qty: 1, image: 'https://cdn.vuetifyjs.com/images/cards/plane.jpg' },
  { id: 6, name: 'Polystation', price: 499.99, oldPrice: 556.65, qty: 0, image: 'https://cdn.vuetifyjs.com/images/cards/house.jpg', status: 'low' },
  { id: 7, name: 'Tamagotchu', price: 33.55, qty: 0, image: 'https://cdn.vuetifyjs.com/images/cards/forest.jpg' },
  { id: 8, name: 'Turbie', price: 11.22, oldPrice: 22.33, qty: 0, image: 'https://cdn.vuetifyjs.com/images/cards/mountain.jpg', status: 'low' },
])

const rRequired = v => (v !== null && v !== undefined && v !== '') || 'Campo requerido'

const grandTotal = computed(() => (model.value.Details ?? []).reduce((acc, d) => acc + Number(d.TotalPrice ?? 0), 0))

let _resolve = null

async function openForm() {
  uiStore.isLoadingBody = true
  await loadMetadata()
  model.value = getDefaultModel()

  uiStore.isLoadingBody = false
  showModal.value = true

  return new Promise(resolve => {
    _resolve = resolve
  })
}

async function onAccept() {
  model.value.GrandTotal = grandTotal.value
  uiStore.isLoadingBody = true

  let txn = await salesServ.create(model.value)
  if (txn) salesServ.addItemPage(txn)

  uiStore.isLoadingBody = false
  if (!txn) return

  _resolve?.(txn)
  _resolve = null
  showModal.value = false
}

function onCancel() {
  _resolve?.(null)
  _resolve = null
  showModal.value = false
}

async function loadMetadata() {
  const meta = await salesServ.getMetadata()
  if (meta) {
    metadata.value = meta
  }
}

function addDetail() {
  model.value.Details.push(createDetail())
}

function onStaticProductValues(prod) {
  // Add logic to add static product to detail
  // This is optional for visual demo, but helps feel "real"
  const detail = createDetail()
  detail._ProdName = prod.name
  detail.UnitPrice = prod.price
  detail.QtyPresent = 1
  calcTotals(detail)
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
    Details: [createDetail()]
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
  detail.TotalPrice = Number(detail.UnitPrice ?? 0) * Number(detail.QtyPresent ?? 0)
}

function selectedIds(exceptDetail = null) {
  return new Set(
    (model.value.Details ?? [])
      .filter(d => d !== exceptDetail && d?.NroProduct != null)
      .map(d => d.NroProduct)
  )
}

const formatCurrency = (amount) => new Intl.NumberFormat('es-BO', { style: 'currency', currency: 'BOB' }).format(amount ?? 0)

const customerName = (customer) => {
  const p = customer?.Person
  if (!p) return ''
  return `${p.Name} ${p.FirstLastName ?? ''} ${p.SecondLastName ?? ''}`.trim()
}

defineExpose({
  openForm
})
</script>

<style scoped>
.centered-input :deep(input) {
  text-align: center;
}

.shadow-left {
  box-shadow: -4px 0 15px rgba(0, 0, 0, 0.05);
}

.hover-card:hover {
  transform: translateY(-2px);
  transition: all 0.2s ease;
}

.z-index-1 {
  z-index: 1;
}

/* Hide scrollbar for Chrome, Safari and Opera */
::-webkit-scrollbar {
  display: none;
}

/* Hide scrollbar for IE, Edge and Firefox */
.v-col {
  -ms-overflow-style: none;
  /* IE and Edge */
  scrollbar-width: none;
  /* Firefox */
}
</style>
