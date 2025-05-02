<script lang="ts" setup>
import Card from 'primevue/card';

defineProps({
    title: {
        type: String,
        default: 'Default Title',
    },
    content: {
        type: String,
        default: 'Default Content',
    },
})

const collapse = ref(false)
</script>

<template>
    <div class="mt-5 shadow-2xl">
        <Card>
            <template #header>
                <div class="p-2 text-lg font-bold text-center text-white bg-green-600 dark:bg-green-900 rounded-t-md">
                    <b>{{ title }}</b>
                    <Icon
                        v-show="!collapse"
                        name="material-symbols:arrow-drop-up"
                        size="2rem"
                        class="ml-2"
                        @click="collapse = !collapse"
                    />
                    <Icon
                        v-show="collapse"
                        name="material-symbols:arrow-drop-down"
                        size="2rem"
                        class="ml-2"
                        @click="collapse = !collapse"
                    />
                </div>
            </template>

            <template #content>
                <div v-auto-animate>
                    <p
                        v-if="!collapse"
                        class="m-0"
                    >
                        {{ content }}
                    </p>
                </div>
            </template>
        </Card>
    </div>
</template>

<style>

</style>
