import {ApiServiceBase} from "./ApiServiceBase.ts";
import type {Product} from "../types/ProductTypes.ts";

class ProductService extends  ApiServiceBase{

    public async createProduct(product: Product): Promise<string> {

        console.log(product);
        const response = await this.invoke<string>({
            method: "POST",
            url: "/api/Product/create",
            data: product,
        })

        return response.data;
    }

    public async uploadProductImage(
        file: File,
        skuId: string,
        isPrimaryImage: boolean
    ): Promise<string> {

        const formData = new FormData();
        formData.append("File", file);
        formData.append("SkuId", skuId);

        const response = await this.invoke<string>({
            method: "POST",
            url: `/api/FileUpload/uploadproduct?isPrimaryImage=${isPrimaryImage}`,
            data: formData,
            headers: {
                "Content-Type": "multipart/form-data"
            }
        });
        return response.data;
    }

}

const productService = new ProductService();
export default productService;