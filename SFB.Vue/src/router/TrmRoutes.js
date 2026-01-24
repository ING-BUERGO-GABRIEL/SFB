export default {
  path: '/main',
  meta: {
    requiresAuth: true
  },
  redirect: '/main',
  component: () => import('@/layouts/dashboard/DashboardLayout.vue'),
  children: [
    {
      name: 'CashBoxs',
      path: '/trm/cashboxes',
      component: () => import('@/views/pages/trm/cashBox/CashBoxsPage.vue')
    }
  ]
}
