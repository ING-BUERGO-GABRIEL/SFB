export default {
  path: '/main',
  meta: {
    requiresAuth: true
  },
  redirect: '/main',
  component: () => import('@/layouts/dashboard/DashboardLayout.vue'),
  children: [
    {
      name: 'Typography',
      path: '/typography',
      component: () => import('@/views/typography/TypographyPage.vue')
    },
    {
      name: 'Colors',
      path: '/colors',
      component: () => import('@/views/colors/ColorPage.vue')
    },
    {
      name: 'Shadow',
      path: '/shadow',
      component: () => import('@/views/shadows/ShadowPage.vue')
    },
    {
      name: 'Color',
      path: '/icon/ant',
      component: () => import('@/views/icons/AntDesignIcons.vue')
    },
    {
      name: 'other',
      path: '/sample-page',
      component: () => import('@/views/StarterPage.vue')
    }
  ]
}
