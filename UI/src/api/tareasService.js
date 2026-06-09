import { apiFetchJson } from "./apiClient";

export async function getTareas() {
    return await apiFetchJson('tareas', {
        method: 'GET',
    });
}

export async function getTareaById(id) {
    return await apiFetchJson(`tareas/${id}`, {
        method: 'GET',
    });
}

export async function getTareasByGrupo(idGrupo) {
    return await apiFetchJson(`tareas/grupo/${idGrupo}`, {
        method: 'GET',
    });
}

export async function createTarea(data) {
    return await apiFetchJson('tareas', {
        method: 'POST',
        body: JSON.stringify(data),
    });
}

export async function editTarea(id, data) {
    return await apiFetchJson(`tareas/${id}`, {
        method: 'PUT',
        body: JSON.stringify(data),
    });
}

export async function toggleTareaState(id) {
    return await apiFetchJson(`tareas/${id}/estado`, {
        method: 'PATCH',
    });
}
