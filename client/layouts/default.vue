<script lang="ts" setup>
import type { RouteLocationNormalizedLoadedGeneric } from 'vue-router'

useHead({
    title: 'T&TEC Template Project',
})

const route = useRoute()

const { theme, setTheme } = useTheme()
const themeOptions = ref(['Dark', 'Light', 'Auto'])
const themeValue = ref<'Dark' | 'Light' | 'Auto'>(theme.value)
watch(themeValue, async () => await setTheme(themeValue.value))

const back = computed(() => route.meta.back as ((route: RouteLocationNormalizedLoadedGeneric) => string) | undefined)

const drawer = ref(false)

const collapsed = useState('collapsed', () => localStorage.getItem('collapsed') === 'true')
const sidebarWidth = computed(() => collapsed.value ? 'w-[4rem]' : 'w-[20rem]')
watch(collapsed, () => {
    localStorage.setItem('collapsed', collapsed.value.toString())
})
</script>

<template>
    <div class="flex flex-col xl:flex-row h-screen overflow-hidden">
        <div
            class="hidden xl:block relative flex-shrink-0 border border-t-0 border-neutral-300 dark:border-neutral-600 bg-neutral-0 text-black dark:bg-neutral-800 dark:text-white overflow-clip transition-all duration-300 ease-in-out"
            :class="sidebarWidth"
        >
            <span
                class="flex flex-row p-2 gap-5 items-center cursor-pointer"
                @click="collapsed = !collapsed"
            >
                <img
                    alt="T&TEC Logo"
                    class="h-10"
                    src="public/logo_full.webp"
                >
                <div
                    v-if="!collapsed"
                    class="text-2xl transition-opacity duration-300 text-nowrap"
                >
                    Template Project
                </div>
            </span>
            <AppMenu :collapsed />
            <div
                v-if="!collapsed"
                class="absolute bottom-0 inset-x-0 transition-opacity duration-300"
            >
                <div class="flex flex-row justify-center pb-4">
                    <SelectButton
                        v-model="themeValue"
                        :options="themeOptions"
                    />
                </div>
            </div>
        </div>

        <div class="xl:hidden flex-shrink-0 w-full z-30 bg-neutral-0 text-black dark:bg-neutral-700 dark:text-white pt-safe">
            <div class="flex flex-row min-h-16 items-center justify-center relative">
                <div class="flex flex-col justify-center flex-shrink-0 absolute left-0 inset-y-0 pl-2 my-2">
                    <div class="flex flex-row gap-4">
                        <Icon
                            name="material-symbols:menu-rounded"
                            size="2rem"
                            @click="drawer = true"
                        />
                        <Icon
                            v-if="back"
                            name="material-symbols:keyboard-backspace-rounded"
                            size="2rem"
                            class="cursor-pointer hidden"
                            @click="navigateTo(back(route))"
                        />
                    </div>

                    <Drawer v-model:visible="drawer">
                        <template #header>
                            <span
                                class="flex flex-row p-2 gap-5 items-center"
                                @click="navigateTo('/')"
                            >
                                <img
                                    alt="T&TEC Logo"
                                    class="h-10"
                                    src="public/logo_full.webp"
                                >
                                <div class="text-2xl">
                                    T&TEC Project Template
                                </div>
                            </span>
                        </template>
                        <AppMenu @dismiss="drawer = false" />
                        <template #footer>
                            <div class="flex flex-row justify-center pb-4">
                                <SelectButton
                                    v-model="themeValue"
                                    :options="themeOptions"
                                />
                            </div>
                        </template>
                    </Drawer>
                </div>
                <div class="text-xl font-semibold">
                    <div v-if="route.meta.title">
                        {{ route.meta.title }}
                    </div>
                    <div v-else>
                        T&TEC Project Template
                    </div>
                </div>
                <div
                    class="flex flex-col flex-shrink-0 absolute end-0 inset-y-0 pr-2 my-2 h-16"
                    @click="navigateTo('/')"
                >
                    <img
                        alt="T&TEC Logo"
                        class="h-12"
                        src="public/logo_full.webp"
                    >
                </div>
            </div>
        </div>

        <div class="flex-grow overflow-auto bg-neutral-0 dark:bg-neutral-700 p-5 xl:p-5">
            <slot />
        </div>
    </div>
</template>
