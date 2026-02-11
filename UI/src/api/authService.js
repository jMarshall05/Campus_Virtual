import { apiFetch } from "./apiClient";

export async function login(email, password) {
    return apiFetch('auth/login', {
        method: 'POST',
        body: JSON.stringify({ email, password }),
    });
}

export async function register(userData) {
    return apiFetch('auth/register', {
        method: 'POST',
        body: JSON.stringify(userData),
    });
}
export async function enable2FA(IdUsuario) {
    return apiFetch(`auth/2fa/enable?IdUsuario=${IdUsuario}`, {
        method: 'POST'
    });

}
export async function disable2FA(IdUsuario) {
    return apiFetch(`auth/2fa/disable?IdUsuario=${IdUsuario}`, {
        method: 'POST'
    });

}
export async function verify2FA(IdUsuario,code) {
    return apiFetch(`auth/2fa/verify?IdUsuario=${IdUsuario}&code=${code}`, {
        method: 'POST'
    });

}

export function logOut() {
    localStorage.removeItem('token');
}