import { useState, useEffect } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { userService } from '../services/userService';
import Input from '../components/ui/Input';
import Button from '../components/ui/Button';
import './Register.css';
import Logotype from '../components/ui/Logo';

export default function Verify() {
    const navigate = useNavigate();
    const location = useLocation();
    const email = location.state?.email || '';

    const [code, setCode] = useState('');
    const [error, setError] = useState('');
    const [loading, setLoading] = useState(false);
    const [resendLoading, setResendLoading] = useState(false);
    const [success, setSuccess] = useState(false);

    useEffect(() => {
        if (!email) {
            navigate('/register');
        }
    }, [email, navigate]);

    const handleChange = (e) => {
        const value = e.target.value.replace(/\D/g, '').slice(0, 6);
        setCode(value);
        if (error) setError('');
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!code || code.length !== 6) {
            setError('Введите 6-значный код');
            return;
        }

        setLoading(true);
        try {
            await userService.verify(email, code);
            setSuccess(true);
            setTimeout(() => {
                navigate('/login', { state: { email } });
            }, 2000);
        } catch {
            setError('Неверный код');
        } finally {
            setLoading(false);
        }
    };

    const handleResend = async () => {
        setResendLoading(true);
        try {
            await userService.resendVerificationCode(email);
            setError('');
            alert('Код отправлен повторно на ' + email);
        } catch {
            setError('Не удалось отправить код повторно');
        } finally {
            setResendLoading(false);
        }
    };

    if (success) {
        return (
            <div className="register-card">
                <Logotype size="lg" position="bottom" />
                <h1 className="register-title">Email подтверждён!</h1>
                <div className="success-box">
                    <p>Ваш аккаунт успешно активирован.</p>
                    <p>Перенаправляем на страницу входа...</p>
                </div>
            </div>
        );
    }

    return (
        <div className="register-card">
            <Logotype size="lg" position="bottom" />
            <h1 className="register-title">Подтверждение email</h1>

            <div className="verify-info">
                <p>Мы отправили 6-значный код на</p>
                <p className="verify-email">{email}</p>
                <p className="verify-hint">Введите код из письма</p>
            </div>

            {error && <div className="error-box">{error}</div>}

            <form onSubmit={handleSubmit} className="form">
                <Input
                    label="Код подтверждения"
                    value={code}
                    onChange={handleChange}
                    error={error}
                    placeholder="000000"
                    maxLength={6}
                />
                <Button type="submit" disabled={loading || code.length !== 6}>
                    {loading ? 'Проверка...' : 'Подтвердить'}
                </Button>
            </form>

            <div className="verify-footer">
                <p>Не получили код?</p>
                <Button
                    className="resend-link"
                    onClick={handleResend}
                    disabled={resendLoading}
                >
                    {resendLoading ? 'Отправка...' : 'Отправить повторно'}
                </Button>
            </div>

            <p className="register-footer">
                <a href="/register">← Назад к регистрации</a>
            </p>
        </div>
    );
}