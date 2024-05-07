import axios from 'axios';
import { Book } from './bookSlice';
import { ODataResult } from '../../../app/models';

export const fetchBooksApiCall = async (query: string) => {
    const url = `/odata/books${query}`;
    const response = await axios.get<ODataResult<Book[]>>(url).catch((ex) => {
        console.log(ex);
    });
    return response?.data;
};

export const addBooksApiCall = async (book: Book) => {
    const url = `/odata/books`;
    const response = await axios.post<Book>(url, book).catch((ex) => {
        console.log(ex);
    });
    return response?.data;
};

export const updateBooksApiCall = async (book: Book) => {
    const url = `/odata/books`;
    const response = await axios.put<Book>(url, book).catch((ex) => {
        console.log(ex);
    });
    return response?.data;
};

export const deleteBookApiCall = async (id: string) => {
    const url = `/odata/books(${id})`;
    await axios.delete(url).catch((ex) => {
        console.log(ex);
    });
};

