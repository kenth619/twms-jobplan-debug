// services/baseAPIService.ts

export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  error?: string;
  statusCode?: number;
}

export interface PagedResponse<T> {
  data: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export interface LookupItem {
  id: number;
  name: string;
  code: string;
}

export abstract class BaseAPIService {
  protected baseApiUrl: string;
  protected config: any;

  constructor() {
    this.config = useRuntimeConfig();
    this.baseApiUrl = this.config.public.apiBase;
    
    if (!this.baseApiUrl) {
      throw new Error('NUXT_PUBLIC_API_BASE environment variable is required');
    }
  }

  /**
   * Helper method to make API calls with proper error handling
   */
  protected async makeRequest<T>(
    endpoint: string,
    options: RequestInit = {}
  ): Promise<ApiResponse<T>> {
    try {
      const url = `${this.baseApiUrl}${endpoint}`;
      
      const response = await $fetch<T>(url, {
        ...options,
        method: options.method as any,
        headers: {
          'Content-Type': 'application/json',
          ...options.headers,
        },
      });

      return {
        success: true,
        data: response,
        statusCode: 200,
      };
    } catch (error: any) {
      return this.handleError(error);
    }
  }

  /**
   * Make a GET request
   */
  protected async get<T>(endpoint: string, params?: Record<string, any>): Promise<ApiResponse<T>> {
    const queryString = params ? this.buildQueryString(params) : '';
    const url = queryString ? `${endpoint}?${queryString}` : endpoint;
    return this.makeRequest<T>(url, { method: 'GET' });
  }

  /**
   * Make a POST request
   */
  protected async post<T>(endpoint: string, data?: any): Promise<ApiResponse<T>> {
    return this.makeRequest<T>(endpoint, {
      method: 'POST',
      body: data ? JSON.stringify(data) : undefined,
    });
  }

  /**
   * Make a PUT request
   */
  protected async put<T>(endpoint: string, data?: any): Promise<ApiResponse<T>> {
    return this.makeRequest<T>(endpoint, {
      method: 'PUT',
      body: data ? JSON.stringify(data) : undefined,
    });
  }

  /**
   * Make a DELETE request
   */
  protected async delete<T>(endpoint: string): Promise<ApiResponse<T>> {
    return this.makeRequest<T>(endpoint, { method: 'DELETE' });
  }

  /**
   * Make a request for blob data (file downloads)
   */
  protected async getBlob(endpoint: string, params?: Record<string, any>): Promise<ApiResponse<Blob>> {
    try {
      const queryString = params ? this.buildQueryString(params) : '';
      const url = queryString ? `${endpoint}?${queryString}` : endpoint;
      const fullUrl = `${this.baseApiUrl}${url}`;
      
      const blob = await $fetch<Blob>(fullUrl, {
        method: 'GET',
        responseType: 'blob',
      });

      return {
        success: true,
        data: blob,
        statusCode: 200,
      };
    } catch (error: any) {
      return this.handleError(error);
    }
  }

  /**
   * Build query string from parameters
   */
  protected buildQueryString(params: Record<string, any>): string {
    const queryParams = new URLSearchParams();
    
    Object.entries(params).forEach(([key, value]) => {
      if (value !== null && value !== undefined && value !== '') {
        queryParams.append(key, value.toString());
      }
    });
    
    return queryParams.toString();
  }

  /**
   * Validate required fields
   */
  protected validateRequired(data: Record<string, any>, requiredFields: string[]): string[] {
    const errors: string[] = [];
    
    requiredFields.forEach(field => {
      const value = data[field];
      if (value === null || value === undefined || (typeof value === 'string' && value.trim() === '')) {
        errors.push(`${field} is required`);
      }
    });
    
    return errors;
  }

  /**
   * Validate field length
   */
  protected validateLength(value: string, fieldName: string, maxLength: number): string[] {
    const errors: string[] = [];
    
    if (value && value.length > maxLength) {
      errors.push(`${fieldName} must be ${maxLength} characters or less`);
    }
    
    return errors;
  }

  /**
   * Validate positive integer
   */
  protected validatePositiveInteger(value: number, fieldName: string): string[] {
    const errors: string[] = [];
    
    if (!value || value <= 0) {
      errors.push(`Valid ${fieldName} is required`);
    }
    
    return errors;
  }

  /**
   * Health check method
   */
  async healthCheck(): Promise<ApiResponse<{ status: string; timestamp: string }>> {
    try {
      const response = await $fetch<any>(`${this.baseApiUrl}/api/health`, {
        method: 'GET',
        timeout: 5000,
      });

      return {
        success: true,
        data: {
          status: 'healthy',
          timestamp: new Date().toISOString(),
          ...response
        },
        statusCode: 200,
      };
    } catch (error: any) {
      return {
        success: false,
        error: `API health check failed: ${error.message}`,
        statusCode: error.status || 0,
      };
    }
  }

  /**
   * Handle API errors consistently
   */
  private handleError(error: any): ApiResponse<any> {
    if (process.dev) {
      console.error('API Service Error:', error);
      console.log('Environment Info:', {
        apiBase: this.config.public.apiBase,
        appUrl: this.config.public.appUrl,
      });
    }

    let errorMessage = 'An unexpected error occurred';
    let statusCode = 500;

    if (error.response) {
      statusCode = error.response.status;
      errorMessage = this.getErrorMessageByStatus(statusCode, error.response._data);
    } else if (error.request) {
      errorMessage = 'Network error - please check your connection and ensure the API server is running';
      statusCode = 0;
      
      if (process.dev && error.code === 'CERT_HAS_EXPIRED') {
        errorMessage += ' (SSL certificate issue - check NODE_TLS_REJECT_UNAUTHORIZED setting)';
      }
    } else if (error.message) {
      errorMessage = error.message;
    }

    if (error.data) {
      errorMessage = this.formatValidationErrors(error.data) || errorMessage;
    }

    return {
      success: false,
      error: errorMessage,
      statusCode: error.status || statusCode,
    };
  }

  /**
   * Get error message by HTTP status code
   */
  private getErrorMessageByStatus(statusCode: number, responseData?: any): string {
    switch (statusCode) {
      case 400:
        return this.formatValidationErrors(responseData) || 'Bad request - please check your input';
      case 401:
        return 'Unauthorized - please log in';
      case 403:
        return 'Forbidden - you do not have permission to perform this action';
      case 404:
        return 'Resource not found';
      case 409:
        return 'Conflict - the resource may have been modified by another user';
      case 422:
        return this.formatValidationErrors(responseData) || 'Validation failed';
      case 500:
        return 'Server error - please try again later';
      default:
        return responseData?.message || `Request failed with status ${statusCode}`;
    }
  }

  /**
   * Format validation errors from API response
   */
  private formatValidationErrors(errorData: any): string {
    if (typeof errorData === 'string') {
      return errorData;
    }

    if (errorData?.errors) {
      const errors: string[] = [];
      Object.keys(errorData.errors).forEach(key => {
        const fieldErrors = errorData.errors[key];
        if (Array.isArray(fieldErrors)) {
          errors.push(...fieldErrors);
        } else {
          errors.push(fieldErrors);
        }
      });
      return errors.join('; ');
    }

    if (errorData?.title) {
      return errorData.title;
    }

    if (errorData?.message) {
      return errorData.message;
    }

    return '';
  }
}
