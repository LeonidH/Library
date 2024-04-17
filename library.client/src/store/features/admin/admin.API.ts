import axios from "axios";
import { User } from "../..";

export const getUsersApiCall = async (page: number, pageSize: number) => {
    const response = await axios
        .get<{users: User[], total: number}>(
            `/user/get?page=${page}&pageSize=${pageSize}`
        )
        .catch((ex) => {
            console.log(ex);
        });
    return response?.data;
};