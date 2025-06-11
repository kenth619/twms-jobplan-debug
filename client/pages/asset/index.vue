<template>
  <div>
    <!-- Loading State -->
    <div v-if="isLoading" class="flex justify-center items-center p-8">
      <div class="flex items-center space-x-2">
        <Icon name="material-symbols:progress-activity" class="animate-spin" size="1.5rem" />
        <span>Loading assets...</span>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="hasError"
      class="bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-md p-4 mb-5">
      <div class="flex items-center space-x-2 text-red-600 dark:text-red-400">
        <Icon name="material-symbols:error-outline" size="1.5rem" />
        <span class="font-medium">Error loading assets</span>
      </div>
      <p class="mt-2 text-sm text-red-600 dark:text-red-400">{{ error }}</p>
      <Button @click="refreshData" size="small" severity="secondary" class="mt-3" :loading="isLoading">
        <Icon name="material-symbols:refresh" size="1rem" class="mr-1" />
        Retry
      </Button>
    </div>

    <!-- Main Content -->
    <div v-else class="space-y-5">
      <!-- Stats Cards -->
      <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-5">
        <div class="bg-white dark:bg-neutral-800 rounded-md border border-blue-200 dark:border-blue-800 p-4 shadow-sm">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-sm font-medium text-gray-600 dark:text-gray-400">Total Assets</p>
              <p class="text-2xl font-bold text-blue-600 dark:text-blue-400">{{ assetStats.total }}</p>
            </div>
            <Icon name="material-symbols:database-outline" size="2rem" class="text-blue-500" />
          </div>
        </div>
        <div class="bg-white dark:bg-neutral-800 rounded-md border border-green-200 dark:border-green-800 p-4 shadow-sm">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-sm font-medium text-gray-600 dark:text-gray-400">Active</p>
              <p class="text-2xl font-bold text-green-600 dark:text-green-400">{{ assetStats.active }}</p>
            </div>
            <Icon name="material-symbols:check-circle-outline" size="2rem" class="text-green-500" />
          </div>
        </div>
        <div class="bg-white dark:bg-neutral-800 rounded-md border border-red-200 dark:border-red-800 p-4 shadow-sm">
          <div class="flex items-center justify-between">
            <div>
              <p class="text-sm font-medium text-gray-600 dark:text-gray-400">Inactive</p>
              <p class="text-2xl font-bold text-red-600 dark:text-red-400">{{ assetStats.inactive }}</p>
            </div>
            <Icon name="material-symbols:cancel-outline" size="2rem" class="text-red-500" />
          </div>
        </div>
      </div>

      <!-- Header with Actions -->
      <div class="bg-white dark:bg-neutral-800 rounded-md border border-green-500 dark:border-green-800 p-4 shadow-xl">
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-xl font-semibold">
            Asset List
            <span class="text-sm font-normal text-gray-500 dark:text-gray-400 ml-2">
              ({{ assets.length }} total)
            </span>
          </h2>
          <div class="flex space-x-2">
            <!-- Add New Asset Button -->
            <Button @click="addAsset" size="small" class="bg-green-600 hover:bg-green-700">
              <Icon name="material-symbols:add" size="1rem" class="mr-1" />
              Add New Asset
            </Button>
            <!-- Export Button -->
            <Button @click="exportAssets" size="small" severity="secondary">
              <Icon name="material-symbols:download" size="1rem" class="mr-1" />
              Export
            </Button>
            <!-- Refresh Button -->
            <Button @click="refreshData" size="small" severity="secondary" :loading="isLoading">
              <Icon name="material-symbols:refresh" size="1rem" />
            </Button>
          </div>
        </div>
        <!-- Filters -->
        <div class="grid grid-cols-1 md:grid-cols-4 gap-4 mb-4">
          <div class="relative">
            <Icon name="material-symbols:search"
              class="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" size="1rem" />
            <InputText v-model="searchTerm" placeholder="    Search assets..." class="pl-10 w-full" />
            <button v-if="searchTerm"
              class="absolute right-3 top-1/2 transform -translate-y-1/2 bg-gray-200 dark:bg-gray-700 hover:bg-gray-300 dark:hover:bg-gray-600 rounded-full p-1"
              @click="clearSearch">
              <Icon name="material-symbols:close" size="1rem" class="text-gray-600 dark:text-gray-400" />
            </button>
          </div>
          <Select v-model="selectedAssetClass" :options="assetClassOptions" option-label="name" option-value="id"
            placeholder="Filter by Asset Class" show-clear class="w-full" />
          <Select v-model="selectedStatus" :options="statusOptions" option-label="name" option-value="value"
            placeholder="Filter by Status" show-clear class="w-full" />
          <Button v-if="hasFiltersApplied" @click="clearAllFilters" size="small" severity="secondary" class="w-full">
            <Icon name="material-symbols:filter-alt-off" size="1rem" class="mr-1" />
            Clear Filters
          </Button>
        </div>
        <!-- Data Table -->
        <DataTable :value="filteredAssets" class="w-full" striped-rows responsive-layout="scroll">
          <!-- Asset ID -->
          <Column field="assetId" header="Asset ID" sortable style="width: 80px">
            <template #body="{ data }">
              <span class="font-mono text-sm">{{ data.assetId }}</span>
            </template>
          </Column>
          <!-- Classification -->
          <Column field="assetClassification" header="Classification" sortable style="width: 150px" />
          <!-- Serial Number -->
          <Column field="serialNumber" header="Serial Number" sortable />
          <!-- Manufacturer -->
          <Column field="manufacturer" header="Manufacturer" sortable />
          <!-- Status -->
          <Column field="status" header="Status" sortable style="width: 120px">
            <template #body="{ data }">
              <Tag :value="data.status" :severity="data.status === 'Active' ? 'success' : 'danger'" class="text-xs" />
            </template>
          </Column>
          <!-- Actions -->
          <Column header="Actions" style="width: 140px">
            <template #body="{ data }">
              <div class="flex space-x-1">
                <Button @click="editAsset(data)" size="small" severity="secondary" v-tooltip="'Edit Asset'">
                  <Icon name="material-symbols:edit-square-outline" size="1rem" />
                </Button>
                <Button @click="deleteAsset(data)" severity="danger" size="small" v-tooltip="'Delete Asset'">
                  <Icon name="material-symbols:delete-outline" size="1rem" />
                </Button>
              </div>
            </template>
          </Column>
        </DataTable>
      </div>
    </div>
    <Toast />
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import Button from 'primevue/button'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import Tag from 'primevue/tag'
import InputText from 'primevue/inputtext'
import Select from 'primevue/select'
import Toast from 'primevue/toast'
import { useToast } from 'primevue/usetoast'

// Dummy data
const getDummyAssets = () => [
  {
    assetId: 1001,
    assetClassification: 'Transformer',
    serialNumber: 'TRX-12345',
    manufacturer: 'Siemens',
    status: 'Active'
  },
  {
    assetId: 1002,
    assetClassification: 'Feeder',
    serialNumber: 'FDR-54321',
    manufacturer: 'GE',
    status: 'Inactive'
  },
  {
    assetId: 1003,
    assetClassification: 'Switchgear',
    serialNumber: 'SWG-78901',
    manufacturer: 'ABB',
    status: 'Active'
  },
  {
    assetId: 1004,
    assetClassification: 'Breaker',
    serialNumber: 'BRK-56789',
    manufacturer: 'Schneider',
    status: 'Inactive'
  },
  {
    assetId: 1005,
    assetClassification: 'Capacitor Bank',
    serialNumber: 'CAP-24680',
    manufacturer: 'Eaton',
    status: 'Active'
  }
]

// State
const isLoading = ref(false)
const hasError = ref(false)
const error = ref('')
const assets = ref(getDummyAssets())
const searchTerm = ref('')
const selectedAssetClass = ref(null)
const selectedStatus = ref(null)

const assetClassOptions = [
  { id: null, name: 'All Asset Classes' },
  { id: 'Transformer', name: 'Transformer' },
  { id: 'Feeder', name: 'Feeder' },
  { id: 'Switchgear', name: 'Switchgear' },
  { id: 'Breaker', name: 'Breaker' },
  { id: 'Capacitor Bank', name: 'Capacitor Bank' }
]
const statusOptions = [
  { name: 'All Statuses', value: null },
  { name: 'Active', value: 'Active' },
  { name: 'Inactive', value: 'Inactive' }
]

// Stats
const assetStats = computed(() => {
  const total = assets.value.length
  const active = assets.value.filter(a => a.status === 'Active').length
  const inactive = assets.value.filter(a => a.status === 'Inactive').length
  return { total, active, inactive }
})

// Filtering
const hasFiltersApplied = computed(() =>
  !!searchTerm.value ||
  (selectedAssetClass.value !== null && selectedAssetClass.value !== undefined) ||
  (selectedStatus.value !== null && selectedStatus.value !== undefined)
)

const filteredAssets = computed(() => {
  let data = assets.value
  if (searchTerm.value) {
    const search = searchTerm.value.toLowerCase()
    data = data.filter(
      a =>
        String(a.assetId).includes(search) ||
        a.assetClassification.toLowerCase().includes(search) ||
        a.serialNumber.toLowerCase().includes(search) ||
        a.manufacturer.toLowerCase().includes(search)
    )
  }
  if (selectedAssetClass.value) {
    data = data.filter(a => a.assetClassification === selectedAssetClass.value)
  }
  if (selectedStatus.value) {
    data = data.filter(a => a.status === selectedStatus.value)
  }
  return data
})

// Actions
const toast = useToast()

function addAsset() {
  // Implement navigation to asset add page or modal
  //toast.add({ severity: 'info', summary: 'Add Asset', detail: 'Implement navigation to Add Asset', life: 2500 })
  navigateTo('/asset/AddAssetForm')
}

function editAsset(rowData: any) {
  toast.add({ severity: 'info', summary: 'Edit Asset', detail: `Implement edit for asset ${rowData.assetId}`, life: 2500 })
}

function deleteAsset(rowData: any) {
  if (confirm(`Delete asset ${rowData.assetId}? (Implement Later)`)) {
    // Implement delete logic
    toast.add({ severity: 'warn', summary: 'Delete Asset', detail: `Implement delete for asset ${rowData.assetId}`, life: 2500 })
  }
}

function refreshData() {
  isLoading.value = true
  setTimeout(() => {
    assets.value = getDummyAssets()
    isLoading.value = false
    hasError.value = false
  }, 800)
}

function exportAssets() {
  toast.add({ severity: 'info', summary: 'Export', detail: 'Export functionality will be implemented', life: 2500 })
}

function clearSearch() {
  searchTerm.value = ''
}

function clearAllFilters() {
  searchTerm.value = ''
  selectedAssetClass.value = null
  selectedStatus.value = null
}
</script>
