import { defineStore } from 'pinia'
import { fetchWrapper } from '@/utils/helpers/fetch-wrapper'
import { hostTool } from '@/utils/hostTool'

const baseUrl = `${hostTool.getUrlBase()}/users`

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
