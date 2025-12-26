export default {
  path: '/main',
  meta: {
    requiresAuth: true
  },
  redirect: '/main',
  component: () => import('@/layouts/dashboard/DashboardLayout.vue'),
  children: [
    {
      name: 'Suppliers',
      path: '/pcm/suppliers',
      component: () => import('@/views/pages/pcm/supplier/SupplierPage.vue')
    },
    {
      name: 'PurchaseTxn',
      path: '/pcm/purchases',
      component: () => import('@/views/pages/pcm/purchases/PurchaseTxn.vue')
    }
  ]
}
