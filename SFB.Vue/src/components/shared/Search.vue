<template>
  <v-text-field v-model="searchText" class="mr-1" :placeholder="placeholder" persistent-placeholder color="primary"
    variant="outlined" hide-details density="compact" clearable @keyup.enter="onSearch" @click:prepend-inner="onSearch"
    @click:clear="onClear" style="margin-top: 0px !important;">
    <template v-slot:prepend-inner>
      <SearchOutlined :style="{ fontSize: '16px', color: 'rgb(var(--v-theme-lightText))', cursor: 'pointer' }" />
    </template>
  </v-text-field>
</template>

<script setup>
import { SearchOutlined } from '@ant-design/icons-vue'
import { defineProps, defineEmits, watch, ref } from 'vue'

const props = defineProps({
  modelValue: String,
  placeholder: { type: String, default: 'Buscar...' }
})

const emit = defineEmits(['update:modelValue', 'search'])

const searchText = ref(props.modelValue)

watch(searchText, (val) => emit('update:modelValue', val))
watch(() => props.modelValue, (val) => {
  if (val !== searchText.value) searchText.value = val
})

function onSearch() {
  emit('search', searchText.value)
}

function onClear() {
  searchText.value = ''
  emit('update:modelValue', '')
  emit('search', '')
}
</script>
