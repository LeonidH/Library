import axios from 'axios';
import { AppResponse } from '../../../app/http';

export const loginApiCall = async (email: string, password: string) => {
    const response = await axios
        .post<AppResponse<{ accessToken: string; refreshToken: string }>>(
            `/user/login`,
            {
                email: email,
                password: password,
            }
        )
        .catch((ex) => {
            console.log(ex);
        });
    return response?.data;
};

export const refreshTokenApiCall = async (data: {
    accessToken: string;
    refreshToken: string;
}) => {
    const response = await axios
        .post<AppResponse<{ accessToken: string; refreshToken: string }>>(
            `/user/refreshToken`,
            data
        )
        .catch((ex) => {
            console.log(ex);
        });
    return response?.data;
};

export const registerApiCall = async (email: string, password: string) => {
    const response = await axios
        .post<AppResponse<object>>(`/user/register`, {
            email: email,
            password: password,
        })
        .catch((ex) => {
            console.log(ex);
        });
    return response?.data;
};

export const logoutApiCall = async () => {
    const response = await axios
        .post<AppResponse<boolean>>(`/user/logout`)
        .catch((ex) => {
            console.log(ex);
        });
    return response?.data;
};

export const profileApi = async () => {
    const response = await axios
        .post(`/user/profile`)
        .catch((ex) => {
            console.log(ex);
        });
    return response?.data;
};
