import { PayloadAction, createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { RootState } from '../..';
import { jwtDecode } from 'jwt-decode';
import { AppResponse, Messages } from '../../../app/http';
import { loginApiCall, logoutApiCall, refreshTokenApiCall, registerApiCall } from './auth.API';

export interface User {
    id: string;
    RoleClaim: Array<string>;
    UserName: string;
    UserRole: string;
}
export interface AuthState {
    status: 'idle' | 'loading' | 'failed';
    accessToken?: string;
    refreshToken?: string;
    user?: User;
    authMessages?: Messages;
}

const initialState: AuthState = {
    status: 'idle',
    accessToken: undefined,
    refreshToken: undefined,
    user: undefined,
    authMessages: undefined,
};

export const login = createAsyncThunk<
    | AppResponse<{
          accessToken: string;
          refreshToken: string;
      }>
    | undefined,
    { email: string; password: string },
    { state: RootState }
>('auth/login', async ({ email, password }) => {
    return await loginApiCall(email, password);
});

export const refreshToken = createAsyncThunk<
    | AppResponse<{
          accessToken: string;
          refreshToken: string;
      }>
    | undefined,
    { accessToken: string; refreshToken: string },
    { state: RootState }
>('auth/refreshToken', async ({ accessToken, refreshToken }) => {
    return await refreshTokenApiCall({ accessToken, refreshToken });
});

export const logout = createAsyncThunk<
    AppResponse<boolean> | undefined,
    void,
    { state: RootState }
>('auth/logout', async () => {
    return await logoutApiCall();
});

export const register = createAsyncThunk<
    AppResponse<object> | undefined,
    { email: string; password: string }
>('auth/register', async ({ email, password }) => {
    return await registerApiCall(email, password);
});

export const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        updateToken: (
            state,
            action: PayloadAction<{ accessToken: string; refreshToken: string }>
        ) => {
            state.accessToken = action.payload.accessToken;
            state.refreshToken = action.payload.refreshToken;
            state.user = jwtDecode<User>(action.payload.accessToken);
        },
        resetToken: (state) => {
            state.accessToken = undefined;
            state.refreshToken = undefined;
            state.user = undefined;
            state.authMessages = undefined;
        },
        setLoading: (state) => {
            state.status = 'loading';
            state.authMessages = undefined;
        },
        resetLoading: (state) => {
            state.status = 'idle';
        },
    },
    extraReducers: (builder) => {
        builder.addCase(login.pending, (state) => {
            state.status = 'loading';
            state.authMessages = undefined;
        });
        builder.addCase(login.fulfilled, (state, action) => {
            if (
                action.payload &&
                action.payload.isSucceed &&
                action.payload.data
            ) {
                state.accessToken = action.payload.data.accessToken;
                state.refreshToken = action.payload.data.refreshToken;
                state.user = jwtDecode<User>(action.payload.data.accessToken, {

                });
                state.status = 'idle';
            } else {
                state.status = 'failed';
                state.authMessages = action.payload?.messages;
            }
        });
        builder.addCase(login.rejected, (state) => {
            state.status = 'failed';
        });
        builder.addCase(refreshToken.pending, (state) => {
            state.status = 'loading';
            state.authMessages = undefined;
        });
        builder.addCase(refreshToken.fulfilled, (state, action) => {
            if (
                action.payload &&
                action.payload.isSucceed &&
                action.payload.data
            ) {
                state.accessToken = action.payload.data.accessToken;
                state.refreshToken = action.payload.data.refreshToken;
                state.user = jwtDecode<User>(action.payload.data.accessToken);
                state.status = 'idle';
            } else {
                state.status = 'failed';
                state.authMessages = action.payload?.messages;
            }
        });
        builder.addCase(refreshToken.rejected, (state) => {
            state.status = 'failed';
        });
        builder.addCase(logout.pending, (state) => {
            state.status = 'loading';
            state.authMessages = undefined;
        });
        builder.addCase(logout.fulfilled, (state, action) => {
            if (action.payload && action.payload.isSucceed) {
                state.accessToken = undefined;
                state.refreshToken = undefined;
                state.user = undefined;
                state.status = 'idle';
            } else {
                state.status = 'failed';
                state.authMessages = action.payload?.messages;
            }
        });
        builder.addCase(logout.rejected, (state) => {
            state.status = 'failed';
        });
        builder.addCase(register.pending, (state) => {
            state.status = 'loading';
            state.authMessages = undefined;
        });
        builder.addCase(register.fulfilled, (state, action) => {
            if (action.payload && action.payload.isSucceed) {
                state.status = 'idle';
            } else {
                state.status = 'failed';
                state.authMessages = action.payload?.messages;
            }
        });
        builder.addCase(register.rejected, (state) => {
            state.status = 'failed';
        });
    },
});

export const { updateToken, resetToken, setLoading, resetLoading } =
    authSlice.actions;
export const selectAuth = (state: RootState) => state.auth;
export default authSlice.reducer;
