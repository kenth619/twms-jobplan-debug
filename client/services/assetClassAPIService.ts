// services/assetClassAPIService.ts
import { BaseAPIService, type ApiResponse } from './baseAPIService';

export interface AssetClass {
  id: number;
  assetClassShortDesc: string;
  assetClassLongDesc: string;
  status: boolean;
  createdDate?: string;
  modifiedDate?: string;
}

export interface CreateAssetClassRequest {
  assetClassShortDesc: string;
  assetClassLongDesc: string;
  status?: boolean;
}

export interface UpdateAssetClassRequest {
  assetClassShortDesc: string;
  assetClassLongDesc: string;
  status: boolean;
}

class AssetClassAPIService extends BaseAPIService {
  private readonly API_ENDPOINTS = {
    ASSET_CLASSES: '/api/AssetClass',
  } as const;

  /**
   * Get all asset classes
   */
  async getAllAssetClasses(): Promise<ApiResponse<AssetClass[]>> {
    return this.get<AssetClass[]>(this.API_ENDPOINTS.ASSET_CLASSES);
  }

  /**
   * Get a specific asset class by ID
   */
  async getAssetClass(id: number): Promise<ApiResponse<AssetClass>> {
    return this.get<AssetClass>(`${this.API_ENDPOINTS.ASSET_CLASSES}/${id}`);
  }

  /**
   * Get asset class by short description
   */
  async getAssetClassByShortDesc(shortDesc: string): Promise<ApiResponse<AssetClass>> {
    return this.get<AssetClass>(`${this.API_ENDPOINTS.ASSET_CLASSES}/by-short-desc/${shortDesc}`);
  }

  /**
   * Search asset classes by keyword
   */
  async searchAssetClasses(keyword: string): Promise<ApiResponse<AssetClass[]>> {
    return this.get<AssetClass[]>(`${this.API_ENDPOINTS.ASSET_CLASSES}/search`, { keyword });
  }

  /**
   * Create a new asset class
   */
  async createAssetClass(assetClass: CreateAssetClassRequest): Promise<ApiResponse<AssetClass>> {
    const validationErrors = this.validateAssetClass(assetClass);
    if (validationErrors.length > 0) {
      return {
        success: false,
        error: validationErrors.join('; '),
        statusCode: 400,
      };
    }

    return this.post<AssetClass>(this.API_ENDPOINTS.ASSET_CLASSES, assetClass);
  }

  /**
   * Update an existing asset class
   */
  async updateAssetClass(id: number, assetClass: UpdateAssetClassRequest): Promise<ApiResponse<AssetClass>> {
    const validationErrors = this.validateAssetClass(assetClass);
    if (validationErrors.length > 0) {
      return {
        success: false,
        error: validationErrors.join('; '),
        statusCode: 400,
      };
    }

    return this.put<AssetClass>(`${this.API_ENDPOINTS.ASSET_CLASSES}/${id}`, assetClass);
  }

  /**
   * Delete an asset class
   */
  async deleteAssetClass(id: number): Promise<ApiResponse<void>> {
    return this.delete<void>(`${this.API_ENDPOINTS.ASSET_CLASSES}/${id}`);
  }

  /**
   * Get only active asset classes
   */
  async getActiveAssetClasses(): Promise<ApiResponse<AssetClass[]>> {
    const response = await this.getAllAssetClasses();
    
    if (response.success && response.data) {
      const activeAssetClasses = response.data.filter(ac => ac.status === true);
      return {
        ...response,
        data: activeAssetClasses,
      };
    }
    
    return response;
  }

  /**
   * Toggle asset class status (activate/deactivate)
   */
  async toggleAssetClassStatus(id: number): Promise<ApiResponse<AssetClass>> {
    const assetClassResponse = await this.getAssetClass(id);
    
    if (!assetClassResponse.success || !assetClassResponse.data) {
      return assetClassResponse;
    }

    const assetClass = assetClassResponse.data;
    const updateRequest: UpdateAssetClassRequest = {
      assetClassShortDesc: assetClass.assetClassShortDesc,
      assetClassLongDesc: assetClass.assetClassLongDesc,
      status: !assetClass.status,
    };

    return this.updateAssetClass(id, updateRequest);
  }

  /**
   * Validate asset class data before sending
   */
  private validateAssetClass(assetClass: CreateAssetClassRequest | UpdateAssetClassRequest): string[] {
    const errors: string[] = [];

    // Use base class validation methods
    errors.push(...this.validateRequired(assetClass, ['assetClassShortDesc', 'assetClassLongDesc']));
    errors.push(...this.validateLength(assetClass.assetClassShortDesc, 'Asset class short description', 50));
    errors.push(...this.validateLength(assetClass.assetClassLongDesc, 'Asset class long description', 200));

    // Additional custom validations
    if (assetClass.assetClassShortDesc && assetClass.assetClassShortDesc.trim().length < 2) {
      errors.push('Asset class short description must be at least 2 characters');
    }

    if (assetClass.assetClassLongDesc && assetClass.assetClassLongDesc.trim().length < 5) {
      errors.push('Asset class long description must be at least 5 characters');
    }

    // Validate short description format (alphanumeric and spaces only)
    if (assetClass.assetClassShortDesc && !/^[a-zA-Z0-9\s]+$/.test(assetClass.assetClassShortDesc)) {
      errors.push('Asset class short description can only contain letters, numbers, and spaces');
    }

    return errors;
  }
}

// Create and export a singleton instance
export const assetClassAPIService = new AssetClassAPIService();

// Export the class for dependency injection if needed
export default AssetClassAPIService;
