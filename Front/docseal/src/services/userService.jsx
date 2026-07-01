import apiClient from '../api/client.jsx';
import { getFingerprint } from '../Helpers/fingerPrint.jsx';

export const userService = {
    register: (Name, Password, Email, Phone) => {
        return apiClient.post('/User/RegisterUser', {
            Name,
            Password,
            Email,
            Phone
        }).then(res => res.data);
    },

    login: (Login, Password) => {
        return apiClient.post('/User/AuthorizeUser', {
            Login,
            Password,
            FingerPrint: getFingerprint()
        }).then(res => res.data);
    },

    verify: (Login, VerificationCode) => {
        return apiClient.post('/User/VerifyUser', {
            Login,
            VerificationCode
        }).then(res => res.data);
    },

    getProfile: () => {
        return apiClient.post('/User/Profile', {}).then(res => res.data);
    },

    changePassword: (OldPassword, NewPassword) => {
        return apiClient.post('/User/ChangePasswordUser', {
            OldPassword,
            NewPassword
        }).then(res => res.data);
    },

    refreshToken: (RefreshToken) => {
        return apiClient.post('/User/RefreshUserToken', {
            RefreshToken,
            FingerPrint: getFingerprint()
        }).then(res => res.data);
    },

    resendVerificationCode: (Email) => {
        return apiClient.post('/User/ReSendCode', {
            Email
        }).then(res => res.data);
    }
};