import { useState } from "react";
import logo from '../assets/LogoInstitucion.png';
import { login } from "../api/authService";
import { GuardarToken } from "../utils/auth";
import { useNavigate } from "react-router-dom";
import Loader from "../components/Loader.jsx";

function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);


  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true);

    try {
      const response = await login(email, password);

      GuardarToken(response.token);
      navigate("/Dashboard")
    } catch (err) {
      setError(`${err.message}`);
    } finally {
      setLoading(false)
    }
  };

  return (
     
      <div className="main-container"> 
      {loading && <Loader />}
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
                  id="userEmail"
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
                  id="userPassword"
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
