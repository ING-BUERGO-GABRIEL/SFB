
import { CmsBaseService } from './cmsBaseService'

export class AmsPersonService extends CmsBaseService {

    constructor() {
        super('AMS', 'Person')
    }

    async getPersonList() {
        try {
            const { Data } = await this.apiClient.get(this.Route('GetPersons'))
            return Data ?? []
        } catch (error) {
            console.error('Error al obtener personas:', error)
            return []
        }
    }
}
