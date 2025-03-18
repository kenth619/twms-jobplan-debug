import Lara from '@primeuix/themes/lara'
import tailwindcss from '@tailwindcss/vite'

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
    modules: [
        '@primevue/nuxt-module',
        '@nuxt/eslint',
        '@hebilicious/vue-query-nuxt',
        '@pinia/nuxt',
        '@nuxt/icon',
        'pinia-plugin-persistedstate/nuxt',
        '@nuxtjs/color-mode',
        '@vueuse/nuxt',
    ],
    ssr: false,
    devtools: { enabled: true },
    css: [
        '@/assets/css/main.css',
        'primeicons/primeicons.css',
    ],
    colorMode: {
        preference: 'system',
        fallback: 'light',
        classSuffix: '',
    },
    runtimeConfig: {
        public: {
            apiBase: '',
        },
    },
    compatibilityDate: '2024-11-01',
    vite: {
        plugins: [
            tailwindcss(),
        ],
    },
    eslint: {
        config: {
            stylistic: {
                indent: 4,
                quotes: 'single',
                semi: false,
            },
        },
    },
    primevue: {
        options: {
            theme: {
                preset: Lara,
                options: {
                    darkModeSelector: '.dark',
                },
            },
        },
    },
})
