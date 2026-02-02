import { apiFetch } from './apiClient.js';

export async function getUsers() {
    return await apiFetch('users', {
        method: 'GET',
    });
}

export async function getUsersByRole(rol) {
    return apiFetch(`users/ByRol?rol=${rol}`
    );
}