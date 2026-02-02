import { leerToken,getUserRole } from '../utils/auth.js';

export default function ProtectedRoute({ children, requiredRole }) {
    const token = leerToken();
    if (!token) {
        return <navigate to="/login" />;
    }
    const userRole = getUserRole();
    if (requiredRole && userRole !== requiredRole) {
        return <navigate to="/unauthorized" />;
    }  
    const now = Date.now() / 1000;
  if (token.exp < now) {
    localStorage.removeItem("token");
    return <navigate to="/login" />;
  }
    
    return children;
}