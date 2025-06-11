<template>
    <div>
        <!-- Loading State -->
        <div v-if="isLoading" class="flex justify-center items-center p-8">
            <div class="flex items-center space-x-2">
                <Icon name="material-symbols:progress-activity" class="animate-spin" size="1.5rem" />
                <span>Loading job plans...</span>
            </div>
        </div>

        <!-- Error State -->
        <div v-else-if="hasError"
            class="bg-red-50 dark:bg-red-900/20 border border-red-200 dark:border-red-800 rounded-md p-4 mb-5">
            <div class="flex items-center space-x-2 text-red-600 dark:text-red-400">
                <Icon name="material-symbols:error-outline" size="1.5rem" />
                <span class="font-medium">Error loading job plans</span>
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
            <div v-if="hasStats" class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-5">
                <div
                    class="bg-white dark:bg-neutral-800 rounded-md border border-blue-200 dark:border-blue-800 p-4 shadow-sm">
                    <div class="flex items-center justify-between">
                        <div>
                            <p class="text-sm font-medium text-gray-600 dark:text-gray-400">Total Job Plans</p>
                            <p class="text-2xl font-bold text-blue-600 dark:text-blue-400">{{
                                jobPlanStats?.totalJobPlans || 0 }}</p>
                        </div>
                        <Icon name="material-symbols:work-outline" size="2rem" class="text-blue-500" />
                    </div>
                </div>

                <div
                    class="bg-white dark:bg-neutral-800 rounded-md border border-green-200 dark:border-green-800 p-4 shadow-sm">
                    <div class="flex items-center justify-between">
                        <div>
                            <p class="text-sm font-medium text-gray-600 dark:text-gray-400">Active</p>
                            <p class="text-2xl font-bold text-green-600 dark:text-green-400">{{
                                jobPlanStats?.activeJobPlans || 0 }}</p>
                        </div>
                        <Icon name="material-symbols:check-circle-outline" size="2rem" class="text-green-500" />
                    </div>
                </div>

                <div
                    class="bg-white dark:bg-neutral-800 rounded-md border border-red-200 dark:border-red-800 p-4 shadow-sm">
                    <div class="flex items-center justify-between">
                        <div>
                            <p class="text-sm font-medium text-gray-600 dark:text-gray-400">Inactive</p>
                            <p class="text-2xl font-bold text-red-600 dark:text-red-400">{{
                                jobPlanStats?.inactiveJobPlans || 0 }}</p>
                        </div>
                        <Icon name="material-symbols:cancel-outline" size="2rem" class="text-red-500" />
                    </div>
                </div>
            </div>

            <!-- Header with Actions -->
            <div
                class="bg-white dark:bg-neutral-800 rounded-md border border-green-500 dark:border-green-800 p-4 shadow-xl">
                <div class="flex justify-between items-center mb-4">
                    <h2 class="text-xl font-semibold">
                        Job Plan List
                        <span v-if="hasPagedData" class="text-sm font-normal text-gray-500 dark:text-gray-400 ml-2">
                            ({{ pageInfo.total }} total)
                        </span>
                    </h2>

                    <div class="flex space-x-2">
                        <!-- Add New Job Plan Button -->
                        <Button @click="addNewJobplan" size="small" class="bg-green-600 hover:bg-green-700">
                            <Icon name="material-symbols:add" size="1rem" class="mr-1" />
                            Add New Job Plan
                        </Button>

                        <!-- Bulk Actions -->
                        <div v-if="selectedJobPlans.length > 0" class="flex space-x-2">
                            <!-- Bulk Status Toggle -->
                            <Button @click="confirmBulkStatusToggle" severity="warning" size="small"
                                :loading="isUpdating">
                                <Icon name="material-symbols:toggle-on" size="1rem" class="mr-1" />
                                Toggle Status ({{ selectedJobPlans.length }})
                            </Button>

                            <!-- Bulk Delete Button -->
                            <Button @click="confirmBulkDelete" severity="danger" size="small" :loading="isDeleting">
                                <Icon name="material-symbols:delete-outline" size="1rem" class="mr-1" />
                                Delete Selected ({{ selectedJobPlans.length }})
                            </Button>
                        </div>

                        <!-- Export Button -->
                        <Button @click="handleExport" size="small" severity="secondary" :loading="isExporting">
                            <Icon name="material-symbols:download" size="1rem" class="mr-1" />
                            Export
                        </Button>

                        <!-- Refresh Button -->
                        <Button @click="refreshData" size="small" severity="secondary" :loading="isAnyLoading">
                            <Icon name="material-symbols:refresh" size="1rem" />
                        </Button>
                    </div>
                </div>

                <!-- Filters -->
                <div class="grid grid-cols-1 md:grid-cols-4 gap-4 mb-4">
                    <!-- Search Input with Clear Button -->
                    <div class="relative">
                        <!-- Search Icon -->
                        <Icon name="material-symbols:search"
                            class="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400" size="1rem" />

                        <!-- Search Input -->
                        <InputText v-model="searchTerm" placeholder="    Search job plans..." class="pl-10 w-full"
                            @input="handleSearchInput" />

                        <!-- Clear Button -->
                        <button v-if="searchTerm"
                            class="absolute right-3 top-1/2 transform -translate-y-1/2 bg-gray-200 dark:bg-gray-700 hover:bg-gray-300 dark:hover:bg-gray-600 rounded-full p-1"
                            @click="clearSearch">
                            <Icon name="material-symbols:close" size="1rem" class="text-gray-600 dark:text-gray-400" />
                        </button>
                    </div>


                    <!-- Asset Class Filter -->
                    <Select v-model="selectedAssetClass" :options="assetClassOptions" option-label="name"
                        option-value="id" placeholder="Filter by Asset Class" show-clear
                        @change="handleAssetClassFilter" class="w-full" />

                    <!-- Status Filter -->
                    <Select v-model="selectedStatus" :options="statusOptions" option-label="name" option-value="value"
                        placeholder="Filter by Status" show-clear @change="handleStatusFilter" class="w-full" />

                    <!-- Clear Filters -->
                    <Button v-if="hasFiltersApplied" @click="clearAllFilters" size="small" severity="secondary"
                        class="w-full">
                        <Icon name="material-symbols:filter-alt-off" size="1rem" class="mr-1" />
                        Clear Filters
                    </Button>
                </div>

                <!-- Data Table -->
                <DataTable :value="displayJobPlans" v-model:selection="selectedJobPlans" selection-mode="multiple"
                    :meta-key-selection="false" class="w-full" :loading="isAnyLoading" striped-rows
                    responsive-layout="scroll" sort-field="dateCreated" :sort-order="-1">
                    <template #empty>
                        <div class="text-center py-8">
                            <Icon name="material-symbols:work-outline" size="3rem" class="text-gray-400 mb-2" />
                            <p class="text-gray-500 dark:text-gray-400">
                                {{ hasFiltersApplied ? 'No job plans match your filters' : 'No job plans added yet' }}
                            </p>
                            <Button v-if="!hasFiltersApplied" @click="addNewJobplan" size="small"
                                class="mt-3 bg-green-600 hover:bg-green-700">
                                <Icon name="material-symbols:add" size="1rem" class="mr-1" />
                                Add First Job Plan
                            </Button>
                        </div>
                    </template>

                    <template #loading>
                        <div class="flex justify-center items-center py-8">
                            <Icon name="material-symbols:progress-activity" class="animate-spin" size="2rem" />
                        </div>
                    </template>

                    <!-- Selection Column -->
                    <Column selection-mode="multiple" header-style="width: 3rem" />

                    <!-- Job Plan ID -->
                    <Column field="jobPlanId" header="ID" sortable style="width: 80px">
                        <template #body="{ data }">
                            <span class="font-mono text-sm">{{ data.jobPlanId }}</span>
                        </template>
                    </Column>

                    <!-- Short Description -->
                    <Column field="jobPlanShortDesc" header="Short Description" sortable>
                        <template #body="{ data }">
                            <div class="max-w-xs">
                                <p class="font-medium truncate" :title="data.jobPlanShortDesc">{{ data.jobPlanShortDesc
                                }}</p>
                                <p class="text-xs text-gray-500 mt-1 truncate" :title="data.jobPlanLongDesc">
                                    {{ data.jobPlanLongDesc }}
                                </p>
                            </div>
                        </template>
                    </Column>

                    <!-- Asset Class -->
                    <Column field="assetClassShortDesc" header="Asset Class" sortable style="width: 150px">
                        <template #body="{ data }">
                            <div class="flex items-center space-x-2">
                                <Icon name="material-symbols:category-outline" size="1rem" class="text-blue-500" />
                                <span>{{ data.assetClassShortDesc }}</span>
                            </div>
                        </template>
                    </Column>

                    <!-- Status -->
                    <Column field="jobPlanStatus" header="Status" sortable style="width: 120px">
                        <template #body="{ data }">
                            <Tag :value="data.jobPlanStatus ? 'Active' : 'Inactive'"
                                :severity="data.jobPlanStatus ? 'success' : 'danger'" class="text-xs" />
                        </template>
                    </Column>

                    <!-- Created Date -->
                    <Column field="dateCreated" header="Created" sortable style="width: 120px">
                        <template #body="{ data }">
                            <div class="flex flex-col">
                                <span class="text-sm text-gray-600 dark:text-gray-400">
                                    {{ formatDate(data.dateCreated) }}
                                </span>
                                <span class="text-xs text-gray-500 dark:text-gray-400">
                                    {{ formatTime(data.dateCreated) }}
                                </span>
                            </div>
                        </template>
                    </Column>

                    <!-- Actions -->
                    <Column header="Actions" style="width: 140px">
                        <template #body="{ data }">
                            <div class="flex space-x-1">
                                <!-- View/Edit Button -->
                                <Button @click="editJobPlan(data)" size="small" severity="secondary"
                                    :loading="isUpdating" v-tooltip="'Edit Job Plan'">
                                    <Icon name="material-symbols:edit-square-outline" size="1rem" />
                                </Button>

                                <!-- Delete Button -->
                                <Button @click="confirmDelete(data)" severity="danger" size="small"
                                    :loading="isDeleting" v-tooltip="'Delete Job Plan'">
                                    <Icon name="material-symbols:delete-outline" size="1rem" />
                                </Button>
                            </div>
                        </template>
                    </Column>
                </DataTable>

                <!-- Pagination -->
                <div v-if="hasPagedData && totalPages > 1" class="flex justify-between items-center mt-4">
                    <div class="text-sm text-gray-600 dark:text-gray-400">
                        Showing {{ pageInfo.from }} to {{ pageInfo.to }} of {{ pageInfo.total }} entries
                    </div>

                    <div class="flex items-center space-x-2">
                        <!-- Page Size Selector -->
                        <div class="flex items-center space-x-2">
                            <span class="text-sm text-gray-600 dark:text-gray-400">Show:</span>
                            <Select :model-value="pageSize" :options="pageSizeOptions"
                                @update:model-value="handlePageSizeChange" class="w-20" />
                        </div>

                        <!-- Pagination Controls -->
                        <Paginator :first="(currentPage - 1) * pageSize" :rows="pageSize" :total-records="totalCount"
                            @page="onPageChange" :rows-per-page-options="[10, 5, 15, 20, 50]"
                            template="FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Confirmation Dialog -->
        <ConfirmDialog />

        <!-- Toast for notifications -->
        <Toast />
    </div>
</template>

<script lang="ts" setup>
import { useModals } from '@outloud/vue-modals'
import { useConfirm } from 'primevue/useconfirm'
import { useToast } from 'primevue/usetoast'
import type { JobPlansHeader } from '~/services/jobPlansAPIService'

// Composables
const modals = useModals()
const confirm = useConfirm()
const toast = useToast()

// Job Plan composable
const {
    // Data
    pagedJobPlans,
    jobPlanStats,

    // Loading states
    isLoading,
    isUpdating,
    isDeleting,
    isAnyLoading,

    // Error states
    hasError,
    error,

    // Pagination
    currentPage,
    pageSize,
    totalCount,
    totalPages,
    pageInfo,
    hasPagedData,

    // Computed
    hasStats,

    // Methods
    fetchJobPlans,
    deleteJobPlan,
    bulkDeleteJobPlans,
    bulkUpdateJobPlanStatus,
    updateJobPlanStatus,
    searchJobPlansWithPagination,
    filterByAssetClass,
    filterByStatus,
    applyFilters,
    changePageSize,
    goToPage,
    clearFilters,
    refreshAll,
    fetchJobPlanStats
} = useJobPlan({ autoFetch: false })

// Local reactive state
const selectedJobPlans = ref<JobPlansHeader[]>([])
const searchTerm = ref('')
const selectedAssetClass = ref<number | null>(null)
const selectedStatus = ref<boolean | null>(null)
const isExporting = ref(false)


// Mock asset class options (replace with actual data when available)
const assetClassOptions = computed(() => [
    { id: null, name: 'All Asset Classes' },
    { id: 1, name: 'Electrical' },
    { id: 2, name: 'Mechanical' },
    { id: 3, name: 'HVAC' },
    { id: 4, name: 'Plumbing' }
])

const statusOptions = computed(() => [
    { name: 'All Statuses', value: null },
    { name: 'Active', value: true },
    { name: 'Inactive', value: false }
])

// Computed properties
const displayJobPlans = computed(() => pagedJobPlans.value?.data || [])

const hasFiltersApplied = computed(() =>
    searchTerm.value || selectedAssetClass.value !== null || selectedStatus.value !== null
)

const pageSizeOptions = [10, 5, 15, 20, 50]

// Updated debounced search function
const debouncedSearch = useDebounceFn(async () => {
    const trimmedSearch = searchTerm.value.trim()
    
    // Always reset to page 1 when search changes
    currentPage.value = 1
    
    if (trimmedSearch) {
        // Search with current filters
        await searchJobPlansWithPagination(
            trimmedSearch,
            1,
            pageSize.value,
            selectedAssetClass.value || undefined,
            selectedStatus.value ?? undefined
        )
    } else {
        // When search is empty, clear search filters and show appropriate data
        clearFilters() // This clears the search from backend/composable
        
        if (selectedAssetClass.value !== null || selectedStatus.value !== null) {
            // Apply only non-search filters
            await applyFilters({
                assetClassId: selectedAssetClass.value || undefined,
                status: selectedStatus.value ?? undefined
            })
        } else {
            // No filters at all, fetch all data
            await fetchJobPlans()
        }
    }
}, 300)

// Enhanced handle search input
const handleSearchInput = async () => {
    await debouncedSearch()
}

// Watch for search term changes to handle all edge cases
watch(searchTerm, async (newValue, oldValue) => {
    // Trigger search when value changes
    await debouncedSearch()
}, { 
    immediate: false,
    flush: 'post' // Ensure DOM updates are complete
})

// Keep your existing clearSearch method
const clearSearch = async () => {
    searchTerm.value = '' // Clear the search term
}


const clearAllFilters = async () => {
    searchTerm.value = ''
    selectedAssetClass.value = null
    selectedStatus.value = null
    
    // Clear filters and reset pagination
    clearFilters()
    await goToPage(1)
    await fetchJobPlans()
}


// Methods
const refreshData = async () => {
    selectedJobPlans.value = []
    await refreshAll()
}

const addNewJobplan = () => {
    navigateTo('/jobplan/addNewJobplan')
}

const editJobPlan = (jobPlan: JobPlansHeader) => {
    navigateTo(`/jobplan/edit/${jobPlan.jobPlanId}`, {
        state: {
            jobPlan,
            mode: 'edit'  // Add mode flag
        }
    })
}

const confirmDelete = (jobPlan: JobPlansHeader) => {
    confirm.require({
        message: `Are you sure you want to delete job plan "${jobPlan.jobPlanShortDesc}"?`,
        header: 'Confirm Deletion',
        icon: 'pi pi-exclamation-triangle',
        rejectClass: 'p-button-secondary p-button-outlined',
        acceptClass: 'p-button-danger',
        accept: () => handleDelete(jobPlan.jobPlanId)
    })
}

const confirmBulkDelete = () => {
    const count = selectedJobPlans.value.length
    confirm.require({
        message: `Are you sure you want to delete ${count} selected job plan${count > 1 ? 's' : ''}?`,
        header: 'Confirm Bulk Deletion',
        icon: 'pi pi-exclamation-triangle',
        rejectClass: 'p-button-secondary p-button-outlined',
        acceptClass: 'p-button-danger',
        accept: () => handleBulkDelete()
    })
}

const confirmBulkStatusToggle = () => {
    const count = selectedJobPlans.value.length
    const activeCount = selectedJobPlans.value.filter(jp => jp.jobPlanStatus).length
    const inactiveCount = count - activeCount

    confirm.require({
        message: `Toggle status for ${count} selected job plan${count > 1 ? 's' : ''}? This will activate ${inactiveCount} and deactivate ${activeCount} job plans.`,
        header: 'Confirm Bulk Status Toggle',
        icon: 'pi pi-question-circle',
        rejectClass: 'p-button-secondary p-button-outlined',
        acceptClass: 'p-button-warning',
        accept: () => handleBulkStatusToggle()
    })
}

const handleDelete = async (jobPlanId: number) => {
    const success = await deleteJobPlan(jobPlanId)
    if (success) {
        selectedJobPlans.value = selectedJobPlans.value.filter(jp => jp.jobPlanId !== jobPlanId)
        toast.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Job plan deleted successfully',
            life: 3000
        })
    } else {
        toast.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to delete job plan',
            life: 5000
        })
    }
}

const handleBulkDelete = async () => {
    const ids = selectedJobPlans.value.map(jp => jp.jobPlanId)
    const success = await bulkDeleteJobPlans(ids)
    if (success) {
        selectedJobPlans.value = []
        toast.add({
            severity: 'success',
            summary: 'Success',
            detail: `${ids.length} job plan(s) deleted successfully`,
            life: 3000
        })
    } else {
        toast.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to delete job plans',
            life: 5000
        })
    }
}

const handleBulkStatusToggle = async () => {
    // Group by current status and toggle each group
    const activeIds = selectedJobPlans.value.filter(jp => jp.jobPlanStatus).map(jp => jp.jobPlanId)
    const inactiveIds = selectedJobPlans.value.filter(jp => !jp.jobPlanStatus).map(jp => jp.jobPlanId)

    const promises = []
    if (activeIds.length > 0) {
        promises.push(bulkUpdateJobPlanStatus(activeIds, false))
    }
    if (inactiveIds.length > 0) {
        promises.push(bulkUpdateJobPlanStatus(inactiveIds, true))
    }

    const results = await Promise.all(promises)
    const allSuccessful = results.every(result => result)

    if (allSuccessful) {
        selectedJobPlans.value = []
        toast.add({
            severity: 'success',
            summary: 'Success',
            detail: 'Job plan statuses updated successfully',
            life: 3000
        })
    } else {
        toast.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to update some job plan statuses',
            life: 5000
        })
    }
}

const handleAssetClassFilter = async () => {
    await applyFilters({
        assetClassId: selectedAssetClass.value || undefined,
        status: selectedStatus.value ?? undefined,
        searchTerm: searchTerm.value || undefined
    })
}

const handleStatusFilter = async () => {
    await applyFilters({
        assetClassId: selectedAssetClass.value || undefined,
        status: selectedStatus.value ?? undefined,
        searchTerm: searchTerm.value || undefined
    })
}

const handleExport = async () => {
    isExporting.value = true
    try {
        // TODO: Implement export functionality when available in the API service
        toast.add({
            severity: 'info',
            summary: 'Info',
            detail: 'Export functionality will be implemented',
            life: 3000
        })
    } catch (error) {
        toast.add({
            severity: 'error',
            summary: 'Error',
            detail: 'Failed to export job plans',
            life: 5000
        })
    } finally {
        isExporting.value = false
    }
}

const handlePageSizeChange = async (newPageSize: number) => {
    await changePageSize(newPageSize)
}

const onPageChange = async (event: any) => {
    const newPage = Math.floor(event.first / event.rows) + 1
    await goToPage(newPage)
}

// Utility functions
const formatDate = (dateString: string): string => {
    if (!dateString) return ''
    return new Date(dateString).toLocaleDateString()
}

const formatTime = (dateString: string): string => {
    if (!dateString) return ''
    return new Date(dateString).toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' })
}

// Lifecycle
onMounted(async () => {
    await refreshData()
})

// Cleanup
onUnmounted(() => {
    selectedJobPlans.value = []
    clearFilters()
})
</script>
