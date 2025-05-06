<script lang="ts" setup>
import type { NavigateToOptions } from '#app/composables/router';
import type { MenuItem } from 'primevue/menuitem';

defineProps<{
    collapsed?: boolean
}>()

const emit = defineEmits(['dismiss'])

const { hasAny } = useRoles()
const authStore = useAuthStore()
const { isAuthenticated } = storeToRefs(authStore)

const logout = async () => {
    try {
        await $api('/api/auth/logout', {
            method: 'POST',
        })
    }
    catch (error) {
        console.error('Logout error:', error)
    }
    finally {
        authStore.logout()
        navigateTo('/auth/login')
    }
}
function navigateToAndDismiss(arg1: string, arg2?: NavigateToOptions) {
    emit('dismiss')
    return navigateTo(arg1, arg2)
}

const items = computed<MenuItem[]>(() => {
    const result = [] as MenuItem[]

    // home page
    result.push({
        label: 'Home',
        icon: 'material-symbols:home',
        command: () => navigateToAndDismiss(''),
    })

    // Admin sections
    if (hasAny(['system-administrator', 'superuser'])) {
        // if (result.length > 0) {
        //     result.push({ separator: true })
        // }
        result.push({
            label: 'Manage Employees',
            icon: 'material-symbols:manage-accounts',
            command: () => navigateToAndDismiss('/sysadmin/manage-employees'),
        })
    }

    // Sign out is always available
    // if (result.length > 0) {
    //     result.push({ separator: true })
    // }

    result.push({
        label: 'New Work Order',
        icon: 'material-symbols:add',
        command: () => navigateToAndDismiss('/workorders/new'),
    })

    result.push({
        label: 'Sign out',
        icon: 'material-symbols:logout',
        command: () => logout(),
    })

    return result
})
</script>

<template>
    <Menu
        v-if="isAuthenticated"
        :model="items"
        class="h-full !rounded-none !border-none !bg-[#384940]"
    >
        <template #item="{ item, props }">
            <a
                v-ripple
                class="flex flex-row items-center text-nowrap !text-white"
                v-bind="props.action"
            >
                <Icon
                    v-if="item.icon"
                    size="1.5rem"
                    :name="item.icon"
                />
                <div
                    v-if="!collapsed"
                    class="ml-2 transition-opacity duration-300 text-nowrap "
                >
                    {{ item.label }}
                </div>
            </a>
        </template>
    </Menu>
</template>
