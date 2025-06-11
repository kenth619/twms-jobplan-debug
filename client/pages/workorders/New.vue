<template>
    <div>
        <div class="flex justify-between mb-2">
            <h1 class="text-2xl font-bold mb-4 dark:text-white">
                New Work Order
            </h1>

            <Button @click="submitWorkOrder">
                <Icon name="material-symbols:save" size="1.5rem" />
                Submit
            </Button>
        </div>

        <WorkOrderHeader v-model="workOrderHeader" />

        <div
            class="bg-white dark:bg-neutral-800 rounded-md border border-green-500 dark:border-green-800 p-4 mb-6 shadow-xl">
            <h2 class="text-xl font-semibold mb-2 dark:text-white">
                Attachments
            </h2>
            <label for="">Enter RFS ID</label>

            <div class="grid grid-cols-3 gap-4 mb-4">
                <InputGroup class="mb-5">
                    <InputNumber placeholder="RFS ID" :use-grouping="false" />
                    <InputGroupAddon>
                        <Button>
                            <Icon name="material-symbols:search" size="1.5rem" />
                            Search
                        </Button>
                    </InputGroupAddon>
                </InputGroup>
            </div>

            <p>Select type of document to upload</p>
            <Select v-model="docType" placeholder="Select Document Type" :options="options" option-label="name"
                show-clear class="mb-5" />

            <FileUpload v-show="docType" ref="fileupload" name="demo[]" url="/api/upload"
                accept="image/*, text/plain, application/pdf, application/vnd.ms-excel, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                :max-file-size="15729000">
                <template #empty>
                    <span><i>Drag and drop files or click on the button above to upload</i></span>
                </template>
            </FileUpload>

            <DataTable class="mt-5">
                <template #empty>
                    No files uploaded
                </template>
                <Column field="id" header="Document Type" sortable />
                <Column field="description" header="File Name" sortable />
                <Column field="status" header="Date Uploaded" sortable />
                <Column field="status" header="Uploaded By" sortable />
                <Column field="status" header="Remarks" />
            </DataTable>
        </div>

        <Button class="mb-2" @click="addWOLine">
            <Icon name="material-symbols:add" size="1.5rem" />
            Add Work Order Line
        </Button>

        <div>
            <WorkOrderLine />
        </div>

        <div class="flex justify-end mb-4">
            <Button @click="submitWorkOrder">
                <Icon name="material-symbols:save" size="1.5rem" />
                Submit
            </Button>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { useWorkOrderHeaderStore } from '@/stores/workorderheader'
import { useModals } from '@outloud/vue-modals'
import Column from 'primevue/column'
import DataTable from 'primevue/datatable'
import InputGroup from 'primevue/inputgroup'
import InputGroupAddon from 'primevue/inputgroupaddon'
import Select from 'primevue/select'
import WorkOrderHeader from '~/components/WorkOrderHeader.vue'
import WorkOrderLine from '~/components/WorkOrderLine.vue'
import ModalAddWorkOrderDetails from '~/components/modal/ModalAddWorkOrderLineDetails.vue'

const workOrderHeader = ref()

const workOrderHeaderStore = useWorkOrderHeaderStore()

const docType = ref('')
const options = ref([
    { name: 'Job Sheet', code: 'job_sheet' },
    { name: 'Drawing', code: 'drawing' },
    { name: 'Job Estimate', code: 'job_estimate' },
    { name: 'Estimation Material List', code: 'estimation_material_list' },
    { name: 'Pole and Guy Notice', code: 'pole_and_guy_notice' },
    { name: 'Condition of Supply Letter', code: 'condition_of_supply_letter' },
])

const modals = useModals()

function addWOLine() {
    modals.open(ModalAddWorkOrderDetails)
}

function close() {
    modals.close(true)
}

const submitWorkOrder = () => {
    // Logic to submit the work order
    workOrderHeaderStore.submitWorkOrderHeader()
    // .then(() => {
    //     // Handle success
    //     console.log('Work order submitted successfully')
    // })
    // .catch((error) => {
    //     // Handle error
    //     console.error('Error submitting work order:', error)
    // })
}
</script>

<style></style>
