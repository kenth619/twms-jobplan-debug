<template>
    <div>
        <div class="bg-white dark:bg-neutral-800 rounded-md border border-green-500 dark:border-green-800 p-4 mb-5 shadow-xl">
            <div class="grid grid-cols-3 gap-4 mb-4">
                <div>
                    <label>Requested By</label>
                    <InputText
                        v-model="requestedBy"
                        type="text"
                        class="w-full p-2"
                        placeholder="Enter your name"
                    />
                </div>
                <div>
                    <label>Area/Department</label>
                    <Select
                        v-model="area"
                        :options="departments"
                        option-label="name"
                        filter
                        show-clear
                        class="w-full"
                        placeholder="Select an area"
                        @change="changePlaceholder(area)"
                    />
                </div>
                <div>
                    <label>Section</label>
                    <Select
                        ref="section-select"
                        v-model="section"
                        :options="area?.sections"
                        :disabled="!area"
                        option-label="name"
                        filter
                        show-clear
                        class="w-full"
                        placeholder="Select a department first"
                        s
                    />
                </div>
                <div>
                    <label>Associated Work Order Number</label>
                    <InputNumber
                        v-model="associatedWO"
                        class="w-full"
                        :use-grouping="false"
                    />
                </div>
                <div class="col-span-2">
                    <Checkbox
                        v-model="rescheduledWO"
                        class="w-full"
                        binary
                    />
                    <label for="rescheduledWO">Rescheduled Work Order</label>
                </div>
                <div>
                    <label>Account</label>
                    <InputText
                        v-model="accountNo"
                        type="text"
                        class="w-full"
                        placeholder="Enter your name"
                    />
                </div>
                <div>
                    <label for="">Cost Centre</label>
                    <InputText
                        v-model="costCentre"
                        type="text"
                        class="w-full"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label for="">Job Number</label>
                    <InputText
                        v-model="jobNumber"
                        type="text"
                        class="w-full"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label>Job Type</label>
                    <InputText
                        v-model="jobType"
                        type="text"
                        class="w-full"
                        placeholder="Enter your name"
                    />
                </div>
                <div>
                    <label>Job Type Sub-Category</label>
                    <InputText
                        v-model="jobTypeSubCategory"
                        type="text"
                        class="w-full"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label>Priority</label>
                    <InputText
                        v-model="priority"
                        type="text"
                        class="w-full"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label>Source Document Type</label>
                    <InputText
                        v-model="sourceDocumentType"
                        type="text"
                        class="w-full"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label>Source Document Number</label>
                    <InputText
                        v-model="sourceDocumentNumber"
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
const requestedBy = ref('')
const area = ref('')
const section = ref('')
const accountNo = ref('')
const costCentre = ref('')
const jobNumber = ref('')
const jobType = ref('')
const jobTypeSubCategory = ref('')
const priority = ref('')
const associatedWO = ref(null)
const rescheduledWO = ref(false)
const sourceDocumentType = ref('')
const sourceDocumentNumber = ref('')

const { data: departments } = useQuery({
    queryKey: ['departments'],
    queryFn: async () => {
        const response = await $api<Department[]>('/api/departments')
        return response || []
    },
})

const changePlaceholder = (e: any) => {
    const selectedArea = e.value
    const sectionSelect = ref('section-select')
    if (selectedArea) {
        sectionSelect.value.placeholder = `Select a section in ${selectedArea.name}`
    }
    else {
        sectionSelect.value.placeholder = 'Select a department first'
    }
}
</script>

<style>

</style>
