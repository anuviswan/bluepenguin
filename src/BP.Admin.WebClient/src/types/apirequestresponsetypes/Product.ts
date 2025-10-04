import type {IResponseBase} from "./Response.ts";
import type {Category, Material} from "../ProductTypes.ts";
export interface ICategoryListResponse extends  IResponseBase<Category[]>{
}

export interface IMaterialListResponse extends IResponseBase<Material[]> {

}

