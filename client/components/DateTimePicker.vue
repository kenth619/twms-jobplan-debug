<script setup lang="ts">
import { useField } from 'vee-validate'

const props = withDefaults(defineProps<{
    name: string
    label?: string
    editing?: boolean
    typeahead?: boolean
    validateOnMount?: boolean
    modelValue?: Date | undefined
}>(), {
    editing: true,
})

const theme = useTheme()

const name = toRef(props, 'name')
const editing = toRef(props, 'editing')

const { value: fieldValue, errorMessage } = useField(name, undefined, {
    initialValue: props.modelValue,
    validateOnMount: props.validateOnMount,
})

const regularInputClasses = '!bg-datepicker-background !text-color-datepicker-text-color !py-datepicker-padding-y !px-datepicker-padding-x !rounded-datepicker-border-radius !border-datepicker-border-color hover:!border-datepicker-border-color-hover'
const invalidInputClasses = '!bg-datepicker-background !text-color-datepicker-text-color !py-datepicker-padding-y !px-datepicker-padding-x !rounded-datepicker-border-radius !border !border-datepicker-border-color-invalid'
const disabledInputClasses = '!bg-datepicker-background-disabled !text-color-datepicker-text-color-disabled !py-datepicker-padding-y !px-datepicker-padding-x !rounded-datepicker-border-radius'
const menuClasses = '!rounded-md !text-neutral-800 dark:!text-white/80 !bg-neutral-0 dark:!bg-neutral-900'

const dark = computed(() => theme.darkMode.value)
const state = computed(() => (errorMessage.value ? false : null))

const ui = computed(() => {
    if (!editing.value) {
        return {
            input: disabledInputClasses,
            menu: menuClasses,
        }
    }
    else if (errorMessage.value) {
        return {
            input: invalidInputClasses,
            menu: menuClasses,
        }
    }
    else {
        return {
            input: regularInputClasses,
            menu: menuClasses,
        }
    }
})
</script>

<template>
    <div class="flex flex-col gap-1">
        <label
            v-if="props.label"
            :for="name"
            class="font-medium"
        >
            {{ label }}
        </label>
        <VueDatePicker
            v-model="fieldValue"
            format="yyyy-MM-dd hh:mm aa"
            :ui
            :dark
            :state
            :disabled="!editing"
            :is-24="false"
            time-picker-inline
            hide-input-icon
        />
        <ErrorMessage :error-message="errorMessage" />
    </div>
</template>
