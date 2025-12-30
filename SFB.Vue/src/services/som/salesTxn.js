import { defineStore } from 'pinia'
import { apiClient } from '@/utils/apiClient'
import { message } from 'ant-design-vue'

const Route = method => `api/SOM/SalesTxn/${method}`

export const salesTxnService = defineStore('salesTxnService', {
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
        message.success('Venta registrada con éxito.')
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
        message.success('Venta actualizada con éxito.')
        return Data
      } catch (err) {
        message.error(err)
        return null
      }
    },
    async getById(txnId) {
      try {
        const { IsSuccess, Data, Message } = await apiClient.get(Route(`GetById/${txnId}`))
        if (!IsSuccess) {
          message.warning(Message)
          return null
        }
        return Data
      } catch (err) {
        message.error(err)
        return null
      }
    },
    async anular(txnId) {
      try {
        const { IsSuccess, Data, Message } = await apiClient.post(Route(`Anular/${txnId}`))
        if (!IsSuccess) {
          message.warning(Message)
          return null
        }
        message.success('Venta anulada con éxito.')
        return Data
      } catch (err) {
        message.error(err)
        return null
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
    },
    async getMetadata() {
      try {
        const { Data } = await apiClient.get(Route('GetMetadata'))
        return Data ?? null
      } catch (error) {
        console.error('Error al obtener metadata:', error)
        return null
      }
    },
    addItemPage(item) {
      this.pageData.Data.unshift(item)
      this.pageData.TotalCount = (this.pageData.TotalCount ?? 0) + 1
    },
    updItemPage(item) {
      const idx = this.pageData.Data.findIndex(p => p.TxnId === item.TxnId)
      if (idx !== -1) this.pageData.Data[idx] = item
    }
  }
})
