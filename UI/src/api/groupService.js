import { apiFetchJson } from "./apiClient";

export async function getGroups() {
    return await apiFetchJson('groups', {
        method: 'GET',
    });
}

export async function editGroup(idUsuario, data) {
    return await apiFetchJson(`groups?IdUsuario=${idUsuario}`, {
        method: 'PUT',
        body: JSON.stringify(data)
    });
}