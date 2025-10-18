import {
    type IResponseBase,
} from '../types/apirequestresponsetypes/Response';
import { type AxiosRequestConfig } from 'axios';
import HttpClient from './HttpClient';

export abstract class ApiServiceBase {
    private httpClient: HttpClient;

    constructor() {
        this.httpClient = new HttpClient();
    }

    protected async invoke<T>(
        request: AxiosRequestConfig
    ): Promise<IResponseBase<T>> {
        return this.httpClient.invoke(request);
    }

    protected async getBlob<T>(request: AxiosRequestConfig): Promise<T | null> {
        return this.httpClient.getBlob(request);
    }

}
