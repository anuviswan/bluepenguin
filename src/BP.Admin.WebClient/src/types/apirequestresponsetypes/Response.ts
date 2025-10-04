export interface IResponseBase<T> {
    hasError: boolean;
    status?: number;
    errors?: Array<string>;
    data: T;
}