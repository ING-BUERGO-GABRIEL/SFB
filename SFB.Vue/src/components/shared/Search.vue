<template>
  <div v-bind="$attrs" style="display: contents;">
    <!-- Desktop Search (Visible on Tablet and Desktop) -->
    <v-sheet class="d-none d-sm-block" width="250">
      <v-text-field v-model="searchText" :placeholder="placeholder" persistent-placeholder color="primary"
        variant="outlined" hide-details density="compact" clearable @keyup.enter="onSearch"
        @click:prepend-inner="onSearch" @click:clear="onClear" style="margin-top: 0px !important;">
        <template v-slot:prepend-inner>
          <SearchOutlined :style="{ fontSize: '12px', color: 'rgb(var(--v-theme-lightText))', cursor: 'pointer' }" />
        </template>
      </v-text-field>
    </v-sheet>

    <!-- Mobile Search (Only on XS) -->
    <v-menu v-model="menu" :close-on-content-click="false" class="hidden-sm-and-up" offset="10, 0">
      <template v-slot:activator="{ props }">
        <v-btn class="hidden-sm-and-up text-secondary ml-1" color="lightsecondary" icon rounded="sm" variant="flat"
          size="small" v-bind="props">
          <SearchOutlined :style="{ fontSize: '17px' }" />
        </v-btn>
      </template>
      <v-sheet class="search-sheet" width="320" elevation="10" rounded="lg">
        <v-text-field ref="mobileSearchRef" v-model="searchText" persistent-placeholder :placeholder="placeholder"
          color="primary" variant="solo" hide-details density="compact" clearable flat @keyup.enter="onSearch"
          @click:prepend-inner="onSearch" @click:clear="onClear">
          <template v-slot:prepend-inner>
            <SearchOutlined :style="{ fontSize: '17px', color: 'rgb(var(--v-theme-lightText))' }" />
          </template>
        </v-text-field>
      </v-sheet>
    </v-menu>
  </div>
</template>

<script setup>
import { SearchOutlined } from '@ant-design/icons-vue'
import { defineProps, defineEmits, watch, ref, nextTick } from 'vue'

defineOptions({
  inheritAttrs: false
})

const props = defineProps({
  modelValue: String,
  placeholder: { type: String, default: 'Buscar...' }
})

const emit = defineEmits(['update:modelValue', 'search'])

const searchText = ref(props.modelValue)
const menu = ref(false)
const mobileSearchRef = ref(null)

watch(searchText, (val) => emit('update:modelValue', val))
watch(() => props.modelValue, (val) => {
  if (val !== searchText.value) searchText.value = val
})

// Focus the input when menu opens
watch(menu, (val) => {
  if (val) {
    nextTick(() => {
      mobileSearchRef.value?.focus()
    })
  }
})

function onSearch() {
  emit('search', searchText.value)
  menu.value = false // Auto close on search if desired
}

function onClear() {
  searchText.value = ''
  emit('update:modelValue', '')
  emit('search', '')
}
</script>

<style scoped>
.search-sheet {
  overflow: hidden;
}
</style>
