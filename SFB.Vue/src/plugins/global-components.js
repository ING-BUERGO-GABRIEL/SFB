
import DialogBody from '@/components/shared/DialogBody.vue'
import HeaderBar from '@/components/shared/HeaderBar.vue'
import Search from '@/components/shared/Search.vue'
import ParentCard from '@/components/shared/UiParentCard.vue';
import TitleCard from '@/components/shared/UiTitleCard.vue';
import PagTable from '@/components/shared/PaginatedTable.vue';

export const registerComponents = {
  install: (app) => {
    app.component('DialogBody', DialogBody)
    app.component('HeaderBar', HeaderBar)
    app.component('SearchField', Search)
    app.component('ParentCard', ParentCard)
    app.component('TitleCard', TitleCard)
    app.component('PagTable', PagTable)
  },
};
