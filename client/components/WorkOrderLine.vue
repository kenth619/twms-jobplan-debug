<template>
    <div>
        <div
            class="bg-white dark:bg-neutral-800 rounded-md border border-green-500 dark:border-green-800 p-4 mb-5 shadow-xl">
            <h2 class="text-xl font-semibold mb-2">
                Work Order Lines
            </h2>
            <DataTable :value="workOrderLines" class="w-full">
                <template #empty>
                    No work order lines added
                </template>
                <Column field="id" header="Line No" sortable />
                <Column field="status" header="Status" sortable />
                <Column field="assetId" header="Asset ID" sortable />
                <Column field="description" header="Asset Description" sortable />
                <Column field="location" header="Location" sortable />
                <Column field="remarks" header="Remarks" />
                <Column>
                    <template #body>
                        <ButtonGroup>
                            <Button class="mb-4" size="small" @click="editWOLine">
                                <Icon name="material-symbols:edit-square-outline" size="1.5rem" />
                            </Button>
                            <Button severity="danger" class="mb-4" size="small"
                                @click="removeWOLine(workOrderLines.id)">
                                <Icon name="material-symbols:delete-outline" size="1.5rem" />
                            </Button>
                        </ButtonGroup>
                    </template>
                </Column>
            </DataTable>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { useModals } from '@outloud/vue-modals'
import ModalEditWorkOrderLineDetails from './modal/ModalEditWorkOrderLineDetails.vue'

const workOrderLines = ref([
    { id: 1, status: 'Open', assetId: 'A123', description: 'Transformer', location: 'Substation A', remarks: 'Urgent' },
    { id: 2, status: 'Closed', assetId: 'B456', description: 'Switchgear', location: 'Substation B', remarks: 'Routine' },
])

const modals = useModals()

function editWOLine() {
    modals.open(ModalEditWorkOrderLineDetails)
}

const removeWOLine = (line) => {
    alert('Remove work order line: ' + line)
    // const index = workOrderLines.value.findIndex(item => item.id === line.id)
    // if (index !== -1) {
    //     workOrderLines.value.splice(index, 1)
    // }
}
</script>

<style></style>
