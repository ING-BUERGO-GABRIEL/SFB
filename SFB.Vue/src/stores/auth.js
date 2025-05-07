import { defineStore } from 'pinia'
import { router } from '@/router'
import { fetchWrapper } from '@/utils/helpers/fetch-wrapper'
import { hostTool } from '@/utils/hostTool'

const baseUrl = `${hostTool.getUrlBase()}/users`

export const useAuthStore = defineStore({
  id: 'auth',
  state: () => ({
    user: JSON.parse(localStorage.getItem('user')) || null,
    returnUrl: null
  }),
  actions: {
    async login(username, password) {
      console.log("Hola",baseUrl);
      const user = await fetchWrapper.post(`${baseUrl}/authenticate`, { username, password })
      this.user = user
      localStorage.setItem('user', JSON.stringify(user))
      router.push(this.returnUrl || '/dashboard')
    },
    logout() {
      this.user = null
      localStorage.removeItem('user')
      router.push('/login')
    }
  }
})
