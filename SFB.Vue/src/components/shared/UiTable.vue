<!-- BaseTable.vue -->
<template>
  <v-data-table
    :headers="props.headers"
    :items="props.items"
    :loading="props.loading"
    :item-key="props.itemKey"
    class="mb-3"
    :header-props="{
      class: 'bg-containerBg text-caption font-weight-bold text-uppercase'
    }"
    hide-default-footer
  >
    <!-- Reexpone todos los slots que reciba el componente -->
    <template v-for="(_, name) in $slots" v-slot:[name]="slotProps">
      <slot :name="name" v-bind="slotProps" />
    </template>

    <template #no-data>
      <div class="text-center pa-4 text-medium-emphasis">
        Sin datos disponibles
      </div>
    </template>
  </v-data-table>
</template>

<script setup>
import { defineProps } from 'vue'

const props = defineProps({
  headers: {
    type: Array,
    required: true
  },
  items: {
    type: Array,
    default: () => []
  },
  loading: {
    type: Boolean,
    default: false
  },
  // por si quieres cambiar la key desde afuera
  itemKey: {
    type: String,
    default: 'id'
  }
})
</script>
