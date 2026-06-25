import { Link, useLocation } from 'react-router-dom';
import OrgSwitcher from '../OrgSwitcher/OrgSwitcher';
import './Sidebar.css';

export default function Sidebar() {
    const location = useLocation();

    return (
        <aside className="sidebar">
            <div className="sidebar-header">
                <h2 className="sidebar-logo">DOCseal</h2>
            </div>

            <OrgSwitcher />

            <nav className="sidebar-nav">
                <Link
                    to="/documents"
                    className={`sidebar-link ${location.pathname === '/documents' ? 'active' : ''}`}
                >
                    Документы
                </Link>
            </nav>

            <div className="sidebar-footer">
                <Link
                    to="/profile"
                    className={`sidebar-link ${location.pathname === '/profile' ? 'active' : ''}`}
                >
                    Профиль
                </Link>
            </div>
        </aside>
    );
}