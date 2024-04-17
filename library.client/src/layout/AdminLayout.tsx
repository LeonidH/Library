import { UsersTable } from "../components/UsersTable/UsersTable";

export const AdminLayout = () => {
    return (
        <div>
            <h1>Library users</h1>
            <UsersTable />
        </div>
    );
}