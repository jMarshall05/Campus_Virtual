// TODO: Backend EventosController pendiente de implementar
// Endpoints esperados:
// GET    /api/eventos                — Listar todos los eventos
// GET    /api/eventos/{id}           — Obtener evento por ID
// POST   /api/eventos               — Crear evento
// PUT    /api/eventos/{id}          — Editar evento
// PATCH  /api/eventos/{id}/estado   — Cambiar estado (borrado lógico)
// import { apiFetchJson } from "./apiClient"; // TODO: Descomentar cuando el backend esté listo

export async function getEventos() {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson('eventos', { method: 'GET' });
    return [];
}

export async function getEventoById(id) {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson(`eventos/${id}`, { method: 'GET' });
    void id;
    return null;
}

export async function createEvento(data) {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson('eventos', {
    //     method: 'POST',
    //     body: JSON.stringify(data),
    // });
    console.log('TODO: createEvento', data);
}

export async function editEvento(id, data) {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson(`eventos/${id}`, {
    //     method: 'PUT',
    //     body: JSON.stringify(data),
    // });
    console.log('TODO: editEvento', id, data);
}

export async function toggleEventoEstado(id) {
    // TODO: Descomentar cuando el backend esté listo
    // return await apiFetchJson(`eventos/${id}/estado`, { method: 'PATCH' });
    console.log('TODO: toggleEventoEstado', id);
}
