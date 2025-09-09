import { defineStore } from 'pinia'
import { apiClient } from '@/utils/apiClient'

const getRoute = method => `api/IAW/Product/${method}`;

export const productService = defineStore('productService', {
  state: () => ({
    pageData: {},
    pageParams: {},
    loadTable:true
  }),
  actions: {
    async create(product) {
      try {
        const route = getRoute('Create');
        const apiResult = await apiClient.post(route, product);
        return apiResult.IsSuccess ? apiResult.Data : null;
      } catch (error) {
        console.error('Error al obtener módulos:', error);
        throw error;
      }
    },
    async loadPage() {
      try {
        this.loadTable = true
        const route = getRoute('GetPage') + apiClient.queryString(this.pageParams);
        const apiResult = await apiClient.get(route);

        if (apiResult.IsSuccess) {
          this.pageData = apiResult.Data
        }
      } catch (error) {
        console.error('Error al obtener módulos:', error);
      }
      finally{
         this.loadTable = false
      }
    }
  }
});
