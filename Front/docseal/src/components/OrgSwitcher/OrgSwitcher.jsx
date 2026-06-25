import './OrgSwitcher.css';

const mockOrganizations = [
    { id: 1, name: 'ООО "НаКипРе"' },
    { id: 2, name: 'ЗАО "парк"' },
];

export default function OrgSwitcher() {
    return (
        <div className="org-switcher">
            <div className="org-list">
                {mockOrganizations.map((org) => (
                    <div key={org.id} className="org-item">
                        {org.name}
                    </div>
                ))}
            </div>
            <div className="org-item create-org">+ Создать организацию</div>
        </div>
    );
}