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
        userId: string;
    };
}