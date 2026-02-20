
import { apiClient } from '@/utils/apiClient'

export class CmsBaseService {
    constructor(moduleRoute, entityName) {
        this.moduleRoute = moduleRoute
        this.entityName = entityName
        this.apiClient = apiClient
        this.pageData = { Data: [], TotalCount: 0 }
        this.pageParams = {}
        this.loadTable = false
    }

    Route(method) {
        return `api/${this.moduleRoute}/${this.entityName}/${method}`
    }

    async loadPage() {
        try {
            this.loadTable = true
            const route = this.Route('GetPage') + this.apiClient.queryString(this.pageParams)
            const apiResult = await this.apiClient.get(route)

            if (apiResult.IsSuccess) {
                this.pageData = apiResult.Data
            }
        } catch (error) {
            console.error('Error al obtener la pagina:', error)
        } finally {
            this.loadTable = false
        }
    }

    async getMetadata() {
        try {
            const { Data } = await this.apiClient.get(this.Route('GetMetadata'))
            return Data ?? null
        } catch (error) {
            console.error('Error al obtener metadata:', error)
            return null
        }
    }

    addItemPage(item) {
        if (!this.pageData.Data) this.pageData.Data = []
        this.pageData.Data.unshift(item)
        this.pageData.TotalCount = (this.pageData.TotalCount ?? 0) + 1
    }

    updItemPage(item, key = 'TxnId') {
        const idx = this.pageData.Data.findIndex(p => p[key] === item[key])
        if (idx !== -1) {
            this.pageData.Data[idx] = { ...this.pageData.Data[idx], ...item }
        }
    }
}
