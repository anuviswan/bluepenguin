import {ApiServiceBase} from "./ApiServiceBase.ts";
import type {Product} from "../types/ProductTypes.ts";

class ProductService extends  ApiServiceBase{

        public async createProduct(product: Product): Promise<Product> {
            return await this.invoke<Product>({
                method: "POST",
                url: "/api/Product/create",
                data: product,
            })
        }
}

const productService = new ProductService();
export default productService;