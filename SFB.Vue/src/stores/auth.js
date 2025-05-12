import { defineStore } from 'pinia'
import { router } from '@/router'
//import { fetchWrapper } from '@/utils/helpers/fetch-wrapper'
import { apiClient } from '@/utils/apiClient'
//import { hostTool } from '@/utils/hostTool'

const baseController = `/AMS/Authentication`

export const useAuthStore = defineStore({
  id: 'auth',
  state: () => ({
    user: JSON.parse(localStorage.getItem('user')) || null,
    returnUrl: null
  }),
  actions: {
    async login(user, password) {
      const login = { User: user, Password: password }
      const user = await apiClient.post(`${baseController}/Login`, login)
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
