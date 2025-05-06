import { defineStore } from 'pinia'
import { fetchWrapper } from '@/utils/helpers/fetch-wrapper'

const baseApiUrl = import.meta.env.MODE === 'production' ? `${window.location.origin}/api` : import.meta.env.VITE_API_URL
const baseUrl = `${baseApiUrl}/users`

export const useUsersStore = defineStore({
  id: 'Authuser',
  state: () => ({
    users: {}
  }),
  actions: {
    async getAll() {
      this.users = { loading: true }
      fetchWrapper
        .get(baseUrl)
        .then((users) => {
          this.users = users
        })
        .catch((error) => {
          this.users = { error }
        })
    }
  }
})
