import {ApiServiceBase} from "./ApiServiceBase.ts";
import type {IMaterialListResponse } from "../types/apirequestresponsetypes/Product.ts";
import type {Material} from "../types/ProductTypes.ts";

class MaterialService extends  ApiServiceBase{

    public async getMaterials(): Promise<Material[]> {

        const response = await this.invoke<IMaterialListResponse>(
            {
                method: 'get',
                url: '/api/Material/getall',
            }
        )

        console.log(response);
        return response.data.map(
            (c:any): Material => ({
                Id: c.id,
                Name: c.name,
            })
        );
    }
}

const materialService = new MaterialService();
export default materialService;