import axios from 'axios';

const apiClient = axios.create({
    baseURL: 'https://localhost:5273/api/User',
    headers: {
        'Content-Type': 'application/json',
    },
});

// добавляем токен если есть
apiClient.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('auth_token');
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    },
    (error) => Promise.reject(error)
);

//редирект на случай протухания токена
apiClient.interceptors.response.use(
    (response) => response,
    (error) => {
        if (error.response?.status === 401) {
            localStorage.removeItem('auth_token');
            window.location.href = '/login';
        }
        return Promise.reject(error);
    }
);

export default apiClient;