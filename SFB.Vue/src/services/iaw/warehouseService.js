import { defineStore } from 'pinia'
import { apiClient } from '@/utils/apiClient'
import { message } from 'ant-design-vue'

const Route = method => `api/IAW/Warehouse/${method}`;

export const warehouseService = defineStore('warehouseService', {
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
