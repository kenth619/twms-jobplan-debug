<template>
  <div>
    <!-- Loading State -->
    <div v-if="isLoading" class="flex justify-center items-center p-8">
      <div class="flex items-center space-x-2">
        <Icon name="material-symbols:progress-activity" class="animate-spin" size="1.5rem" />
        <span>Loading specifications...</span>
      </div>
    </div>

    <!-- Error State -->
    <div v-else-if="hasError"
      class="bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-md p-4 mb-5">
      <div class="flex items-center space-x-2 text-red-600 dark:text-red-400">
        <Icon name="material-symbols:error-outline" size="1.5rem" />
        <span class="font-medium">Error loading specifications</span>
      </div>
      <p class="mt-2 text-sm text-red-600 dark:text-red-400">{{ error }}</p>
      <Button @click="refreshData" size="small" severity="secondary" class="mt-3" :loading="isLoading">
        <Icon name="material-symbols:refresh" size="1rem" class="mr-1" />
        Retry
      </Button>
    </div>

    <!-- Main Content -->
    <div v-else class="space-y-5">
      <!-- Header with Actions -->
      <div class="bg-white dark:bg-neutral-800 rounded-md border border-green-500 dark:border-green-800 p-4 shadow-xl">
        <div class="flex justify-between items-center mb-4">
          <h2 class="text-xl font-semibold">
            Technical Specifications
            <span class="text-sm font-normal text-gray-500 dark:text-gray-400 ml-2">
              ({{ specs.length }} total)
            </span>
          </h2>
          <div class="flex space-x-2">
            <Button @click="addSpec" size="small" class="bg-green-600 hover:bg-green-700">
              <Icon name="material-symbols:add" size="1rem" class="mr-1" />
              Add New Spec
            </Button>
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
            <InputText v-model="searchTerm" placeholder="    Search specifications..." class="pl-10 w-full" />
            <button v-if="searchTerm"
              class="absolute right-3 top-1/2 transform -translate-y-1/2 bg-gray-200 dark:bg-gray-700 hover:bg-gray-300 dark:hover:bg-gray-600 rounded-full p-1"
              @click="clearSearch">
              <Icon name="material-symbols:close" size="1rem" class="text-gray-600 dark:text-gray-400" />
            </button>
          </div>
          <div class="md:col-span-2"></div>
        </div>
        <!-- Data Table -->
        <DataTable :value="filteredSpecs" class="w-full" striped-rows responsive-layout="scroll">
          <Column field="attribute" header="Attribute" sortable />
          <Column field="description" header="Description" sortable />
          <Column field="nominalValue" header="Nominal Value" sortable />
          <Column field="lastUpdatedValue" header="Last Updated Value" sortable />
          <Column field="dateLastUpdated" header="Date Last Updated" sortable />
          <!-- Actions -->
          <Column header="Actions" style="width: 140px">
            <template #body="{ data }">
              <div class="flex space-x-1">
                <Button @click="editSpec(data)" size="small" severity="secondary" v-tooltip="'Edit Spec'">
                  <Icon name="material-symbols:edit-square-outline" size="1rem" />
                </Button>
                <Button @click="deleteSpec(data)" severity="danger" size="small" v-tooltip="'Delete Spec'">
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
import InputText from 'primevue/inputtext'
import Toast from 'primevue/toast'
import { useToast } from 'primevue/usetoast'

// Dummy technical specs data
const getDummySpecs = () => [
  {
    id: 1,
    attribute: 'Voltage',
    description: 'Rated voltage of equipment',
    nominalValue: '11kV',
    lastUpdatedValue: '11kV',
    dateLastUpdated: '2024-06-01'
  },
  {
    id: 2,
    attribute: 'Current',
    description: 'Rated current capacity',
    nominalValue: '630A',
    lastUpdatedValue: '600A',
    dateLastUpdated: '2024-05-15'
  },
  {
    id: 3,
    attribute: 'Frequency',
    description: 'Operating frequency',
    nominalValue: '50Hz',
    lastUpdatedValue: '50Hz',
    dateLastUpdated: '2024-06-07'
  },
  {
    id: 4,
    attribute: 'Phase',
    description: 'Number of phases',
    nominalValue: '3',
    lastUpdatedValue: '3',
    dateLastUpdated: '2024-06-01'
  },
  {
    id: 5,
    attribute: 'Power',
    description: 'Rated power output',
    nominalValue: '500kVA',
    lastUpdatedValue: '480kVA',
    dateLastUpdated: '2024-05-30'
  }
]

const isLoading = ref(false)
const hasError = ref(false)
const error = ref('')
const specs = ref(getDummySpecs())
const searchTerm = ref('')
const dateFilter = ref('')

const specStats = computed(() => {
  const total = specs.value.length
  const recent = specs.value.filter(s => {
    const days = (new Date().getTime() - new Date(s.dateLastUpdated).getTime()) / (1000 * 3600 * 24)
    return days <= 30
  }).length
  const outdated = total - recent
  return { total, recent, outdated }
})

const filteredSpecs = computed(() => {
  let data = specs.value
  if (searchTerm.value) {
    const search = searchTerm.value.toLowerCase()
    data = data.filter(
      s =>
        s.attribute.toLowerCase().includes(search) ||
        s.description.toLowerCase().includes(search) ||
        s.nominalValue.toLowerCase().includes(search) ||
        s.lastUpdatedValue.toLowerCase().includes(search)
    )
  }
  if (dateFilter.value) {
    data = data.filter(s => s.dateLastUpdated === dateFilter.value)
  }
  return data
})

const toast = useToast()

function addSpec() {
  toast.add({ severity: 'info', summary: 'Add Spec', detail: 'Implement add spec modal or navigation', life: 2500 })
}
function editSpec(rowData: any) {
  toast.add({ severity: 'info', summary: 'Edit Spec', detail: `Implement edit for ${rowData.attribute}`, life: 2500 })
}
function deleteSpec(rowData: any) {
  if (confirm(`Delete specification "${rowData.attribute}"? (Implement Later)`)) {
    toast.add({ severity: 'warn', summary: 'Delete Spec', detail: `Implement delete for ${rowData.attribute}`, life: 2500 })
  }
}
function refreshData() {
  isLoading.value = true
  setTimeout(() => {
    specs.value = getDummySpecs()
    isLoading.value = false
    hasError.value = false
  }, 800)
}
function clearSearch() {
  searchTerm.value = ''
}
</script>
