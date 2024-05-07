import {
    ChangeEvent,
    memo,
    useCallback,
    useEffect,
    useMemo,
    useState,
} from 'react';
import {
    Button,
    Icon,
    Loader,
    Label,
    Modal,
    Pagination,
    Table,
    TextInput,
    withTableActions,
    withTableSorting,
    TableDataItem,
    TableActionConfig,
} from '@gravity-ui/uikit';
import { Plus, Xmark } from '@gravity-ui/icons';
import { useAppDispatch, useAppSelector } from '../../store/hooks';
import {
    Book,
    addBook,
    deleteBook,
    loadBooks,
    selectBooks,
    setCurrentPage,
    setSearch,
    setSorting,
    updateBook,
} from '../../store/features/book/bookSlice';
import './BooksTable.scss';
import { useDebouncedCallback } from 'use-debounce';

type ColumnSortOrder = 'asc' | 'desc';
interface ColumnSortState {
    column: string;
    order: ColumnSortOrder;
}
type SortState = ColumnSortState[];

const SortableTable = withTableActions(withTableSorting(Table));

enum ModalDialogMode {
    Create = 'Create',
    Update = 'Update',
}

const Toolbar = memo(({ onAddNewBook }: { onAddNewBook?: () => void }) => {
    const dispatch = useAppDispatch();
    const { search } = useAppSelector(selectBooks);

    const searchCallback = useCallback(
        (value: string | undefined) => {
            if (value !== search) {
                dispatch(setSearch(value));
                dispatch(loadBooks());
            }
        },
        [dispatch, search]
    );

    const searchDebounced = useDebouncedCallback(searchCallback, 500);

    const onChange = useCallback(
        (evnt: ChangeEvent<HTMLInputElement>) => {
            searchDebounced(evnt.target.value);
        },
        [searchDebounced]
    );

    return (
        <div className="toolbar_books">
            <Button view="outlined-action" onClick={onAddNewBook}>
                Add
                <Icon data={Plus} size={18} />
            </Button>
            <TextInput
                defaultValue={search}
                onChange={onChange}
                autoFocus
                className="toolbar_books__search"
                placeholder="Search"
                endContent={
                    <Button
                        onClick={() => {
                            searchCallback('');
                        }}
                        view="flat"
                        size="xs"
                    >
                        <Icon data={Xmark} size={12} />
                    </Button>
                }
            />
        </div>
    );
});

const BookCreateUpdateModalDialog = memo(
    ({
        modalDlgMode,
        data,
        onSubmit,
        onClose,
    }: {
        modalDlgMode: ModalDialogMode | undefined;
        data?: Book;
        onSubmit: (book: Book) => void;
        onClose: () => void;
    }) => {
        const [title, setTitle] = useState('');
        const [author, setAuthor] = useState('');

        useEffect(() => {
            if (data) {
                setTitle(data.title);
                setAuthor(data.author);
            }
        }, [data]);

        return (
            <Modal
                className="modal_update-book"
                open={!!modalDlgMode}
                onClose={() => onClose()}
            >
                <div className="modal_update-book__header">
                    <Label theme="utility">
                        {modalDlgMode === ModalDialogMode.Create
                            ? 'Add'
                            : 'Update'}{' '}
                        book
                    </Label>
                    <Button view="outlined" size="l" onClick={() => onClose()}>
                        <Icon data={Xmark} size={18} />
                    </Button>
                </div>
                <div className="modal_update-book__content">
                    <TextInput
                        label="Title"
                        value={title}
                        onChange={(evnt: ChangeEvent<HTMLInputElement>) => {
                            setTitle(evnt.target.value);
                        }}
                    />
                    <TextInput
                        label="Author"
                        value={author}
                        onChange={(evnt: ChangeEvent<HTMLInputElement>) => {
                            setAuthor(evnt.target.value);
                        }}
                    />
                    <Button
                        view="outlined-action"
                        onClick={() => {
                            onClose();
                            onSubmit({
                                ...(data ?? {}),
                                title,
                                author,
                            });
                        }}
                    >
                        {modalDlgMode === ModalDialogMode.Create
                            ? 'Add'
                            : 'Update'}
                    </Button>
                </div>
            </Modal>
        );
    }
);

export const BooksTable = memo(() => {
    const dispatch = useAppDispatch();
    const {
        books,
        status,
        sorting,
        pagination: { currentPage, totalCount, pageSize },
    } = useAppSelector(selectBooks);
    const [modalDlgMode, setModalDlgMode] = useState<
        ModalDialogMode | undefined
    >(undefined);

    const [bookToUpdate, setBookToUpdate] = useState<Book | undefined>();

    useEffect(() => {
        dispatch(loadBooks());
    }, [dispatch]);

    const handleUpdate = useCallback(
        (page: number) => {
            dispatch(setCurrentPage(page));
            dispatch(loadBooks());
        },
        [dispatch]
    );

    const getRowActions = useCallback(
        (item: TableDataItem): TableActionConfig<TableDataItem>[] => {
            return [
                {
                    text: 'Update',
                    handler: () => {
                        setBookToUpdate(item as Book);
                        setModalDlgMode(ModalDialogMode.Update);
                    },
                    theme: 'normal',
                },
                {
                    text: 'Remove',
                    handler: () => {
                        dispatch(deleteBook(item.id as string));
                    },
                    theme: 'danger',
                },
            ];
        },
        [dispatch]
    );

    const updateSorting = useCallback(([sortState]: SortState) => {
        dispatch(
            setSorting(
                sortState
                    ? {
                          sortBy: sortState.column,
                          sortByDescending:
                              sortState.order ===
                              'desc',
                      }
                    : undefined
            )
        );
        dispatch(loadBooks());
    }, [dispatch]);

    const columns = useMemo(
        () => [
            {
                id: 'title',
                name: 'Title',
                meta: { sort: true },
            },
            { id: 'author', name: 'Author', meta: { sort: true } },
        ],
        []
    );

    return (
        <>
            {(() => {
                switch (status) {
                    case 'loading':
                        return <Loader />;
                    case 'idle':
                        return (
                            <>
                                <Toolbar
                                    onAddNewBook={() =>
                                        setModalDlgMode(ModalDialogMode.Create)
                                    }
                                />
                                <SortableTable
                                    columns={columns}
                                    defaultSortState={[{
                                        column: sorting.sortBy,
                                        order: sorting.sortByDescending
                                            ? 'desc'
                                            : 'asc',
                                    }]}
                                    data={books}
                                    getRowActions={getRowActions}
                                    onSortStateChange={updateSorting}
                                    className="books-table"
                                />
                                <Pagination
                                    page={currentPage}
                                    pageSize={pageSize}
                                    total={totalCount}
                                    onUpdate={handleUpdate}
                                />
                                <BookCreateUpdateModalDialog
                                    modalDlgMode={modalDlgMode}
                                    data={bookToUpdate}
                                    onSubmit={(book: Book) => {
                                        if (
                                            modalDlgMode ===
                                            ModalDialogMode.Update
                                        ) {
                                            dispatch(updateBook(book));
                                        }
                                        if (
                                            modalDlgMode ===
                                            ModalDialogMode.Create
                                        ) {
                                            dispatch(addBook(book));
                                        }
                                    }}
                                    onClose={() => {
                                        setModalDlgMode(undefined);
                                        setBookToUpdate(undefined);
                                    }}
                                />
                            </>
                        );
                    default:
                        return <div>Failed to load books</div>;
                }
            })()}
        </>
    );
});
