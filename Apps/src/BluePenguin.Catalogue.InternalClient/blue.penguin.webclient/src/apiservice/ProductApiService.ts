import { IGetAllProductsResponse} from "@/types/ApiRequestResponseTypes";
import {IProduct} from "@/types/UserTypes"
import { ApiServiceBase } from "@/apiService/ApiServiceBase";

class ProductApiService extends ApiServiceBase {

    public async getAllProducts():Promise<IProduct[]>{
        
        const result =  await this.invoke<IGetAllProductsResponse[]>({method:'get', url:import.meta.env.VITE_API_GETALLPRODUCTS});

        console.log(result)
        const response = result.map(x => {
                const product : IProduct = { 
                    name : x.Name,
                    collection : x.Collection,
                    category : x.Category,
                    discountPrice : x.DiscountPrice,
                    mrp : x.MRP,
                    tags: x.Tags,
                    url:''
                  };
    
                return product;
            })
        
        
        
        return response;
    }
}

export const productsApiService = new ProductApiService();