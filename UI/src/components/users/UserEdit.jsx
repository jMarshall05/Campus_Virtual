import { faUser, faEnvelope, faPlusCircle, faTrash, faInfoCircle, faIdCardAlt, faFingerprint, faCalendarAlt, faCog, faUserTag, faTimes, faSave } from "../../content/icons.js";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import "../../content/users/userEdit.css";
import { faAddressBook } from "@fortawesome/free-solid-svg-icons";
import { useState, useEffect } from "react";
import Loader from "../Loader.jsx";
import { getGroups } from "../../api/groupsService.js";
import { editUserAdmin } from "../../api/userService.js";

export default function UserEdit({ usuario, onClose }) {

    const [grupos, setGrupos] = useState([]);
    const [loadingGrupos, setLoadingGrupos] = useState(true);
    const [saving, setSaving] = useState(false);
    const [telefonos, setTelefonos] = useState([]);
    const [tipoIdentificacion, setTipoIdentificacion] = useState(usuario?.tipoIdentificacion || "");

    useEffect(() => {
        const cargarGrupos = async () => {
            try {
                if (usuario.rol !== "Estudiantes") {
                    setGrupos([]);
                    return;
                }
                const gruposData = await getGroups();
                setGrupos(gruposData);
            }
            finally {
                setLoadingGrupos(false);
            };

        };
        cargarGrupos();
        if (usuario.telefonos) {
            setTelefonos(usuario.telefonos);
        }
    }, [usuario]);

    const agregarTelefono = () => {
        const ultimo = telefonos[telefonos.length - 1];
        if (ultimo && (!ultimo.codigo || !ultimo.telefono || !ultimo.tipo)) {
            alert("Completa el teléfono anterior primero 😉");
            return;
        }
        setTelefonos(prev => [
            ...prev, {
                id: 0,
                codigo: "",
                telefono: "",
                tipo: "",
                estado: true
            }
        ])
    }
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
        setSaving(true);

        const form = e.currentTarget;
        const data = {

            Nombre: form.Nombre.value,
            Apellido: form.Apellido.value,
            Email: form.Email.value,
            Identificacion: form.Identificacion.value.replaceAll("-", ""),
            TipoIdentificacion: form.TipoIdentificacion.value,
            Rol: form.Rol.value,
            FechaDeNacimiento: form.FechaDeNacimiento.value,
            Estado: form.Estado.checked,
            Telefonos: telefonos.map((tel, i) => ({
                Id: tel.id,
                Codigo: form[`Telefonos[${i}].Codigo`].value,
                Telefono: form[`Telefonos[${i}].Telefono`].value,
                Tipo: form[`Telefonos[${i}].Tipo`].value,
                Estado: form[`Telefonos[${i}].Estado`]?.checked ?? false,
            }))
            

        };

        if (!data.FechaDeNacimiento) {
            delete data.FechaDeNacimiento;
        }
        try {
            const response = await editUserAdmin(usuario.idUsuario, data);
            if (response) {
                alert("Usuario actualizado exitosamente");
                setSaving(false);

                onClose();
            } else {
                alert("Error al actualizar usuario");
            }

        }
        catch (error) {
            alert("Error al actualizar usuario: " + error.message);
        }
        finally {
            setSaving(false);
        }
    }



    if (loadingGrupos) return <Loader />
    return (
        <div className="edit-modal">
            <form onSubmit={handleSubmit} id="editUserForm">
                <div className="edit-content">

                    {/* Header */}
                    <div className="edit-header">
                        <div className="edit-header-info">
                            <div className="edit-avatar">
                                <FontAwesomeIcon icon={faUser} />
                            </div>
                            <div>
                                <h2 className="edit-title">Editar Usuario</h2>
                                <p className="edit-subtitle">{usuario?.nombre} {usuario?.apellido}</p>
                            </div>
                        </div>
                        <button className="btn-close-modal" onClick={onClose} type="button">
                            <FontAwesomeIcon icon={faTimes} />
                        </button>
                    </div>

                    <div className="edit-body">


                        <div className="two-col-grid">

                            <div className="form-section">
                                <div className="section-header">
                                    <span className="section-icon"><FontAwesomeIcon icon={faUser} /></span>
                                    <h3 className="section-title">Información Básica</h3>
                                </div>

                                <div className="form-row">
                                    <div className="form-group">
                                        <label className="form-label">
                                            Nombre <span className="required-mark">*</span>
                                        </label>
                                        <div className="input-wrapper">
                                            <span className="input-icon"><FontAwesomeIcon icon={faUser} /></span>
                                            <input
                                                className="form-control"
                                                name="Nombre"
                                                defaultValue={usuario?.nombre}
                                                placeholder="Nombre"
                                                required
                                            />
                                        </div>
                                    </div>

                                    <div className="form-group">
                                        <label className="form-label">
                                            Apellido <span className="required-mark">*</span>
                                        </label>
                                        <div className="input-wrapper">
                                            <span className="input-icon"><FontAwesomeIcon icon={faUser} /></span>
                                            <input
                                                className="form-control"
                                                name="Apellido"
                                                defaultValue={usuario?.apellido}
                                                placeholder="Apellido"
                                                required
                                            />
                                        </div>
                                    </div>
                                </div>

                                <div className="form-group mt-2">
                                    <label className="form-label">
                                        Email <span className="required-mark">*</span>
                                    </label>
                                    <div className="input-wrapper">
                                        <span className="input-icon"><FontAwesomeIcon icon={faEnvelope} /></span>
                                        <input
                                            className="form-control"
                                            name="Email"
                                            type="email"
                                            defaultValue={usuario?.email}
                                            autoComplete="true"
                                            placeholder="correo@ejemplo.com"
                                            required
                                        />
                                    </div>
                                </div>
                            </div>

                            <div className="form-section">
                                <div className="section-header">
                                    <span className="section-icon"><FontAwesomeIcon icon={faInfoCircle} /></span>
                                    <h3 className="section-title">Información Adicional</h3>
                                </div>

                                <div className="form-row">
                                    <div className="form-group">
                                        <label className="form-label">
                                            Tipo ID <span className="required-mark">*</span>
                                        </label>
                                        <div className="input-wrapper">
                                            <span className="input-icon"><FontAwesomeIcon icon={faIdCardAlt} /></span>
                                            <select
                                                className="form-control"
                                                name="TipoIdentificacion"
                                                id="tipoIdentificacion"
                                                defaultValue={usuario?.tipoIdentificacion}
                                                onChange={(e) => tipoIdentificacionChange(e.target.value)}
                                                required
                                            >
                                                <option value="">Seleccione tipo</option>
                                                <option value="Fisica">Cédula Física</option>
                                                <option value="Dimex">DIMEX</option>
                                                <option value="Pasaporte">Pasaporte</option>
                                            </select>
                                        </div>
                                    </div>

                                    <div className="form-group">
                                        <label className="form-label">
                                            Número ID <span className="required-mark">*</span>
                                        </label>

                                        <div className="input-wrapper">
                                            <span className="input-icon">
                                                <FontAwesomeIcon icon={faFingerprint} />
                                            </span>

                                            {tipoIdentificacion === "Fisica" ? (
                                                <input
                                                    className="form-control"
                                                    name="Identificacion"
                                                    id="numeroIdentificacion"
                                                    defaultValue={usuario?.identificacion}
                                                    placeholder="Ej: 123456789"
                                                    minLength={9}
                                                    maxLength={9}
                                                    inputMode="numeric"
                                                    pattern="[0-9]{9}"
                                                    required
                                                    onInput={(e) => soloNumeros(e, 9)}
                                                />
                                            ) : tipoIdentificacion === "Dimex" ? (
                                                <input
                                                    className="form-control"
                                                    name="Identificacion"
                                                    id="numeroIdentificacion"
                                                    defaultValue={usuario?.identificacion}
                                                    placeholder="Ej: 12345678901"
                                                    minLength={11}
                                                    maxLength={12}
                                                    inputMode="numeric"
                                                    pattern="[0-9]{11,12}"
                                                    required
                                                    onInput={(e) => soloNumeros(e, 12)}
                                                />
                                            ) : tipoIdentificacion === "Pasaporte" ? (
                                                <input
                                                    className="form-control"
                                                    name="Identificacion"
                                                    id="numeroIdentificacion"
                                                    defaultValue={usuario?.identificacion}
                                                    placeholder="Ej: A1234567"
                                                    minLength={6}
                                                    maxLength={12}
                                                    pattern="[A-Za-z0-9]{6,12}"
                                                    required
                                                    onInput={(e) => alfaNumerico(e, 12)}
                                                />
                                            ) : (
                                                <input
                                                    className="form-control"
                                                    name="Identificacion"
                                                    id="numeroIdentificacion"
                                                    placeholder="Seleccione tipo de identificación"
                                                    disabled
                                                    required
                                                />
                                            )}
                                        </div>

                                        <small className="form-text" id="formatoAyuda">
                                            {tipoIdentificacion === "Fisica" && "9 dígitos numéricos"}
                                            {tipoIdentificacion === "Dimex" && "11 a 12 dígitos numéricos (DIMEX)"}
                                            {tipoIdentificacion === "Pasaporte" && "6–12 caracteres alfanuméricos"}
                                        </small>
                                    </div>

                                </div>

                                <div className="form-row">
                                    <div className="form-group">
                                        <label className="form-label">Fecha de Nacimiento</label>
                                        <div className="input-wrapper">
                                            <span className="input-icon"><FontAwesomeIcon icon={faCalendarAlt} /></span>
                                            <input
                                                className="form-control"
                                                type="date"
                                                name="FechaDeNacimiento"
                                                required={true}
                                                defaultValue={usuario?.fechaDeNacimiento
                                                    ? new Date(usuario.fechaDeNacimiento).toISOString().split("T")[0]
                                                    : ""}
                                            />
                                        </div>
                                    </div>

                                    <div className="form-group">
                                        <label className="form-label">
                                            Rol <span className="required-mark">*</span>
                                        </label>
                                        <div className="input-wrapper">
                                            <span className="input-icon"><FontAwesomeIcon icon={faUserTag} /></span>
                                            <select
                                                className="form-control"
                                                name="Rol"
                                                id="rolSelector"
                                                defaultValue={usuario?.rol}
                                                required
                                            >
                                                <option value="Estudiantes">Estudiante</option>
                                                <option value="Profesores">Profesor</option>
                                                <option value="Administradores">Administrador</option>
                                            </select>
                                        </div>
                                    </div>
                                </div>

                                {usuario?.rol === "Estudiantes" && (
                                    <div className="form-group">
                                        <label className="form-label">Grupo</label>
                                        <div className="input-wrapper">
                                            <select className="form-control" name="IdGrupo" defaultValue={usuario?.grupo.id_grupo ?? ""}>
                                                <option key="default-grupo" value="">Seleccione un grupo</option>

                                                {grupos.map((grupo, index) => (
                                                    <option key={`grupo-${grupo.id_grupo}-${index}`} value={grupo.id_grupo}>
                                                        {grupo.nombre}
                                                    </option>
                                                ))}
                                            </select>
                                        </div>
                                    </div>
                                )}
                            </div>
                        </div>

                        <div className="form-section">
                            <div className="section-header">
                                <span className="section-icon"><FontAwesomeIcon icon={faAddressBook} /></span>
                                <h3 className="section-title">Teléfonos</h3>
                                <button type="button" id="addTelefono" className="btn btn-add-tel" onClick={agregarTelefono}>
                                    <FontAwesomeIcon icon={faPlusCircle} className="me-1" />Agregar
                                </button>
                            </div>

                            <div id="telefonosContainer" className="telefonos-list">
                                {telefonos.length > 0 ? (
                                    telefonos.map((tel, i) => (
                                        <div className="telefono-item" key={i}>
                                            <input type="hidden" name={`Telefonos[${i}].Id`} defaultValue={tel.id} />

                                            <div className="tel-row">
                                                <div className="tel-field tel-code">
                                                    <label className="form-label">Código</label>
                                                    <input
                                                        className="form-control"
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

                                                <div className="tel-field tel-number">
                                                    <label className="form-label">Número</label>
                                                    <input
                                                        className="form-control"
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

                                                <div className="tel-field tel-type">
                                                    <label className="form-label">Tipo</label>
                                                    <select
                                                        className="form-control"
                                                        name={`Telefonos[${i}].Tipo`}
                                                        defaultValue={tel.tipo}
                                                        required
                                                    >
                                                        <option value="">Tipo</option>
                                                        <option value="Personal">Personal</option>
                                                        <option value="Trabajo">Trabajo</option>
                                                        <option value="Hogar">Hogar</option>
                                                        <option value="Encargado">Encargado</option>
                                                        <option value="Otro">Otro</option>
                                                    </select>
                                                </div>

                                                <div className="tel-field tel-status">
                                                    <label className="form-label">Estado</label>
                                                    <div className="toggle-wrapper">
                                                        <label className="toggle-label">
                                                            <input
                                                                type="checkbox"
                                                                className="toggle-input"
                                                                name={`Telefonos[${i}].Estado`}
                                                                defaultChecked={tel.estado}
                                                            />
                                                            <span className="toggle-slider"></span>
                                                        </label>
                                                    </div>
                                                </div>

                                                <div className="tel-field tel-action">
                                                    <label className="form-label">&nbsp;</label>
                                                    <button type="button" className="btn btn-remove-tel btn-remove-telefono" onClick={() => eliminarTelefono(i)}>
                                                        <FontAwesomeIcon icon={faTrash} />
                                                    </button>
                                                </div>
                                            </div>
                                        </div>
                                    ))
                                ) : (
                                    <div className="empty-state">
                                        <FontAwesomeIcon icon={faInfoCircle} className="me-2" />
                                        No hay teléfonos registrados.
                                    </div>
                                )}
                            </div>
                        </div>

                        <div className="form-section account-status-section">
                            <div className="section-header">
                                <span className="section-icon"><FontAwesomeIcon icon={faCog} /></span>
                                <h3 className="section-title">Estado de Cuenta</h3>
                            </div>

                            <div className="status-row">
                                <div className="status-toggle-container">

                                    <div className="status-text">
                                        <span className={`status-badge ${usuario?.estado ? "active" : "inactive"}`}>
                                            {usuario?.estado ? "Activo" : "Inactivo"}
                                        </span>
                                        <small className="text-muted">
                                            <FontAwesomeIcon icon={faInfoCircle} className="me-1" />
                                            Los usuarios inactivos no podrán acceder al sistema
                                        </small>
                                    </div>
                                    <div className="toggle-wrapper justify-content-upper">
                                        <label className="toggle-label-large">
                                            <input
                                                type="checkbox"
                                                className="toggle-input"
                                                id="estadoUsuario"
                                                name="Estado"
                                                defaultChecked={usuario?.estado}
                                            />
                                            <span className="toggle-slider-large"></span>
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div className="edit-footer">
                        <button type="button" className="btn btn-cancel" onClick={onClose}>
                            <FontAwesomeIcon icon={faTimes} className="me-2" />Cancelar
                        </button>
                        <button type="submit" className="btn btn-save" id="btnGuardar" disabled={saving}>
                            <FontAwesomeIcon icon={faSave} className="me-2" />
                            {saving ? "Guardando..." : "Guardar Cambios"}
                        </button>
                    </div>

                </div>
            </form>
        </div>
    );
}