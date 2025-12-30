export default {
  path: '/main',
  meta: {
    requiresAuth: true
  },
  redirect: '/main',
  component: () => import('@/layouts/dashboard/DashboardLayout.vue'),
  children: [
    {
      name: 'SalesTxn',
      path: '/som/sales',
      component: () => import('@/views/pages/som/sales/SalesTxn.vue')
    }
  ]
}
