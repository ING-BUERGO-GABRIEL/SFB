export default {
  path: '/tam-routes',
  children: [
    {
      path: '/main',
      meta: {
        requiresAuth: true
      },
      redirect: '/main',
      component: () => import('@/layouts/dashboard/DashboardLayout.vue'),
      children: [
        {
          name: 'Formularios de Examen',
          path: '/tam/exam-forms',
          component: () => import('@/views/pages/tam/ExamFormsPage.vue')
        }
      ]
    },
    {
      path: '',
      meta: {
        requiresAuth: false
      },
      component: () => import('@/layouts/blank/BlankLayout.vue'),
      children: [
        {
          name: 'Registrar formulario de examen',
          path: '/tam/register-exam-form',
          component: () => import('@/views/pages/tam/RegisterExamFormPage.vue')
        }
      ]
    }
  ]
}
