
import DialogBody from '@/components/shared/DialogBody.vue'
import HeaderBar from '@/components/shared/HeaderBar.vue'
import Search from '@/components/shared/Search.vue'


export function registerComponents(app) {

  app.component('DialogBody', DialogBody)
  app.component('HeaderBar', HeaderBar)
  app.component('SearchField', Search)

}
