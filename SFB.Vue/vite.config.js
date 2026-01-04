import { fileURLToPath, URL } from 'url'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import vuetify from 'vite-plugin-vuetify'

const abs = (relativePath) => fileURLToPath(new URL(relativePath, import.meta.url));

export default defineConfig(({ mode }) => {

  const isMaui = mode === "maui";
  const isWebMode = mode === "web";
  return {
    plugins: [
      {
        name: 'inject-maui-script',
        transformIndexHtml() {
          if (isMaui) {
            return [
              {
                tag: 'script',
                attrs: { src: '_hwv/HybridWebView.js' },
                injectTo: 'head',
              },
            ]
          }
        },
      },
      vue({
        template: {
          compilerOptions: {
            isCustomElement: (tag) => ['v-list-recognize-title'].includes(tag)
          }
        }
      }),
      vuetify({
        autoImport: true
      })
    ],
    resolve: {
      alias: {
        '@': fileURLToPath(new URL('./src', import.meta.url)),
        'vue-i18n': 'vue-i18n/dist/vue-i18n.esm-bundler.js'
      }
    },
    css: {
      preprocessorOptions: {
        scss: {}
      }
    },
    build: {

      outDir: isMaui
        ? abs("../SFB.Maui/Resources/Raw/wwwroot")
        : isWebMode
          ? abs("../SFB.Server/wwwroot")
          : abs("../SFB.Server/wwwroot"),

      emptyOutDir: true,
      chunkSizeWarningLimit: 1024 * 1024,
    },
    optimizeDeps: {
      exclude: ['vuetify'],
      entries: ['./src/**/*.vue']
    }
  }
})
