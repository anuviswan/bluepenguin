import { IGetAllProductsResponse} from "../types/ApiRequestResponseTypes";
import { ApiServiceBase } from "./ApiServiceBase";

class ProductApiService extends ApiServiceBase {

    public async getAllProducts():Promise<IGetAllProductsResponse>{
        return await this.invoke<IGetAllProductsResponse>({method:'get', url:"/api/User/ValidateUser"});
    }
}

export const userApiService = new ProductApiService();