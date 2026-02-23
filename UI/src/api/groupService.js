import { apiFetchJson,apiFetchBlob } from "./apiClient";

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

export async function createGroup(idUsuario, data) {
    return await apiFetchJson(`groups?IdUsuario=${idUsuario}`, {
        method: 'POST',
        body: JSON.stringify(data)
    });
}

export async function exportGroupPdf(IdGrupo) {
    return await apiFetchBlob(`groups/exportarPdf/${IdGrupo}`, {
        method: 'GET',
    });
}

export async function exportGroupQr(IdGrupo) {
    return await apiFetchBlob(`groups/exportarQr?IdGrupo=${IdGrupo}`, {
        method: 'GET',
    });
}
