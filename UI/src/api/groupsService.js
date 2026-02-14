import { apiFetchJson } from "./apiClient";

export async function getGroups() {
    return await apiFetchJson('groups', {
        method: 'GET',
    });
}