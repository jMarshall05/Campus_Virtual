const urlBase = "http://localhost:5099/api";

export async function apiFetch(endpoint, options = {}) {
    const url = `${urlBase}/${endpoint}`;
    const token = localStorage.getItem('token') ;
    const response = await fetch(url,{
        headers: {
            'Content-Type': 'application/json',
            ...(token && { 'Authorization': `Bearer ${token}` }),
            ...options.headers,
        },
        ...options,
    });
    if (!response.ok) {
        const error =await response.text();
        throw new Error(error || 'Error en la solicitud');
    }
    return response; 
}