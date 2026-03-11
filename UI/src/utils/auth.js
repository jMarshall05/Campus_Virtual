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
  if (!token || typeof token !== "string") return false;
  try {
    const payload = jwtDecode(token);
    const now = Date.now() / 1000;

    if (payload.exp < now) {
      localStorage.removeItem("token");
      return false;
    }

    return true;
  } catch (error) {
    console.error("Token invalido:", error);
    return false;
  }

}

export async function disable2Factor(IdUsuario) {
  const tokenNuevo = await disable2FA(IdUsuario);
  return tokenNuevo;
}

export function cambiarToken(tokenNuevo){
  localStorage.removeItem("token")
  localStorage.setItem("token", tokenNuevo);
}