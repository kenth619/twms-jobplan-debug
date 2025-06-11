<script lang="ts" setup>
import { useModal } from '@outloud/vue-modals'

const props = withDefaults(
    defineProps<{
        title: string
        width?: string
        height?: string
        onClose?: () => void
    }>(),
    {
        width: 'w-[90dvw] xl:w-auto',
        height: 'max-h-[90dvh]',
    },
)

const { close } = useModal()

function handleClose() {
    if (props.onClose) {
        props.onClose()
    }
    close()
}
</script>

<template>
    <Card
        :pt-options="{ mergeSections: true, mergeProps: true }"
        :pt:root:class="`${props.width} ${props.height} m-auto overflow-hidden`"
    >
        <template #title>
            <div class="flex flex-row justify-between items-center">
                <div class="flex-grow flex flex-row gap-2 items-center">
                    <slot name="titleExtra" />
                    {{ title }}
                </div>

                <Button
                    icon="pi pi-times"
                    size="small"
                    class="btn-transparent"
                    @click="handleClose()"
                />
            </div>
        </template>
        <template #content>
            <slot />
        </template>
    </Card>
</template>
