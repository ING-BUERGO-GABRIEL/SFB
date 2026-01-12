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
        message.error(err)
        return null
      }
    },
    async update(product) {
      try {
        const route = Route('Update');
        const { IsSuccess, Data, Message } = await apiClient.put(route, product);
        if (!IsSuccess) {
          message.warning(Message)
          return null
        }
        message.success('Producto actualizado con éxito.')
        return Data
      } catch (err) {
        message.error(err)
        return null
      }
    },
    async remove(id) {
      try {
        const route = Route(`Delete/${id}`);
        const { IsSuccess, Message } = await apiClient.delete(route);
        if (!IsSuccess) {
          message.warning(Message)
        }
        message.success('Producto eliminado con éxito.')
        return IsSuccess
      } catch (err) {
        message.error(err)
        return false
      }
    },
    async loadPage() {
      try {
        this.loadTable = true
        const route = Route('GetPage') + apiClient.queryString(this.pageParams);
        const apiResult = await apiClient.get(route);

        if (apiResult.IsSuccess) {
          this.pageData = apiResult.Data
          return apiResult.Data
        }
      } catch (error) {
        console.error('Error al obtener módulos:', error);
        return null
      }
      finally {
        this.loadTable = false
      }
    },
    async getMetadata() {
      try {
        const { Data } = await apiClient.get(Route('GetMetadata'));
        return Data ?? null
      } catch (error) {
        console.error('Error al obtener metadata:', error);
        return null
      }
    },
  }
});
