import { defineStore } from 'pinia'

export const hostTool = defineStore({
  id: 'hostTool',
  actions: {
    GetUrlBase() {
      return import.meta.env.MODE === 'production' ? `${window.location.origin}/api` : import.meta.env.VITE_API_URL
    }
  }
})
