// src/utils/hostTool.js
export const hostTool = {
  getUrlBase() {
    return import.meta.env.MODE === 'production'
      ? `${window.location.origin}/`
      : import.meta.env.VITE_API_URL
  }
}
