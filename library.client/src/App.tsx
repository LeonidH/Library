import { AuthState, selectAuth } from './store';
import { useAppSelector } from './store/hooks';

import './App.scss';
import { Route, Routes } from 'react-router-dom';
import { DefaultLayout } from './layout/DefaultLayout';
import { UserLayout } from './layout/UserLayout';
import { AdminLayout } from './layout/AdminLayout';
import { LibrarianLayout } from './layout/LibrarianLayout';

const getAuthorzedUserLayout = (auth: AuthState) => {
    switch (auth.user?.UserRole.toLowerCase()) {
        case 'administrator':
            return <AdminLayout />;
        case 'user':
            return <UserLayout />;
        case 'librarian':
            return <LibrarianLayout />;
        default:
            return <DefaultLayout />;
    }
};

function App() {
    const auth = useAppSelector(selectAuth);

    return (
        <Routes>
            {auth.user ? (
                <Route
                    path="/"
                    element={getAuthorzedUserLayout(auth)}
                ></Route>
            ) : (
                <Route path="/" element={<DefaultLayout />}></Route>
            )}
        </Routes>
    );
}

export default App;
