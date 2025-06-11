<script setup lang="ts">
defineProps<{
    errorMessage: string | undefined
}>()
</script>

<template>
    <small
        v-if="errorMessage"
        class="text-red-500"
    >
        {{ errorMessage }}
    </small>
</template>
