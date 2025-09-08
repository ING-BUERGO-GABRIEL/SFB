<!-- PagTable.vue -->
<template>
  <v-data-table-server :headers="props.headers" :items="props.service?.pageData?.Data ?? []" :loading="loading"
    :header-props="{ class: 'bg-containerBg text-caption font-weight-bold text-uppercase' }" item-key="NroProduct"
    :items-length="Math.max(1, props.service?.pageData?.TotalCount ?? 0)"
    :items-per-page="itemsPerPage" :items-per-page-options="[7, 15, 25]"
    items-per-page-text="Ítems por página:" page-text="{0}-{1} de {2}"               
    @update:items-per-page="onItemsPerPage" @update:page="onPage" >
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

const itemsPerPage = ref(7)

const onPage = async (newPage) => {
  console.log(newPage)

  // const direction = newPage > lastPage.value ? 'next' : (newPage < lastPage.value ? 'prev' : 'stay')
  // lastPage.value = newPage

  // page.value = newPage
  // // 👉 aquí ya “capturaste” el evento:
  // console.log('Cambio de página:', { page: newPage, direction })
  // fetchPage()
}

const onItemsPerPage = async (newSize) => {

  console.log(newSize)
  itemsPerPage.value = newSize
  // page.value = 1 // conviene resetear a la primera página
  // // 👉 evento capturado:
  // console.log('Cambio de pageSize:', { itemsPerPage: newSize })
  // fetchPage()
}



onMounted(async () => {
  loading.value = true
  var pageParams = {
    PageSize:7,
    PageNumber:1,
  }
  props.service.pageParams = pageParams
  await props.service.loadPage()
  loading.value = false
})
</script>
