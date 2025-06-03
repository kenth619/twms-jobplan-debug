// composables/useJobPlan.ts
import { ref, computed, onMounted, readonly } from 'vue'
import type { 
  JobPlansHeader, 
  JobPlansLine,
  CreateJobPlanRequest, 
  UpdateJobPlanRequest,
  UpdateJobPlanStatusRequest,
  CreateJobPlanLineRequest,
  UpdateJobPlanLineRequest,
  JobPlanQueryParameters,
  JobPlanStatsResponse,
  AssetClassJobPlanCount,
} from '~/services/jobPlansAPIService'
import ApiResponse from '~/services/jobPlansAPIService'
import { jobPlansAPIService } from '~/services/jobPlansAPIService'

// Import PagedResponse from baseAPIService
import type { PagedResponse } from '~/services/baseAPIService'

export interface UseJobPlanOptions {
  autoFetch?: boolean
  initialPage?: number
  initialPageSize?: number
}

export interface JobPlanFilters {
  assetClassId?: number
  status?: boolean
  searchTerm?: string
}

export function createJobPlanRequest(
    data: JobPlanSummaryData,
    tasks: Task[]
): CreateJobPlanRequest {
    return {
        jobPlanId: data.jobPlanId!,
        assetClassId: data.assetClassId!,
        jobPlanShortDesc: data.jobPlanShortDesc.trim(),
        assetClassShortDesc: data.assetClassShortDesc,
        jobPlanLongDesc: data.jobPlanLongDesc?.trim() || '',
        jobPlanStatusId: data.jobPlanStatus ? 1 : 0,
        jobPlanLines: tasks.map(task => ({
            jobPlanLineNo: task.taskNo,
            jobPlanLineDesc: task.description.trim()
        }))
    };
}

export const useJobPlan = (options: UseJobPlanOptions = {}) => {
  // Reactive state
  const jobPlans = ref<JobPlansHeader[]>([])
  const currentJobPlan = ref<JobPlansHeader | null>(null)
  const pagedJobPlans = ref<PagedResponse<JobPlansHeader> | null>(null)
  const activeJobPlans = ref<JobPlansHeader[]>([])
  const inactiveJobPlans = ref<JobPlansHeader[]>([])
  const jobPlanStats = ref<JobPlanStatsResponse | null>(null)
  
  // Loading states
  const isLoading = ref(false)
  const isCreating = ref(false)
  const isUpdating = ref(false)
  const isDeleting = ref(false)
  const isFetchingStats = ref(false)
  
  // Error states
  const error = ref<string | null>(null)
  const validationErrors = ref<string[]>([])
  
  // Pagination
  const currentPage = ref(options.initialPage || 1)
  const pageSize = ref(options.initialPageSize || 5)
  const totalCount = ref(0)
  const totalPages = ref(0)
  
  // Filters
  const filters = ref<JobPlanFilters>({})

  // Computed properties
  const hasJobPlans = computed(() => jobPlans.value.length > 0)
  const hasPagedData = computed(() => pagedJobPlans.value !== null)
  const hasError = computed(() => error.value !== null)
  const hasValidationErrors = computed(() => validationErrors.value.length > 0)
  const isAnyLoading = computed(() => 
    isLoading.value || isCreating.value || isUpdating.value || isDeleting.value || isFetchingStats.value
  )

  // Pagination computed
  const hasPreviousPage = computed(() => currentPage.value > 1)
  const hasNextPage = computed(() => currentPage.value < totalPages.value)
  const pageInfo = computed(() => ({
    current: currentPage.value,
    size: pageSize.value,
    total: totalCount.value,
    totalPages: totalPages.value,
    from: ((currentPage.value - 1) * pageSize.value) + 1,
    to: Math.min(currentPage.value * pageSize.value, totalCount.value)
  }))

  // Stats computed
  const hasStats = computed(() => jobPlanStats.value !== null)
  const statsBreakdown = computed(() => jobPlanStats.value?.assetClassBreakdown || [])

  // Clear functions
  const clearError = () => {
    error.value = null
    validationErrors.value = []
  }

  const clearJobPlans = () => {
    jobPlans.value = []
    pagedJobPlans.value = null
    currentJobPlan.value = null
    activeJobPlans.value = []
    inactiveJobPlans.value = []
  }

  const clearFilters = () => {
    filters.value = {}
  }

  const clearStats = () => {
    jobPlanStats.value = null
  }

  // Error handling
  const handleApiResponse = <T>(response: ApiResponse<T>): T | null => {
    if (response.success && response.data !== undefined) {
      clearError()
      return response.data
    } else {
      error.value = response.error || 'An unknown error occurred'
      return null
    }
  }

  // Fetch job plans with pagination
  const fetchJobPlans = async (params?: JobPlanQueryParameters): Promise<PagedResponse<JobPlansHeader> | null> => {
    isLoading.value = true
    clearError()
    
    try {
      const queryParams = {
        page: currentPage.value,
        pageSize: pageSize.value,
        ...filters.value,
        ...params
      }
      
      const response = await jobPlansAPIService.getJobPlans(queryParams)
      const data = handleApiResponse(response)
      
      if (data) {
        pagedJobPlans.value = data
        totalCount.value = data.totalCount
        totalPages.value = data.totalPages
        currentPage.value = data.page
        pageSize.value = data.pageSize
        return data
      }
      return null
    } finally {
      isLoading.value = false
    }
  }

  // Fetch single job plan
  const fetchJobPlan = async (jobPlanId: number): Promise<JobPlansHeader | null> => {
    isLoading.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.getJobPlan(jobPlanId)
      const data = handleApiResponse(response)
      
      if (data) {
        currentJobPlan.value = data
        return data
      }
      return null
    } finally {
      isLoading.value = false
    }
  }

  // Fetch active job plans
  const fetchActiveJobPlans = async (): Promise<JobPlansHeader[]> => {
    isLoading.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.getActiveJobPlans()
      const data = handleApiResponse(response)
      
      if (data) {
        activeJobPlans.value = data
        return data
      }
      return []
    } finally {
      isLoading.value = false
    }
  }

  // Fetch inactive job plans
  const fetchInactiveJobPlans = async (): Promise<JobPlansHeader[]> => {
    isLoading.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.getInactiveJobPlans()
      const data = handleApiResponse(response)
      
      if (data) {
        inactiveJobPlans.value = data
        return data
      }
      return []
    } finally {
      isLoading.value = false
    }
  }

  // Fetch job plans by asset class
  const fetchJobPlansByAssetClass = async (assetClassId: number): Promise<JobPlansHeader[]> => {
    isLoading.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.getJobPlansByAssetClass(assetClassId)
      const data = handleApiResponse(response)
      
      if (data) {
        return data
      }
      return []
    } finally {
      isLoading.value = false
    }
  }

  // Search job plans
  const searchJobPlans = async (searchTerm: string): Promise<JobPlansHeader[]> => {
    isLoading.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.searchJobPlans(searchTerm)
      const data = handleApiResponse(response)
      
      if (data) {
        return data
      }
      return []
    } finally {
      isLoading.value = false
    }
  }

  // Search with pagination
  const searchJobPlansWithPagination = async (
    searchTerm: string, 
    page = 1, 
    pageSize = 10,
    assetClassId?: number,
    status?: boolean
  ): Promise<PagedResponse<JobPlansHeader> | null> => {
    filters.value.searchTerm = searchTerm
    if (assetClassId !== undefined) filters.value.assetClassId = assetClassId
    if (status !== undefined) filters.value.status = status
    currentPage.value = page
    return await fetchJobPlans({ searchTerm, page, pageSize, assetClassId, status })
  }

  // Fetch job plan statistics
  const fetchJobPlanStats = async (): Promise<JobPlanStatsResponse | null> => {
    isFetchingStats.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.getJobPlanStats()
      const data = handleApiResponse(response)
      
      if (data) {
        jobPlanStats.value = data
        return data
      }
      return null
    } finally {
      isFetchingStats.value = false
    }
  }

  // Get job plans summary
  const fetchJobPlansSummary = async (): Promise<{ active: number; inactive: number; total: number } | null> => {
    isFetchingStats.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.getJobPlansSummary()
      const data = handleApiResponse(response)
      
      if (data) {
        return data
      }
      return null
    } finally {
      isFetchingStats.value = false
    }
  }

  // Create job plan
  const createJobPlan = async (jobPlan: CreateJobPlanRequest): Promise<JobPlansHeader | null> => {
    isCreating.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.createJobPlan(jobPlan)
      const data = handleApiResponse(response)
      
      if (data) {
        // Add to local state
        if (pagedJobPlans.value) {
          pagedJobPlans.value.data = [data, ...pagedJobPlans.value.data]
          pagedJobPlans.value.totalCount += 1
        }
        currentJobPlan.value = data
        
        // Add to appropriate status list
        if (data.jobPlanStatus) {
          activeJobPlans.value = [data, ...activeJobPlans.value]
        } else {
          inactiveJobPlans.value = [data, ...inactiveJobPlans.value]
        }
        
        // Refresh stats
        await fetchJobPlanStats()
        
        return data
      }
      return null
    } finally {
      isCreating.value = false
    }
  }

  // Update job plan
  const updateJobPlan = async (jobPlanId: number, jobPlan: UpdateJobPlanRequest): Promise<JobPlansHeader | null> => {
    isUpdating.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.updateJobPlan(jobPlanId, jobPlan)
      const data = handleApiResponse(response)
      
      if (data) {
        // Update local state
        if (currentJobPlan.value?.jobPlanId === jobPlanId) {
          currentJobPlan.value = data
        }
        
        // Update in paged data
        if (pagedJobPlans.value) {
          const index = pagedJobPlans.value.data.findIndex(jp => jp.jobPlanId === jobPlanId)
          if (index !== -1) {
            pagedJobPlans.value.data[index] = data
          }
        }
        
        // Update in status-specific lists
        const updateStatusLists = () => {
          activeJobPlans.value = activeJobPlans.value.filter(jp => jp.jobPlanId !== jobPlanId)
          inactiveJobPlans.value = inactiveJobPlans.value.filter(jp => jp.jobPlanId !== jobPlanId)
          
          if (data.jobPlanStatus) {
            activeJobPlans.value.push(data)
          } else {
            inactiveJobPlans.value.push(data)
          }
        }
        updateStatusLists()
        
        // Refresh stats
        await fetchJobPlanStats()
        
        return data
      }
      return null
    } finally {
      isUpdating.value = false
    }
  }

  // Update job plan status
  const updateJobPlanStatus = async (jobPlanId: number, status: boolean): Promise<JobPlansHeader | null> => {
    isUpdating.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.updateJobPlanStatus(jobPlanId, status)
      const data = handleApiResponse(response)
      
      if (data) {
        // Update local state
        if (currentJobPlan.value?.jobPlanId === jobPlanId) {
          currentJobPlan.value = data
        }
        
        // Update in paged data
        if (pagedJobPlans.value) {
          const index = pagedJobPlans.value.data.findIndex(jp => jp.jobPlanId === jobPlanId)
          if (index !== -1) {
            pagedJobPlans.value.data[index] = data
          }
        }
        
        // Update status-specific lists
        activeJobPlans.value = activeJobPlans.value.filter(jp => jp.jobPlanId !== jobPlanId)
        inactiveJobPlans.value = inactiveJobPlans.value.filter(jp => jp.jobPlanId !== jobPlanId)
        
        if (data.jobPlanStatus) {
          activeJobPlans.value.push(data)
        } else {
          inactiveJobPlans.value.push(data)
        }
        
        // Refresh stats
        await fetchJobPlanStats()
        
        return data
      }
      return null
    } finally {
      isUpdating.value = false
    }
  }

  // Delete job plan
  const deleteJobPlan = async (jobPlanId: number): Promise<boolean> => {
    isDeleting.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.deleteJobPlan(jobPlanId)
      
      if (response.success) {
        // Remove from local state
        if (currentJobPlan.value?.jobPlanId === jobPlanId) {
          currentJobPlan.value = null
        }
        
        // Remove from paged data
        if (pagedJobPlans.value) {
          pagedJobPlans.value.data = pagedJobPlans.value.data.filter(jp => jp.jobPlanId !== jobPlanId)
          pagedJobPlans.value.totalCount -= 1
        }
        
        // Remove from status-specific lists
        activeJobPlans.value = activeJobPlans.value.filter(jp => jp.jobPlanId !== jobPlanId)
        inactiveJobPlans.value = inactiveJobPlans.value.filter(jp => jp.jobPlanId !== jobPlanId)
        
        // Refresh stats
        await fetchJobPlanStats()
        
        return true
      } else {
        error.value = response.error || 'Failed to delete job plan'
        return false
      }
    } finally {
      isDeleting.value = false
    }
  }

  // Bulk delete job plans
  const bulkDeleteJobPlans = async (jobPlanIds: number[]): Promise<boolean> => {
    isDeleting.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.bulkDeleteJobPlans(jobPlanIds)
      
      if (response.success) {
        // Remove from local state
        if (currentJobPlan.value && jobPlanIds.includes(currentJobPlan.value.jobPlanId)) {
          currentJobPlan.value = null
        }
        
        // Remove from paged data
        if (pagedJobPlans.value) {
          pagedJobPlans.value.data = pagedJobPlans.value.data.filter(jp => !jobPlanIds.includes(jp.jobPlanId))
          pagedJobPlans.value.totalCount -= jobPlanIds.length
        }
        
        // Remove from status-specific lists
        activeJobPlans.value = activeJobPlans.value.filter(jp => !jobPlanIds.includes(jp.jobPlanId))
        inactiveJobPlans.value = inactiveJobPlans.value.filter(jp => !jobPlanIds.includes(jp.jobPlanId))
        
        // Refresh stats
        await fetchJobPlanStats()
        
        return true
      } else {
        error.value = response.error || 'Failed to delete job plans'
        return false
      }
    } finally {
      isDeleting.value = false
    }
  }

  // Bulk update job plan status
  const bulkUpdateJobPlanStatus = async (jobPlanIds: number[], status: boolean): Promise<boolean> => {
    isUpdating.value = true
    clearError()
    
    try {
      const response = await jobPlansAPIService.bulkUpdateJobPlanStatus(jobPlanIds, status)
      if (response.success) {
        // Refresh all data since multiple items changed
        await Promise.all([
          fetchJobPlans(),
          fetchActiveJobPlans(),
          fetchInactiveJobPlans(),
          fetchJobPlanStats()
        ])
        
        return true
      } else {
        error.value = response.error || 'Failed to update job plan statuses'
        return false
      }
    } finally {
      isUpdating.value = false
    }
  }

  // Check if job plan exists
  const checkJobPlanExists = async (jobPlanId: number): Promise<boolean> => {
    try {
      const response = await jobPlansAPIService.jobPlanExists(jobPlanId)
      const data = handleApiResponse(response)
      return data?.exists || false
    } catch {
      return false
    }
  }

  // Filter operations
  const filterByAssetClass = async (assetClassId: number): Promise<PagedResponse<JobPlansHeader> | null> => {
    filters.value.assetClassId = assetClassId
    currentPage.value = 1
    return await fetchJobPlans()
  }

  const filterByStatus = async (status: boolean): Promise<PagedResponse<JobPlansHeader> | null> => {
    filters.value.status = status
    currentPage.value = 1
    return await fetchJobPlans()
  }

  const applyFilters = async (newFilters: JobPlanFilters): Promise<PagedResponse<JobPlansHeader> | null> => {
    filters.value = { ...newFilters }
    currentPage.value = 1
    return await fetchJobPlans()
  }

  // Pagination operations
  const goToPage = async (page: number): Promise<PagedResponse<JobPlansHeader> | null> => {
    if (page >= 1 && page <= totalPages.value) {
      currentPage.value = page
      return await fetchJobPlans()
    }
    return null
  }

  const nextPage = async (): Promise<PagedResponse<JobPlansHeader> | null> => {
    if (hasNextPage.value) {
      return await goToPage(currentPage.value + 1)
    }
    return null
  }

  const previousPage = async (): Promise<PagedResponse<JobPlansHeader> | null> => {
    if (hasPreviousPage.value) {
      return await goToPage(currentPage.value - 1)
    }
    return null
  }

  const changePageSize = async (newPageSize: number): Promise<PagedResponse<JobPlansHeader> | null> => {
    pageSize.value = newPageSize
    currentPage.value = 1
    return await fetchJobPlans()
  }

  // Utility functions
  const refreshCurrentPage = async (): Promise<void> => {
    await Promise.all([
      fetchJobPlans(),
      fetchJobPlanStats()
    ])
  }

  const refreshAll = async (): Promise<void> => {
    await Promise.all([
      fetchJobPlans(),
      fetchActiveJobPlans(),
      fetchInactiveJobPlans(),
      fetchJobPlanStats()
    ])
  }

  const getJobPlanById = (jobPlanId: number): JobPlansHeader | undefined => {
    return pagedJobPlans.value?.data.find(jp => jp.jobPlanId === jobPlanId) ||
           activeJobPlans.value.find(jp => jp.jobPlanId === jobPlanId) ||
           inactiveJobPlans.value.find(jp => jp.jobPlanId === jobPlanId)
  }

  const getAssetClassJobPlanCount = (assetClassId: number): number => {
    const breakdown = statsBreakdown.value.find(item => item.assetClassId === assetClassId)
    return breakdown?.jobPlanCount || 0
  }

  // Helper methods using the API service helpers
  const createJobPlanRequest = (
    shortDesc: string,
    longDesc: string,
    assetClassId: number,
    assetClassShortDesc: string,
    status: boolean = true,
    lines?: CreateJobPlanLineRequest[]
  ): CreateJobPlanRequest => {
    return jobPlansAPIService.createJobPlanRequest(
      shortDesc, 
      longDesc, 
      assetClassId, 
      assetClassShortDesc, 
      status, 
      lines
    )
  }

  const createJobPlanLineRequest = (lineNo: number, description: string): CreateJobPlanLineRequest => {
    return jobPlansAPIService.createJobPlanLineRequest(lineNo, description)
  }

  // Auto-fetch on mount if enabled
  if (options.autoFetch) {
    onMounted(async () => {
      await refreshAll()
    })
  }

  return {
    // State
    jobPlans: readonly(jobPlans),
    currentJobPlan: readonly(currentJobPlan),
    pagedJobPlans: readonly(pagedJobPlans),
    activeJobPlans: readonly(activeJobPlans),
    inactiveJobPlans: readonly(inactiveJobPlans),
    jobPlanStats: readonly(jobPlanStats),
    
    // Loading states
    isLoading: readonly(isLoading),
    isCreating: readonly(isCreating),
    isUpdating: readonly(isUpdating),
    isDeleting: readonly(isDeleting),
    isFetchingStats: readonly(isFetchingStats),
    isAnyLoading,
    
    // Error states
    error: readonly(error),
    validationErrors: readonly(validationErrors),
    hasError,
    hasValidationErrors,
    
    // Pagination
    currentPage: readonly(currentPage),
    pageSize: readonly(pageSize),
    totalCount: readonly(totalCount),
    totalPages: readonly(totalPages),
    pageInfo,
    hasPreviousPage,
    hasNextPage,
    
    // Filters
    filters: readonly(filters),
    
    // Computed
    hasJobPlans,
    hasPagedData,
    hasStats,
    statsBreakdown,
    
    // Methods - CRUD operations
    fetchJobPlans,
    fetchJobPlan,
    fetchActiveJobPlans,
    fetchInactiveJobPlans,
    fetchJobPlansByAssetClass,
    createJobPlan,
    updateJobPlan,
    updateJobPlanStatus,
    deleteJobPlan,
    bulkDeleteJobPlans,
    bulkUpdateJobPlanStatus,
    
    // Methods - Search operations
    searchJobPlans,
    searchJobPlansWithPagination,
    
    // Methods - Stats operations
    fetchJobPlanStats,
    fetchJobPlansSummary,
    
    // Methods - Filter operations
    filterByAssetClass,
    filterByStatus,
    applyFilters,
    
    // Methods - Pagination
    goToPage,
    nextPage,
    previousPage,
    changePageSize,
    
    // Methods - Utilities
    checkJobPlanExists,
    clearError,
    clearJobPlans,
    clearFilters,
    clearStats,
    refreshCurrentPage,
    refreshAll,
    getJobPlanById,
    getAssetClassJobPlanCount,
    
    // Methods - Helpers
    createJobPlanRequest,
    createJobPlanLineRequest,
  }
}

// Provide a global instance for app-wide state management
export const useGlobalJobPlan = () => {
  return useJobPlan({ autoFetch: true })
}

// Type exports for external use
export type {
  JobPlansHeader,
  JobPlansLine,
  CreateJobPlanRequest,
  UpdateJobPlanRequest,
  CreateJobPlanLineRequest,
  UpdateJobPlanLineRequest,
  JobPlanQueryParameters,
  JobPlanStatsResponse,
  AssetClassJobPlanCount,
  JobPlanFilters,
  UseJobPlanOptions
}
