export interface IResponseBase {
    hasError: boolean,
    status?: number,
    errors?: Array<string>
}

export interface IGetAllProductsResponse extends IResponseBase {
    name : string,
    category : string,
    collection : string
}
export interface IRegisterUserRequest {
    userName: string,
    displayName?: string,
    password: string,
}

export interface IRegisterUserResponse extends IResponseBase {
    data: {
        userName: string
    }
}

export interface IValidateUserRequest {
    userName: string,
    password: string
}


export interface IValidateUserResponse extends IResponseBase {
    data: {
        token: string,
        isAuthenticated: boolean,
        loginTime: Date,
        userName: string,
        displayName? : string,
        bio? : string
    }
}

export interface IChangePasswordRequest {
    oldPassword : string,
    newPassword : string
}

export interface IChangePasswordResponse extends IResponseBase{
    data: {
        
    }
}

