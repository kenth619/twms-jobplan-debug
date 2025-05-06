<template>
    <div>
        <div class="flex justify-between mb-5">
            <h1 class="text-2xl font-bold mb-4 dark:text-white">
                New Work Order
            </h1>

            <Button>
                <Icon
                    name="material-symbols:save"
                    size="1.5rem"
                />
                Submit
            </Button>
        </div>

        <div class="bg-white dark:bg-neutral-900 rounded-md border border-green-500 dark:border-green-800 p-4 mb-5 shadow-2xl">
            <div class="grid grid-cols-3 gap-4 mb-4">
                <div>
                    <label for="">Requested By</label>
                    <InputText
                        v-model="requestedBy"
                        type="text"
                        class="w-full p-2"
                        placeholder="Enter your name"
                    />
                </div>
                <div>
                    <label for="">Area/Department</label>
                    <InputText
                        type="text"
                        class="w-full p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label for="">Section</label>
                    <InputText
                        type="text"
                        class="w-full p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label for="">Account</label>
                    <InputText
                        v-model="requestedBy"
                        type="text"
                        class="w-full p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                        placeholder="Enter your name"
                    />
                </div>
                <div>
                    <label for="">Cost Centre</label>
                    <InputText
                        type="text"
                        class="w-full p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label for="">Job Number</label>
                    <InputText
                        type="text"
                        class="w-full p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label for="">Job Type</label>
                    <InputText
                        v-model="requestedBy"
                        type="text"
                        class="w-full p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                        placeholder="Enter your name"
                    />
                </div>
                <div>
                    <label for="">Job Type Sub-Category</label>
                    <InputText
                        type="text"
                        class="w-full p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label for="">Priority</label>
                    <InputText
                        type="text"
                        class="w-full p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label for="">Source Document Type</label>
                    <InputText
                        type="text"
                        class="w-full p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                        placeholder="Enter work order number"
                    />
                </div>
                <div>
                    <label for="">Source Document Number</label>
                    <InputText
                        type="text"
                        class="w-full p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                        placeholder="Enter work order number"
                    />
                </div>
            </div>
        </div>

        <div class="bg-white dark:bg-neutral-900 rounded-md border border-green-500 dark:border-green-800 p-4 mb-6 shadow-2xl">
            <h2 class="text-xl font-semibold mb-2 dark:text-white">
                Attachments
            </h2>
            <div>
                <label for="">Enter RFS ID</label>

                <InputGroup class="mb-5 w-1/2">
                    <InputText
                        placeholder="RFS ID"
                    />
                    <InputGroupAddon>
                        <Button>
                            <Icon
                                name="material-symbols:search"
                                size="1.5rem"
                            />
                            Search
                        </Button>
                    </InputGroupAddon>
                </InputGroup>

                <p>Select type of document to upload</p>
                <Select
                    v-model="docType"
                    placeholder="Select Document Type"
                    :options="options"
                    option-label="name"
                    show-clear
                    class="mb-5"
                />

                <FileUpload
                    v-show="docType"
                    ref="fileupload"
                    name="demo[]"
                    url="/api/upload"
                    accept="image/*"
                    :max-file-size="1000000"
                >
                    <template #empty>
                        <span>Drag and drop files or click on the button above to upload</span>
                    </template>
                </FileUpload>

                <DataTable class="mt-5">
                    <template #empty>
                        No files uploaded
                    </template>
                    <Column
                        field="id"
                        header="Document Type"
                        sortable
                    />
                    <Column
                        field="description"
                        header="File Name"
                        sortable
                    />
                    <Column
                        field="status"
                        header="Date Uploaded"
                        sortable
                    />
                    <Column
                        field="status"
                        header="Uploaded By"
                        sortable
                    />
                    <Column
                        field="status"
                        header="Remarks"
                    />
                </DataTable>
            </div>
        </div>

        <Button class="mb-2">
            <Icon
                name="material-symbols:add"
                size="1.5rem"
            />
            Add Work Order Line
        </Button>

        <WorkOrderLine />

        <div class="bg-white dark:bg-neutral-900 rounded-md border border-green-500 dark:border-green-800 p-4 mb-6 shadow-2xl">
            <h2 class="text-xl font-semibold mb-2 dark:text-white">
                Work Order Plan Priority
            </h2>
            <div class="grid grid-cols-3 gap-4 mb-4">
                <div>
                    <label for="">Job Plan ID</label>
                    <InputGroup class="mb-5">
                        <InputText
                            placeholder="Job Plan ID"
                        />
                        <InputGroupAddon>
                            <Button>
                                <Icon
                                    name="material-symbols:search"
                                    size="1.5rem"
                                />
                                Search
                            </Button>
                        </InputGroupAddon>
                    </InputGroup>
                </div>
                <div class="col-span-2">
                    <label>Description</label>
                    <InputText
                        type="text"
                        class="w-full p-2"
                    />
                </div>
            </div>
            <label>Summary of Work Planned</label>
            <Textarea
                type="text"
                class="w-full p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                placeholder="Enter asset description"
                rows="3"
            />
        </div>

        <div class="flex justify-end">
            <Button>
                <Icon
                    name="material-symbols:save"
                    size="1.5rem"
                />
                Submit
            </Button>
        </div>
    </div>
</template>

<script lang="ts" setup>
import Column from 'primevue/column'
import DataTable from 'primevue/datatable'
import InputGroup from 'primevue/inputgroup'
import InputGroupAddon from 'primevue/inputgroupaddon'
import Select from 'primevue/select'
import Textarea from 'primevue/textarea'
import WorkOrderLine from '~/components/WorkOrderLine.vue'

const requestedBy = ref('')
const docType = ref('')
const options = ref([
    { name: 'Job Sheet', code: 'job_sheet' },
    { name: 'Drawing', code: 'drawing' },
    { name: 'Job Estimate', code: 'job_estimate' },
    { name: 'Estimation Material List', code: 'estimation_material_list' },
    { name: 'Pole and Guy Notice', code: 'pole_and_guy_notice' },
    { name: 'Condition of Supply Letter', code: 'condition_of_supply_letter' },
])
</script>

<style>

</style>
