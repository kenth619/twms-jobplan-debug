// services/jobPlansLineAPIService.ts
import { BaseAPIService, type ApiResponse } from "./baseAPIService";

// JobPlansLine model interface
export interface JobPlansLine {
  jobPlanLineId: number;
  jobPlanId: number;
  jobPlanLineNo: number;
  jobPlanLineDesc: string;
}

// Create JobPlansLine request interface
export interface CreateJobPlansLineRequest {
  jobPlanId: number;
  jobPlanLineNo: number;
  jobPlanLineDesc: string;
}

// Update JobPlansLine request interface
export interface UpdateJobPlansLineRequest {
  jobPlanLineId: number;
  jobPlanId: number;
  jobPlanLineNo: number;
  jobPlanLineDesc: string;
}

class JobPlansLineAPIService extends BaseAPIService {
  private readonly API_ENDPOINTS = {
    JOB_PLAN_LINES: "/api/jobplansline",
    BY_JOB_PLAN_ID: "/api/jobplansline/by-jobplan-id",
  } as const;

  /**
   * Get all job plan lines
   */
  async getAllJobPlansLines(): Promise<ApiResponse<JobPlansLine[]>> {
    return this.get<JobPlansLine[]>(this.API_ENDPOINTS.JOB_PLAN_LINES);
  }

  /**
   * Get a specific job plan line by ID
   */
  async getJobPlansLineById(
    jobPlanLineId: number
  ): Promise<ApiResponse<JobPlansLine>> {
    return this.get<JobPlansLine>(
      `${this.API_ENDPOINTS.JOB_PLAN_LINES}/${jobPlanLineId}`
    );
  }

  /**
   * Get all job plan lines by JobPlanId
   */
  async getJobPlansLinesByJobPlanId(
    jobPlanId: number
  ): Promise<ApiResponse<JobPlansLine[]>> {
    return this.get<JobPlansLine[]>(
      `${this.API_ENDPOINTS.BY_JOB_PLAN_ID}/${jobPlanId}`
    );
  }

  /**
   * Create a new job plan line
   */
  async createJobPlansLine(
    jobPlansLine: CreateJobPlansLineRequest
  ): Promise<ApiResponse<JobPlansLine>> {
    const validationErrors = this.validateJobPlansLine(jobPlansLine);
    if (validationErrors.length > 0) {
      return {
        success: false,
        error: validationErrors.join("; "),
        statusCode: 400,
      };
    }

    return this.post<JobPlansLine>(
      this.API_ENDPOINTS.JOB_PLAN_LINES,
      jobPlansLine
    );
  }

  /**
   * Update an existing job plan line
   */
  async updateJobPlansLine(
    jobPlanLineId: number,
    jobPlansLine: UpdateJobPlansLineRequest
  ): Promise<ApiResponse<JobPlansLine>> {
    if (jobPlansLine.jobPlanLineId !== jobPlanLineId) {
      return {
        success: false,
        error: "Job plan line ID mismatch",
        statusCode: 400,
      };
    }

    const validationErrors = this.validateJobPlansLine(jobPlansLine);
    if (validationErrors.length > 0) {
      return {
        success: false,
        error: validationErrors.join("; "),
        statusCode: 400,
      };
    }

    return this.put<JobPlansLine>(
      `${this.API_ENDPOINTS.JOB_PLAN_LINES}/${jobPlanLineId}`,
      jobPlansLine
    );
  }

  /**
   * Delete a job plan line by ID
   */
  async deleteJobPlansLine(jobPlanLineId: number): Promise<ApiResponse<void>> {
    return this.delete<void>(
      `${this.API_ENDPOINTS.JOB_PLAN_LINES}/${jobPlanLineId}`
    );
  }

  /**
   * Validate job plan line data before sending
   */
  private validateJobPlansLine(
    jobPlansLine: CreateJobPlansLineRequest | UpdateJobPlansLineRequest
  ): string[] {
    const errors: string[] = [];

    // Validate required fields
    if (
      !jobPlansLine.jobPlanLineDesc ||
      jobPlansLine.jobPlanLineDesc.trim() === ""
    ) {
      errors.push("Job plan line description is required");
    }

    if (
      !Number.isInteger(jobPlansLine.jobPlanLineNo) ||
      jobPlansLine.jobPlanLineNo <= 0
    ) {
      errors.push("Job plan line number must be a positive integer");
    }

    if (
      !Number.isInteger(jobPlansLine.jobPlanId) ||
      jobPlansLine.jobPlanId <= 0
    ) {
      errors.push("Job plan ID must be a positive integer");
    }

    // Validate length of description
    if (jobPlansLine.jobPlanLineDesc.length > 500) {
      errors.push("Job plan line description must not exceed 500 characters");
    }
    return errors;
  }
}

// Create and export a singleton instance
export const jobPlansLineAPIService = new JobPlansLineAPIService();

// Export the class for dependency injection if needed
export default JobPlansLineAPIService;
