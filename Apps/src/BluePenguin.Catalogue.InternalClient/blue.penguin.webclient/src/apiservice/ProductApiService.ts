import { IGetAllProductsResponse} from "@/types/ApiRequestResponseTypes";
import {IProduct} from "@/types/UserTypes"
import { ApiServiceBase } from "./ApiServiceBase";

class ProductApiService extends ApiServiceBase {

    public async getAllProducts():Promise<IProduct[]>{
        
        const result =  await this.invoke<IGetAllProductsResponse>({method:'get', url:"api/GetAllProducts"});

        const response = result.map(x=> {
            const product : IProduct = { 
                name : x.Name,
                collection : x.Collection,
                category : x.Category,
              };

            return product;
        });
        
        return response;
    }
}

export const productsApiService = new ProductApiService();