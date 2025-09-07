<!-- PagTable.vue -->
<template>
  <v-data-table-server :headers="props.headers" :items="props.service?.pageData?.Data ?? []" :loading="loading"
    :header-props="{ class: 'bg-containerBg text-caption font-weight-bold text-uppercase' }" item-key="NroProduct"
    :items-length="Math.max(1, props.service?.pageData?.TotalCount ?? 0)"
    :items-per-page="props.service?.pageData?.PageSize ?? 10" :items-per-page-options="[5, 10, 25]"
    items-per-page-text="Ítems por página:" page-text="{0}-{1} de {2}">
    <template v-for="(_, name) in $slots" v-slot:[name]="slotProps">
      <slot :name="name" v-bind="slotProps"></slot>
    </template>
    <template #no-data>
      <div class="text-center pa-4 text-medium-emphasis">
        Sin datos disponibles
      </div>
    </template>
  </v-data-table-server>
</template>

<script setup>
import { defineProps, onMounted, ref } from 'vue'

const props = defineProps({
  service: { type: Object, required: true },
  headers: { type: Array, required: true }
})

const loading = ref(false)

onMounted(async () => {
  loading.value = true
  await props.service.loadPage()
  loading.value = false
})
</script>
