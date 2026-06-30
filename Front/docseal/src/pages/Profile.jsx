import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { userService } from '../services/userService';
import { useAuth } from '../context/AuthContext';
import Input from '../components/ui/Input';
import Button from '../components/ui/Button';
import './Register.css';
import './Profile.css';
import Logotype from '../components/ui/Logo';

export default function Profile() {
    const navigate = useNavigate();
    const { logout } = useAuth();

    const [user, setUser] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const [showChangePassword, setShowChangePassword] = useState(false);
    const [passwordForm, setPasswordForm] = useState({ oldPassword: '', newPassword: '', confirmPassword: '' });
    const [passwordErrors, setPasswordErrors] = useState({});
    const [passwordLoading, setPasswordLoading] = useState(false);
    const [passwordMessage, setPasswordMessage] = useState('');

    useEffect(() => {
        loadProfile();
    }, []);

    const loadProfile = async () => {
        try {
            const data = await userService.getProfile();
            setUser(data);
        } catch (err) {
            setError('Не удалось загрузить профиль');
        } finally {
            setLoading(false);
        }
    };

    const handleLogout = () => {
        logout();
        navigate('/login');
    };

    const handlePasswordChange = (field) => (e) => {
        setPasswordForm({ ...passwordForm, [field]: e.target.value });
        if (passwordErrors[field]) setPasswordErrors({ ...passwordErrors, [field]: '' });
        if (passwordMessage) setPasswordMessage('');
    };

    const validatePassword = () => {
        const e = {};
        if (!passwordForm.oldPassword) e.oldPassword = 'Введите старый пароль';
        if (!passwordForm.newPassword) e.newPassword = 'Введите новый пароль';
        else if (passwordForm.newPassword.length < 6) e.newPassword = 'Минимум 6 символов';
        if (!passwordForm.confirmPassword) e.confirmPassword = 'Повторите пароль';
        else if (passwordForm.newPassword !== passwordForm.confirmPassword) e.confirmPassword = 'Пароли не совпадают';
        setPasswordErrors(e);
        return Object.keys(e).length === 0;
    };

    const handleSubmitPassword = async (e) => {
        e.preventDefault();
        if (!validatePassword()) return;

        setPasswordLoading(true);
        try {
            await userService.changePassword(passwordForm.oldPassword, passwordForm.newPassword);
            setPasswordMessage('Пароль успешно изменён');
            setPasswordForm({ oldPassword: '', newPassword: '', confirmPassword: '' });
            setTimeout(() => setShowChangePassword(false), 1500);
        } catch (err) {
            setPasswordMessage(err.response?.data?.message || 'Ошибка смены пароля');
        } finally {
            setPasswordLoading(false);
        }
    };

    if (loading) {
        return (
            <div className="profile-container">
                <div className="register-card">
                    <p style={{ textAlign: 'center' }}>Загрузка...</p>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="profile-container">
                <div className="register-card">
                    <div className="error-box">{error}</div>
                </div>
            </div>
        );
    }

    return (
        <div className="profile-container">
            <div className="register-card">
                <Logotype size="lg" position="bottom" />
                <h1 className="register-title">Профиль</h1>

                <div className="profile-info">
                    <div className="profile-row">
                        <span className="profile-label">Имя:</span>
                        <span className="profile-value">{user?.name}</span>
                    </div>
                    <div className="profile-row">
                        <span className="profile-label">Email:</span>
                        <span className="profile-value">{user?.email}</span>
                    </div>
                    <div className="profile-row">
                        <span className="profile-label">Телефон:</span>
                        <span className="profile-value">{user?.phone}</span>
                    </div>
                </div>

                <div className="profile-actions">
                    <Button onClick={() => setShowChangePassword(!showChangePassword)}>
                        {showChangePassword ? 'Отмена' : 'Сменить пароль'}
                    </Button>
                    <Button variant="secondary" onClick={handleLogout}>
                        Выйти
                    </Button>
                </div>

                {showChangePassword && (
                    <form onSubmit={handleSubmitPassword} className="form" style={{ marginTop: 'var(--space-lg)' }}>
                        <h3 style={{ fontSize: '16px', marginBottom: 'var(--space-sm)' }}>Смена пароля</h3>
                        {passwordMessage && (
                            <div className={passwordMessage.includes('успешно') ? 'success-box' : 'error-box'}>
                                {passwordMessage}
                            </div>
                        )}
                        <Input
                            label="Старый пароль"
                            type="password"
                            value={passwordForm.oldPassword}
                            onChange={handlePasswordChange('oldPassword')}
                            error={passwordErrors.oldPassword}
                        />
                        <Input
                            label="Новый пароль"
                            type="password"
                            value={passwordForm.newPassword}
                            onChange={handlePasswordChange('newPassword')}
                            error={passwordErrors.newPassword}
                        />
                        <Input
                            label="Повторите пароль"
                            type="password"
                            value={passwordForm.confirmPassword}
                            onChange={handlePasswordChange('confirmPassword')}
                            error={passwordErrors.confirmPassword}
                        />
                        <Button type="submit" disabled={passwordLoading}>
                            {passwordLoading ? 'Сохранение...' : 'Сохранить'}
                        </Button>
                    </form>
                )}
            </div>
        </div>
    );
}