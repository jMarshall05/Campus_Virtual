import { jwtDecode } from "jwt-decode"
import { disable2FA } from "../api/authService";

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
  } catch {
    console.error("Token inválido");
    return null;
  }
}
export function getUserRole() {
  const payload = leerToken();
  if (!payload) return null;
  return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ?? null;
}

export function validarToken() {
  const payload = leerToken();
  if (!payload) return false;
  const now = Date.now() / 1000;
  if (payload.exp < now) {
    localStorage.removeItem("token");
    return false;
  }
  return true;
}

// TODO: Revisar si disable2Factor sigue siendo necesario — se puede refactorizar a llamar disable2FA directamente
export async function disable2Factor(IdUsuario) {
  const tokenNuevo = await disable2FA(IdUsuario);
  return tokenNuevo;
}

export function cambiarToken(tokenNuevo){
  localStorage.removeItem("token")
  localStorage.setItem("token", tokenNuevo);
}