import { faLock, faFingerprint, faIdCardAlt, faInfoCircle, faTrash, faUser, faUserTag, faEnvelope, faPhone, faPlus } from "../../content/icons.js";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useState } from "react";
import "../../content/users/addUser.css";
import { Link, useNavigate } from "react-router-dom";
import { register } from "../../api/authService.js";
import Loader from "../../components/Loader.jsx";
import Swal from "sweetalert2";

export default function AddUser() {
    const [telefonos, setTelefonos] = useState([]);
    const [tipoIdentificacion, setTipoIdentificacion] = useState("");
    const [loading, setLoading] = useState(false);
    const navigate = useNavigate();
    const agregarTelefono = () => {
        const ultimo = telefonos[telefonos.length - 1];
        if (ultimo && (!ultimo.codigo || !ultimo.telefono || !ultimo.tipo)) {
            Swal.fire({
                title: 'Completa el teléfono anterior primero 😉',
                icon: 'warning',
                timer: 2000
            });
            return;
        };
        setTelefonos([...telefonos,
        {
            id: 0,
            codigo: "",
            telefono: "",
            tipo: "",
            estado: true
        }]);
    };

    const eliminarTelefono = (index) => {
        setTelefonos(prev => prev.filter((_, i) => i !== index));
    }

    const tipoIdentificacionChange = (tipo) => {
        setTipoIdentificacion(tipo);
    };
    const soloNumeros = (e, max) => {
        e.target.value = e.target.value.replace(/[^0-9]/g, "").slice(0, max);
    };

    const alfaNumerico = (e, max) => {
        e.target.value = e.target.value.replace(/[^a-zA-Z0-9]/g, "").slice(0, max);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);

        const form = e.currentTarget;
        const data = {
            Nombre: form.Nombre.value,
            Apellido: form.Apellido.value,
            Email: form.Email.value,
            TipoIdentificacion: form.TipoIdentificacion.value,
            Identificacion: form.Identificacion.value,
            FechaDeNacimiento: form.FechaDeNacimiento.value,
            Rol: form.Rol.value,
            Password: form.password.value,
            ConfirmPassword: form.confirmPassword.value,
            Telefonos: telefonos.map((tel, i) => ({
                codigo: form[`Telefonos[${i}].Codigo`].value,
                telefono: form[`Telefonos[${i}].Telefono`].value,
                tipo: form[`Telefonos[${i}].Tipo`].value,
            }))
        };
        if (data.Contraseña !== data.ConfirmarContraseña) {
            Swal.fire({
                title: 'Las contrasenas no coinciden',
                icon: 'error',
                timer: 2000
            })
            setLoading(false);
            return;
        }
        try {
            const response = await register(data);
            if (response) {
                Swal.fire({
                    title: 'Usuario registrado exitosamente',
                    icon: 'succes',
                    timer: 2000
                })
                form.reset();
                setTelefonos([]);
                setTipoIdentificacion("");
                navigate("/users");
            } else {
                Swal.fire({
                    title: 'Error al registrar usuario',
                    icon: 'error',
                    timer: 2000
                })
            }
        } catch (error) {
            console.error(error.message);
            Swal.fire({
                title: 'Error al registrar usuario',
                icon: 'error',
                timer: 2000
            })

        } finally {
            setLoading(false);
        }

    }

    if (loading) return <Loader />;
    return (
        <div className="au-main-container">
            <div className="au-register-card">

                <div className="au-left-panel">
                    <div className="au-left-panel__orb au-left-panel__orb--top" />
                    <div className="au-left-panel__orb au-left-panel__orb--bottom" />
                    <div className="au-left-panel__content">
                        <div className="au-left-panel__logo-wrap">
                            <img src="src/assets/LogoInstitucion.png" alt="Logo Colegio Santa Ana" />
                        </div>
                        <div className="au-left-panel__divider" />
                        <ul className="au-left-panel__features">
                            <li><span className="au-feature-dot" />Registro seguro</li>
                            <li><span className="au-feature-dot" />Acceso inmediato</li>
                            <li><span className="au-feature-dot" />Soporte educativo</li>
                        </ul>
                    </div>
                </div>

                {/* Panel derecho */}
                <div className="au-right-panel">
                    <div className="au-register-box">
                        <div className="au-register-box__header">
                            <h2>Registro de Usuarios</h2>
                            <p className="au-subtitle">Completa el formulario para registrarte</p>
                        </div>
                        <form onSubmit={handleSubmit}>
                            <div className="au-form-section">
                                <div className="au-form-section__title">Información Personal</div>

                                <div className="au-form-row">
                                    <div className="au-mb-3">
                                        <label>Nombre</label>
                                        <div className="au-input-wrapper">
                                            <FontAwesomeIcon icon={faUser} className="au-input-icon" />
                                            <input className="au-form-control" name="Nombre" placeholder="Nombre" required />
                                        </div>
                                    </div>
                                    <div className="au-mb-3">
                                        <label>Apellido</label>
                                        <div className="au-input-wrapper">
                                            <FontAwesomeIcon icon={faUser} className="au-input-icon" />
                                            <input className="au-form-control" name="Apellido" placeholder="Apellido" required />
                                        </div>
                                    </div>
                                </div>

                                <div className="au-mb-3">
                                    <label>Correo Electrónico</label>
                                    <div className="au-input-wrapper">
                                        <FontAwesomeIcon icon={faEnvelope} className="au-input-icon" />
                                        <input className="au-form-control" name="Email" placeholder="correo@ejemplo.com" type="email" required />
                                    </div>
                                </div>
                            </div>

                            {/* Teléfonos */}
                            <div className="au-phone-section">
                                <div className="au-phone-section__header">
                                    <h5><FontAwesomeIcon icon={faPhone} className="au-me-2" />Teléfonos</h5>
                                    <span className="au-phone-section__count">{telefonos.length} registrado{telefonos.length !== 1 ? "s" : ""}</span>
                                </div>

                                <div id="au-telefonosContainer" className="au-telefonos-list">
                                    {telefonos.length > 0 ? (
                                        telefonos.map((tel, i) => (
                                            <div className="au-telefono-item" key={i}>
                                                <div className="au-telefono-item__index">{i + 1}</div>
                                                <input type="hidden" name={`Telefonos[${i}].Id`} defaultValue={tel.id} />
                                                <div className="au-tel-row">
                                                    <div className="au-tel-field au-tel-code">
                                                        <label className="au-form-label">Código</label>
                                                        <input
                                                            className="au-form-control"
                                                            name={`Telefonos[${i}].Codigo`}
                                                            defaultValue={tel.codigo}
                                                            placeholder="+506"
                                                            maxLength={3}
                                                            required
                                                            type="tel"
                                                            inputMode="numeric"
                                                            pattern="[0-9]*"
                                                        />
                                                    </div>
                                                    <div className="au-tel-field au-tel-number">
                                                        <label className="au-form-label">Número</label>
                                                        <input
                                                            className="au-form-control"
                                                            name={`Telefonos[${i}].Telefono`}
                                                            defaultValue={tel.telefono}
                                                            placeholder="12345678"
                                                            minLength={8}
                                                            maxLength={8}
                                                            required
                                                            type="tel"
                                                            inputMode="numeric"
                                                            pattern="[0-9]*"
                                                        />
                                                    </div>
                                                    <div className="au-tel-field au-tel-type">
                                                        <label className="au-form-label">Tipo</label>
                                                        <select className="au-form-control" name={`Telefonos[${i}].Tipo`} defaultValue={tel.tipo} required>
                                                            <option value="">Tipo</option>
                                                            <option value="Personal">Personal</option>
                                                            <option value="Trabajo">Trabajo</option>
                                                            <option value="Hogar">Hogar</option>
                                                            <option value="Encargado">Encargado</option>
                                                            <option value="Otro">Otro</option>
                                                        </select>
                                                    </div>
                                                    <div className="au-tel-field au-tel-action">
                                                        <label className="au-form-label">&nbsp;</label>
                                                        <button type="button" className="au-btn-remove-tel" onClick={() => eliminarTelefono(i)}>
                                                            <FontAwesomeIcon icon={faTrash} />
                                                        </button>
                                                    </div>
                                                </div>
                                            </div>
                                        ))
                                    ) : (
                                        <div className="au-empty-state">
                                            <FontAwesomeIcon icon={faInfoCircle} className="au-me-2" />
                                            No hay teléfonos registrados.
                                        </div>
                                    )}
                                </div>

                                <button type="button" id="au-addTelefono" className="au-btn-add-telefono" onClick={agregarTelefono}>
                                    <FontAwesomeIcon icon={faPlus} className="au-me-2" /> Agregar Teléfono
                                </button>
                            </div>

                            <div className="au-form-section">
                                <div className="au-form-section__title">Identificación</div>

                                <div className="au-form-row">
                                    <div className="au-mb-3">
                                        <label>Tipo de Identificación</label>
                                        <div className="au-input-wrapper">
                                            <span className="au-input-icon"><FontAwesomeIcon icon={faIdCardAlt} /></span>
                                            <select className="au-form-control"
                                                name="TipoIdentificacion"
                                                id="tipoIdentificacion"
                                                onChange={(e) => tipoIdentificacionChange(e.target.value)}
                                                required>
                                                <option value="">Seleccione tipo</option>
                                                <option value="Fisica">Cédula Física</option>
                                                <option value="Dimex">DIMEX</option>
                                                <option value="Pasaporte">Pasaporte</option>
                                            </select>
                                        </div>
                                    </div>
                                    <div className="au-mb-3">
                                        <label>Número de Identificación</label>
                                        {tipoIdentificacion === "Fisica" ? (
                                            <div className="au-input-wrapper">
                                                <FontAwesomeIcon icon={faFingerprint} className="au-input-icon" />
                                                <input
                                                    className="au-form-control"
                                                    name="Identificacion"
                                                    placeholder="Ej: 123456789"
                                                    minLength={9}
                                                    maxLength={9}
                                                    inputMode="numeric"
                                                    pattern="[0-9]{9}"
                                                    required
                                                    onInput={(e) => soloNumeros(e, 9)}
                                                />
                                                <small className="au-form-text" id="formatoAyuda">
                                                    9 dígitos numéricos
                                                </small>
                                            </div>
                                        ) : tipoIdentificacion === "Dimex" ? (
                                            <div className="au-input-wrapper">
                                                <FontAwesomeIcon icon={faFingerprint} className="au-input-icon" />
                                                <input
                                                    className="au-form-control"
                                                    name="Identificacion"
                                                    placeholder="Ej: 12345678901"
                                                    minLength={11}
                                                    maxLength={12}
                                                    inputMode="numeric"
                                                    pattern="[0-9]{11,12}"
                                                    required
                                                    onInput={(e) => soloNumeros(e, 12)}
                                                />
                                                <small className="au-form-text" id="formatoAyuda">
                                                    11 a 12 dígitos numéricos (DIMEX)
                                                </small>
                                            </div>
                                        ) : tipoIdentificacion === "Pasaporte" ? (
                                            <div className="au-input-wrapper">
                                                <FontAwesomeIcon icon={faFingerprint} className="au-input-icon" />
                                                <input
                                                    className="au-form-control"
                                                    name="Identificacion"
                                                    placeholder="Ej: A1234567"
                                                    minLength={6}
                                                    maxLength={12}
                                                    pattern="[A-Za-z0-9]{6,12}"
                                                    required
                                                    onInput={(e) => alfaNumerico(e, 12)}
                                                />
                                                <small className="au-form-text" id="formatoAyuda">
                                                    6–12 caracteres alfanuméricos
                                                </small>
                                            </div>
                                        ) : (
                                            <div className="au-input-wrapper">
                                                <FontAwesomeIcon icon={faFingerprint} className="au-input-icon" />
                                                <input
                                                    className="au-form-control"
                                                    name="Identificacion"
                                                    placeholder="Seleccione tipo de identificación"
                                                    disabled
                                                    required
                                                />
                                                <small className="au-form-text" id="formatoAyuda"></small>
                                            </div>
                                        )}
                                    </div>
                                </div>

                                <div className="au-mb-3">
                                    <label>Fecha de Nacimiento</label>
                                    <div className="au-input-wrapper au-input-wrapper--no-icon">
                                        <input className="au-form-control" type="date" name="FechaDeNacimiento" required />
                                    </div>
                                </div>
                            </div>

                            <div className="au-form-section">
                                <div className="au-form-section__title">Acceso y Rol</div>

                                <div className="au-mb-3">
                                    <label>Rol</label>
                                    <div className="au-input-wrapper">
                                        <FontAwesomeIcon icon={faUserTag} className="au-input-icon" />
                                        <select className="au-form-control" name="Rol" id="rolSelector" required>
                                            <option value="Estudiantes">Estudiante</option>
                                            <option value="Profesores">Profesor</option>
                                            <option value="Administradores">Administrador</option>
                                        </select>
                                    </div>
                                </div>

                                <div className="au-form-row">
                                    <div className="au-mb-3">
                                        <label>Contraseña</label>
                                        <div className="au-input-wrapper">
                                            <FontAwesomeIcon icon={faLock} className="au-input-icon" />
                                            <input className="au-form-control" type="password" name="password" required placeholder="••••••••" />
                                        </div>
                                    </div>
                                    <div className="au-mb-3">
                                        <label>Confirmar Contraseña</label>
                                        <div className="au-input-wrapper">
                                            <FontAwesomeIcon icon={faLock} className="au-input-icon" />
                                            <input className="au-form-control" type="password" name="confirmPassword" required placeholder="••••••••" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div className="au-mb-3">
                                <input type="submit" value="Registrarse" className="au-btn-register" />
                            </div>
                            <div className="au-mb-3">
                                <Link to="/users" className="au-btn-cancel">Cancelar</Link>
                            </div>
                        </form>
                    </div>
                </div>

            </div>
        </div>
    );
}