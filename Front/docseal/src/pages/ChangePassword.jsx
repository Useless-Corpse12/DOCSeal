import { useState } from 'react';
import { userService } from '../services/userService.jsx';

export default function ChangePassword() {
    const [oldPass, setOldPass] = useState('');
    const [newPass, setNewPass] = useState('');
    const [message, setMessage] = useState('');

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const data = await userService.changePassword(oldPass, newPass);
            setMessage(data.message);
        } catch (err) {
            setMessage('Ошибка: ' + (err.response?.data?.title || 'Неверный старый пароль'));
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <h2>Смена пароля</h2>
            {message && <p>{message}</p>}
            <input type="password" placeholder="Старый пароль" value={oldPass} onChange={e => setOldPass(e.target.value)} required />
            <input type="password" placeholder="Новый пароль" value={newPass} onChange={e => setNewPass(e.target.value)} required />
            <button type="submit">Сменить</button>
        </form>
    );
}