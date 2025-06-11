<template>
    <Dialog 
        v-model:visible="isVisible" 
        modal 
        :header="header"
        :style="{ width: width }"
        :closable="closable"
        @update:visible="handleVisibilityChange"
    >
        <div class="flex items-center gap-3">
            <Icon 
                :name="iconName" 
                size="2rem" 
                :class="iconClass"
            />
            <div>
                <p class="text-lg font-medium mb-2">{{ title }}</p>
                <p class="text-gray-600 dark:text-gray-300">
                    {{ message }}
                </p>
            </div>
        </div>
        
        <template #footer>
            <div class="flex justify-end gap-2">
                <Button 
                    :label="cancelLabel" 
                    :severity="cancelSeverity" 
                    :outlined="cancelOutlined"
                    :loading="loading"
                    @click="handleCancel"
                />
                <Button 
                    :label="confirmLabel" 
                    :severity="confirmSeverity"
                    :loading="loading"
                    @click="handleConfirm"
                />
            </div>
        </template>
    </Dialog>
</template>

<script lang="ts" setup>
import { computed } from 'vue'
import Dialog from 'primevue/dialog'
import Button from 'primevue/button'

// Define severity types
type Severity = 'success' | 'info' | 'warn' | 'danger' | 'help' | 'primary' | 'secondary' | 'contrast'

// Props
interface Props {
    visible: boolean
    header?: string
    title: string
    message: string
    width?: string
    closable?: boolean
    loading?: boolean
    
    // Icon props
    iconName?: string
    iconType?: 'warning' | 'error' | 'info' | 'success' | 'question'
    
    // Button props
    confirmLabel?: string
    cancelLabel?: string
    confirmSeverity?: Severity
    cancelSeverity?: Severity
    cancelOutlined?: boolean
}

const props = withDefaults(defineProps<Props>(), {
    header: 'Confirm Action',
    width: '450px',
    closable: true,
    loading: false,
    iconType: 'warning',
    confirmLabel: 'Confirm',
    cancelLabel: 'Cancel',
    confirmSeverity: 'danger',
    cancelSeverity: 'secondary',
    cancelOutlined: true
})

// Emits
const emit = defineEmits<{
    'update:visible': [value: boolean]
    'confirm': []
    'cancel': []
}>()

// Computed properties
const isVisible = computed({
    get: () => props.visible,
    set: (value: boolean) => emit('update:visible', value)
})

const iconName = computed(() => {
    if (props.iconName) return props.iconName
    
    switch (props.iconType) {
        case 'warning':
            return 'material-symbols:warning'
        case 'error':
            return 'material-symbols:error'
        case 'info':
            return 'material-symbols:info'
        case 'success':
            return 'material-symbols:check-circle'
        case 'question':
            return 'material-symbols:help'
        default:
            return 'material-symbols:warning'
    }
})

const iconClass = computed(() => {
    switch (props.iconType) {
        case 'warning':
            return 'text-orange-500'
        case 'error':
            return 'text-red-500'
        case 'info':
            return 'text-blue-500'
        case 'success':
            return 'text-green-500'
        case 'question':
            return 'text-purple-500'
        default:
            return 'text-orange-500'
    }
})

// Methods
function handleVisibilityChange(value: boolean) {
    emit('update:visible', value)
}

function handleConfirm() {
    emit('confirm')
}

function handleCancel() {
    emit('cancel')
}
</script>
