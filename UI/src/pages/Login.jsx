import { useState } from "react";
import '../content/login.css';
import logo from '../assets/LogoInstitucion.png';
import { login } from "../api/authService";
import { GuardarToken } from "../utils/auth";
import { useNavigate } from "react-router-dom";

function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    try {
      const response = await login(email, password);

      GuardarToken(response.token);
      navigate("/Dashboard")
    } catch (err) {
      setError(`${err.message}`);
    }
  };

  return (
    <div className="main-container">
      <div className="left-panel">
        <img src={logo} alt="Logo" className="Logo Institucion" />
      </div>
      <div className="right-panel">
        <div className="login-box">
          <h2>Bievenido a Campus Virtual</h2>

          <form onSubmit={handleSubmit}>
            <div className="mb-3">
              <input
                type="email"
                className="form-control"
                placeholder="Correo Electrónico"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
              />
            </div>
            <div className="mb-3 password-container">
              <input
                type="password"
                className="form-control"
                placeholder="Contraseña"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>

            {error && <p className="text-danger">{error}</p>}

            <div className="mb-3" >
              <button type="submit" className="btn btn-login">
                Iniciar Sesión
              </button>
            </div>
            <div className="text-link">
              ¿Olvidaste tu contraseña? <a href="#">Restablecela</a>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}

export default Login;
