import type {IResponseBase} from "./Response.ts";

export interface IValidateUserRequest {
    userName: string;
    password: string;
}

export interface IValidateUserResponse extends IResponseBase {
    data: {
        token: string;
        isAuthenticated: boolean;
        loginTime: Date;
        userName: string;
        displayName?: string;
        bio?: string;
        followers: string[];
    };
}