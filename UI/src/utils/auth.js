import { jwtDecode } from "jwt-decode"

export function GuardarToken(token) {
  localStorage.setItem('token', token);
}
export function EliminarToken() {
  localStorage.removeItem('token');
}
export function leerToken() {
  const token = localStorage.getItem("token");
  if (!token) return null;

  try {
    const payload = jwtDecode(token);
    return payload;
  } catch (err) {
    console.error("Token inválido");
    return null;
  }
}
export function getUserRole() {
  try {
    const token = localStorage.getItem("token");

    if (!token || typeof token !== "string") return null;

    const payload = jwtDecode(token);

    return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ?? null;
  } catch (error) {
    console.error("Error decodificando JWT:", error);
    return null;
  }
}

export function validarToken() {
  const token = localStorage.getItem("token");

  if (!token) return false;

  const now = Date.now() / 1000;

  if (token.exp < now) {
    localStorage.removeItem("token");
    return false;
  }

  return true;
}