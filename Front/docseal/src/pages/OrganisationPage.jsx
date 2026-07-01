import { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { orgService } from '../services/orgService.jsx';
import './OrganisationPage.css';

export default function OrganisationPage() {
    const { orgId } = useParams();
    const navigate = useNavigate();

    const [org, setOrg] = useState(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState('');

    const [showInviteModal, setShowInviteModal] = useState(false);
    const [showRoleEditModal, setShowRoleEditModal] = useState(false);
    const [showRoleCreateModal, setShowRoleCreateModal] = useState(false);
    const [editingRole, setEditingRole] = useState('');
    const [inviteCodes, setInviteCodes] = useState([]);
    const [inviteForm, setInviteForm] = useState({
        role: 'Employee',
        isOneTime: false,
        durationDays: 7,
        sendEmails: ''
    });
    const [newRoleName, setNewRoleName] = useState('');
    const [creating, setCreating] = useState(false);

    useEffect(() => {
        loadOrgInfo();
        if (org?.isOwner) loadInviteCodes();
    }, [orgId]);

    const loadOrgInfo = async () => {
        try {
            setLoading(true);
            setError('');
            const data = await orgService.getOrganisationInfo(orgId);
            setOrg(data);
        } catch (err) {
            console.error("Не удалось загрузить организацию", err);
            setError(err.response?.data?.message || "Нет доступа к организации");
            setTimeout(() => navigate('/documents'), 2000);
        } finally {
            setLoading(false);
        }
    };

    const loadInviteCodes = async () => {
        if (!org?.isOwner) return;
        try {
            const codes = await orgService.getInviteCodes(orgId);
            setInviteCodes(codes);
        } catch (err) {
            console.error("Не удалось загрузить коды", err);
        }
    };

    const handleCreateInvite = async () => {
        try {
            setCreating(true);

            const code = await orgService.createInviteLink({
                orgId,
                role: inviteForm.role,
                isOneTime: inviteForm.isOneTime,
                durationDays: inviteForm.durationDays
            });

            if (inviteForm.sendEmails.trim()) {
                const emails = inviteForm.sendEmails.split(',').map(e => e.trim()).filter(e => e);
                await orgService.multiEmailInvite(orgId, code, emails);
            }

            setInviteCodes(prev => [...prev, {
                code,
                role: inviteForm.role,
                isOneTime: inviteForm.isOneTime,
                durationDays: inviteForm.durationDays,
                createdAt: new Date().toISOString()
            }]);

            setInviteForm({ role: 'Employee', isOneTime: false, durationDays: 7, sendEmails: '' });
            setShowInviteModal(false);
            await loadInviteCodes();
        } catch (err) {
            alert('Ошибка при создании приглашения: ' + (err.response?.data?.message || err.message));
        } finally {
            setCreating(false);
        }
    };

    const handleDeleteInviteCode = async (code) => {
        if (!confirm('Удалить этот код приглашения?')) return;
        try {
            await orgService.deleteInviteCode(orgId, code);
            await loadInviteCodes();
        } catch (err) {
            alert('Ошибка при удалении: ' + (err.response?.data?.message || err.message));
        }
    };

    const handleCopyCode = (code) => {
        navigator.clipboard.writeText(code);
        alert('Код скопирован!');
    };

    const handleCopyLink = (code) => {
        const link = `${window.location.origin}/invite?code=${code}`;
        navigator.clipboard.writeText(link);
        alert('Ссылка скопирована!');
    };

    const handleEmailInvite = async (code) => {
        const emails = prompt('Введите email ящики через запятую:');
        if (!emails) return;

        const emailList = emails.split(',').map(e => e.trim()).filter(e => e);
        try {
            await orgService.multiEmailInvite(orgId, code, emailList);
            alert(`Отправлено ${emailList.length} приглашений!`);
        } catch (err) {
            alert('Ошибка при отправке: ' + (err.response?.data?.message || err.message));
        }
    };

    const handleEditRole = (roleName) => {
        setEditingRole(roleName);
        setShowRoleEditModal(true);
    };

    const handleSaveRoleEdit = async (newName) => {
        try {
            await orgService.updateRole(orgId, editingRole, newName);
            await loadOrgInfo();
            setShowRoleEditModal(false);
        } catch (err) {
            alert('Ошибка при редактировании: ' + (err.response?.data?.message || err.message));
        }
    };

    const handleCreateRole = async () => {
        if (!newRoleName.trim()) return;
        try {
            await orgService.createRole(orgId, newRoleName.trim());
            setNewRoleName('');
            setShowRoleCreateModal(false);
            await loadOrgInfo();
        } catch (err) {
            alert('Ошибка при создании: ' + (err.response?.data?.message || err.message));
        }
    };

    const handleDeleteRole = async (roleName) => {
        if (!confirm(`Удалить роль "${roleName}"? Все сотрудники с этой ролью будут переведены на другую.`)) return;
        try {
            await orgService.deleteRole(orgId, roleName);
            await loadOrgInfo();
        } catch (err) {
            alert('Ошибка при удалении: ' + (err.response?.data?.message || err.message));
        }
    };

    if (loading) return <div className="loading-container">Загрузка организации...</div>;
    if (error) return <div className="error-container"><h2>Ошибка</h2><p>{error}</p></div>;
    if (!org) return null;

    return (
        <div className="organisation-page org-page">
            <header className="org-header">
                <h1>{org.name}</h1>
                {org.isOwner && <span className="owner-badge">👑 Владелец</span>}
            </header>

            {org.isOwner && (
                <div className="invite-section">
                    <button className="btn-primary" onClick={() => setShowInviteModal(true)}>
                        + Пригласить сотрудника
                    </button>
                </div>
            )}

            {showInviteModal && (
                <div className="modal-overlay" onClick={() => !creating && setShowInviteModal(false)}>
                    <div className="modal-content" onClick={e => e.stopPropagation()}>
                        <h2>Создание приглашения</h2>

                        <div className="form-group">
                            <label>Роль:</label>
                            <select
                                value={inviteForm.role}
                                onChange={e => setInviteForm({...inviteForm, role: e.target.value})}
                            >
                                {org.possiblePositions
                                    .filter(role => role !== 'Big Boss')
                                    .map(role => (
                                        <option key={role} value={role}>{role}</option>
                                    ))
                                }
                            </select>
                        </div>

                        <div className="form-group">
                            <label>
                                <input
                                    type="checkbox"
                                    checked={inviteForm.isOneTime}
                                    onChange={e => setInviteForm({...inviteForm, isOneTime: e.target.checked})}
                                />
                                Одноразовый код
                            </label>
                        </div>

                        <div className="form-group">
                            <label>Срок действия (дней):</label>
                            <input
                                type="number"
                                min="1"
                                max="365"
                                value={inviteForm.durationDays}
                                onChange={e => setInviteForm({...inviteForm, durationDays: parseInt(e.target.value)})}
                            />
                            <small>0 = бессрочно</small>
                        </div>

                        <div className="form-group">
                            <label>Отправить на emailы (через запятую):</label>
                            <textarea
                                placeholder="user1@example.com, user2@example.com"
                                value={inviteForm.sendEmails}
                                onChange={e => setInviteForm({...inviteForm, sendEmails: e.target.value})}
                            />
                        </div>

                        <div className="modal-actions">
                            <button className="btn-secondary" onClick={() => setShowInviteModal(false)}>
                                Отмена
                            </button>
                            <button className="btn-primary" onClick={handleCreateInvite} disabled={creating}>
                                {creating ? 'Создание...' : 'Создать и пригласить'}
                            </button>
                        </div>
                    </div>
                </div>
            )}

            {showRoleEditModal && (
                <div className="modal-overlay" onClick={() => setShowRoleEditModal(false)}>
                    <div className="modal-content" onClick={e => e.stopPropagation()}>
                        <h2>Редактирование роли</h2>
                        <div className="form-group">
                            <label>Новое название:</label>
                            <input
                                type="text"
                                defaultValue={editingRole}
                                onKeyDown={e => e.key === 'Enter' && handleSaveRoleEdit(e.target.value)}
                            />
                        </div>
                        <div className="modal-actions">
                            <button className="btn-secondary" onClick={() => setShowRoleEditModal(false)}>Отмена</button>
                            <button className="btn-primary" onClick={(e) => handleSaveRoleEdit(e.target.closest('.modal-content').querySelector('input').value)}>Сохранить</button>
                        </div>
                    </div>
                </div>
            )}

            {showRoleCreateModal && (
                <div className="modal-overlay" onClick={() => setShowRoleCreateModal(false)}>
                    <div className="modal-content" onClick={e => e.stopPropagation()}>
                        <h2>Создание роли</h2>
                        <div className="form-group">
                            <label>Название роли:</label>
                            <input
                                type="text"
                                value={newRoleName}
                                onChange={e => setNewRoleName(e.target.value)}
                                onKeyDown={e => e.key === 'Enter' && handleCreateRole()}
                            />
                        </div>
                        <div className="modal-actions">
                            <button className="btn-secondary" onClick={() => setShowRoleCreateModal(false)}>Отмена</button>
                            <button className="btn-primary" onClick={handleCreateRole}>Создать</button>
                        </div>
                    </div>
                </div>
            )}

            <div className="org-content-grid">
                {org.isOwner && (
                    <section className="invite-codes-section">
                        <div className="section-header">
                            <h2>Приглашения ({inviteCodes.length})</h2>
                            <button className="btn-small btn-primary" onClick={() => setShowInviteModal(true)}>
                                + Создать
                            </button>
                        </div>

                        {inviteCodes.length === 0 ? (
                            <div className="empty-state">
                                <p>Нет активных приглашений</p>
                            </div>
                        ) : (
                            <div className="invite-codes-list">
                                {inviteCodes.map((invite, idx) => (
                                    <div key={idx} className="invite-code-card-compact">
                                        <div className="invite-meta-compact">
                                            <span className="invite-role">{invite.role}</span>
                                            <span className="invite-info">
                                                {invite.isOneTime ? '🔒' : '🔄'} {invite.durationDays > 0 ? `${invite.durationDays}дн.` : '∞'}
                                            </span>
                                        </div>
                                        <div className="invite-actions-compact">
                                            <button className="btn-icon" onClick={() => handleCopyCode(invite.code)} title="Копировать код">📋</button>
                                            <button className="btn-icon" onClick={() => handleCopyLink(invite.code)} title="Копировать ссылку">🔗</button>
                                            <button className="btn-icon" onClick={() => handleEmailInvite(invite.code)} title="Отправить email">✉️</button>
                                            <button className="btn-icon delete" onClick={() => handleDeleteInviteCode(invite.code)} title="Удалить">🗑️</button>
                                        </div>
                                    </div>
                                ))}
                            </div>
                        )}
                    </section>
                )}

                {org.isOwner && (
                    <section className="roles-section">
                        <div className="section-header">
                            <h2>Доступные роли</h2>
                            <button className="btn-small btn-primary" onClick={() => setShowRoleCreateModal(true)}>+</button>
                        </div>
                        <div className="roles-list">
                            {org.possiblePositions.map((role, idx) => (
                                <div key={idx} className="role-item">
                                    <span className="role-name">{role}</span>
                                    <div className="role-actions">
                                        {role === 'Big Boss' ? (
                                            <span className="role-badge">Системная</span>
                                        ) : (
                                            <>
                                                <button className="btn-icon" onClick={() => handleEditRole(role)} title="Редактировать">✏️</button>
                                                <button className="btn-icon" onClick={() => handleDeleteRole(role)} title="Удалить">🗑️</button>
                                            </>
                                        )}
                                    </div>
                                </div>
                            ))}
                        </div>
                    </section>
                )}
            </div>

            <section className="employees-section">
                <h2>Сотрудники ({org.employees?.length || 0})</h2>
                {org.employees && org.employees.length > 0 ? (
                    <div className="employees-list">
                        {org.employees.map(emp => (
                            <div key={emp.userId} className="employee-card">
                                <div className="employee-info">
                                    <span className="employee-name">{emp.userName}</span>
                                    <span className="employee-email">{emp.email}</span>
                                </div>
                                <div className="employee-role">
                                    {emp.position}
                                </div>
                            </div>
                        ))}
                    </div>
                ) : (
                    <div className="empty-state">
                        <p>Пока нет сотрудников</p>
                    </div>
                )}
            </section>
        </div>
    );
}