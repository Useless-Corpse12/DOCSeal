import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { orgService } from '../../services/orgService.jsx';
import './OrgSwitcher.css';

export default function OrgSwitcher() {
    const [orgs, setOrgs] = useState([]);
    const [loading, setLoading] = useState(true);
    const navigate = useNavigate();

    const loadOrgs = async () => {
        try {
            const response = await orgService.getOrganisations();
            const sortedOrgs = response.orgs.sort((a, b) => {
                if (a.isOwner === b.isOwner) return 0;
                return a.isOwner ? -1 : 1;
            });
            setOrgs(sortedOrgs);
        } catch (err) {
            console.error("Не удалось загрузить организации", err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadOrgs();
    }, []);

    const handleCreateOrg = async () => {
        const name = prompt('Введите название организации:');
        if (!name || !name.trim()) return;

        try {
            await orgService.createOrganisation(name.trim());
            await loadOrgs();
        } catch (err) {
            console.error("Не удалось создать организацию", err);
            alert('Ошибка при создании организации');
        }
    };

    const handleEnterInviteCode = () => {
        const code = prompt('Введите код приглашения:');
        if (!code || !code.trim()) return;

        const trimmedCode = code.trim();
        navigate(`/invite?code=${encodeURIComponent(trimmedCode)}`);
    };

    return (
        <div className="org-switcher">
            <div className="org-list">
                {loading ? (
                    <div className="org-item loading">Загрузка...</div>
                ) : (
                    orgs.map((org) => (
                        <div
                            key={org.id}
                            className={`org-item ${org.isOwner ? 'owner-org' : ''}`}
                            onClick={() => navigate(`/organisation/${org.id}`)}
                        >
                            <span className="org-name">{org.name}</span>
                            {org.isOwner && <span className="owner-badge"></span>}
                        </div>
                    ))
                )}
            </div>

            <div className="org-item create-org" onClick={handleCreateOrg}>
                + Создать организацию
            </div>
            <div className="org-item create-org" onClick={handleEnterInviteCode}>
                § Ввести invite code
            </div>
        </div>
    );
}