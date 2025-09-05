<template>
  <title-card title="Lista Productos" class-name="px-0 pb-0 rounded-md">
    <!-- <v-table class="bordered-table" hover density="comfortable">
      <thead class="bg-containerBg">
        <tr>
          <th class="text-left text-caption font-weight-bold text-uppercase">Id Prod.</th>
          <th class="text-left text-caption font-weight-bold text-uppercase">Nombre Pro.</th>
          <th class="text-right text-caption font-weight-bold text-uppercase" style="min-width: 100px">Total Stock</th>
          <th class="text-left text-caption font-weight-bold text-uppercase">Estado</th>
          <th class="text-right text-caption font-weight-bold text-uppercase">Precio</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in productServ.productPage.Data" :key="item.name">
          <td class="py-3 text-secondary ">
            {{ item.NroProduct }}
             <router-link to="/dashboard/default" class="text-secondary link-hover">{{ item.number }}</router-link> 
          </td>
          <td class="py-3">{{ item.Name }}</td>
          <td class="py-3 text-right" style="min-width: 100px">{{ 0 }}</td>
          <td class="py-3">
            <v-chip variant="text" size="small" class="px-0" v-if="item.priority === 'rejected'">
              <v-avatar size="8" color="error" variant="flat" class="mr-2"></v-avatar>
              <p class="text-h6 mb-0">Rejected</p>
            </v-chip>
            <v-chip variant="text" size="small" class="px-0" v-else-if="item.priority === 'approved'">
              <v-avatar size="8" color="success" variant="flat" class="mr-2"></v-avatar>
              <p class="text-h6 mb-0">Approved</p>
            </v-chip>
            <v-chip variant="text" size="small" class="px-0" v-else>
              <v-avatar size="8" color="warning" variant="flat" class="mr-2"></v-avatar>
              <p class="text-h6 mb-0">Pending</p>
            </v-chip>
          </td>
          <td class="py-3 text-right" style="min-width: 100px">Bs.{{ item.Price }}</td>
        </tr>
      </tbody> 
    </v-table> -->
    <pag-table :headers="headers" :service="productServ">

    </pag-table>

  </title-card>
</template>
<script setup>
import { onMounted, inject, ref } from 'vue';

const { productServ } = inject('services')

const headers = ref([
  { title: 'ID PORD.', key: 'NroProduct', },
  { title: 'NOMBRE PRO.', key: 'Name' },
  { title: 'TOTAL STOCK', key: 'fat', align: 'end' },
  { title: 'VENTAS', key: 'IsSales' },
  { title: 'PRECIO Bs.', key: 'Price', align: 'end' },
])

onMounted(async () => {
  //await productServ.loadPage()
});


</script>
