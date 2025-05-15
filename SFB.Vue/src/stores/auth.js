import { defineStore } from 'pinia'
import { router } from '@/router'
import { apiClient } from '@/utils/apiClient'

const baseController = `api/AMS/Authentication/`
const getRoute = method => {
  return `${baseController}${method}`;
}

export const useAuthStore = defineStore({
  id: 'auth',
  state: () => ({
    user: JSON.parse(localStorage.getItem('user')) || null,
    returnUrl: null
  }),
  actions: {
    async login(user, password) {
      const route = getRoute('Login');
      const userData = await apiClient.post(route, { User: user, Password: password })
      this.user = userData
      localStorage.setItem('user', JSON.stringify(userData))
      router.push(this.returnUrl || '/dashboard')
    },
    logout() {
      this.user = null
      localStorage.removeItem('user')
      router.push('/login')
    }
  }
})
