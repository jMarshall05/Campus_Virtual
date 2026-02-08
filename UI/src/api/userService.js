import { Await } from 'react-router-dom';
import { apiFetch } from './apiClient.js';

export async function getUsers() {
    return await apiFetch('users', {
        method: 'GET',
    });
};

export async function getUserById(Id) {
    return await apiFetch(`users/${Id}`);

}

export async function getUsersByRole(rol) {
    return apiFetch(`users/ByRol?rol=${rol}`
    );
}

export async function editUser(userId, userData) {
    return await apiFetch(`user/${userId}`), {
        method: 'PATCH',
        body: JSON.stringify(userData),
    }

}