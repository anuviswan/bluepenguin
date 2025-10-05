import type {
    IResponseBase
} from '../types/apirequestresponsetypes/Response';
import axios, { type AxiosInstance, type AxiosRequestConfig } from 'axios';
import { useUserStore } from '../stores/userStore';

class HttpClient {
    private axiosInstance: AxiosInstance;

    constructor() {
        const headers = {
            //"Access-Control-Allow-Origin": "*",
            //"Access-Control-Allow-Headers": "*", // this will allow all CORS requests
            //"Access-Control-Allow-Methods": "OPTIONS,POST,GET", // this states the allowed methods
            'Content-Type': 'application/json', // this shows the expected content type
        };

        console.log('base URL' + import.meta.env.VITE_APP_API_URL);
        this.axiosInstance = axios.create({
            baseURL: import.meta.env.VITE_APP_API_URL,
            headers: headers,
        });

        this.axiosInstance.interceptors.request.use(function (config) {
            const userStoreInstance = useUserStore();
            if (userStoreInstance.Token) {
                console.log('Submitting with token ' + userStoreInstance.Token);
                config.headers.Authorization = `Bearer ${userStoreInstance.Token}`;
            } else {
                console.log('Token not available');
            }
            return config;
        });
    }

    public async invoke<T>(
        config: AxiosRequestConfig
    ): Promise<IResponseBase<T>> {
        try {
            const response = await this.axiosInstance.request<T>(config);
            return {
                data: response.data
            } as IResponseBase<T>;
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                return {
                    status: error.response?.status,
                    hasError: true,
                    errors: error.response?.data.errors,
                } as IResponseBase<T>;
            } else {
                console.log('Some other error ?? ' + error);
            }
        }
        return {} as IResponseBase<T>;
    }
    public async getBlob<T>(config: AxiosRequestConfig): Promise<T | null> {
        try {
            console.log('Sending profile image request for:', config);
            const response = await this.axiosInstance.request<T>(config);

            if (response.status == 204) {
                // No Content
                return null;
            }

            return response.data;
        } catch (error: unknown) {
            if (axios.isAxiosError(error)) {
                return null;
            } else {
                console.log('Some other error ?? ' + error);
            }
        }
        return { } as T;
    }


}

export default HttpClient;
