import type {IResponseBase} from "./Response.ts";
import type {Category, Collection, Feature, Material} from "../ProductTypes.ts";
export interface ICategoryListResponse extends  IResponseBase<Category[]>{
}

export interface IMaterialListResponse extends IResponseBase<Material[]> {}

export interface IFeatureListResponse extends IResponseBase<Feature[]>{}

export interface ICollectionListResponse extends IResponseBase<Collection[]> {}

