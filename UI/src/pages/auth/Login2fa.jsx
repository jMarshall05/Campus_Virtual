import { Link,useNavigate } from "react-router-dom";
import "../../content/auth/login2fa.css";
import { useState } from "react";
import Loader from "../../components/Loader";
import { leerToken } from "../../utils/auth";
import { login2FA } from "../../api/authService";
import { cambiarToken } from "../../utils/auth";

export default function Login2faView() {
    const [code, setCode] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();

    const handleSubmit = async (e) => {
            e.preventDefault();
            setError("");
            setLoading(true);
    
            try {
                const token = leerToken();
                const userId = token.sub;
                const response = await login2FA(userId, code);
                if(response){
                cambiarToken(response.token);
                navigate("/dashboard");
                } else {
                    setError("Código de verificación incorrecto");
                }
            } catch (err) {
                setError(`${err.message}`);
            } finally {
                setLoading(false);
            }
        };

        if(loading) return<Loader/>;
    return (
        <div className="container-fluid d-flex justify-content-center">
            <div className="twofa-wrapper">
                <div className="card shadow-lg border-0 rounded-4 twofa-card">
                    <div className="card-header bg-primary text-white text-center py-4 border-0">
                        <div className="mb-3">
                            <i className="bi bi-shield-lock-fill display-4"></i>
                        </div>
                        <h3 className="mb-1">Verificación en Dos Pasos</h3>
                        <p className="mb-0 small opacity-75">Protegiendo tu cuenta</p>
                    </div>

                    <div className="card-body p-4 p-md-5">
                        <div className="alert alert-info border-0 mb-4" role="alert">
                            <div className="d-flex align-items-start">
                                <i className="bi bi-phone-fill fs-4 me-3 mt-1"></i>
                                <div>
                                    <p className="text-center mb-0">
                                        Si tienes la verificación de dos pasos activada, abre{" "}
                                        <strong>Google Authenticator</strong> en tu dispositivo móvil e
                                        introduce el código de 6 dígitos que aparece.
                                    </p>
                                </div>
                            </div>
                        </div>
                        {error && <div className="text-center alert alert-danger" role="alert">{error}</div>}
                        <form className="needs-validation" onSubmit={handleSubmit} noValidate>
                            <div className="mb-4">
                                <label
                                    htmlFor="Code"
                                    className="form-label fw-semibold text-center d-block mb-3"
                                >
                                    <i className="bi bi-key-fill me-2"></i>
                                    Código de verificación
                                </label>

                                <input
                                    type="text"
                                    inputMode="numeric"
                                    className="form-control form-control-lg text-center font-monospace"
                                    id="Code"
                                    name="Code"
                                    value={code}
                                    onChange={(e) => {
                                        const val = e.target.value.replace(/\D/g, "");
                                        setCode(val);
                                    }}
                                    placeholder="000000"
                                    maxLength="6"
                                    pattern="[0-9]{6}"
                                    required
                                    autoComplete="off"
                                />

                                <div className="invalid-feedback text-center">
                                    Por favor, introduce un código de 6 dígitos válido.
                                </div>
                            </div>

                            <div className="text-center mb-4">
                                <small className="text-muted">
                                    <i className="bi bi-clock-history me-1"></i>
                                    El código cambia cada 30 segundos
                                </small>
                            </div>

                            <div className="d-grid gap-2 mb-3">
                                <button type="submit" className="btn btn-primary btn-lg py-3">
                                    <i className="bi bi-shield-check me-2"></i>
                                    Verificar código
                                </button>
                            </div>

                            <div className="d-grid">
                                <Link to="/login" className="btn btn-outline-secondary">
                                    <i className="bi bi-arrow-left me-2"></i>
                                    Volver al inicio de sesión
                                </Link>
                            </div>
                        </form>
                    </div>

                    <div className="card-footer bg-light border-0 text-center py-3">
                        <button
                            className="btn btn-link btn-sm text-decoration-none"
                            type="button"
                            data-bs-toggle="collapse"
                            data-bs-target="#helpCollapse"
                        >
                            <i className="bi bi-question-circle me-1"></i>
                            ¿Problemas para iniciar sesión?
                        </button>

                        <div className="collapse" id="helpCollapse">
                            <div className="card card-body border-0 bg-light mt-2 text-start">
                                <h6 className="mb-2">
                                    <i className="bi bi-lightbulb me-2"></i>
                                    Consejos:
                                </h6>
                                <ul className="small mb-2">
                                    <li>Verifica que la hora de tu dispositivo esté sincronizada</li>
                                    <li>Asegúrate de estar usando el código más reciente</li>
                                    <li>El código debe tener exactamente 6 dígitos</li>
                                    <li>
                                        Si borraste el código{" "}
                                        <Link to="/#">Click aquí</Link>
                                    </li>
                                </ul>
                                <hr className="my-2" />
                                <p className="small mb-0">
                                    <i className="bi bi-info-circle me-1"></i>
                                    Si perdiste acceso a tu autenticador, contacta al administrador.
                                </p>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="text-center mt-4">
                    <small className="text-muted">
                        <i className="bi bi-lock-fill me-1"></i>
                        Conexión segura y encriptada
                    </small>
                </div>
            </div>
        </div>
    );
}
