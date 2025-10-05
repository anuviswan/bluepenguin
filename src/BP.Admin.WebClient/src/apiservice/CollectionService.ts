import {ApiServiceBase} from "./ApiServiceBase.ts";
import type {ICollectionListResponse} from "../types/apirequestresponsetypes/Product.ts";
import type {Collection} from "../types/ProductTypes.ts";

class CollectionService extends  ApiServiceBase{

    public async getFeatures(): Promise<Collection[]> {

        const response = await this.invoke<ICollectionListResponse>(
            {
                method: 'get',
                url: '/api/collection/getall',
            }
        )

        console.log(response);
        return response.data.map(
            (c:any): Collection => ({
                Id: c.id,
                Name: c.name,
            })
        );
    }
}

const collectionService = new CollectionService();
export default collectionService;