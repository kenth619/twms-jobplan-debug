<script setup lang="ts">
import { OModalsContainer, useModals } from '@outloud/vue-modals'
import { ModalSessionExpiring } from '#components'

const modals = useModals()
const $content = ref<HTMLElement>()
modals.content = $content

const authStore = useAuthStore()
const { clearToken, isSessionExpiringSoon } = authStore
const { isSessionExpired, expiresAt } = storeToRefs(authStore)

const isSessionExpiringModalOpen = ref(false)

async function showSessionExpiringModal() {
    if (!isSessionExpiringModalOpen.value && expiresAt.value) {
        isSessionExpiringModalOpen.value = true
        modals.open(ModalSessionExpiring)
    }
}

onMounted(async () => {
    useIntervalFn(async () => {
        if (isSessionExpired.value) {
            clearToken()
            navigateTo(`/auth/login?inactiveLogout=true`, { redirectCode: 401 })
            isSessionExpiringModalOpen.value = false
        }
        else if (isSessionExpiringSoon()) {
            await showSessionExpiringModal()
        }
    }, 10000)
})
</script>

<template>
    <div>
        <NuxtLayout>
            <NuxtPage />
        </NuxtLayout>
    </div>
    <OModalsContainer />
    <div class="w-screen flex flex-row items-center xl:items-end">
        <Toast class="z-40" />
    </div>
</template>
