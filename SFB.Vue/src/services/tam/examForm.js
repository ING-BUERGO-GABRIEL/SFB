import { defineStore } from 'pinia'
import { apiClient } from '@/utils/apiClient'
import { message } from 'ant-design-vue'

const Route = method => `api/TAM/ExamForm/${method}`

export const examFormService = defineStore('examFormService', {
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
        message.success('Formulario registrada con éxito.')
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
    },
    async getStatus() {
      try {
        const { Data } = await apiClient.get(Route('GetStatus'))
        return Data ?? []
      } catch (error) {
        console.error('Error al obtener metadata:', error)
        return []
      }
    },
    async patchUpdate(nroForm, patchParams = {}) {
      try {
        const route = Route(`Update/${nroForm}`)
        const apiResult = await apiClient.patch(route, patchParams)

        if (apiResult.IsSuccess) {
          message.success('Formulario actualizado con éxito.')
          return apiResult.Data
        }

        message.warning(apiResult.Message)
      } catch (error) {
        console.error('Error al obtener metadata:', error)
        return null
      }
    }
  }
})
