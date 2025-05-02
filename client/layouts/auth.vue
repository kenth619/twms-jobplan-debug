<script lang="ts" setup>
useHead({
    title: 'Login - T&TEC Work Management System',
})
</script>

<template>
    <div class="flex flex-col xl:flex-row h-screen overflow-hidden">
        <div class="flex-grow overflow-auto bg-neutral-0 dark:bg-neutral-700 p-5 xl:p-5">
            <slot />
        </div>
    </div>
</template>
