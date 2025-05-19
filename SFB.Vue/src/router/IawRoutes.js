export default {
  path: '/main',
  meta: {
    requiresAuth: true
  },
  redirect: '/main',
  component: () => import('@/layouts/dashboard/DashboardLayout.vue'),
  children: [
    {
      name: 'Products',
      path: '/iaw/products',
      component: () => import('@/views/modules/iaw/products/Products.vue')
    }

  ]
}
