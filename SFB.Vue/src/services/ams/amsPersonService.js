


import { defineCmsStore } from '@/services/cms/cmsBaseStore'

export const amsPersonService = defineCmsStore('amsPersonService', 'AMS', 'Person', {
    state: () => ({
        // miAtributoExtra: "Hola",
    }),
    actions: {
        async getPersonList(codPersonType = null) {
            try {
                const { Data } = await this.apiClient.get(this.getRoute('GetPersons'), { type: codPersonType })
                return Data ?? []
            } catch (error) {
                console.error('Error al obtener personas:', error)
                return []
            }
        }
    }
})
