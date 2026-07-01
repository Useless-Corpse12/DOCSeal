import apiClient from '../api/client.jsx';

export const orgService = {
    getOrganisations: () => {
        return apiClient.post('/Organization/GetOrganisations', {}).then(res => res.data);
    },

    createOrganisation: (Name) => {
        return apiClient.post('/Organization/CreateMyOrganisation', { Name }).then(res => res.data);
    },

    getOrganisationInfo: (orgId) => {
        return apiClient.post('/Organization/GetOrganisationInfo', { OrgId: orgId }).then(res => res.data);
    },

    createInviteLink: (data) => {
        return apiClient.post('/Organization/InviteLinkGeneration', {
            OrgId: data.orgId,
            Role: data.role,
            IsOneTime: data.isOneTime,
            DurationDays: data.durationDays
        }).then(res => res.data.inviteCode);
    },

    deleteInviteCode: (orgId, code) => {
        return apiClient.post('/Organization/DeleteInviteCode', { OrgId: orgId, Code: code }).then(res => res.data);
    },

    multiEmailInvite: (orgId, inviteCode, emails) => {
        return apiClient.post('/Organization/MultiEmailInvite', {
            OrgId: orgId,
            InviteCode: inviteCode,
            Emails: emails
        }).then(res => res.data);
    },

    directInvite: (orgId, role, durationDays, targetEmail) => {
        return apiClient.post('/Organization/DirectInvite', {
            OrgId: orgId,
            Role: role,
            DurationDays: durationDays,
            TargetEmail: targetEmail
        }).then(res => res.data);
    },

    getInviteInfo: (code) => {
        return apiClient.post('/Organization/GetInviteInfo', { Code: code }).then(res => res.data);
    },

    acceptInvite: (code) => {
        return apiClient.post('/Organization/AcceptInvite', { Code: code }).then(res => res.data);
    },

    getInviteCodes: (orgId) => {
        return apiClient.post('/Organization/GetInviteCodes', { OrgId: orgId }).then(res => res.data);
    },

    createRole: (orgId, roleName) => {
        return apiClient.post('/Organization/CreateRole', { OrgId: orgId, RoleName: roleName }).then(res => res.data);
    },

    updateRole: (orgId, oldRoleName, newRoleName) => {
        return apiClient.post('/Organization/UpdateRole', { OrgId: orgId, OldRoleName: oldRoleName, NewRoleName: newRoleName }).then(res => res.data);
    },

    deleteRole: (orgId, roleName) => {
        return apiClient.post('/Organization/DeleteRole', { OrgId: orgId, RoleName: roleName }).then(res => res.data);
    }
};