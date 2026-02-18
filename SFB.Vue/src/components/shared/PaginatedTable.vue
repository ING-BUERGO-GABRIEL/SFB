<!-- PagTable.vue -->
<template>
  <v-data-table-server :headers="props.headers" :items="props.service?.pageData?.Data ?? []"
    :loading="props.service?.loadTable"
    :header-props="{ class: 'bg-containerBg text-caption font-weight-bold text-uppercase' }" item-key="NroProduct"
    :items-length="Math.max(1, props.service?.pageData?.TotalCount ?? 0)"
    :items-per-page="props.service?.pageData?.PageSize" :items-per-page-options="[7, 15, 25]"
    items-per-page-text="Ítems por página:" page-text="{0}-{1} de {2}" @update:items-per-page="onItemsPerPage"
    @update:page="onPage">
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
import { defineProps, onMounted } from 'vue'

const props = defineProps({
  service: { type: Object, required: true },
  headers: { type: Array, required: true },
  initParams: { type: Object, required: false }
})

const defaultParams = {
  pageSize: 7,
  pageNumber: 1,
  filter: ''
}

const onPage = async (newPage) => {
  // eslint-disable-next-line vue/no-mutating-props
  props.service.pageParams.pageNumber = newPage
  await props.service.loadPage()
}

const onItemsPerPage = async (newSize) => {
  // eslint-disable-next-line vue/no-mutating-props
  props.service.pageParams.pageSize = newSize
  // eslint-disable-next-line vue/no-mutating-props
  props.service.pageParams.pageNumber = 1
  await props.service.loadPage()
}

onMounted(async () => {
  // eslint-disable-next-line vue/no-mutating-props
  props.service.pageParams = { ...defaultParams, ...props.params }
  await props.service.loadPage()
})
</script>
