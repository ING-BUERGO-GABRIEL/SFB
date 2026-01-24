<template>
  <v-autocomplete :items="items" :item-title="getLabel" :item-value="getValue" :loading="service.loadTable"
    :multiple="false" :model-value="modelValue" v-bind="$attrs" @update:search="onSearch" @update:modelValue="onPick">
    <template v-for="(_, name) in $slots" #[name]="slotData">
      <slot :name="name" v-bind="slotData || {}" />
    </template>
  </v-autocomplete>
</template>

<script setup>
import { computed, onMounted } from 'vue'

const props = defineProps({
  modelValue: [Number, String, null],
  service: { type: Object, required: true },
  takenIds: { type: [Array, Set], default: () => [] },
  selectedLabel: { type: String, default: null },
  labelKey: { type: [String, Function], default: 'Name' },
  valueKey: { type: String, default: 'NroProduct' },
  minChars: { type: Number, default: 3 },
  pageSize: { type: Number, default: 8 },
})

const emit = defineEmits(['update:modelValue', 'picked'])

const listRaw = computed(() => props.service.pageData?.Data ?? [])

/** Convierte takenIds a Set para filtrar fácil */
const takenSet = computed(() =>
  props.takenIds instanceof Set ? props.takenIds : new Set(props.takenIds)
)

const getByPath = (obj, path) => {
  if (!obj || !path) return undefined
  return path.split('.').reduce((acc, key) => acc?.[key], obj)
}

const setByPath = (obj, path, value) => {
  if (!path) return
  const parts = path.split('.')
  let cursor = obj
  for (let i = 0; i < parts.length - 1; i++) {
    const key = parts[i]
    if (cursor[key] == null || typeof cursor[key] !== 'object') cursor[key] = {}
    cursor = cursor[key]
  }
  cursor[parts[parts.length - 1]] = value
}

const getValue = (item) => getByPath(item, props.valueKey)

const getLabel = (item) => {
  if (item?.__label != null) return item.__label
  if (typeof props.labelKey === 'function') return props.labelKey(item)
  return getByPath(item, props.labelKey)
}

/** Items visibles: oculta los ya tomados y preserva el seleccionado actual */
const items = computed(() => {
  const current = props.modelValue

  let base = listRaw.value.filter(p => !takenSet.value.has(getValue(p)))

  // Si el seleccionado actual no está, lo inyectamos al inicio
  if (
    current != null &&
    !base.some(p => getValue(p) === current)
  ) {
    const injected = { __label: props.selectedLabel ?? 'Seleccionado' }
    setByPath(injected, props.valueKey, current)
    base = [injected, ...base]
  }
  return base
})

async function onSearch(text) {
  if (!text || text.length < props.minChars) return
  props.service.pageParams = { filter: text, pageNumber: 1, pageSize: props.pageSize }
  await props.service.loadPage()
}

function onPick(val) {
  // Emitimos el id y el objeto seleccionado (por si el padre quiere guardar el nombre)
  emit('update:modelValue', val)
  const found = listRaw.value.find(p => getValue(p) === val) || null
  emit('picked', found)
}

onMounted(() => {
  props.service.loadTable = false
})
</script>
