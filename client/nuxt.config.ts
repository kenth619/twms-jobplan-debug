import tailwindcss from '@tailwindcss/vite'
import { MyPreset } from './preset'

// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
    modules: [
        '@primevue/nuxt-module',
        '@nuxt/eslint',
        '@hebilicious/vue-query-nuxt',
        '@pinia/nuxt',
        '@nuxt/icon',
        '@vee-validate/nuxt',
        'pinia-plugin-persistedstate/nuxt',
        '@nuxtjs/color-mode',
        '@vueuse/nuxt',
        '@outloud/nuxt-modals',
        '@formkit/auto-animate/nuxt',
    ],
    ssr: false,
    devtools: { enabled: false },
    css: [
        '@/assets/css/main.css',
        'primeicons/primeicons.css',
    ],
    colorMode: {
        fallback: 'light',
        classSuffix: '',
        storage: 'localStorage',
    },
    runtimeConfig: {
        public: {
            apiBase: process.env.NUXT_PUBLIC_API_BASE || 'https://localhost:5001', //Provides a fallback value if the environment variable is missing
            appUrl: process.env.NUXT_PUBLIC_APP_URL || 'http://localhost:3000',
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
                preset: MyPreset,
                options: {
                    darkModeSelector: '.dark',
                },
            },
        },
        components: {
            include: '*',
            exclude: ['Chart', 'Editor', 'Form', 'FormField', 'DatePicker'],
        },
    },
})
