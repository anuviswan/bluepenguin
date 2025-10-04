import {ApiServiceBase} from "./ApiServiceBase.ts";
import type {ICategoryListResponse} from "../types/apirequestresponsetypes/Product.ts";

class CategoryService extends  ApiServiceBase{

    public async getCategories(): Promise<ICategoryListResponse> {

        return await this.invoke<ICategoryListResponse>(
            {
                method: 'get',
                url: '/api/Category/getall',
              }
        )
    }
}

const categoryService = new CategoryService();
export default categoryService;