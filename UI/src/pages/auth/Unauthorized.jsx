import { Link } from "react-router-dom";

export default function Unauthorized() {
  return (
    <div className="container">
      <h1 className="tittle">🚫 Acceso no autorizado</h1>
      <p className="text">
        No tienes permisos para acceder a esta sección.
      </p>

      <div className="actions">
        <Link to="/login" className="linkSecondary">
          Volver al Login   
        </Link>
      </div>
    </div>
  );
}