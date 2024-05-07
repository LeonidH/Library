import { Button, TextInput } from '@gravity-ui/uikit';
import { useCallback, useEffect, useState } from 'react';
import { ExclamationShape } from '@gravity-ui/icons';

import './LoginRegistrationForm.scss';
import { useAppDispatch, useAppSelector } from '../../store/hooks';
import { login, register, resetToken, selectAuth } from '../../store';

export const LoginRegistrationForm = () => {
    const dispatch = useAppDispatch();
    const auth = useAppSelector(selectAuth);

    useEffect(() => {
        dispatch(resetToken());
    }, [dispatch]);

    const [email, setEmail] = useState('librarian@library.com');
    const [password, setPassword] = useState('Password123!');

    const handleEmailChange = useCallback(
        (e: React.ChangeEvent<HTMLInputElement>) => {
            setEmail(e.target.value);
        },
        []
    );

    const handlePasswordChange = useCallback(
        (e: React.ChangeEvent<HTMLInputElement>) => {
            setPassword(e.target.value);
        },
        []
    );

    console.log('Auth: ', auth);

    return (
        <div className="loginregistration-form">
            <TextInput
                label="Email"
                type="email"
                value={email}
                onChange={handleEmailChange}
            />
            <TextInput
                label="Password"
                type="password"
                value={password}
                onChange={handlePasswordChange}
            />
            <div className="loginregistration-form__actions">
                <Button
                    view="outlined-action"
                    disabled={!password || !email}
                    onClick={() => {
                        dispatch(login({ email, password }));
                    }}
                >
                    Login
                </Button>
                <Button
                    view="outlined-action"
                    disabled={!password || !email}
                    onClick={() => {
                        dispatch(register({ email, password }));
                    }}
                >
                    Register
                </Button>
            </div>
            {auth.authMessages && (
                <ul className='error-messages'>
                    {Object.keys(auth.authMessages).reduce(
                        (acc: React.ReactNode[], key) => {
                            if (!auth.authMessages) {
                                return acc;
                            }
                            return acc.concat(
                                auth.authMessages[key].map((message, index) => (
                                    <li key={index}>
                                        <ExclamationShape />
                                        {message}
                                    </li>
                                ))
                            );
                        },
                        []
                    )}
                </ul>
            )}
        </div>
    );
};
