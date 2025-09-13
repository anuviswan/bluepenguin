export interface IResponseBase {
    hasError: boolean;
    status?: number;
    errors?: Array<string>;
}