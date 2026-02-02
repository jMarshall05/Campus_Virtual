import { apiFetch } from './apiClient.js';

export async function getUsers() {
    return await apiFetch('users', {
        method: 'GET',
    });
}