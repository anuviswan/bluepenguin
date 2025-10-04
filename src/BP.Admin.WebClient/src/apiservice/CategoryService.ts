import {ApiServiceBase} from "./ApiServiceBase.ts";
import type {ICategoryListResponse} from "../types/apirequestresponsetypes/Product.ts";
import type {Category} from "../types/ProductTypes.ts";

class CategoryService extends  ApiServiceBase{

    public async getCategories(): Promise<Category[]> {

        const response = await this.invoke<ICategoryListResponse>(
            {
                method: 'get',
                url: '/api/Category/getall',
              }
        )

        console.log(response);
        return response.data.map(
            (c): Category => ({
                Id: c.id,
                Name: c.name,
            })
        );
    }
}

const categoryService = new CategoryService();
export default categoryService;