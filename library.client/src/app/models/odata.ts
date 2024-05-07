export type ODataResult<T> = {
    readonly '@odata.context': string;
    readonly '@odata.nextLink'?: string;
    readonly '@odata.count'?: number;
    readonly value: T;
  }