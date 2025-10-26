
<template>  
  <v-autocomplete
    :items="items"
    :item-title="labelKey"
    :item-value="valueKey"
    :loading="service.loadTable"
    :multiple="false"
    :model-value="modelValue"
    v-bind="$attrs"
    @update:search="onSearch"
    @update:modelValue="onPick"
  />
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  modelValue: [Number, String, null],
  service: { type: Object, required: true },
  takenIds: { type: [Array, Set], default: () => [] },
  selectedLabel: { type: String, default: null },
  labelKey: { type: String, default: 'Name' },
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

/** Items visibles: oculta los ya tomados y preserva el seleccionado actual */
const items = computed(() => {
  const vKey = props.valueKey
  const lKey = props.labelKey
  const current = props.modelValue

  let base = listRaw.value.filter(p => !takenSet.value.has(p[vKey]))

  // Si el seleccionado actual no está, lo inyectamos al inicio
  if (
    current != null &&
    !base.some(p => p[vKey] === current)
  ) {
    base = [{ [vKey]: current, [lKey]: props.selectedLabel ?? 'Seleccionado' }, ...base]
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
  const found = listRaw.value.find(p => p?.[props.valueKey] === val) || null
  emit('picked', found)
}
</script>
