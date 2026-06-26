import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { userService } from '../services/userService.jsx';
import { useAuth } from '../context/AuthContext.jsx';

export default function Login() {
    const [login, setLogin] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const { login: saveToken } = useAuth();
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError('');
        try {
            const data = await userService.login(login, password);
            saveToken(data.token);
            navigate('/dashboard');
        } catch (err) {
            setError(err.response?.data?.message || 'Ошибка входа');
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Вход в DOCSeal</h2>
            {error && <p style={{color: 'red'}}>{error}</p>}
            <input placeholder="Логин" value={login} onChange={e => setLogin(e.target.value)} required />
            <input type="password" placeholder="Пароль" value={password} onChange={e => setPassword(e.target.value)} required />
            <button type="submit">Войти</button>
        </form>
    );
}