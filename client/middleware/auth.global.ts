const guards = [
    {
        pagePrefix: '/admin',
        requiredRoles: ['administrator'],
    },
] as const

function isPagePublic(path: string): boolean {
    return path.startsWith('/auth')
}

export default defineNuxtRouteMiddleware(async (to, _) => {
    const { isAuthenticated } = useAuthStore()
    const { hasAny } = useEmployeeData()

    const publicPage = isPagePublic(to.path)

    if (isAuthenticated && publicPage)
        return navigateTo('/', { redirectCode: 302 })

    if (!isAuthenticated && !publicPage)
        return navigateTo(`/auth/login?redirect=${encodeURIComponent(to.fullPath)}`, { redirectCode: 401 })

    if (isAuthenticated) {
        for (const { pagePrefix, requiredRoles } of guards) {
            if (to.fullPath.startsWith(pagePrefix) && !hasAny(['superuser', ...requiredRoles])) {
                return navigateTo('/', { redirectCode: 302 })
            }
        }
    }
})
