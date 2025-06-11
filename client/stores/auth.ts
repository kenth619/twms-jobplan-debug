import { differenceInSeconds, isFuture, isPast } from 'date-fns'
import { jwtDecode } from 'jwt-decode'

interface JwtPayload {
    exp: number
}

export const useAuthStore = defineStore('auth', () => {
    const token = ref<string | null>()
    const employeeData = ref<EmployeeWithRoles | null>()
    const expiresAt = ref<Date | null>()

    const queryClient = useQueryClient()

    const isAuthenticated = computed(() => !!token.value && !!expiresAt.value && isFuture(expiresAt.value))
    const isSessionExpired = computed(() => !!token.value && !!expiresAt.value && isPast(expiresAt.value))
    const isSessionExpiringSoon = () => {
        return !!token.value && !!expiresAt.value && isFuture(expiresAt.value) && (differenceInSeconds(expiresAt.value, new Date()) < 60)
    }

    async function setToken(newToken: string) {
        token.value = null
        expiresAt.value = null

        try {
            const decodedToken = jwtDecode<JwtPayload>(newToken)
            const newExpiresAt = decodedToken?.exp ? new Date(decodedToken.exp * 1000) : null

            if (newExpiresAt) {
                token.value = newToken
                expiresAt.value = newExpiresAt
            }
        }
        catch (error) {
            console.error('Failed to decode token:', error)
        }
    }

    async function login(newToken: string, newEmployeeData: EmployeeWithRoles) {
        employeeData.value = newEmployeeData
        await setToken(newToken)
    }

    function logout() {
        employeeData.value = null
        token.value = null
        expiresAt.value = null
        queryClient.invalidateQueries({ refetchType: 'none' })
        localStorage.clear()
    }

    async function refreshToken() {
        try {
            const response = await $fetch<{ message: string, token: string }>('/api/auth/refresh', {
                method: 'POST',
                headers: {
                    'Authorization': `Bearer ${token.value}`,
                    'Content-Type': 'application/json',
                    'Accept': 'application/json',
                },
            })

            if (response.token) {
                await setToken(response.token)
                return true
            }
            return false
        }
        catch (error) {
            console.error('Failed to refresh token:', error)
            return false
        }
    }

    return {
        employeeData,
        expiresAt,
        token,
        isAuthenticated,
        isSessionExpired,
        isSessionExpiringSoon,
        setToken,
        refreshToken,
        login,
        logout,
    }
}, {
    persist: {
        storage: localStorage,
    },
})
