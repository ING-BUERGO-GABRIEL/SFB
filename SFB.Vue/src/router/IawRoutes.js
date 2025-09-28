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
      component: () => import('@/views/pages/iaw/products/ProductsPage.vue')
    },
    {
      name: 'Warehouse',
      path: '/iaw/warehouses',
      component: () => import('@/views/pages/iaw/warehouse/WarehousesPage.vue')
    }
  ]
}
