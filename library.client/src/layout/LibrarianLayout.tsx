import { BooksTable } from "../components/BooksTable/BooksTable";

import './LibrarianLayout.scss'

export const LibrarianLayout = () => {
    return (
        <div className="layout">
            <h1>Books</h1>
            <BooksTable />
        </div>
    );
};
