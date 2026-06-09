// TODO: Backend EntregasController pendiente de implementar
// Endpoints esperados:
// GET    /api/entregas                 — Listar todas las entregas
// GET    /api/entregas/{id}            — Obtener entrega por ID
// GET    /api/entregas/mis-entregas    — Mis entregas (estudiante)
// POST   /api/entregas                 — Subir entrega (multipart/form-data)
// PUT    /api/entregas/{id}            — Editar entrega
// PATCH  /api/entregas/{id}/estado     — Cambiar estado (borrado lógico)
// import { apiFetchJson, apiFetchBlob } from "./apiClient"; // TODO: Descomentar cuando el backend esté listo

export async function getEntregas() {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson('entregas', { method: 'GET' });
    return [];
}

// UNUSED — TODO: Descomentar cuando el backend esté listo
// export async function getEntregaById() {
//     return await apiFetchJson(`entregas/${id}`, { method: 'GET' });
// }

// UNUSED — TODO: Descomentar cuando el backend esté listo
// export async function getMisEntregas() {
//     return await apiFetchJson('entregas/mis-entregas', { method: 'GET' });
// }

export async function subirEntrega(formData) {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchBlob('entregas', {
    //     method: 'POST',
    //     body: formData,
    // });
    console.log('TODO: subirEntrega', formData);
}

// UNUSED — TODO: Descomentar cuando el backend esté listo
// export async function editEntrega(id, data) {
//     return await apiFetchJson(`entregas/${id}`,{
//         method: 'PUT',
//         body: JSON.stringify(data),
//     });
// }

export async function toggleEntregaEstado(id) {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson(`entregas/${id}/estado`, { method: 'PATCH' });
    console.log('TODO: toggleEntregaEstado', id);
}
