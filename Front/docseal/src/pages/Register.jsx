import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { userService } from '../services/userService';
import Input from '../components/ui/Input';
import Button from '../components/ui/Button';
import './Register.css';
import Logotype from '../components/ui/Logo';

export default function Register() {
    const navigate = useNavigate();
    const [form, setForm] = useState({
        name: '',
        email: '',
        phone: '',
        password: '',
        confirmPassword: ''
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
        if (!form.name.trim()) e.name = 'Введите имя';
        if (!form.email.trim()) e.email = 'Введите email';
        else if (!/\S+@\S+\.\S+/.test(form.email)) e.email = 'Некорректный email';
        if (!form.phone.trim()) e.phone = 'Введите телефон';
        if (!form.password) e.password = 'Введите пароль';
        else if (form.password.length < 6) e.password = 'Минимум 6 символов';
        if (!form.confirmPassword) e.confirmPassword = 'Повторите пароль';
        else if (form.password !== form.confirmPassword) e.confirmPassword = 'Пароли не совпадают';
        setErrors(e);
        return Object.keys(e).length === 0;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!validate()) return;
        setLoading(true);
        try {
            await userService.register(form.name, form.password, form.email, form.phone);
            navigate('/verify-email', { state: { email: form.email } });
        } catch (err) {
            setServerError(err.response?.data?.message || 'Ошибка регистрации');
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="register-card">
            <Logotype size="lg" position="bottom" />
            <h1 className="register-title">Регистрация</h1>
            {serverError && <div className="error-box">{serverError}</div>}
            <form onSubmit={handleSubmit} className="form">
                <Input label="Имя" value={form.name} onChange={handleChange('name')} error={errors.name} placeholder="Иван Иванов" />
                <Input label="Email" type="email" value={form.email} onChange={handleChange('email')} error={errors.email} placeholder="example@mail.com" />
                <Input label="Телефон" value={form.phone} onChange={handleChange('phone')} error={errors.phone} placeholder="+7 (999) 123-45-67" />
                <Input label="Пароль" type="password" value={form.password} onChange={handleChange('password')} error={errors.password} placeholder="Минимум 6 символов" />
                <Input label="Повторите пароль" type="password" value={form.confirmPassword} onChange={handleChange('confirmPassword')} error={errors.confirmPassword} placeholder="Повторите пароль" />
                <Button type="submit" disabled={loading}>{loading ? 'Регистрация...' : 'Зарегистрироваться'}</Button>
            </form>
            <p className="register-footer">Уже есть аккаунт? <a href="/login">Войти</a></p>
        </div>
    );
}