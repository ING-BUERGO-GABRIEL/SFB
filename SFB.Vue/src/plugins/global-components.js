
import DialogBody from '@/components/shared/DialogBody.vue'
import CardDialogBody from '@/components/shared/CardDialogBody.vue'
import HeaderBar from '@/components/shared/HeaderBar.vue'
import Search from '@/components/shared/Search.vue'
import ParentCard from '@/components/shared/UiParentCard.vue';
import TitleCard from '@/components/shared/UiTitleCard.vue';
import PagTable from '@/components/shared/PaginatedTable.vue';
import SelectPage from '@/components/shared/SelectPage.vue';
import UiTable from '@/components/shared/UiTable.vue';
import UiIcon from '@/components/shared/UiIcon.vue';
import { VDateInput } from 'vuetify/labs/VDateInput'

export const registerComponents = {
  install: (app) => {
    app.component('DialogBody', DialogBody)
    app.component('HeaderBar', HeaderBar)
    app.component('SearchField', Search)
    app.component('ParentCard', ParentCard)
    app.component('TitleCard', TitleCard)
    app.component('PagTable', PagTable)
    app.component('CardDialog', CardDialogBody)
    app.component('SelectPage', SelectPage)
    app.component('UiTable', UiTable)
    app.component('UiIcon', UiIcon)
    app.component('VDateInput', VDateInput)
  },
};
