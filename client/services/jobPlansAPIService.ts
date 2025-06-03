// services/jobPlansAPIService.ts
import {
  BaseAPIService,
  type ApiResponse,
  type PagedResponse,
} from "./baseAPIService";

export interface JobPlansHeader {
  jobPlanId: number;
  jobPlanStatus: boolean;
  jobPlanShortDesc: string;
  jobPlanLongDesc: string;
  assetClassId: number;
  assetClassShortDesc: string;
  assetClassLongDesc: string;
  dateCreated: string;
  dateModified: string;
  createdBy: string;
  modifiedBy: string;
  jobPlansLines: JobPlansLine[];
}

export interface JobPlansLine {
  jobPlanLineId: number;
  jobPlanId: number;
  jobPlanLineNo: number;
  jobPlanLineDesc: string;
}

export interface CreateJobPlanRequest {
  jobPlanStatus: boolean;
  jobPlanShortDesc: string;
  jobPlanLongDesc: string;
  assetClassId: number;
  assetClassShortDesc: string;
  jobPlanLines?: CreateJobPlanLineRequest[];
}

export interface UpdateJobPlanRequest {
  jobPlanStatus: boolean;
  jobPlanShortDesc: string;
  jobPlanLongDesc: string;
  assetClassId: number;
  assetClassShortDesc: string;
  jobPlanLines?: UpdateJobPlanLineRequest[];
}

export interface UpdateJobPlanStatusRequest {
  jobPlanStatus: boolean;
}

export interface CreateJobPlanLineRequest {
  jobPlanLineNo: number;
  jobPlanLineDesc: string;
}

export interface UpdateJobPlanLineRequest {
  jobPlanLineId: number;
  jobPlanLineNo: number;
  jobPlanLineDesc: string;
}

export interface JobPlanQueryParameters {
  page?: number;
  pageSize?: number;
  assetClassId?: number;
  status?: boolean;
  searchTerm?: string;
}

export interface JobPlanStatsResponse {
  totalJobPlans: number;
  activeJobPlans: number;
  inactiveJobPlans: number;
  assetClassBreakdown: AssetClassJobPlanCount[];
}

export interface AssetClassJobPlanCount {
  assetClassId: number;
  assetClassShortDesc: string;
  jobPlanCount: number;
}

// Add new response model for job plan with tasks
export interface JobPlanWithTasksResponse {
  jobPlanId: number;
  jobPlanStatus: boolean;
  jobPlanShortDesc: string;
  jobPlanLongDesc: string;
  assetClassId: number;
  assetClassShortDesc: string;
  dateCreated: string;
  dateModified: string;
  createdBy: string;
  modifiedBy: string;
  tasks: TaskResponse[];
}

// Task response model
export interface TaskResponse {
  jobPlanLineId: number;
  jobPlanLineNo: number;
  jobPlanLineDesc: string;
}

class JobPlansAPIService extends BaseAPIService {
  private readonly API_ENDPOINTS = {
    JOB_PLANS: "/api/jobplans",
    NEXT_ID: "/api/jobplans/next-id",
    ACTIVE: "/api/jobplans/active",
    INACTIVE: "/api/jobplans/inactive",
    BY_ASSET_CLASS: "/api/jobplans/by-asset-class",
    SEARCH: "/api/jobplans/search",
    STATS: "/api/jobplans/stats",
    STATUS: "/api/jobplans/{id}/status",
    DETAILS: "/api/jobplans/{id}/details",
  } as const;

  /**
   * Get job plans with pagination and filtering
   */
  async getJobPlans(
    params?: JobPlanQueryParameters
  ): Promise<ApiResponse<PagedResponse<JobPlansHeader>>> {
    return this.get<PagedResponse<JobPlansHeader>>(
      this.API_ENDPOINTS.JOB_PLANS,
      params
    );
  }

  /**
   * Get a specific job plan by ID
   */
  async getJobPlan(jobPlanId: number): Promise<ApiResponse<JobPlansHeader>> {
    return this.get<JobPlansHeader>(
      `${this.API_ENDPOINTS.JOB_PLANS}/${jobPlanId}`
    );
  }
  /**
   * Get a specific job plan by ID with its associated tasks
   */
  async getJobPlanAndTask(
    jobPlanId: number
  ): Promise<ApiResponse<JobPlanWithTasksResponse>> {
    return this.get<JobPlanWithTasksResponse>(
      this.API_ENDPOINTS.DETAILS.replace("{id}", jobPlanId.toString())
    );
  }

  /**
   * Create a new job plan
   */
  async createJobPlan(
    jobPlan: CreateJobPlanRequest
  ): Promise<ApiResponse<JobPlansHeader>> {
    const validationErrors = this.validateJobPlan(jobPlan);
    if (validationErrors.length > 0) {
      return {
        success: false,
        error: validationErrors.join("; "),
        statusCode: 400,
      };
    }

    return this.post<JobPlansHeader>(this.API_ENDPOINTS.JOB_PLANS, jobPlan);
  }

  /**
   * Update an existing job plan
   */
  async updateJobPlan(
    jobPlanId: number,
    jobPlan: UpdateJobPlanRequest
  ): Promise<ApiResponse<JobPlansHeader>> {
    const validationErrors = this.validateJobPlan(jobPlan);
    if (validationErrors.length > 0) {
      return {
        success: false,
        error: validationErrors.join("; "),
        statusCode: 400,
      };
    }

    return this.put<JobPlansHeader>(
      `${this.API_ENDPOINTS.JOB_PLANS}/${jobPlanId}`,
      jobPlan
    );
  }

  /**
   * Update job plan status only
   */
  async updateJobPlanStatus(
    jobPlanId: number,
    status: boolean
  ): Promise<ApiResponse<JobPlansHeader>> {
    const request: UpdateJobPlanStatusRequest = { jobPlanStatus: status };
    return this.put<JobPlansHeader>(
      this.API_ENDPOINTS.STATUS.replace("{id}", jobPlanId.toString()),
      request
    );
  }

  /**
   * Delete a job plan
   */
  async deleteJobPlan(jobPlanId: number): Promise<ApiResponse<void>> {
    return this.delete<void>(`${this.API_ENDPOINTS.JOB_PLANS}/${jobPlanId}`);
  }

  /**
   * Get all active job plans
   */
  async getActiveJobPlans(): Promise<ApiResponse<JobPlansHeader[]>> {
    return this.get<JobPlansHeader[]>(this.API_ENDPOINTS.ACTIVE);
  }

  /**
   * Get all inactive job plans
   */
  async getInactiveJobPlans(): Promise<ApiResponse<JobPlansHeader[]>> {
    return this.get<JobPlansHeader[]>(this.API_ENDPOINTS.INACTIVE);
  }

  /**
   * Get job plans by asset class
   */
  async getJobPlansByAssetClass(
    assetClassId: number
  ): Promise<ApiResponse<JobPlansHeader[]>> {
    return this.get<JobPlansHeader[]>(
      `${this.API_ENDPOINTS.BY_ASSET_CLASS}/${assetClassId}`
    );
  }
  /**
   * Get next available Job Plan ID
   */
  async getNextJobPlanId(): Promise<ApiResponse<number>> {
    try {
      const response = await this.get<number>(this.API_ENDPOINTS.NEXT_ID);

      if (response.success && response.data !== undefined) {
        return {
          success: true,
          data: response.data,
        };
      }

      return {
        success: false,
        error: response.error || "Failed to fetch next Job Plan ID",
        statusCode: response.statusCode || 500,
      };
    } catch (error: any) {
      return {
        success: false,
        error: error.message || "Network error while fetching next ID",
        statusCode: 500,
      };
    }
  }

  /**
   * Search job plans by term
   */
  async searchJobPlans(
    searchTerm: string
  ): Promise<ApiResponse<JobPlansHeader[]>> {
    if (!searchTerm || searchTerm.trim().length === 0) {
      return {
        success: false,
        error: "Search term is required",
        statusCode: 400,
      };
    }

    return this.get<JobPlansHeader[]>(this.API_ENDPOINTS.SEARCH, {
      searchTerm,
    });
  }

  /**
   * Get job plan statistics
   */
  async getJobPlanStats(): Promise<ApiResponse<JobPlanStatsResponse>> {
    return this.get<JobPlanStatsResponse>(this.API_ENDPOINTS.STATS);
  }

  /**
   * Get job plans with pagination and filtering (alternative method name for backward compatibility)
   */
  async getJobPlansByPage(
    params?: JobPlanQueryParameters
  ): Promise<ApiResponse<PagedResponse<JobPlansHeader>>> {
    return this.getJobPlans(params);
  }

  /**
   * Get job plans by status with pagination
   */
  async getJobPlansByStatus(
    status: boolean,
    page = 1,
    pageSize = 10
  ): Promise<ApiResponse<PagedResponse<JobPlansHeader>>> {
    return this.getJobPlans({ status, page, pageSize });
  }

  /**
   * Advanced search with pagination
   */
  async searchJobPlansWithPagination(
    searchTerm: string,
    page = 1,
    pageSize = 10,
    assetClassId?: number,
    status?: boolean
  ): Promise<ApiResponse<PagedResponse<JobPlansHeader>>> {
    return this.getJobPlans({
      searchTerm,
      page,
      pageSize,
      assetClassId,
      status,
    });
  }

  /**
   * Bulk delete job plans
   */
  async bulkDeleteJobPlans(jobPlanIds: number[]): Promise<ApiResponse<void>> {
    try {
      const results = await Promise.allSettled(
        jobPlanIds.map((id) => this.deleteJobPlan(id))
      );

      const failures = results
        .map((result, index) => ({ result, id: jobPlanIds[index] }))
        .filter(
          ({ result }) => result.status === "rejected" || !result.value.success
        );

      if (failures.length > 0) {
        const failedIds = failures.map(({ id }) => id);
        return {
          success: false,
          error: `Failed to delete job plans: ${failedIds.join(", ")}`,
          statusCode: 207,
        };
      }

      return { success: true };
    } catch (error: any) {
      return {
        success: false,
        error: error.message || "Bulk delete operation failed",
        statusCode: 500,
      };
    }
  }

  /**
   * Bulk update job plan status
   */
  async bulkUpdateJobPlanStatus(
    jobPlanIds: number[],
    status: boolean
  ): Promise<ApiResponse<void>> {
    try {
      const results = await Promise.allSettled(
        jobPlanIds.map((id) => this.updateJobPlanStatus(id, status))
      );

      const failures = results
        .map((result, index) => ({ result, id: jobPlanIds[index] }))
        .filter(
          ({ result }) => result.status === "rejected" || !result.value.success
        );

      if (failures.length > 0) {
        const failedIds = failures.map(({ id }) => id);
        return {
          success: false,
          error: `Failed to update status for job plans: ${failedIds.join(
            ", "
          )}`,
          statusCode: 207,
        };
      }

      return { success: true };
    } catch (error: any) {
      return {
        success: false,
        error: error.message || "Bulk status update operation failed",
        statusCode: 500,
      };
    }
  }

  /**
   * Check if a job plan exists
   */
  async jobPlanExists(
    jobPlanId: number
  ): Promise<ApiResponse<{ exists: boolean }>> {
    try {
      const response = await this.getJobPlan(jobPlanId);

      if (response.success) {
        return {
          success: true,
          data: { exists: true },
        };
      }

      if (response.statusCode === 404) {
        return {
          success: true,
          data: { exists: false },
        };
      }

      return {
        success: false,
        error: response.error || "Failed to check job plan existence",
        statusCode: response.statusCode,
      };
    } catch (error: any) {
      return {
        success: false,
        error: error.message || "Failed to check job plan existence",
        statusCode: 500,
      };
    }
  }

  /**
   * Get job plans summary (counts by status)
   */
  async getJobPlansSummary(): Promise<
    ApiResponse<{ active: number; inactive: number; total: number }>
  > {
    try {
      const statsResponse = await this.getJobPlanStats();

      if (!statsResponse.success || !statsResponse.data) {
        return {
          success: false,
          error: "Failed to retrieve job plans statistics",
          statusCode: statsResponse.statusCode || 500,
        };
      }

      return {
        success: true,
        data: {
          active: statsResponse.data.activeJobPlans,
          inactive: statsResponse.data.inactiveJobPlans,
          total: statsResponse.data.totalJobPlans,
        },
      };
    } catch (error: any) {
      return {
        success: false,
        error: error.message || "Failed to get job plans summary",
        statusCode: 500,
      };
    }
  }

  /**
   * Validate job plan data before sending
   */
  private validateJobPlan(
    jobPlan: CreateJobPlanRequest | UpdateJobPlanRequest
  ): string[] {
    const errors: string[] = [];

    // Validate required fields
    errors.push(
      ...this.validateRequired(jobPlan, ["jobPlanShortDesc", "assetClassId"])
    );

    // Validate field lengths
    errors.push(
      ...this.validateLength(
        jobPlan.jobPlanShortDesc,
        "Job plan short description",
        100
      )
    );
    errors.push(
      ...this.validateLength(
        jobPlan.assetClassShortDesc,
        "Asset class short description",
        100
      )
    );

    // Validate positive integers
    errors.push(
      ...this.validatePositiveInteger(jobPlan.assetClassId, "asset class ID")
    );

    // Validate job plan lines
    if (jobPlan.jobPlanLines && jobPlan.jobPlanLines.length > 0) {
      jobPlan.jobPlanLines.forEach((line, index) => {
        const lineErrors = this.validateJobPlanLine(line, `Line ${index + 1}`);
        errors.push(...lineErrors);
      });

      // Check for duplicate line numbers
      const lineNumbers = jobPlan.jobPlanLines.map(
        (line) => line.jobPlanLineNo
      );
      const duplicates = lineNumbers.filter(
        (num, index) => lineNumbers.indexOf(num) !== index
      );
      if (duplicates.length > 0) {
        errors.push(`Duplicate line numbers found: ${duplicates.join(", ")}`);
      }
    }

    return errors;
  }

  /**
   * Validate job plan line data
   */
  private validateJobPlanLine(
    line: CreateJobPlanLineRequest | UpdateJobPlanLineRequest,
    prefix = ""
  ): string[] {
    const errors: string[] = [];
    const errorPrefix = prefix ? `${prefix}: ` : "";

    if (!line.jobPlanLineDesc || line.jobPlanLineDesc.trim().length === 0) {
      errors.push(`${errorPrefix}Line description is required`);
    }

    errors.push(
      ...this.validateLength(
        line.jobPlanLineDesc,
        `${errorPrefix}Line description`,
        500
      )
    );
    errors.push(
      ...this.validatePositiveInteger(
        line.jobPlanLineNo,
        `${errorPrefix}line number`
      )
    );

    return errors;
  }

  /**
   * Helper method to create a job plan with default values
   */
  createJobPlanRequest(
    shortDesc: string,
    longDesc: string,
    assetClassId: number,
    assetClassShortDesc: string,
    status: boolean = true,
    lines?: CreateJobPlanLineRequest[]
  ): CreateJobPlanRequest {
    return {
      jobPlanStatus: status,
      jobPlanShortDesc: shortDesc,
      jobPlanLongDesc: longDesc,
      assetClassId,
      assetClassShortDesc,
      jobPlanLines: lines,
    };
  }

  /**
   * Helper method to create a job plan line request
   */
  createJobPlanLineRequest(
    lineNo: number,
    description: string
  ): CreateJobPlanLineRequest {
    return {
      jobPlanLineNo: lineNo,
      jobPlanLineDesc: description,
    };
  }
}

// Create and export a singleton instance
export const jobPlansAPIService = new JobPlansAPIService();

// Export the class for dependency injection if needed
export default JobPlansAPIService;
