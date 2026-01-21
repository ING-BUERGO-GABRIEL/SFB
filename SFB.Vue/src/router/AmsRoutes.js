export default {
  path: '/main',
  meta: {
    requiresAuth: true
  },
  redirect: '/main',
  component: () => import('@/layouts/dashboard/DashboardLayout.vue'),
  children: [
    {
      name: 'Customers',
      path: '/ams/customers',
      component: () => import('@/views/pages/ams/customers/Customers.vue')
    }
  ]
}
