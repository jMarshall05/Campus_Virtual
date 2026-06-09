// TODO: Backend CalificacionesController pendiente de implementar
// Endpoints esperados:
// GET    /api/calificaciones           — Listar todas las calificaciones
// GET    /api/calificaciones/{id}      — Obtener calificación por ID
// GET    /api/calificaciones/mis-calificaciones — Mis calificaciones (estudiante)
// POST   /api/calificaciones           — Crear calificación
// PUT    /api/calificaciones/{id}      — Editar calificación
// PATCH  /api/calificaciones/{id}/estado — Cambiar estado (borrado lógico)
// import { apiFetchJson } from "./apiClient"; // TODO: Descomentar cuando el backend esté listo

export async function getCalificaciones() {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson('calificaciones', { method: 'GET' });
    return [];
}

export async function getCalificacionById() {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson(`calificaciones/${id}`, { method: 'GET' });
    return null;
}

export async function getMisCalificaciones() {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson('calificaciones/mis-calificaciones', { method: 'GET' });
    return [];
}

export async function createCalificacion(data) {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson('calificaciones', {
    //     method: 'POST',
    //     body: JSON.stringify(data),
    // });
    console.log('TODO: createCalificacion', data);
}

export async function editCalificacion(id, data) {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson(`calificaciones/${id}`, {
    //     method: 'PUT',
    //     body: JSON.stringify(data),
    // });
    console.log('TODO: editCalificacion', id, data);
}

export async function toggleCalificacionEstado(id) {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson(`calificaciones/${id}/estado`, { method: 'PATCH' });
    console.log('TODO: toggleCalificacionEstado', id);
}
