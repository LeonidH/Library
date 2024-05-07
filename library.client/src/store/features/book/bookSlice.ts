import { PayloadAction, createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import buildQuery from 'odata-query';
import { IPagination, ISorting, ODataResult } from '../../../app/models';
import { RootState } from '../..';
import { convertToOdataOptions } from '../../../app/utils/odata';
import { addBooksApiCall, deleteBookApiCall, fetchBooksApiCall, updateBooksApiCall } from './book.API';

export interface Book {
    id?: string;
    title: string;
    author: string;
}

export interface BookState {
    books: Book[];
    pagination: IPagination;
    sorting: ISorting;
    search?: string;
    status: 'idle' | 'loading' | 'failed';
}

const defaultSorting: ISorting = {
    sortBy: 'title',
    sortByDescending: false,
};

const initialState: BookState = {
    books: [],
    pagination: {
        currentPage: 1,
        pageSize: 10,
        totalCount: 0,
    },
    sorting: defaultSorting,
    status: 'loading',
};

const getQuery = (state: RootState) => {
    const {
        books: { pagination, sorting, search },
    } = state;

    const options = convertToOdataOptions<Book>({
        count: pagination.currentPage === 1 && pagination.pageSize > 0,
        pagination: pagination,
        sorting: sorting,
    });

    const filter = [
        {
            or: [
                { title: { contains: search } },
                { author: { contains: search } },
            ],
        },
    ];

    options.filter = filter;

    const query = buildQuery(options);

    return query;
};

export const loadBooks = createAsyncThunk<
    ODataResult<Book[]> | undefined,
    void,
    { state: RootState }
>('book/loadBooks', async (_, { getState }) => {
    const query = getQuery(getState());
    return await fetchBooksApiCall(query);
});

export const addBook = createAsyncThunk<
    ODataResult<Book[]> | undefined,
    Book,
    { state: RootState }
>('book/addBook', async (book, { getState }) => {
    await addBooksApiCall(book);
    const query = getQuery(getState());
    return await fetchBooksApiCall(query);
});

export const updateBook = createAsyncThunk<
    Book | undefined,
    Book,
    { state: RootState }
>('book/updateBook', async (book: Book) => {
    return await updateBooksApiCall(book);
});

export const deleteBook = createAsyncThunk<
    ODataResult<Book[]> | undefined,
    string,
    { state: RootState }
>('book/deleteBook', async (id, { getState }) => {
    await deleteBookApiCall(id);
    const query = getQuery(getState());
    return await fetchBooksApiCall(query);
});

export const bookSlice = createSlice({
    name: 'book',
    initialState,
    reducers: {
        addBook: (state, action: PayloadAction<Book>) => {
            state.books.push(action.payload);
        },
        setCurrentPage: (state, action: PayloadAction<number>) => {
            state.pagination.currentPage = action.payload;
        },
        setSorting: (state, action: PayloadAction<ISorting | undefined>) => {
            state.sorting = action.payload ?? defaultSorting;
        },
        setSearch: (state, action: PayloadAction<string | undefined>) => {
            state.search = action.payload;
            state.pagination.currentPage = 1;
        },
    },
    extraReducers: (builder) => {
        builder.addCase(loadBooks.pending, (state) => {
            state.status = 'loading';
        });
        builder.addCase(loadBooks.fulfilled, (state, action) => {
            state.status = 'idle';
            state.books = action.payload?.value || [];

            const totalCount = action.payload?.['@odata.count'];
            if (totalCount != undefined) {
                state.pagination.totalCount = totalCount;
            }
        });
        builder.addCase(deleteBook.pending, (state) => {
            state.status = 'loading';
        });
        builder.addCase(deleteBook.fulfilled, (state, action) => {
            state.status = 'idle';
            state.books = action.payload?.value || [];

            const totalCount = action.payload?.['@odata.count'];
            if (totalCount != undefined) {
                state.pagination.totalCount = totalCount;
            }
        });
        builder.addCase(updateBook.pending, (state) => {
            state.status = 'loading';
        });
        builder.addCase(updateBook.fulfilled, (state, action) => {
            state.status = 'idle';
            const updatedBook = action.payload;

            if(!updatedBook) return;

            state.books = state.books.map((book) => {
                if (book.id === updatedBook.id) {
                    return updatedBook;
                }
                return book;
            });
        });
        builder.addCase(addBook.pending, (state) => {
            state.status = 'loading';
        });
        builder.addCase(addBook.fulfilled, (state, action) => {
            state.status = 'idle';
            state.books = action.payload?.value || [];

            const totalCount = action.payload?.['@odata.count'];
            if (totalCount != undefined) {
                state.pagination.totalCount = totalCount;
            }
        });
    },
});

export const { setCurrentPage, setSearch, setSorting } = bookSlice.actions;

export const selectBooks = (state: RootState) => state.books;
export default bookSlice.reducer;
