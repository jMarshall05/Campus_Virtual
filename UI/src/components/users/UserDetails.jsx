import {faUsers,faEyeSlash,faEye,faDownload, faInfoCircle, faEdit, faArrowLeft, faIdCard, faPassport, faPlane, faUser, faCircle,faEnvelope,faPhone,faFingerprint,faCalendarAlt,faBirthdayCake,faUserPlus  } from "../../content/icons.js";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { useEffect, useMemo, useState } from "react";
import { exportUserQr, exportUserPdf } from "../../api/userService.js";
import "../../content/users/userDetails.css";

const handleExportUserPdf = async (idUsuario) => {
    try {
        const response = await exportUserPdf(idUsuario);
        const url = URL.createObjectURL(response);
        window.open(url, "_blank");
    } catch (error) {
        console.error("Error al generar el PDF:", error);
    }
};

function getTipoIdInfo(tipoIdentificacion) {
    switch (tipoIdentificacion) {
        case "Fisica":
            return { icon: faIdCard, text: "Cédula de Ciudadanía", color: "text-primary" };
        case "DIMEX":
            return { icon: faPassport, text: "DIMEX", color: "text-info" };
        case "Pasaporte":
            return { icon: faPlane, text: "Pasaporte", color: "text-success" };
        default:
            return { icon: faUser, text: "Desconocido", color: "text-secondary" };
    }
}

export default function UserDetails({ usuario, onClose }) {
    const [visibles, setVisibles] = useState({});
    const [qr, setQr] = useState();
    const { icon: tipoIdIcon, text: tipoIdText, color: tipoIdColor } = useMemo(
        () => getTipoIdInfo(usuario.tipoIdentificacion),
        [usuario.tipoIdentificacion]
    );

    useEffect(() => {
        const cargarQr = async () => {
            try {
                const response = await exportUserQr(usuario.idUsuario);
                const url = URL.createObjectURL(response);
                setQr(url);
            } catch (error) {
                console.error("Error al cargar el QR:", error);
            }
        };
        cargarQr();
    }, [usuario]);

    const togglePhone = (index) => {
        setVisibles((prev) => ({
            ...prev,
            [index]: !prev[index],
        }));
    };
    return (
        <>
            <div className="detail-header">
                <div className="header-content">
                    <div className="user-avatar-large">
                        <div className="avatar-placeholder-large">
                            {usuario.nombre.charAt(0).toUpperCase()}{usuario.apellido.charAt(0).toUpperCase()}
                        </div>
                    </div>
                    <div className="user-info-header">
                        <h2 className="user-name-large">{usuario.nombre} {usuario.apellido}</h2>
                        <p className="user-role-badge">{usuario.rol}</p> 
                        <span className="status-badge active">
                            {usuario.estado ? "Activo" : "Inactivo"}
                        </span>
                    </div>
                </div>
            </div>
            <div className="detail-content">
                <div className="info-grid">
                    <div className="info-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faUser} className="me-2" />Información Personal
                        </h3>
                        <div className="info-items">
                            <div className="info-item">
                                <span className="info-label">Email</span>
                                <span className="info-value email-value">
                                    <FontAwesomeIcon icon={faEnvelope} className="me-2 text-primary" />
                                    <span>{usuario.email}</span>
                                </span>
                            </div>

                            <div className="info-item">
                                <span className="info-label">Teléfonos</span>
                                <div className="info-value flex-column w-100">
                                    {usuario.telefonos && usuario.telefonos.length > 0 ? (

                                        <ul className="list-unstyled mb-0">
                                            {usuario?.telefonos?.map((telefono, index) => {
                                                const telefonoStr = telefono.telefono.toString();
                                                const telefonoFormateado =
                                                    telefonoStr.substring(0, 4) + "-" + telefonoStr.substring(4);
                                                const telefonoOculto =
                                                    "****-" + telefonoStr.substring(telefonoStr.length - 4);

                                                const visible = visibles[index];

                                                return (
                                                    <li className="mb-2 phone-item" key={index}>
                                                        <div className="phone-content-wrapper">
                                                            <div className="phone-info">
                                                                <FontAwesomeIcon icon={faPhone} className="me-2 text-success" />
                                                                <strong className="phone-code">(+{telefono.codigo})</strong>

                                                                <span className="phone-number">
                                                                    {visible ? telefonoFormateado : telefonoOculto}
                                                                </span>

                                                                <span className="phone-type">{telefono.tipo}</span>
                                                            </div>

                                                            <div className="phone-actions">
                                                                <button
                                                                    className="btn-toggle-phone"
                                                                    type="button"
                                                                    title="Mostrar/Ocultar teléfono"
                                                                    onClick={() => togglePhone(index)}
                                                                >
                                                                    <FontAwesomeIcon icon={visible ? faEyeSlash : faEye} className="text-primary" style={{color : "black"}} />
                                                                </button>

                                                                {telefono.estado ? (
                                                                    <span className="badge bg-success">Activo</span>
                                                                ) : (
                                                                    <span className="badge bg-danger">Inactivo</span>
                                                                )}
                                                            </div>
                                                        </div>
                                                    </li>
                                                );
                                            })}
                                        </ul>

                                    ) : (
                                        <span className="text-muted"><FontAwesomeIcon icon={faPhone} className="me-2 text-success" />No se registraron teléfonos</span>
                                    )}

                                </div>
                            </div>

                            <div className="info-item">
                                <span className="info-label">Tipo de Identificación</span>
                                <span className="info-value tipo-id-value">

                                    <FontAwesomeIcon icon={tipoIdIcon} className={`fas ${tipoIdColor} me-2`} />
                                    <span>{tipoIdText}</span>
                                </span>
                            </div>

                            <div className="info-item">
                                <span className="info-label">Número de Identificación</span>
                                <span className="info-value Identificacion-value">
                                    <FontAwesomeIcon icon={faFingerprint} className="me-2 text-warning" />
                                    <span>{usuario.identificacion}</span>
                                </span>
                            </div>
                        </div>
                    </div>
                    <div className="info-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faCalendarAlt} className="me-2" />Fechas Importantes
                        </h3>
                        <div className="info-items">
                            <div className="info-item">
                                <span className="info-label">Fecha de Nacimiento</span>
                                <span className="info-value">
                                    <FontAwesomeIcon icon={faBirthdayCake} className="me-2 text-info" />
                                    {usuario.fechaDeNacimiento ? new Date(usuario.fechaDeNacimiento).toLocaleDateString() : "No registrada"}
                                </span>
                            </div>

                            <div className="info-item">
                                <span className="info-label">Fecha de Registro</span>
                                <span className="info-value">
                                    <FontAwesomeIcon icon={faUserPlus} className="me-2 text-success" />
                                    {usuario.fechaDeRegistro ? new Date(usuario.fechaDeRegistro).toLocaleDateString() : "No registrada"}
                                </span>
                            </div>

                            {usuario.fechaDeModificacion && (
                                <div className="info-item">
                                    <span className="info-label">Fecha de Modificación</span>
                                    <span className="info-value">
                                        <FontAwesomeIcon icon={faEdit} className="me-2 text-warning" />
                                        {new Date(usuario.fechaDeModificacion).toLocaleDateString()}
                                    </span>
                                </div>)
                            }
                        </div>
                    </div>
                    <div className="info-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faInfoCircle} className="me-2" />Información Adicional
                        </h3>
                        <div className="info-items">
                            {usuario.rol === "Estudiantes" && usuario.grupo !== null && (
                                <div className="info-item">
                                    <span className="info-label">Grupo</span>
                                    <span className="info-value">
                                        <FontAwesomeIcon icon={faUsers} className="me-2 text-primary" />
                                        {usuario.grupo.nombre}
                                    </span>
                                </div>
                            )}

                            <div className="info-item">
                                <span className="info-label">Estado</span>
                                <span className="info-value">
                                    <FontAwesomeIcon icon={faCircle} className={usuario.estado ? "text-success" : "text-danger"} />
                                    {usuario.estado ? "Activo" : "Inactivo"}
                                </span>
                            </div>
                        </div>
                    </div>
                    <div className="info-section justify-content-center text-center">
                        <button type="button" onClick={() => handleExportUserPdf(usuario.idUsuario)}
                            className="qr-link btn" style={{background: 'none', border: 'none', padding: 0}}>
                            <div className="qr-container">
                                <img src={`${qr}`}
                                    alt="Código QR"
                                    className="img-fluid"
                                    style={{ maxWidth: "250px" }} />
                                <div className="qr-overlay">
                                    <FontAwesomeIcon icon={faDownload} className="me-2" />
                                    <span>Descargar Reporte</span>
                                </div>
                            </div>
                        </button>
                        <p className="qr-description">Escanea el código para descargar el reporte completo</p>

                    </div>
                </div>

                <div className="detail-footer">
                    <button type="button" className="btn btn-secondary"
                        data-bs-dismiss="modal"
                        onClick={onClose}>
                        <FontAwesomeIcon icon={faArrowLeft} className=" me-2" />Volver a la lista
                    </button>
                </div>
            </div>
        </ >
    );

}