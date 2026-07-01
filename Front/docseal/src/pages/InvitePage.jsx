import { useState, useEffect } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import { orgService } from '../services/orgService.jsx';
import { useAuth } from '../context/AuthContext.jsx';
import './InvitePage.css';

export default function InvitePage() {
    const [searchParams] = useSearchParams();
    const code = searchParams.get('code');
    const navigate = useNavigate();
    const { user, isAuthenticated } = useAuth(); // Получаем текущего юзера

    const [info, setInfo] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');
    const [accepting, setAccepting] = useState(false);
    const [success, setSuccess] = useState(false);


    useEffect(() => {
        if (!code) {
            setError('Код приглашения не указан');
            setLoading(false);
            return;
        }

        const fetchInfo = async () => {
            try {
                const data = await orgService.getInviteInfo(code);

                if (!data.isValid) {
                    setError('Приглашение недействительно, истекло или уже использовано');
                    setLoading(false);
                    return;
                }

                setInfo(data);

                // Если юзер НЕ залогинен
                if (!isAuthenticated) {
                    // Сохраняем в sessionStorage
                    sessionStorage.setItem('pending_invite_code', code);
                    if (data.targetEmail) {
                        sessionStorage.setItem('pending_invite_email', data.targetEmail);
                    }
                    navigate('/register');
                }
                else if (data.targetEmail && data.targetEmail.toLowerCase() !== user.email.toLowerCase()) {
                    setError(`Это приглашение предназначено для ${data.targetEmail}. Вы вошли как ${user.email}.`);
                }

            } catch (err) {
                setError('Ошибка при проверке приглашения');
            } finally {
                setLoading(false);
            }
        };

        fetchInfo();
    }, [code, isAuthenticated, user, navigate]);

    const handleAccept = async () => {
        try {
            setAccepting(true);
            await orgService.acceptInvite(code);
            setSuccess(true);

            sessionStorage.removeItem('pending_invite_code');
            sessionStorage.removeItem('pending_invite_email');

            setTimeout(() => navigate('/documents'), 2000);
        } catch (err) {
            setError(err.response?.data?.message || 'Ошибка при принятии приглашения');
        } finally {
            setAccepting(false);
        }
    };

    if (loading) return <div className="invite-container">Проверка приглашения...</div>;

    return (
        <div className="invite-container">
            <div className="invite-card">
                {error ? (
                    <>
                        <h2>Ошибка</h2>
                        <p className="error-text">{error}</p>
                        <button onClick={() => navigate('/')} className="btn-primary">На главную</button>
                    </>
                ) : success ? (
                    <>
                        <h2>Успех!</h2>
                        <p>Вы успешно вступили в организацию <b>{info.orgName}</b>!</p>
                        <p>Перенаправляем в рабочий стол...</p>
                    </>
                ) : info ? (
                    <>
                        <h2>Приглашение в организацию</h2>
                        <p>Вас приглашают в <b>"{info.orgName}"</b></p>
                        <p>Предлагаемая роль: <span className="role-badge">{info.role}</span></p>

                        {info.targetEmail && (
                            <p className="target-email">Приглашение для: {info.targetEmail}</p>
                        )}

                        <button
                            onClick={handleAccept}
                            disabled={accepting}
                            className="btn-primary accept-btn"
                        >
                            {accepting ? 'Вступление...' : 'Принять приглашение'}
                        </button>
                    </>
                ) : null}
            </div>
        </div>
    );
}