import { defineCmsStore } from '@/services/cms/cmsBaseStore'

export const tamExamFormService = defineCmsStore('tamExamFormService', 'TAM', 'ExamForm', {
  state: () => ({
    // Si necesitas atributos/variables adicionales, agrégalos aquí:
    // miAtributoExtra: "Hola",
  }),
  actions: {
    // Si necesitas agregar o sobrescribir métodos, decláralos aquí:
    /*
    async miMetodoPropio() {
      // lógica extra que accede a this.pageData
    },
    */

    // Sobrescribimos updItemPage tal cual estaba en el archivo original, por si se usa en alguna vista:
    // updItemPage(item) {
    //   const idx = this.pageData.Data.findIndex(p => p.TxnId === item.TxnId)
    //   if (idx !== -1) this.pageData.Data[idx] = item
    // }
  }
})
