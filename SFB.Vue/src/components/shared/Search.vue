<template>
  <v-text-field v-model="searchText" :placeholder="placeholder" persistent-placeholder color="primary"
    variant="outlined" hide-details density="compact" clearable>
    <template v-slot:prepend-inner>
      <SearchOutlined :style="{ fontSize: '12px', color: 'rgb(var(--v-theme-lightText))' }" />
    </template>
  </v-text-field>
</template>

<script setup>
import { SearchOutlined } from '@ant-design/icons-vue';
import { defineProps, defineEmits, watch, ref } from 'vue';

// Props: placeholder y modelValue
const props = defineProps({
  modelValue: String,
  placeholder: {
    type: String,
    default: 'Buscar...'
  }
});

// Emitir evento al escribir
const emit = defineEmits(['update:modelValue']);

// Valor interno editable (v-model)
const searchText = ref(props.modelValue);

// Sincroniza con padre
watch(searchText, (val) => emit('update:modelValue', val));

// Opcional: sincroniza desde fuera
watch(() => props.modelValue, (val) => {
  if (val !== searchText.value) {
    searchText.value = val;
  }
});
</script>
