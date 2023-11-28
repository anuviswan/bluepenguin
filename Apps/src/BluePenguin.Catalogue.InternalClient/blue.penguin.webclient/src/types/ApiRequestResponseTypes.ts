import {IProduct} from "@/types/UserTypes"
export interface IResponseBase {
    hasError: boolean,
    status?: number,
    errors?: Array<string>
}


export interface IGetAllProductsResponse extends IResponseBase {
    Products : IProduct[]
}


