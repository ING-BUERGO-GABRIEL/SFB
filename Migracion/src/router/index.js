import { createRouter, createWebHistory } from 'vue-router'
import MainRoutes from './MainRoutes'
import PublicRoutes from './PublicRoutes'
import { useAuthStore } from '@/stores/auth'
import { useUIStore } from '@/stores/ui'

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes: [
        {
            path: '/:pathMatch(.*)*',
            component: () => import('@/views/pages/maintenance/error/Error404Page.vue')
        },
        MainRoutes,
        PublicRoutes,
    ]
})

// Middleware de autenticación
router.beforeEach((to, from, next) => {
    const publicPages = ['/']
    const auth = useAuthStore()
    const isPublicPage = publicPages.includes(to.path)
    const authRequired = !isPublicPage && to.matched.some((record) => record.meta.requiresAuth)

    if (authRequired && !auth.user) {
        auth.returnUrl = to.fullPath
        next('/login')
    } else if (auth.user && to.path === '/login') {
        next({
            query: {
                ...to.query,
                redirect: auth.returnUrl !== '/' ? to.fullPath : undefined
            }
        })
    } else {
        next()
    }
})

router.beforeEach(() => {
    const uiStore = useUIStore()
    uiStore.isLoading = true
})

router.afterEach(() => {
    const uiStore = useUIStore()
    uiStore.isLoading = false
})

export { router }
