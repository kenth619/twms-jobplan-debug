<script lang="ts" setup>
const colorMode = useColorMode()
</script>

<template>
    <div class="p-4 dark:bg-gray-900 rounded">
        <h1 class="text-2xl font-bold mb-4 dark:text-white">
            Welcome to T&TEC Template Project
        </h1>
        <p class="dark:text-gray-300">
            Select an option from the menu to get started.
        </p>

        <div class="mt-6 p-4 bg-white dark:bg-gray-800 rounded shadow">
            <h2 class="text-xl font-semibold mb-2 dark:text-white">
                Current Color Mode
            </h2>
            <p class="dark:text-gray-300">
                Current mode: <span class="font-bold">{{ colorMode.value }}</span>
            </p>
            <p class="dark:text-gray-300">
                Preference: <span class="font-bold">{{ colorMode.preference }}</span>
            </p>

            <div class="mt-4">
                <select
                    v-model="colorMode.preference"
                    class="p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                >
                    <option value="system">
                        System
                    </option>
                    <option value="light">
                        Light
                    </option>
                    <option value="dark">
                        Dark
                    </option>
                </select>
            </div>
        </div>
    </div>
</template>
