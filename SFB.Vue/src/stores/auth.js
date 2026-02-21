import { defineStore } from 'pinia'
import { router } from '@/router'
import { apiClient } from '@/utils/apiClient'

const baseController = `api/AMS/Authentication/`
const getRoute = method => `${baseController}${method}`

function parseJwt(token) {
  try {
    const base64Url = token.split('.')[1]
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/')
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    )
    return JSON.parse(jsonPayload)
  } catch (e) {
    console.error('parseJwt error:', e)
    return null
  }
}

const getUserdata = () => {
  const token = localStorage.getItem('user')
  if (!token) {
    return null
  }
  return parseJwt(token)
}

export const useAuthStore = defineStore({
  id: 'auth',
  state: () => ({
    user: getUserdata() ?? null,
    returnUrl: null
  }),
  actions: {
    async login(user, password) {
      const route = getRoute('Login');
      const apiResult = await apiClient.post(route, { User: user, Password: password })
      const token = apiResult.Data
      localStorage.setItem('user', token)
      this.user = parseJwt(token)
      router.push(this.returnUrl || '/dashboard')
    },
    logout() {
      this.user = null
      localStorage.removeItem('user')
      router.push('/login')
    }
  }
})
