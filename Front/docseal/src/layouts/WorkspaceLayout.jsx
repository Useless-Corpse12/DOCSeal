import Sidebar from '../components/Sidebar/Sidebar';
import './WorkspaceLayout.css';

export default function WorkspaceLayout({ children }) {
    return (
        <div className="workspace-layout">
            <Sidebar />
            <main className="workspace-main">
                {children}
            </main>
        </div>
    );
}