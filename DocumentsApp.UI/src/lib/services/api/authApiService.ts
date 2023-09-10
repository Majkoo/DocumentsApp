import {callApi} from "$lib/services/api/callApi";
import type {JwtDataDto} from "$lib/models/dtos/auth/JwtDataDto";
import {appSettings} from "$lib/configs/appSettings";
import type {LoginDto} from "$lib/models/dtos/auth/LoginDto";
import type {RegisterDto} from "$lib/models/dtos/auth/RegisterDto";

export const login = async (loginData: LoginDto) => {
    await callApi<JwtDataDto>(
        `${appSettings.apiUrl}/auth/login`,
        "POST",
        loginData
    )
}

export const register = async (registerData: RegisterDto) => {
    await callApi<boolean>(
        `${appSettings.apiUrl}/auth/register`,
        "POST",
        registerData
    )
}

export const refreshLogin = async (refreshToken: string) => {
    await callApi<JwtDataDto>(
        `${appSettings.apiUrl}/auth/refresh`,
        "POST",
        refreshToken
    )
}
