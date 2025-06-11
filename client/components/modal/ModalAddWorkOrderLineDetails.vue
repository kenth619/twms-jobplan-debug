<template>
    <ModalFrame
        title="Add Work Order Line"
        width="w-[90%]"
    >
        <WorkOrderLineDetails
            class="max-h-[80vh] overflow-y-auto"
            @close="close"
        />
    </Modalframe>
</template>

<script lang="ts" setup>
import { useModal } from '@outloud/vue-modals';
import WorkOrderLineDetails from '../WorkOrderLineDetails.vue';

const { close } = useModal()
</script>

<style>

</style>
