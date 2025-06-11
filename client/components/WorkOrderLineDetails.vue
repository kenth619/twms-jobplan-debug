<script lang="ts" setup>
import { useWorkorderLineStore, type WorkOrderLine } from '#imports'

const workOrderLine = useWorkorderLineStore()

const workOrderLineLocal = ref({} as WorkOrderLine)

const addLine = () => {
    workOrderLine.submitWorkOrderLine(workOrderLineLocal.value)
}
</script>

<template>
    <div>
        <div class="mb-2">
            <h3 class="text-lg font-semibold">
                Asset Information
            </h3>
        </div>
        <div class="grid grid-cols-2 gap-4">
            <div>
                <label>Asset ID</label>
                <InputText
                    v-model="workOrderLineLocal.AssetID"
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter asset ID"
                />
                <label>Asset Classification</label>
                <InputText
                    v-model="workOrderLineLocal.AssetClassification"
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter asset classification"
                />
            </div>
            <div>
                <label>Asset Description</label>
                <Textarea
                    v-model="workOrderLineLocal.AssetDescription"
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter asset description"
                    rows="4"
                    auto-resize
                />
            </div>
            <div>
                <label>Ring</label>
                <InputText
                    v-model="workOrderLineLocal.Ring"
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter Ring info"
                />
            </div>
            <div>
                <label>TX Circuit Name</label>
                <InputText
                    v-model="workOrderLineLocal.TXCircuitName"
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter TX circuit name"
                />
            </div>
            <div>
                <label>Substation Name</label>
                <InputText
                    v-model="workOrderLineLocal.SubstationName"
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter Substation Name"
                />
            </div>
            <div>
                <label>Feeder Name</label>
                <InputText
                    v-model="workOrderLineLocal.FeederName"
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter Feeder name"
                />
            </div>
            <div>
                <label>Zone</label>
                <InputText
                    v-model="workOrderLineLocal.Zone"
                    type="text"
                    class="w-full p-2"
                />
            </div>
            <div class="grid grid-cols-2 gap-4">
                <div>
                    <label>Segment From</label>
                    <InputText
                        v-model="workOrderLineLocal.SegmentFrom"
                        type="text"
                        class="w-full p-2"
                    />
                </div>
                <div>
                    <label>Segment To</label>
                    <InputText
                        v-model="workOrderLineLocal.SegmentTo"
                        type="text"
                        class="w-full p-2"
                    />
                </div>
            </div>
            <div>
                <label>Street Name/Label</label>
                <InputText
                    v-model="workOrderLineLocal.StreetName"
                    type="text"
                    class="w-full p-2"
                />
            </div>
            <div />
            <div>
                <label>Remarks</label>
                <InputText
                    v-model="workOrderLineLocal.Remarks"
                    type="text"
                    class="w-full p-2"
                />
            </div>
        </div>

        <div class="mt-6">
            <h2 class="text-lg font-semibold mb-2 dark:text-white">
                Work Order Plan Priority
            </h2>
            <div class="grid grid-cols-3 gap-4 mb-4">
                <div>
                    <label for="">Job Plan ID</label>
                    <InputGroup class="mb-5">
                        <InputText
                            v-model="workOrderLineLocal.JobPlanID"
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
                        v-model="workOrderLineLocal.JobPlanDescription"
                        type="text"
                        class="w-full p-2"
                    />
                </div>
            </div>
            <label>Summary of Work Planned</label>
            <Textarea
                v-model="workOrderLineLocal.WorkPlanSummary"
                type="text"
                class="w-full p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                placeholder="Enter asset description"
                rows="3"
                auto-resize
            />
        </div>

        <div class="mt-4 flex justify-end">
            <Button @click="addLine">
                <Icon
                    name="material-symbols:save"
                    size="1.5rem"
                />
                Save
            </Button>
        </div>
    </div>
</template>

<style>

</style>
