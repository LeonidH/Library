import {
    Table,
    withTableActions,
    PaginationProps,
    Pagination,
} from '@gravity-ui/uikit';
import { User } from '../../store';
import React from 'react';
import { getUsersApiCall } from '../../store/features/admin/admin.API';

const CustomnTable = withTableActions(Table);

const columns = [{ id: 'userName' }, { id: 'email' }];

// const RowAction = (item: TableDataItem) => [
//     {
//         text: 'Action 1',
//         handler: () => console.log('Action 1 clicked: ', item.text),
//     },
// ];

export const UsersTable = () => {
    const [state, setState] = React.useState({ page: 1, pageSize: 100 });
    const [users, setUsers] = React.useState<User[]>([]);
    const [total, setTotal] = React.useState(0);

    const handleUpdate: PaginationProps['onUpdate'] = (page, pageSize) =>
        setState((prevState) => ({ ...prevState, page, pageSize }));

    React.useEffect(() => {
        getUsersApiCall(state.page, state.pageSize).then((response) => {
            const { users, total } = response as { users: User[]; total: number };
            setUsers(users);
            setTotal(total);
        });
    }, [state.page, state.pageSize]);
    return (
        <>
            <CustomnTable
                data={users}
                columns={columns}
            />
            <Pagination
                page={state.page}
                pageSize={state.pageSize}
                total={total}
                onUpdate={handleUpdate}
            />
        </>
    );
};
