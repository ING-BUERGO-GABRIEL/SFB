<template>
  <card-dialog v-model="showModal" title="Métodos de pago" height="460" @accept="onAccept" @cancel="onCancel">
    <v-row class="pa-4 pb-0">
      <v-col cols="12" class="d-flex align-center justify-space-between mb-2">
        <div class="text-subtitle-2 font-weight-medium">Detalle de pagos</div>
        <v-btn variant="outlined" size="small" color="teal-darken-1" @click="addPayment">
          Agregar pago
        </v-btn>
      </v-col>

      <v-col cols="12" v-for="(detail, idx) in details" :key="idx" class="pb-0">
        <v-card variant="outlined" class="pa-3">
          <v-row dense>
            <v-col cols="12" sm="4">
              <v-select
                v-model="detail.PaymentMethodCode"
                :items="paymentMethods"
                item-title="Name"
                item-value="Code"
                label="Método"
                density="compact"
                variant="outlined"
                hide-details
              />
            </v-col>
            <v-col cols="12" sm="4">
              <v-text-field
                v-model.number="detail.Amount"
                label="Monto"
                type="number"
                min="0"
                density="compact"
                variant="outlined"
                hide-details
              />
            </v-col>
            <v-col cols="12" sm="3">
              <v-text-field
                v-model="detail.PaymentRef"
                label="Referencia"
                density="compact"
                variant="outlined"
                hide-details
              />
            </v-col>
            <v-col cols="12" sm="1" class="d-flex justify-end align-center">
              <v-btn icon variant="text" color="grey-darken-1" @click="removePayment(idx)" :disabled="details.length === 1">
                <ui-icon name="DeleteOutlined" size="16" />
              </v-btn>
            </v-col>
          </v-row>
        </v-card>
      </v-col>

      <v-col cols="12" class="mt-4">
        <v-divider class="mb-3" />
        <div class="d-flex justify-space-between text-body-2">
          <span>Total venta:</span>
          <strong>{{ formatCurrency(totalAmount) }}</strong>
        </div>
        <div class="d-flex justify-space-between text-body-2">
          <span>Total pagos:</span>
          <strong>{{ formatCurrency(totalPayments) }}</strong>
        </div>
        <div class="d-flex justify-space-between text-body-2">
          <span>Saldo:</span>
          <strong :class="balanceClass">{{ formatCurrency(balance) }}</strong>
        </div>
      </v-col>
    </v-row>
  </card-dialog>
</template>

<script setup>
import { computed, ref } from 'vue'
import { message } from 'ant-design-vue'

const showModal = ref(false)
const details = ref([])
const totalAmount = ref(0)

const paymentMethods = [
  { Code: 'CASH', Name: 'Efectivo' },
  { Code: 'CRD', Name: 'Tarjeta' },
  { Code: 'QR', Name: 'QR' },
  { Code: 'TRF', Name: 'Transferencia' }
]

const totalPayments = computed(() =>
  details.value.reduce((acc, item) => acc + Number(item.Amount ?? 0), 0)
)

const balance = computed(() => Number(totalAmount.value ?? 0) - totalPayments.value)
const balanceClass = computed(() => (Math.abs(balance.value) < 0.01 ? 'text-success' : 'text-error'))

let _resolve = null

function addPayment(amount = null) {
  details.value.push({
    PaymentMethodCode: paymentMethods[0].Code,
    Amount: amount ?? 0,
    PaymentRef: null
  })
}

function removePayment(idx) {
  if (details.value.length > 1) {
    details.value.splice(idx, 1)
  }
}

function formatCurrency(amount) {
  return new Intl.NumberFormat('es-BO', { style: 'currency', currency: 'BOB' }).format(amount ?? 0)
}

async function openForm(total) {
  totalAmount.value = Number(total ?? 0)
  details.value = []
  addPayment(totalAmount.value)
  showModal.value = true

  return new Promise(resolve => {
    _resolve = resolve
  })
}

function onAccept() {
  if (!details.value.length) {
    message.warning('Debe registrar al menos un método de pago.')
    return
  }

  const invalid = details.value.some(detail => Number(detail.Amount ?? 0) <= 0)
  if (invalid) {
    message.warning('Ingrese montos válidos para los pagos.')
    return
  }

  if (Math.abs(balance.value) >= 0.01) {
    message.warning('El total de pagos debe coincidir con el total de la venta.')
    return
  }

  _resolve?.(details.value)
  _resolve = null
  showModal.value = false
}

function onCancel() {
  _resolve?.(null)
  _resolve = null
  showModal.value = false
}

defineExpose({
  openForm
})
</script>
