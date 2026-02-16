import { leerToken, getUserRole, validarToken } from '../../utils/auth.js';
import { Navigate } from 'react-router-dom';

export default function ProtectedRoute({ children, requiredRole }) {
  const token = leerToken();
  if (!token) {
    return <Navigate to="/login" replace />;
  }

  if (!validarToken()) {
    return <Navigate to="/login" replace />;
  }

  const userRole = getUserRole();

  if (requiredRole && userRole !== requiredRole) {
    return <Navigate to="/unauthorized" replace />;
  }
  return children;
}