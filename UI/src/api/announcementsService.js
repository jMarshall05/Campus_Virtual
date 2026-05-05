import { apiFetchJson, apiFetchBlob } from "./apiClient";

export async function getAnnouncements() {
    return apiFetchJson('announcements', {
        method: 'GET'
    });
}

export async function getAnnouncementById(id) {
    return apiFetchJson(`announcements/${id}`, {
        method: 'GET'
    });
}

export async function addAnnouncement(formData) {
    return apiFetchBlob('announcements', {
        method: 'POST',
        body: formData
    });
}

export async function editAnnouncement(formData) {
    return apiFetchBlob('announcements', {
        method: 'PUT',
        body: formData
    });
}

export async function toggleAnnouncementStatus(id) {
    return apiFetchJson(`announcements/${id}`, {
        method: 'PATCH'
    });
}
