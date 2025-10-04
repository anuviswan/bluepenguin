import type {IResponseBase} from "./Response.ts";
import type {Category} from "../ProductTypes.ts";
export interface ICategoryListResponse extends  IResponseBase{
    Categories: Category[];
}

