import {ITag} from "@/types/UserTypes"
export interface IResponseBase {
    hasError: boolean,
    status?: number,
    errors?: Array<string>
}


export interface IGetAllProductsResponse extends IResponseBase {
    Name: string;
    Url: string;
    Category: string;
    Collection: string;
    MRP: number;
    DiscountPrice: number;
    Tags:ITag
}


