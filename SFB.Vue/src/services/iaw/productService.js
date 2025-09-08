import { defineStore } from 'pinia'
import { apiClient } from '@/utils/apiClient'

const getRoute = method => `api/IAW/Product/${method}`;

export const productService = defineStore('productService', {
  state: () => ({
    pageData: {
      TotalCount: 0,
      PageSize: 1,
      CurrentPage: 1,
      TotalPages: 0,
      HasNext: 0,
      HasPrevious: 0,
      Data: []
    },
    pageParams:{}
  }),
  actions: {
    async create(product) {
      try {
        const route = getRoute('Create');
        const apiResult = await apiClient.post(route, product);
        return apiResult.Status ? apiResult.Result : null;
      } catch (error) {
        console.error('Error al obtener módulos:', error);
        throw error;
      }
    },
    async loadPage() {
      try {

        const route = getRoute('GetPage') + apiClient.queryString(this.pageParams);
        const apiResult = await apiClient.get(route);

        if (apiResult.IsSuccess) {
          this.pageData = apiResult.Data
        }

      } catch (error) {
        console.error('Error al obtener módulos:', error);
      }
    }
  }
});
