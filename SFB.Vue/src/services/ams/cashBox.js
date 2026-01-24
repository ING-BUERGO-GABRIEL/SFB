import { defineStore } from 'pinia'
import { apiClient } from '@/utils/apiClient'
import { message } from 'ant-design-vue'

const Route = method => `api/TRM/CashBox/${method}`

export const cashBoxService = defineStore('cashBoxService', {
  state: () => ({
    pageData: { Data: [], TotalCount: 0 },
    pageParams: {},
    loadTable: true
  }),
  actions: {
    async create(model) {
      try {
        const { IsSuccess, Data, Message } = await apiClient.post(Route('Create'), model)
        if (!IsSuccess) {
          message.warning(Message)
          return null
        }
        message.success('Caja creada con éxito.')
        return Data
      } catch (err) {
        message.error(err)
        return null
      }
    },
    async update(model) {
      try {
        const { IsSuccess, Data, Message } = await apiClient.put(Route('Update'), model)
        if (!IsSuccess) {
          message.warning(Message)
          return null
        }
        message.success('Caja actualizada con éxito.')
        return Data
      } catch (err) {
        message.error(err)
        return null
      }
    },
    async remove(id) {
      try {
        const { IsSuccess, Message } = await apiClient.delete(Route(`Delete/${id}`))
        if (!IsSuccess) {
          message.warning(Message)
        }
        message.success('Caja eliminada con éxito.')
        return IsSuccess
      } catch (err) {
        message.error(err)
        return false
      }
    },
    async loadPage() {
      try {
        this.loadTable = true
        const route = Route('GetPage') + apiClient.queryString(this.pageParams)
        const apiResult = await apiClient.get(route)

        if (apiResult.IsSuccess) {
          this.pageData = apiResult.Data
        }
      } catch (error) {
        console.error('Error al obtener la pagina:', error)
      } finally {
        this.loadTable = false
      }
    }
  }
})
