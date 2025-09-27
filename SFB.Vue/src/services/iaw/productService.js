import { defineStore } from 'pinia'
import { apiClient } from '@/utils/apiClient'
import { message } from 'ant-design-vue'

const Route = method => `api/IAW/Product/${method}`;

export const productService = defineStore('productService', {
  state: () => ({
    pageData: {},
    pageParams: {},
    loadTable: true
  }),
  actions: {
    async create(product) {
      try {
        const { IsSuccess, Data, Message } = await apiClient.post(Route('Create'), product)
        if (!IsSuccess) {
          message.warning(Message)
          return null
        }
        message.success('Producto creado con éxito.')
        return Data
      } catch (err) {
        const msg = err?.response?.data?.message || err?.message || String(err)
        message.error(msg)
        return null
      }
    },
    async update(product) {
      try {
        const route = Route('Update');
        const apiResult = await apiClient.put(route, product);
        return apiResult.IsSuccess ? apiResult.Data : null;
      } catch (error) {
        console.error('Error al actualizar producto:', error);
        throw error;
      }
    },
    async remove(id) {
      try {
        const route = Route(`Delete/${id}`);
        const apiResult = await apiClient.delete(route);
        return apiResult.IsSuccess;
      } catch (error) {
        console.error('Error al eliminar producto:', error);
        throw error;
      }
    },
    async loadPage() {
      try {
        this.loadTable = true
        const route = Route('GetPage') + apiClient.queryString(this.pageParams);
        const apiResult = await apiClient.get(route);

        if (apiResult.IsSuccess) {
          this.pageData = apiResult.Data
        }
      } catch (error) {
        console.error('Error al obtener módulos:', error);
      }
      finally {
        this.loadTable = false
      }
    }
  }
});
