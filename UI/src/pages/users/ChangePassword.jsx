import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faLock, faEye, faEyeSlash, faSave, faArrowLeft, faShieldAlt
} from "../../content/icons.js";
// TODO: Descomentar cuando el backend endpoint POST /api/auth/change-password esté listo
// import { changePassword } from "../../api/authService.js";
import Swal from "sweetalert2";

const inputStyle = { borderRadius: "10px" };

export default function ChangePassword() {
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [showCurrentPassword, setShowCurrentPassword] = useState(false);
    const [showNewPassword, setShowNewPassword] = useState(false);
    const [showConfirmPassword, setShowConfirmPassword] = useState(false);
    const [form, setForm] = useState({
        currentPassword: "",
        newPassword: "",
        confirmPassword: "",
    });
    const [errors, setErrors] = useState({});

    const handleChange = (e) => {
        const { name, value } = e.target;
        setForm({ ...form, [name]: value });
        // Limpiar error al escribir
        if (errors[name]) {
            setErrors({ ...errors, [name]: "" });
        }
    };

    const validate = () => {
        const newErrors = {};

        if (!form.currentPassword) {
            newErrors.currentPassword = "La contraseña actual es requerida";
        }

        if (!form.newPassword) {
            newErrors.newPassword = "La nueva contraseña es requerida";
        } else if (form.newPassword.length < 6) {
            newErrors.newPassword = "La contraseña debe tener al menos 6 caracteres";
        }

        if (!form.confirmPassword) {
            newErrors.confirmPassword = "Confirme la nueva contraseña";
        } else if (form.newPassword !== form.confirmPassword) {
            newErrors.confirmPassword = "Las contraseñas no coinciden";
        }

        if (form.currentPassword && form.newPassword && form.currentPassword === form.newPassword) {
            newErrors.newPassword = "La nueva contraseña debe ser diferente a la actual";
        }

        setErrors(newErrors);
        return Object.keys(newErrors).length === 0;
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        if (!validate()) return;

        setLoading(true);
        try {
            // TODO: Descomentar cuando el backend endpoint esté listo
            // await changePassword({
            //     currentPassword: form.currentPassword,
            //     newPassword: form.newPassword,
            // });

            // Simulación exitosa mientras el backend no existe
            Swal.fire({
                title: "Contraseña actualizada",
                text: "Tu contraseña ha sido cambiada exitosamente.",
                icon: "success",
                timer: 2000,
            });
            navigate("/profile");
        } catch (error) {
            console.error(error);
            Swal.fire({
                title: "Error al cambiar contraseña",
                text: error.message || "Verifica tu contraseña actual e intenta de nuevo.",
                icon: "error",
                confirmButtonText: "OK",
            });
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="admin-container-fluid">
            <div className="admin-header">
                <div className="header-content">
                    <div className="header-title">
                        <div
                            className="title-icon"
                            style={{ background: "linear-gradient(135deg, #ef4444, #dc2626)" }}
                        >
                            <FontAwesomeIcon icon={faShieldAlt} />
                        </div>
                        <div>
                            <h1>Cambiar Contraseña</h1>
                            <p className="header-subtitle">
                                Actualiza tu contraseña de acceso
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            <div className="premium-card">
                <div className="card-body-premium p-4">
                    {/* TODO: Mostrar aviso cuando el backend no esté listo */}
                    <div className="alert alert-warning d-flex align-items-center mb-4" role="alert"
                        style={{ borderRadius: "12px" }}>
                        <FontAwesomeIcon icon={faLock} className="me-3 fa-lg" />
                        <div>
                            <strong>Módulo en desarrollo</strong>
                            <p className="mb-0 small">
                                El backend para cambiar contraseña (
                                <code>POST /api/auth/change-password</code>) aún no está disponible.
                                El formulario está funcional pero no realizará cambios reales.
                            </p>
                        </div>
                    </div>

                    <form onSubmit={handleSubmit} style={{ maxWidth: "600px" }}>
                        {/* Contraseña Actual */}
                        <div className="mb-4">
                            <label htmlFor="currentPassword" className="form-label fw-semibold">
                                <FontAwesomeIcon icon={faLock} className="me-2" />
                                Contraseña Actual
                            </label>
                            <div className="input-group">
                                <input
                                    id="currentPassword"
                                    type={showCurrentPassword ? "text" : "password"}
                                    className={`form-control ${errors.currentPassword ? "is-invalid" : ""}`}
                                    name="currentPassword"
                                    value={form.currentPassword}
                                    onChange={handleChange}
                                    placeholder="Ingresa tu contraseña actual"
                                    style={inputStyle}
                                />
                                <button
                                    type="button"
                                    className="btn btn-outline-secondary"
                                    onClick={() => setShowCurrentPassword(!showCurrentPassword)}
                                    style={{ borderRadius: "0 10px 10px 0" }}
                                    aria-label={showCurrentPassword ? "Ocultar contraseña actual" : "Mostrar contraseña actual"}
                                >
                                    <FontAwesomeIcon icon={showCurrentPassword ? faEyeSlash : faEye} />
                                </button>
                                {errors.currentPassword && (
                                    <div className="invalid-feedback">{errors.currentPassword}</div>
                                )}
                            </div>
                        </div>

                        {/* Nueva Contraseña */}
                        <div className="mb-4">
                            <label htmlFor="newPassword" className="form-label fw-semibold">
                                <FontAwesomeIcon icon={faLock} className="me-2" />
                                Nueva Contraseña
                            </label>
                            <div className="input-group">
                                <input
                                    id="newPassword"
                                    type={showNewPassword ? "text" : "password"}
                                    className={`form-control ${errors.newPassword ? "is-invalid" : ""}`}
                                    name="newPassword"
                                    value={form.newPassword}
                                    onChange={handleChange}
                                    placeholder="Ingresa la nueva contraseña"
                                    style={inputStyle}
                                />
                                <button
                                    type="button"
                                    className="btn btn-outline-secondary"
                                    onClick={() => setShowNewPassword(!showNewPassword)}
                                    style={{ borderRadius: "0 10px 10px 0" }}
                                    aria-label={showNewPassword ? "Ocultar nueva contraseña" : "Mostrar nueva contraseña"}
                                >
                                    <FontAwesomeIcon icon={showNewPassword ? faEyeSlash : faEye} />
                                </button>
                                {errors.newPassword && (
                                    <div className="invalid-feedback">{errors.newPassword}</div>
                                )}
                            </div>
                            {form.newPassword && (
                                <div className="mt-2">
                                    <div className="progress" style={{ height: "6px", borderRadius: "3px" }}>
                                        <div
                                            className={`progress-bar ${
                                                form.newPassword.length >= 8 ? "bg-success" :
                                                form.newPassword.length >= 6 ? "bg-warning" : "bg-danger"
                                            }`}
                                            style={{
                                                width: `${Math.min((form.newPassword.length / 12) * 100, 100)}%`,
                                                borderRadius: "3px",
                                            }}
                                        />
                                    </div>
                                    <small className="text-muted">
                                        {form.newPassword.length < 6
                                            ? "Débil"
                                            : form.newPassword.length < 8
                                            ? "Media"
                                            : "Fuerte"}
                                    </small>
                                </div>
                            )}
                        </div>

                        {/* Confirmar Contraseña */}
                        <div className="mb-4">
                            <label htmlFor="confirmPassword" className="form-label fw-semibold">
                                <FontAwesomeIcon icon={faLock} className="me-2" />
                                Confirmar Nueva Contraseña
                            </label>
                            <div className="input-group">
                                <input
                                    id="confirmPassword"
                                    type={showConfirmPassword ? "text" : "password"}
                                    className={`form-control ${errors.confirmPassword ? "is-invalid" : ""}`}
                                    name="confirmPassword"
                                    value={form.confirmPassword}
                                    onChange={handleChange}
                                    placeholder="Confirma la nueva contraseña"
                                    style={inputStyle}
                                />
                                <button
                                    type="button"
                                    className="btn btn-outline-secondary"
                                    onClick={() => setShowConfirmPassword(!showConfirmPassword)}
                                    style={{ borderRadius: "0 10px 10px 0" }}
                                    aria-label={showConfirmPassword ? "Ocultar confirmación de contraseña" : "Mostrar confirmación de contraseña"}
                                >
                                    <FontAwesomeIcon icon={showConfirmPassword ? faEyeSlash : faEye} />
                                </button>
                                {errors.confirmPassword && (
                                    <div className="invalid-feedback">{errors.confirmPassword}</div>
                                )}
                            </div>
                            {form.confirmPassword && form.newPassword === form.confirmPassword && (
                                <small className="text-success">
                                    ✓ Las contraseñas coinciden
                                </small>
                            )}
                        </div>

                        {/* Botones */}
                        <div className="d-flex justify-content-end gap-2 mt-4">
                            <button
                                type="button"
                                className="btn btn-outline-secondary"
                                onClick={() => navigate("/profile")}
                                style={{ borderRadius: "10px", fontWeight: 600 }}
                            >
                                <FontAwesomeIcon icon={faArrowLeft} className="me-1" />
                                Cancelar
                            </button>
                            <button
                                type="submit"
                                className="btn"
                                disabled={loading}
                                style={{
                                    background: "linear-gradient(135deg, #ef4444, #dc2626)",
                                    color: "white",
                                    borderRadius: "10px",
                                    fontWeight: 600,
                                }}
                            >
                                <FontAwesomeIcon icon={faSave} className="me-1" />
                                {loading ? "Guardando..." : "Cambiar Contraseña"}
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    );
}
