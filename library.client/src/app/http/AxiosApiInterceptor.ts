import { useEffect } from "react";
import axios from "axios";
import { useAppDispatch, useAppSelector } from "../../store/hooks";
import { resetToken, selectAuth, updateToken } from "../../store";
import { refreshTokenApiCall } from "../../store/features/auth/auth.API";

export const AxiosApiInterceptor = () => {
  const authData = useAppSelector(selectAuth);
  const dispatch = useAppDispatch();
  useEffect(() => {
    const requestInterceptor = axios.interceptors.request.use(
      async (config) => {
        const accessToken = authData.accessToken;
        if (accessToken && !config.headers.Authorization) {
          config.headers.Authorization = `Bearer ${accessToken}`;
        }
        return config;
      }
    );

    const responseInterceptor = axios.interceptors.response.use(
      (response) => response,
      async (error) => {
        if (error.response && error.response.status === 401) {
          if (authData.refreshToken && authData.accessToken) {
            try {
              const response = await refreshTokenApiCall({
                accessToken: authData.accessToken,
                refreshToken: authData.refreshToken,
              });
              if (response && response.isSucceed && response.data) {
                dispatch(updateToken(response.data));
                error.config.headers.Authorization = `Bearer ${response.data.accessToken}`;
                return axios.request(error.config);
              } else {
                dispatch(resetToken());
              }
            } catch (refreshError) {
              dispatch(resetToken());
              throw refreshError;
            }
          } else {
            dispatch(resetToken());
          }
        }

        return Promise.reject(error);
      }
    );

    return () => {
      axios.interceptors.request.eject(requestInterceptor);
      axios.interceptors.response.eject(responseInterceptor);
    };
  }, [authData, dispatch]);

  return null;
};
