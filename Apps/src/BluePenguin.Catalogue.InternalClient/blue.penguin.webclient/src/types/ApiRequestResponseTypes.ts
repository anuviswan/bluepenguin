export interface IResponseBase {
    hasError: boolean,
    status?: number,
    errors?: Array<string>
}


export interface IGetAllProductsResponse extends IResponseBase {
    name: string;
    url: string;
    category: string;
    collection: string;
}


