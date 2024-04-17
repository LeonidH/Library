import { Action, ThunkAction, configureStore } from "@reduxjs/toolkit";
import storage from "redux-persist/lib/storage"; 
import { persistReducer } from "redux-persist";
import authReducer from "./features/auth/authSlice";

const authPersistConfig = {
  key: "auth",
  storage:storage,
};

export const store = configureStore({
    reducer: {
      auth: persistReducer(authPersistConfig, authReducer)
    },
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;

export * from "./features/auth/authSlice";
export * from "./features/auth/auth.API";