import type { FetchOptions, FetchResponse } from 'ofetch'
import { FetchError } from 'ofetch'
import { defu } from 'defu'

export async function $api<T>(
    url: string,
    options: FetchOptions = {},
): Promise<T> {
    const config = useRuntimeConfig()
    const authStore = useAuthStore()
    const { clearToken, setToken } = authStore

    function updateAuthState(headers: Headers) {
        const newToken = headers.get('x-new-token')
        if (newToken) {
            setToken(newToken)
        }
    }

    const headers: Record<string, string> = {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
    }

    if (authStore.token) {
        headers['Authorization'] = `Bearer ${authStore.token}`
    }

    const defaults = {
        baseURL: config.public.apiBase,
        headers,
        async onResponse({ response }: { response: FetchResponse<T> }) {
            updateAuthState(response.headers)
        },
    }

    const params = defu(options, defaults)

    try {
        // eslint-disable-next-line @typescript-eslint/no-explicit-any
        const response = await $fetch(url, params as unknown as any) as T
        return response
    }
    catch (error) {
        if (error instanceof FetchError && error.response?.status === 401) {
            // We no longer need to manually refresh tokens on 401 errors since the server proactively refreshes tokens
            // Just logout and redirect to login page
            clearToken()
            navigateTo('/auth/login')
        }
        throw error
    }
}
