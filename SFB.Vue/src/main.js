import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import { router } from './router'
import vuetify from './plugins/vuetify'
import { registerComponents } from './plugins/global-components'

// Plugins
import { PerfectScrollbarPlugin } from 'vue3-perfect-scrollbar'
import VueTablerIcons from 'vue-tabler-icons'
import VueApexCharts from 'vue3-apexcharts'
import Antd from 'ant-design-vue'
import 'ant-design-vue/dist/reset.css'

// Estilos
import '@/scss/style.scss'
import '@fontsource/public-sans/400.css'
import '@fontsource/public-sans/500.css'
import '@fontsource/public-sans/600.css'
import '@fontsource/public-sans/700.css'

const app = createApp(App)

app.use(router)
app.use(PerfectScrollbarPlugin)
app.use(createPinia())
app.use(VueTablerIcons)
app.use(Antd)
app.use(VueApexCharts)
registerComponents(app)
app.use(vuetify).mount('#app');
