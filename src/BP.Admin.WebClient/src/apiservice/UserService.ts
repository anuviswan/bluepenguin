import {ApiServiceBase} from "./ApiServiceBase.ts";
import type {IValidateUserRequest, IValidateUserResponse} from "../types/apirequestresponsetypes/User.ts";

class UserService extends ApiServiceBase {
    public async validateUser(
        user: IValidateUserRequest
    ): Promise<IValidateUserResponse> {
        const response = await this.invoke<IValidateUserResponse>({
            method: 'post',
            url: '/api/User/ValidateUser',
            data: user,
        });
        console.log('Response from validation');
        console.log(response);
        return response;
    }
}

const userService = new UserService();
export default userService;