const urlBase = "http://localhost:5099/api";
//export const urlBase = "http://192.168.100.116:5000/api"
//export const urlBase = "/api";  

function getToken() {
    return localStorage.getItem('token');
}

export async function apiFetchJson(endpoint, options = {}) {
    const url = `${urlBase}/${endpoint}`;
    const token = getToken();

    const response = await fetch(url, {
        method: options.method || 'GET',
        headers: {
            ...(options.body && { 'Content-Type': 'application/json' }),
            ...(token && { 'Authorization': `Bearer ${token}` }),
            ...options.headers,
        },
        ...options,
    });

    if (!response.ok) {
        let errorMsg = "Error en la solicitud";

        const contentType = response.headers.get("content-type") || "";
        if (contentType.includes("application/json")) {
            const errJson = await response.json();
            errorMsg = errJson.message || JSON.stringify(errJson);
        } else {
            const errText = await response.text();
            errorMsg = errText || errorMsg;
        }

        throw new Error(errorMsg);
    }

    if (response.status === 204) return null;

    const contentType = response.headers.get("content-type") || "";
    if (contentType.includes("application/json")) {
        return response.json();
    }

    return response.text();
}


export async function apiFetchBlob(endpoint, options = {}) {
    const url = `${urlBase}/${endpoint}`;
    const token = getToken();

    const response = await fetch(url, {
        method: options.method || 'GET',
        headers: {
            ...(token && { 'Authorization': `Bearer ${token}` }),
            ...options.headers,
        },
        ...options,
    });

    if (!response.ok) {
        let errorMsg = "Error en la solicitud";

        const contentType = response.headers.get("content-type") || "";
        if (contentType.includes("application/json")) {
            const errJson = await response.json();
            errorMsg = errJson.message || JSON.stringify(errJson);
        } else {
            const errText = await response.text();
            errorMsg = errText || errorMsg;
        }

        throw new Error(errorMsg);
    }

    return response.blob();
}
