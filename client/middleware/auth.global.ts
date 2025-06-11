const guards = [
    {
        pagePrefix: '/sysadmin',
        requiredRoles: ['system-administrator'],
    },
] as const

function isPagePublic(path: string): boolean {
    return path.startsWith('/auth')
}

export default defineNuxtRouteMiddleware(async (to, _) => {
    // ✅ Bypass all auth in development mode for UI testing
    if (process.dev) {
        return
    }

    const { isAuthenticated, isSessionExpired, logout } = useAuthStore()
    const { hasAny } = useRoles()

    if (isSessionExpired) {
        logout()
        return navigateTo(`/auth/login?redirect=${encodeURIComponent(to.fullPath)}&inactiveLogout=true`, { redirectCode: 401 })
    }

    const publicPage = isPagePublic(to.path)

    if (isAuthenticated && publicPage) {
        return navigateTo('/', { redirectCode: 302 })
    }

    if (!isAuthenticated && !publicPage) {
        return navigateTo(`/auth/login?redirect=${encodeURIComponent(to.fullPath)}`, { redirectCode: 401 })
    }

    if (isAuthenticated) {
        for (const { pagePrefix, requiredRoles } of guards) {
            if (to.fullPath.startsWith(pagePrefix) && !hasAny(['superuser', ...requiredRoles])) {
                return navigateTo('/', { redirectCode: 302 })
            }
        }
    }
})
