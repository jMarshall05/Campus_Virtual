import { Link } from "react-router-dom";
import '../content/unauthorized.css';

export default function Unauthorized() {
  return (
    <div className="container">
      <h1 className="tittle">🚫 Acceso no autorizado</h1>
      <p className="text">
        No tienes permisos para acceder a esta sección.
      </p>

      <div className="actions">
        <Link to="/dashboard" className="link">
          Ir al Dashboard
        </Link>
        <Link to="/login" className="linkSecondary">
          Volver al Login   
        </Link>
      </div>
    </div>
  );
}