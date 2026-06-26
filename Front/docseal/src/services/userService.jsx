import apiClient from '../api/client.jsx';

export const userService = {

    register: (Name, Password, Email, Phone) => {
        return apiClient.post('/RegisterUser', {
            Name,
            Password,
            Email,
            Phone
        }).then(res => res.data);
    },

    login: (Login, Password) => {
        return apiClient.post('/AuthorizeUser', {
            Login,
            Password
        }).then(res => res.data);
    },

    verify: (Login, VerificationCode) => {
        return apiClient.post('/VerifyUser', {
            Login,
            VerificationCode
        }).then(res => res.data);
    },

    changePassword: (OldPassword, NewPassword) => {
        return apiClient.post('/ChangePasswordUser', {
            OldPassword,
            NewPassword
        }).then(res => res.data);
    }
};