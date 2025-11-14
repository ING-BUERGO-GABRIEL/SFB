import { defineStore } from 'pinia'
import { apiClient } from '@/utils/apiClient'
import { message } from 'ant-design-vue'

const Route = method => `api/IAW/Presentation/${method}`

export const presentationService = defineStore('presentationService', {
  state: () => ({
    pageData: {},
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
        message.success('Presentación creada con éxito.')
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
        message.success('Presentación actualizada con éxito.')
        return Data
      } catch (err) {
        message.error(err)
        return null
      }
    },
    async remove(code) {
      try {
        const { IsSuccess, Message } = await apiClient.delete(Route(`Delete/${code}`))
        if (!IsSuccess) {
          message.warning(Message)
          return false
        }
        message.success('Presentación eliminada con éxito.')
        return true
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
        console.error('Error al obtener la página:', error)
      } finally {
        this.loadTable = false
      }
    }
  }
})
