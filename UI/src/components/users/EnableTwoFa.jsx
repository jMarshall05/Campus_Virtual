import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { enable2FA, verify2FA } from "../../api/authService";
import { leerToken, GuardarToken, cambiarToken } from "../../utils/auth";
import Loader from "../Loader";


export default function Enable2FA({ onClose,onEnabled }) {
    const [qr, setQr] = useState("");
    const [code, setCode] = useState("");
    const [loading, setLoading] = useState(true);
    const [userId, setId] = useState("");
    const [secretKey, setSecret] = useState("");
    const [error, setError] = useState("");
    const [copied, setCopied] = useState(false);

    useEffect(() => {
        const cargarQr = async () => {
            try {
                const userId = leerToken().sub;
                const QrSecret = await enable2FA(userId);
                setQr(QrSecret.qr);
                setSecret(QrSecret.secretKey);
                setId(userId);
            } catch (err) {
                setError("Error al cargar el código QR. Intenta de nuevo.");
            } finally {
                setLoading(false);
            }
        };
        cargarQr();
    }, []);

    const copySecret = async () => {
        try {
            await navigator.clipboard.writeText(secretKey);
            setCopied(true);
            setTimeout(() => setCopied(false), 2000);
        } catch {
            // Fallback para navegadores sin soporte de clipboard AP
            const input = document.getElementById("secretKey");
            input.select();
            document.execCommand("copy");
            setCopied(true);
            setTimeout(() => setCopied(false), 2000);
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");
        setLoading(true);

        try {
            const response = await verify2FA(userId, code);
            onEnabled(response)
        } catch (err) {
            setError(`${err.message}`);
        } finally {
            setLoading(false);
        }
    };

    if (loading) return <Loader />;

    return (
        <div className="container-fluid d-flex justify-content-center">

            <div className="card shadow-sm">
                <div className="card-header bg-primary text-white text-center py-3">
                    <h4 className="mb-0">
                        <i className="bi bi-shield-lock-fill me-2"></i>
                        Configurar Autenticación de Dos Factores
                    </h4>
                </div>

                <div className="card-body p-4">
                    <div className="alert alert-info" role="alert">
                        <i className="bi bi-info-circle-fill me-2"></i>
                        <strong>Paso 1:</strong> Descarga Google Authenticator en tu dispositivo móvil si aún no lo tienes.
                    </div>

                    <div className="text-center mb-4">
                        <h5 className="mb-3">
                            <i className="bi bi-qr-code me-2"></i>
                            Escanea este código QR
                        </h5>
                        <div className="qr-container bg-light p-4 rounded d-inline-block">
                            <img
                                src={`data:image/png;base64,${qr}`}
                                alt="Código QR para Google Authenticator"
                                className="img-fluid"
                                style={{ maxWidth: "250px" }}
                            />
                        </div>
                    </div>

                    <div className="text-center mb-4">
                        <span className="text-muted">- O -</span>
                    </div>

                    <div className="mb-4">
                        <h6 className="mb-2">
                            <i className="bi bi-key-fill me-2"></i>
                            Introduce la clave manualmente:
                        </h6>
                        <div className="input-group">
                            <input
                                type="text"
                                className="form-control font-monospace text-center fs-5"
                                id="secretKey"
                                value={secretKey}
                                readOnly
                            />
                            <button
                                className={`btn ${copied ? "btn-success" : "btn-outline-secondary"}`}
                                type="button"
                                onClick={copySecret}
                                title="Copiar clave"
                            >
                                <i className={`bi ${copied ? "bi-check-lg" : "bi-clipboard"}`}></i>
                                {" "}{copied ? "Copiado" : "Copiar"}
                            </button>
                        </div>
                        <small className="text-muted d-block mt-1">
                            <i className="bi bi-lightbulb"></i>
                            {" "}Guarda esta clave en un lugar seguro por si necesitas reconfigurar tu autenticador.
                        </small>
                    </div>

                    <hr className="my-4" />

                    <div className="alert alert-warning" role="alert">
                        <i className="bi bi-exclamation-triangle-fill me-2"></i>
                        <strong>Paso 2:</strong> Introduce el código de 6 dígitos que aparece en tu app.
                    </div>

                    {error && (
                        <div className="alert alert-danger" role="alert">
                            <i className="bi bi-x-circle-fill me-2"></i>
                            {error}
                        </div>
                    )}

                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label htmlFor="Code" className="form-label fw-bold">
                                Código de verificación:
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
                                autoFocus
                            />
                            <div className="invalid-feedback">
                                Por favor, introduce un código de 6 dígitos válido.
                            </div>
                        </div>

                        <div className="d-grid gap-2">
                            <button type="submit" className="btn btn-primary btn-lg">
                                <i className="bi bi-check-circle me-2"></i>
                                Verificar y Activar
                            </button>
                            <button
                                type="button"
                                onClick={onClose}
                                className="btn btn-outline-secondary"
                            >
                                <i className="bi bi-arrow-left me-2"></i>
                                Cancelar
                            </button>
                        </div>
                    </form>

                    <hr className="my-4" />

                    <div className="accordion" id="helpAccordion">
                        <div className="accordion-item">
                            <h2 className="accordion-header">
                                <button
                                    className="accordion-button collapsed"
                                    type="button"
                                    data-bs-toggle="collapse"
                                    data-bs-target="#helpCollapse"
                                >
                                    <i className="bi bi-question-circle me-2"></i>
                                    ¿Necesitas ayuda?
                                </button>
                            </h2>
                            <div id="helpCollapse" className="accordion-collapse collapse" data-bs-parent="#helpAccordion">
                                <div className="accordion-body">
                                    <h6>Descarga Google Authenticator:</h6>
                                    <ul>
                                        <li>
                                            <a href="https://apps.apple.com/app/google-authenticator/id388497605" target="_blank" rel="noreferrer">
                                                iOS (App Store)
                                            </a>
                                        </li>
                                        <li>
                                            <a href="https://play.google.com/store/apps/details?id=com.google.android.apps.authenticator2" target="_blank" rel="noreferrer">
                                                Android (Google Play)
                                            </a>
                                        </li>
                                    </ul>
                                    <h6>Problemas comunes:</h6>
                                    <ul>
                                        <li>Asegúrate de que la hora de tu dispositivo esté sincronizada</li>
                                        <li>El código cambia cada 30 segundos, introduce el código actual</li>
                                        <li>Si el código no funciona, verifica que escaneaste el QR correcto</li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="card-footer text-center text-muted py-3">
                    <small>
                        <i className="bi bi-lock-fill"></i>
                        {" "}Tu seguridad es nuestra prioridad. Esta información está encriptada.
                    </small>
                </div>
            </div>
        </div>
    );
}