import { IResponseBase } from "@/types/ApiRequestResponseTypes";
import axios, {AxiosInstance, AxiosRequestConfig, AxiosError, AxiosResponse} from "axios";
import {useUserStore} from "@/stores/userStore";

class HttpClient{

    private axiosInstance : AxiosInstance;
    
    constructor(){
        const headers = {
            "Access-Control-Allow-Origin": "*",
            "Access-Control-Allow-Headers": "*", // this will allow all CORS requests
            "Access-Control-Allow-Methods": "OPTIONS,POST,GET", // this states the allowed methods
            "Content-Type": "application/json", // this shows the expected content type
          };
        this.axiosInstance = axios.create({baseURL: "http://localhost:8001/", headers:headers});

        

        
        this.axiosInstance.interceptors.request.use(function (config) 
        {
            const userStoreInstance = useUserStore();
            if(userStoreInstance.Token){
                console.log("Submitting with token " + userStoreInstance.Token)
                config.headers.Authorization  = `Bearer ${userStoreInstance.Token}`; 
            }
            else{
                console.log("Token not available")
            }
            return config;
        });
    }

    public async invoke<T extends IResponseBase,R = AxiosResponse<T>>(config:AxiosRequestConfig):Promise<T> {

        try{
            const response =  await this.axiosInstance.request<T>(config); 

            console.log(response.data)
            return response.data;
        }catch(error : AxiosError | any){

            if(axios.isAxiosError(error)){
                return <T>{
                    status : error.response?.status,
                    hasError : true,
                    errors : error.response?.data.errors
                } 
            }
            else{
                console.log("Some other error ?? " + error);
            }
        } 

        return <T>{};

        
    }
}

export default HttpClient;