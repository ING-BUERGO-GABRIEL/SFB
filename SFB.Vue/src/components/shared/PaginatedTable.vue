<template>
  <v-data-table :header-props="{ class: 'bg-containerBg text-caption font-weight-bold text-uppercase' }"
  :items="pageData?.Data">
      <template v-slot:item.IsSales="{ value }">
        <v-chip variant="text" size="small" class="px-0">
          <v-avatar size="8" :color="value ?'success':'error'" variant="flat" class="mr-2"></v-avatar>
          <p class="text-h6 mb-0">{{value ?'Habilitado':'Desabilitado'}}</p>
        </v-chip>
      </template>
  </v-data-table>
</template>

<script setup>
import { defineProps,onMounted, ref } from 'vue';
const props = defineProps({ service: { type: Object, required: true } })

var pageData = ref({})

onMounted(async () => {
  pageData.value = props.service.pageData
  await props.service.loadPage()
});

</script>
