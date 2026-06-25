import { Routes, Route } from 'react-router-dom';
import AuthLayout from './layouts/AuthLayout';
import WorkspaceLayout from './layouts/WorkspaceLayout';

import Register from './pages/Register';
import VerifyEmail from './pages/VerifyEmail';
import Profile from './pages/Profile';
import OrgSetup from './pages/OrgSetup';
import Documents from './pages/Documents';
import OrgSettings from './pages/OrgSettings';

function App() {
    return (
        <Routes>
            <Route path="/register" element={
                <AuthLayout><Register /></AuthLayout>
            } />
            <Route path="/verify-email" element={
                <AuthLayout><VerifyEmail /></AuthLayout>
            } />

            <Route path="/profile" element={
                <WorkspaceLayout><Profile /></WorkspaceLayout>
            } />
            <Route path="/org-setup" element={
                <WorkspaceLayout><OrgSetup /></WorkspaceLayout>
            } />
            <Route path="/documents" element={
                <WorkspaceLayout><Documents /></WorkspaceLayout>
            } />
            <Route path="/org-settings" element={
                <WorkspaceLayout><OrgSettings /></WorkspaceLayout>
            } />

            <Route path="/" element={
                <AuthLayout><Register /></AuthLayout>
            } />
        </Routes>
    );
}

export default App;