import { apiFetchBlob, apiFetchJson } from './apiClient.js';

export async function getUsers() {
    return await apiFetchJson('users', {
        method: 'GET',
    });
};

export async function getUserById(Id) {
    return await apiFetchJson(`users/${Id}`);

}

export async function getUsersByRole(rol) {
    return apiFetchJson(`users/ByRol?rol=${rol}`
    );
}

export async function editUser(userId, userData) {
    return await apiFetchJson(`user/${userId}`), {
        method: 'PATCH',
        body: JSON.stringify(userData),
    }

}
export async function editUserAdmin(userId, userData, grupo) {
    if (grupo) {
        return await apiFetchJson(`users/${userId}/admin?grupo=${grupo}`, {
            method: 'PUT',
            body: JSON.stringify(userData),
        });
    } else {
        return await apiFetchJson(`users/${userId}/admin`, {
            method: 'PUT',
            body: JSON.stringify(userData),
        });
    }
}

export async function exportUserPdf(IdUsuario) {
    return await apiFetchBlob(`users/exportarPdf/${IdUsuario}`, {
        method: 'GET',
    });
}

export async function exportUsersPdf() {
    return await apiFetchBlob(`users/exportarPdf`, {
        method: 'GET',
    });
}

export async function exportUserQr(IdUsuario) {
    if (IdUsuario) {
        return await apiFetchBlob(`users/exportarQr?IdUsuario=${IdUsuario}`, {
            method: 'GET',
        });
    } else {
        return await apiFetchBlob(`users/exportarQr`, {
            method: 'GET',
        });
    }
}
