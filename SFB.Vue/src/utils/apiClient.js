import axios from 'axios';
import { useAuthStore } from '@/stores/auth';
import config from '@/config';

const api = axios.create({
  baseURL: config.apiUrl,
  headers: { 'Content-Type': 'application/json' }
});

api.interceptors.request.use(config => {
  const { user } = useAuthStore();
  if (user?.MinToken) {
    config.headers.Authorization = `Bearer ${user.MinToken}`;
  }
  return config;
});

api.interceptors.response.use(
  res => res.data,
  err => {
    const status = err.response?.status;
    if ([401, 403].includes(status)) {
      useAuthStore().logout();
    }
    return Promise.reject(err.response?.data?.Message || err.message);
  }
);

const queryString = (params) => {
  if (params) {
    let queryString = Object.keys(params)
      .filter(key => {
        const value = params[key];
        return value !== null && value !== undefined && value.toString().trim() !== '';
      })
      .map(key => `${key}=${encodeURIComponent(params[key])}`)
      .join('&');
    return queryString ? `?${queryString}` : '';
  }
  return "";
};

export const apiClient = {
  get: (url, params) => api.get(url + queryString(params)),
  post: (url, body) => api.post(url, body),
  put: (url, body) => api.put(url, body),
  patch: (url, params, body) => api.patch(url + queryString(params), body),
  delete: url => api.delete(url),
  queryString: params => queryString(params)
};
