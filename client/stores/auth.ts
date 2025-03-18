import { isFuture } from 'date-fns'
import { jwtDecode } from 'jwt-decode'

interface JwtPayload {
    exp: number
    unique_name: string
    username: string
    email?: string | undefined | null
    employeeNumber: string
    [key: string]: unknown
}

export const useAuthStore = defineStore('auth', () => {
    const token = ref<string | null>()
    const username = ref<string | null>()
    const employeeName = ref<string | null>()
    const employeeNumber = ref<string | null>()
    const employeeEmail = ref<string | null>()
    const expiresAt = ref<Date | null>()

    const queryClient = useQueryClient()

    const isAuthenticated = computed(() => !!token.value && !!expiresAt.value && isFuture(expiresAt.value))

    function setToken(newToken: string) {
        clearToken()

        try {
            const decodedToken = jwtDecode<JwtPayload>(newToken)
            const newUsername = decodedToken.username
            const newEmployeeName = decodedToken.unique_name
            const newEmployeeNumber = decodedToken.employeeNumber
            const newEmail = decodedToken.email ?? null
            const newExpiresAt = decodedToken?.exp ? new Date(decodedToken.exp * 1000) : null

            if (newUsername && newEmployeeName && newEmployeeNumber && newExpiresAt) {
                token.value = newToken
                username.value = newUsername
                employeeName.value = newEmployeeName
                employeeNumber.value = newEmployeeNumber
                employeeEmail.value = newEmail
                expiresAt.value = newExpiresAt
            }
        }
        catch (error) {
            console.error('Failed to decode token:', error)
        }
    }

    function clearToken() {
        token.value = null
        username.value = null
        employeeName.value = null
        employeeNumber.value = null
        employeeEmail.value = null
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
                setToken(response.token)
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
        username,
        employeeNumber,
        employeeName,
        employeeEmail,
        expiresAt,
        token,
        isAuthenticated,
        setToken,
        refreshToken,
        clearToken,
    }
}, {
    persist: {
        storage: localStorage,
    },
})
