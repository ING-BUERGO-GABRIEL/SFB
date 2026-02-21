import { defineStore } from 'pinia'
import { apiClient } from '@/utils/apiClient'
import { message } from 'ant-design-vue'

export function defineCmsStore(storeId, moduleRoute, entityName, overrideOptions = {}) {
    return defineStore(storeId, {
        state: () => ({
            apiClient: apiClient,
            pageData: { Data: [], TotalCount: 0 },
            pageParams: {},
            loadTable: false,
            // Agregamos el state personalizado extra que venga en las opciones
            ...(overrideOptions.state ? overrideOptions.state() : {})
        }),
        getters: {
            ...overrideOptions.getters
        },
        actions: {
            getRoute(method) {
                return `api/${moduleRoute}/${entityName}/${method}`
            },
            async create(model) {
                try {
                    const { IsSuccess, Data, Message } = await apiClient.post(this.getRoute('Create'), model)
                    if (!IsSuccess) {
                        message.warning(Message)
                        return null
                    }
                    message.success('Formulario registrado con éxito.')
                    return Data
                } catch (err) {
                    message.error(err)
                    return null
                }
            },
            async update(model) {
                try {
                    const { IsSuccess, Data, Message } = await apiClient.put(this.getRoute('Update'), model)
                    if (!IsSuccess) {
                        message.warning(Message)
                        return null
                    }
                    message.success('Actualizado con éxito.')
                    return Data
                } catch (err) {
                    message.error(err)
                    return null
                }
            },
            async getById(idProperty) {
                try {
                    const { IsSuccess, Data, Message } = await apiClient.get(this.getRoute(`GetById/${idProperty}`))
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
            async anular(idProperty) {
                try {
                    const { IsSuccess, Data, Message } = await apiClient.post(this.getRoute(`Anular/${idProperty}`))
                    if (!IsSuccess) {
                        message.warning(Message)
                        return null
                    }
                    message.success('Anulado con éxito.')
                    return Data
                } catch (err) {
                    message.error(err)
                    return null
                }
            },
            async loadPage() {
                try {
                    this.loadTable = true
                    const route = this.getRoute('GetPage') + apiClient.queryString(this.pageParams)
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
                    const { Data } = await apiClient.get(this.getRoute('GetMetadata'))
                    return Data ?? null
                } catch (error) {
                    console.error('Error al obtener metadata:', error)
                    return null
                }
            },
            addItemPage(item) {
                if (!this.pageData.Data) this.pageData.Data = []
                this.pageData.Data.unshift(item)
                this.pageData.TotalCount = (this.pageData.TotalCount ?? 0) + 1
            },
            updItemPage(item, key = 'Id') {
                const idx = this.pageData.Data.findIndex(p => p[key] === item[key])
                if (idx !== -1) {
                    this.pageData.Data[idx] = { ...this.pageData.Data[idx], ...item }
                }
            },
            async getStatus() {
                try {
                    const { Data } = await apiClient.get(this.getRoute('GetStatus'))
                    return Data ?? []
                } catch (error) {
                    console.error('Error al obtener estados:', error)
                    return []
                }
            },
            async patchUpdate(idProperty, patchParams = {}) {
                try {
                    const route = this.getRoute(`Update/${idProperty}`)
                    const apiResult = await apiClient.patch(route, patchParams)

                    if (apiResult.IsSuccess) {
                        message.success('Formulario actualizado con éxito.')
                        return apiResult.Data
                    }

                    message.warning(apiResult.Message)
                } catch (error) {
                    console.error('Error al parchear:', error)
                    return null
                }
            },

            // Sobrescribe o agrega acciones extra
            ...overrideOptions.actions
        }
    })
}
