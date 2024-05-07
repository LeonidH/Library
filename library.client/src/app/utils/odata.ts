import { QueryOptions } from "odata-query";
import { IPagination, ISorting } from "../models";

export const convertToOdataOptions = <T>({ sorting, pagination, count }
    : { sorting?: ISorting, pagination?: IPagination, count?: boolean })
    : Partial<QueryOptions<T>> => {
    const options: Partial<QueryOptions<T>> = {};
  
    if (sorting) {
      const sortableProperties = sorting.sortBy.split(',');
      options.orderBy = sortableProperties.map(prop => `${prop}${sorting.sortByDescending ? ' desc' : ''}`).join();
    }
  
    if (pagination && pagination.pageSize > 0) {
      const page = pagination.currentPage;
      const perPage = pagination.pageSize;
      options.top = perPage;
      options.skip = perPage * (page - 1);
    }
    options.count = count;
    return options;
  }