import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { userService } from '../services/userService';
import { useAuth } from '../context/AuthContext';
import Input from '../components/ui/Input';
import Button from '../components/ui/Button';
import './Register.css';
import Logotype from '../components/ui/Logo';

export default function Login() {
    const navigate = useNavigate();
    const { login } = useAuth();

    const [form, setForm] = useState({
        login: '',
        password: ''
    });
    const [errors, setErrors] = useState({});
    const [serverError, setServerError] = useState('');
    const [loading, setLoading] = useState(false);

    const handleChange = (field) => (e) => {
        setForm({ ...form, [field]: e.target.value });
        if (errors[field]) setErrors({ ...errors, [field]: '' });
        if (serverError) setServerError('');
    };

    const validate = () => {
        const e = {};
        if (!form.login.trim()) e.login = 'Введите логин';
        if (!form.password) e.password = 'Введите пароль';
        setErrors(e);
        return Object.keys(e).length === 0;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validate()) return;
        setLoading(true);
        try {
            const response = await userService.login(form.login, form.password);
            login(response.token);
            navigate('/profile');
        } catch (err) {
            setServerError(err.response?.data?.message || 'Неверный логин или пароль');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="register-card">
            <Logotype size="lg" position="bottom" />
            <h1 className="register-title">Вход</h1>
            {serverError && <div className="error-box">{serverError}</div>}
            <form onSubmit={handleSubmit} className="form">
                <Input label="Логин" value={form.login} onChange={handleChange('login')} error={errors.login} placeholder="Email или телефон" />
                <Input label="Пароль" type="password" value={form.password} onChange={handleChange('password')} error={errors.password} placeholder="Введите пароль" />
                <Button type="submit" disabled={loading}>{loading ? 'Вход...' : 'Войти'}</Button>
            </form>
            <p className="register-footer">Нет аккаунта? <a href="/register">Зарегистрироваться</a></p>
        </div>
    );
}