
import DialogBody from '@/components/shared/DialogBody.vue'
import HeaderBar from '@/components/shared/HeaderBar.vue'
import Search from '@/components/shared/Search.vue'
import ParentCard from '@/components/shared/UiParentCard.vue';
import TitleCard from '@/components/shared/UiTitleCard.vue';


export function registerComponents(app) {

  app.component('DialogBody', DialogBody)
  app.component('HeaderBar', HeaderBar)
  app.component('SearchField', Search)
  app.component('ParentCard', ParentCard)
  app.component('TitleCard', TitleCard)

}
