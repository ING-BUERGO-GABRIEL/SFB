import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import { router } from './router'

//Plugins custom
import vuetify from './plugins/vuetify'
import { registerComponents } from './plugins/global-components'
import { configServices } from './plugins/config-services'
import { messagePlugin } from './plugins/message-dialog'

// Plugins
import { PerfectScrollbarPlugin } from 'vue3-perfect-scrollbar'
import VueTablerIcons from 'vue-tabler-icons'
import VueApexCharts from 'vue3-apexcharts'
import Antd from 'ant-design-vue'
import 'ant-design-vue/dist/reset.css'

// Fonts/Icons primero
import '@mdi/font/css/materialdesignicons.css'
import '@fontsource/public-sans/400.css'
import '@fontsource/public-sans/500.css'
import '@fontsource/public-sans/600.css'
import '@fontsource/public-sans/700.css'

// Luego tus estilos
import '@/scss/style.scss'


const app = createApp(App)

app.use(router)
app.use(PerfectScrollbarPlugin)
app.use(createPinia())
app.use(VueTablerIcons)
app.use(Antd)
app.use(VueApexCharts)
app.use(configServices)
app.use(registerComponents)
app.use(messagePlugin)
app.use(vuetify).mount('#app');
