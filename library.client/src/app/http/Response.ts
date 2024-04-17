export interface Messages {
    [key: string]: string[];
}
export interface AppResponse<T> {
    data?: T;
    messages: Messages;
    isSucceed: boolean;
}

export interface Error{
    response?: {
        data: never;
        status: number;
    },
}