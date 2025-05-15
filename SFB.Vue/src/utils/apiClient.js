import axios from 'axios';
import { useAuthStore } from '@/stores/auth';
import { hostTool } from '@/utils/hostTool';

const api = axios.create({
  baseURL: hostTool.getUrlBase(),
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

export const apiClient = {
  get:    url => api.get(url),
  post:   (url, body) => api.post(url, body),
  put:    (url, body) => api.put(url, body),
  delete: url => api.delete(url)
};
