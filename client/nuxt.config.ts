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
    devtools: { enabled: true },
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
