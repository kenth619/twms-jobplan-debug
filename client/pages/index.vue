<script lang="ts" setup>
import Column from 'primevue/column'
import DataTable from 'primevue/datatable'
import MyCard from '~/components/MyCard.vue'

const colorMode = useColorMode()

const toast = useToast()
const selectedWorksOrders = ref([])
const worksOrders = ref([
    { id: 1, description: 'Work Order 1', status: 'Pending' },
    { id: 2, description: 'Work Order 2', status: 'Approved' },
    { id: 3, description: 'Work Order 3', status: 'Pending' },
])

const repsponse = await $api('/api/employees/loggedinuser', {
    method: 'GET',
})

const approveSelected = async () => {
    if (selectedWorksOrders.value.length === 0) {
        toast.add({ severity: 'warn', summary: 'Warning', detail: 'Select work orders to be approved!', life: 3000 })
        return
    }

    const selectedIds = selectedWorksOrders.value.map(wo => wo.id)

    // Call the API to approve the selected works orders
    await $api('/api/workorder/approve', {
        method: 'POST',
        body: selectedIds,
    })
        .then(() => {
        // Update the store to remove the approved works orders
            // worksOrders.value = worksOrders.value.filter(
            //     wo => !selectedIds.includes(wo.id),
            // )
            // selectedWorksOrders.value = []
        })
        .catch((error) => {
            console.error('Error approving works orders:', error)
            toast.add({ severity: 'error', summary: 'Error', detail: `Error approving works orders: ${error.message}`, life: 3000 })
        })
}
</script>

<template>
    <div class="p-4 dark:bg-gray-900 rounded">
        <h1 class="text-2xl font-bold mb-4 dark:text-white">
            Welcome to TWMS, {{ repsponse }}!
        </h1>
        <p class="dark:text-gray-300 mb-3">
            Select an option from the menu to get started.
        </p>

        <!-- <div class="mt-6 p-4 bg-white dark:bg-gray-800 rounded shadow">
            <h2 class="text-xl font-semibold mb-2 dark:text-white">
                Current Color Mode
            </h2>
            <p class="dark:text-gray-300">
                Current mode: <span class="font-bold">{{ colorMode.value }}</span>
            </p>
            <p class="dark:text-gray-300">
                Preference: <span class="font-bold">{{ colorMode.preference }}</span>
            </p>

            <div class="mt-4">
                <select
                    v-model="colorMode.preference"
                    class="p-2 border rounded dark:bg-gray-700 dark:text-white dark:border-gray-600"
                >
                    <option value="system">
                        System
                    </option>
                    <option value="light">
                        Light
                    </option>
                    <option value="dark">
                        Dark
                    </option>
                </select>
            </div>
        </div> -->

        <div class="bg-white dark:bg-neutral-900 rounded-md border border-green-500 dark:border-green-700 p-4 shadow-2xl">
            <h2 class="text-xl font-semibold mb-2 mt-6 dark:text-white">
                Works Order Requiring Approval
            </h2>
            <DataTable
                v-model:selection="selectedWorksOrders"
                :value="worksOrders"
                :paginator="true"
                :rows="10"
                :show-gridlines="true"
                class="dark:bg-gray-800 dark:text-white"
            >
                <Column
                    selection-mode="multiple"
                    style="width: 3rem"
                />
                <Column
                    field="id"
                    header="ID"
                    sortable
                />
                <Column
                    field="description"
                    header="Description"
                    sortable
                />
                <Column
                    field="status"
                    header="Status"
                    sortable
                />
            </DataTable>
            <Button
                class="mt-4"
                severity="primary"
                @click="approveSelected"
            >
                <Icon
                    name="material-symbols:check"
                    size="1.5rem"
                    class="mr-2"
                />
                Approve Selected
            </Button>
        </div>

        <MyCard
            title="My Card Title"
            content="This is the content of my card. It can be anything you want."
        />

        <MyCard
            title="My Card Title"
            content="This is the content of my card. It can be anything you want."
        />

        <div class="mt-4 grid grid-cols-7 gap-4">
            <Button severity="primary">
                Primary
            </Button>
            <Button severity="secondary">
                Secondary
            </Button>
            <Button severity="warn">
                Warning
            </Button>
            <Button severity="success">
                Success
            </Button>
            <Button severity="info">
                Info
            </Button>
            <Button severity="help">
                Help
            </Button>
            <Button severity="danger">
                Danger
            </Button>
        </div>

        <div class="mt-4 grid grid-cols-7 gap-4">
            <Button
                severity="primary"
                variant="outlined"
            >
                Primary
            </Button>
            <Button
                severity="secondary"
                variant="outlined"
            >
                Secondary
            </Button>
            <Button
                severity="warn"
                variant="outlined"
            >
                Warning
            </Button>
            <Button
                severity="success"
                variant="outlined"
            >
                Success
            </Button>
            <Button
                severity="info"
                variant="outlined"
            >
                Info
            </Button>
            <Button
                severity="help"
                variant="outlined"
            >
                Help
            </Button>
            <Button
                severity="danger"
                variant="outlined"
            >
                Danger
            </Button>
        </div>

        <div class="mt-4">
            <ButtonGroup>
                <Button
                    severity="primary"
                    icon="pi pi-check"
                    label="Approve"
                />
                <Button
                    severity="danger"
                    icon="pi pi-times"
                    label="Reject"
                />
            </ButtonGroup>
        </div>
    </div>
</template>
