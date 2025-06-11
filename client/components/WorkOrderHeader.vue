<template>
    <div>
        <div class="bg-white dark:bg-neutral-800 rounded-md border border-green-500 dark:border-green-800 p-4 mb-5 shadow-xl">
            <div class="grid grid-cols-3 gap-4 mb-4">
                <div>
                    <label>Requested By</label>
                    <InputText
                        v-model="workOrderHeader.requestedBy"
                        type="text"
                        class="w-full p-2"
                        placeholder="Enter your name"
                    />
                </div>
                <div>
                    <label>Area/Department</label>
                    <Select
                        v-model="workOrderHeader.area"
                        :options="departments"
                        option-label="name"
                        filter
                        show-clear
                        class="w-full"
                        placeholder="Select an area"
                        @change="console.log(workOrderHeader.area)"
                    />
                </div>
                <div>
                    <label>Section</label>
                    <Select
                        ref="section-select"
                        v-model="workOrderHeader.section"
                        :options="workOrderHeader.area?.sections"
                        :disabled="!workOrderHeader.area"
                        option-label="name"
                        filter
                        show-clear
                        class="w-full"
                        placeholder="Select a department first"
                    />
                </div>
                <div>
                    <label>Associated Work Order Number</label>
                    <InputNumber
                        v-model="workOrderHeader.associatedWO"
                        class="w-full"
                        :use-grouping="false"
                    />
                </div>
                <div class="col-span-2" />

                <div>
                    <label>Job Type</label>
                    <InputText
                        v-model="workOrderHeader.jobType"
                        type="text"
                        class="w-full"
                        placeholder="Enter your name"
                    />
                </div>
                <div>
                    <label>Job Type Sub-Category</label>
                    <InputText
                        v-model="workOrderHeader.jobTypeSubCategory"
                        type="text"
                        class="w-full"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label>Priority</label>
                    <InputText
                        v-model="workOrderHeader.priority"
                        type="text"
                        class="w-full"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label>Source Document Type</label>
                    <InputText
                        v-model="workOrderHeader.sourceDocumentType"
                        type="text"
                        class="w-full"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label>Source Document Number</label>
                    <InputText
                        v-model="workOrderHeader.sourceDocumentNumber"
                        type="text"
                        class="w-full"
                        placeholder="Enter work order number"
                    />
                </div>
                <div />
                <div>
                    <label>Account</label>
                    <InputText
                        v-model="workOrderHeader.accountNo"
                        type="text"
                        class="w-full"
                        placeholder="Enter your name"
                    />
                </div>
                <div>
                    <label for="">Cost Centre</label>
                    <InputText
                        v-model="workOrderHeader.costCentre"
                        type="text"
                        class="w-full"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label for="">Job Number</label>
                    <InputText
                        v-model="workOrderHeader.jobNumber"
                        type="text"
                        class="w-full"
                        placeholder="Enter work order number"
                    />
                </div>
            </div>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { useWorkOrderHeaderStore } from '@/stores/workorderheader'

const workOrderHeaderStore = useWorkOrderHeaderStore()
const workOrderHeader = workOrderHeaderStore.workOrderHeader

const { data: departments } = useQuery({
    queryKey: ['departments'],
    queryFn: async () => {
        const response = await $api<Department[]>('/api/departments')
        return response || []
    },
})

// const changePlaceholder = (e: { value: { name: string } | null }) => {
//     const selectedArea = e.value
//     const sectionSelect = ref('section-select')
//     if (selectedArea) {
//         sectionSelect.value.placeholder = `Select a section in ${selectedArea.name}`
//     }
//     else {
//         sectionSelect.value.placeholder = 'Select a department first'
//     }
// }
</script>

<style>

</style>
