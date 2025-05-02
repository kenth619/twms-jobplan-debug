<script lang="ts" setup>
import type { Pausable } from '@vueuse/core'
import { useModal } from '@outloud/vue-modals'
import { differenceInSeconds } from 'date-fns'

const { close } = useModal()
const remainingSeconds = ref(60)
const progressValue = ref(100)

const authStore = useAuthStore()
const { logout, refreshToken } = authStore
const { expiresAt } = storeToRefs(authStore)

const interval = ref<Pausable>()

onMounted(() => {
    updateRemainingTime()
    if (interval.value) {
        interval.value.pause()
    }
    interval.value = useIntervalFn(updateRemainingTime, 1000)
})

onBeforeUnmount(() => {
    if (interval.value) {
        interval.value.pause()
    }
})

async function updateRemainingTime() {
    if (!expiresAt.value) {
        return
    }
    const secondsLeft = Math.max(0, differenceInSeconds(expiresAt.value, new Date()))
    if (secondsLeft > 60) {
        close()
    }
    remainingSeconds.value = Math.min(secondsLeft, 60)
    progressValue.value = (remainingSeconds.value / 60) * 100

    if (remainingSeconds.value <= 0) {
        if (interval.value) {
            interval.value.pause()
        }
        logout()
        await navigateTo(`/auth/login?inactiveLogout=true`, { redirectCode: 401 })
        close()
    }
}

async function renewSession() {
    await refreshToken()
    close()
}
</script>

<template>
    <ModalFrame
        title="Session Expiring Soon"
        width="w-[90%] lg:w-1/3"
    >
        <div class="flex flex-col gap-4">
            <p class="text-center">
                Your session will expire in {{ remainingSeconds }} seconds due to inactivity.
            </p>

            <div class="w-full bg-gray-200 dark:bg-gray-700 rounded-full h-2.5 mb-4">
                <div
                    class="bg-blue-600 h-2.5 rounded-full"
                    :style="{ width: `${progressValue}%` }"
                />
            </div>

            <div class="flex justify-center">
                <Button
                    label="I'm Still Here"
                    icon="pi pi-check"
                    class="btn-ttec"
                    @click="renewSession"
                />
            </div>
        </div>
    </ModalFrame>
</template>
