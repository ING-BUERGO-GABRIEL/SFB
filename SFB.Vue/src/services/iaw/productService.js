import { defineStore } from 'pinia'
import { apiClient } from '@/utils/apiClient'

const getRoute = method => `api/IAW/Product/${method}`;

export const productService = defineStore('productService', {
  state: () => ({
    options: []
  }),
  actions: {
    async createProduct(product) {
      try {
        const route = getRoute('Create');

        const apiResult = await apiClient.post(route,product);

        console.log(apiResult)

        return this.options;
      } catch (error) {
        console.error('Error al obtener módulos:', error);
        throw error;
      }
    }
  }
});
