<template>
  <Header />
  <v-row class="mb-0">
    <v-col cols="12" md="8">
      <DataTable />
    </v-col>
    <v-col cols="12" md="4">
      <AnalyticsReport />
    </v-col>
  </v-row>
</template>
<script setup>
import Header from './components/HeaderBar.vue';
import DataTable from './components/DataTable.vue';
import AnalyticsReport from './components/AnalyticsReport.vue';
</script>
