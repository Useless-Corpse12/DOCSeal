import { Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';

import AuthLayout from './layouts/AuthLayout';
import WorkspaceLayout from './layouts/WorkspaceLayout';

import Register from './pages/Register';
import VerifyEmail from './pages/Verify';
import Login from './pages/Login';
import Profile from './pages/Profile';
import OrgSetup from './pages/OrgSetup';
import Documents from './pages/Documents';
import OrgSettings from './pages/OrgSettings';
import OrganisationPage from './pages/OrganisationPage';
import InvitePage from './pages/InvitePage';

function App() {
    return (
        <AuthProvider>
            <Routes>
                {/* Паблик */}
                <Route path="/register" element={
                    <AuthLayout><Register /></AuthLayout>
                } />
                <Route path="/verify-email" element={
                    <AuthLayout><VerifyEmail /></AuthLayout>
                } />
                <Route path="/login" element={
                    <AuthLayout><Login /></AuthLayout>
                } />

                <Route path="/invite" element={<InvitePage />} />

                {/* Секур */}
                <Route path="/profile" element={
                    <ProtectedRoute>
                        <WorkspaceLayout><Profile /></WorkspaceLayout>
                    </ProtectedRoute>
                } />
                <Route path="/org-setup" element={
                    <ProtectedRoute>
                        <WorkspaceLayout><OrgSetup /></WorkspaceLayout>
                    </ProtectedRoute>
                } />
                <Route path="/documents" element={
                    <ProtectedRoute>
                        <WorkspaceLayout><Documents /></WorkspaceLayout>
                    </ProtectedRoute>
                } />
                <Route path="/org-settings" element={
                    <ProtectedRoute>
                        <WorkspaceLayout><OrgSettings /></WorkspaceLayout>
                    </ProtectedRoute>
                } />
                <Route path="/organisation/:orgId" element={
                    <ProtectedRoute>
                        <WorkspaceLayout><OrganisationPage /></WorkspaceLayout>
                    </ProtectedRoute>
                } />

                <Route path="/" element={<Navigate to="/register" replace />} />
            </Routes>
        </AuthProvider>
    );
}

export default App;