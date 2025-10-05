import {ApiServiceBase} from "./ApiServiceBase.ts";
import type {IFeatureListResponse} from "../types/apirequestresponsetypes/Product.ts";
import type {Feature} from "../types/ProductTypes.ts";

class FeatureService extends  ApiServiceBase{

    public async getFeatures(): Promise<Feature[]> {

        const response = await this.invoke<IFeatureListResponse>(
            {
                method: 'get',
                url: '/api/feature/getall',
            }
        )

        console.log(response);
        return response.data.map(
            (c:any): Feature => ({
                Id: c.id,
                Name: c.name,
            })
        );
    }
}

const featureService = new FeatureService();
export default featureService;