<script setup lang="ts">
import { computed, shallowRef } from 'vue'
import UiTitleCard from '@/components/shared/UiTitleCard.vue'

const chartOptions1 = computed(() => {
  return {
    chart: {
      type: 'line',
      height: 340,
      fontFamily: `inherit`,
      foreColor: '#a1aab2',
      toolbar: {
        show: false
      }
    },
    colors: ['rgba(var(--v-theme-primary), var(--v-medium-opacity))'],
    dataLabels: {
      enabled: false
    },
    labels: ['Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep'],
    xaxis: {
      crosshairs: {
        width: 1
      },
      labels: {
        offsetX: 8
      },
      axisTicks: {
        show: false
      },
      axisBorder: {
        show: false
      }
    },
    yaxis: {
      show: false
    },
    stroke: {
      curve: 'smooth',
      width: 1.5
    },
    grid: {
      strokeDashArray: 4,
      borderColor: 'rgba(var(--v-theme-borderLight), var(--v-high-opacity))'
    },
    tooltip: {
      fixed: {
        enabled: false
      },
      x: {
        show: false
      },
      marker: {
        show: false
      }
    }
  }
})

const lineChart1 = {
  series: [
    {
      name: 'Presentaciones activas',
      data: [12, 18, 25, 21, 24, 27, 30, 34]
    }
  ]
}

const reports = shallowRef([
  {
    name: 'Total de presentaciones',
    percent: '34'
  },
  {
    name: 'Presentaciones recientes',
    percent: '8'
  },
  {
    name: 'Más utilizadas',
    percent: 'Granulado'
  }
])
</script>

<template>
  <UiTitleCard title="Resumen de Presentaciones" class-name="px-0 rounded-md overflow-hidden">
    <v-list class="py-0" aria-busy="true" aria-label="Report list" border>
      <v-list-item :value="item.name" v-for="(item, i) in reports" :key="i">
        <div class="d-inline-flex align-center justify-space-between w-100 ga-2">
          <h6 class="text-h6 mb-0">{{ item.name }}</h6>
          <h5 class="ml-auto text-h5 mb-0">{{ item.percent }}</h5>
        </div>
      </v-list-item>
    </v-list>
    <v-divider></v-divider>
    <apexchart type="line" height="340" :options="chartOptions1" :series="lineChart1.series"> </apexchart>
  </UiTitleCard>
</template>
