import { apiFetchJson } from "./apiClient";

export async function getMaterias() {
    return await apiFetchJson('materias', {
        method: 'GET',
    });
}