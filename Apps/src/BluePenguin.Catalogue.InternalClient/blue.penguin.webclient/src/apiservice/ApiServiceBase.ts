import { IResponseBase } from "@/types/ApiRequestResponseTypes";
import { AxiosRequestConfig, AxiosResponse } from "axios";
import HttpClient from "@/apiService/HttpClient";


export abstract class ApiServiceBase
{
    private httpClient : HttpClient;

    constructor(){
        this.httpClient = new HttpClient();
    }

    protected async invoke<T extends IResponseBase,R = AxiosResponse<T>>(request:AxiosRequestConfig):Promise<T> {
        return this.httpClient.invoke(request);
    }
}