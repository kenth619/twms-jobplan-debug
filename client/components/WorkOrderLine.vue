<template>
    <div class="bg-white dark:bg-neutral-800 rounded-md border border-green-500 dark:border-green-800 p-4 mb-5 shadow-2xl">
        <div class="flex justify-end">
            <Button
                severity="danger"
                class="mb-4"
                @onlick="removeWOLine"
            >
                <Icon
                    name="material-symbols:delete-outline"
                    size="1.5rem"
                />
            </Button>
        </div>
        <div class="grid grid-cols-2 gap-4">
            <div>
                <label>Asset ID</label>
                <InputText
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter asset ID"
                />
                <label>Asset Classification</label>
                <InputText
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter asset classification"
                />
            </div>
            <div>
                <label>Asset Description</label>
                <Textarea
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
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter Ring info"
                />
            </div>
            <div>
                <label>TX Circuit Name</label>
                <InputText
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter TX circuit name"
                />
            </div>
            <div>
                <label>Substation Name</label>
                <InputText
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter Substation Name"
                />
            </div>
            <div>
                <label>Feeder Name</label>
                <InputText
                    type="text"
                    class="w-full p-2"
                    placeholder="Enter Feeder name"
                />
            </div>
            <div>
                <label>Zone</label>
                <InputText
                    type="text"
                    class="w-full p-2"
                />
            </div>
            <div class="grid grid-cols-2 gap-4">
                <div>
                    <label>Segment From</label>
                    <InputText
                        type="text"
                        class="w-full p-2"
                    />
                </div>
                <div>
                    <label>Segment To</label>
                    <InputText
                        type="text"
                        class="w-full p-2"
                    />
                </div>
            </div>
            <div>
                <label>Street Name/Label</label>
                <InputText
                    type="text"
                    class="w-full p-2"
                />
            </div>
            <div />
            <div>
                <label>Remarks</label>
                <InputText
                    type="text"
                    class="w-full p-2"
                />
            </div>
        </div>
    </div>
</template>

<script lang="ts" setup>
const removeWOLine = () => {
    if (this.counter.value > 1) {
        this.counter.value--
    }
}
</script>

<style>

</style>
