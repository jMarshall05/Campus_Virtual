export function GuardarToken(token) {
    localStorage.setItem('token', token);
}
export function ObtenerToken() {
    return localStorage.getItem('token');
}
export function EliminarToken() {
    localStorage.removeItem('token');
}
export function leerToken() {
    const token = localStorage.getItem("token");
  if (!token) return null;

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    return payload; 
  } catch (err) {
    console.error("Token inválido");
    return null;
  }
}
export function getUserRole() {
  const token = localStorage.getItem("token");
  if (!token) return null;

  try {
    const payload = JSON.parse(atob(token.split(".")[1]));
    return payload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  } catch {
    return null;
  }
}