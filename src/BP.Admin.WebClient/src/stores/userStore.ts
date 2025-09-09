import type {LoggedInUser} from "../types/StoreTypes.ts"
import { defineStore } from "pinia";
import { computed, ref } from "vue";

export const useUserStore = defineStore('UserStore',()=>{

    // state
    const loggedInUser = ref<LoggedInUser>({
        userName : '',
        token : '',
    });

    // getters
    const UserName = computed(()=> loggedInUser.value.userName);
    const Token = computed(( )=> loggedInUser.value.token)

    // methods
    const SaveUser = (user:LoggedInUser):void=>{
        loggedInUser.value = user;
    }

    const Reset = ():void => {
        loggedInUser.value = {
            userName : '',
            token : '',
        } as LoggedInUser;

    }

    return {
        loggedInUser,
        UserName,
        Token,
        SaveUser,
        Reset
    };
});