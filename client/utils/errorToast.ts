import { FetchError } from 'ofetch'

export function errorToast(error: unknown) {
    const nuxtApp = useNuxtApp()
    const getToast: typeof useToast = () => nuxtApp.vueApp.config.globalProperties.$toast
    const toast = getToast()

    let errorMessage = 'An unknown error occured!'
    if (error instanceof FetchError && error.data) {
        errorMessage = error.data
    }

    toast.add({ severity: 'error', summary: errorMessage, life: 3000 })
}
