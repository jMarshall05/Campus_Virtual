import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import "../../content/groups/groupDetails.css";
import { faUserSlash, faEnvelope, faUsers, faArrowLeft, faChartBar, faUserGraduate, faCheckCircle, faQrcode, faDownload } from "../../content/icons.js";

export default function GroupDetails({ grupo, onClose }) {

    return (
        <div className="group-detail-modal">
            <div className="detail-header">
                <div className="header-content">
                    <div className="group-avatar-large">
                        <div className="avatar-placeholder-large">
                            <FontAwesomeIcon icon={faUsers} />
                        </div>
                    </div>
                    <div className="group-info-header">
                        <h2 className="group-name-large">{grupo.nombre_grupo}</h2>
                        <p className="group-description">{grupo.descripcion}</p>
                        <span className={`status-badge ${grupo.estado ? "active" : "inactive"}`}>
                            {grupo.estado ? "Activo" : "Inactivo"}
                        </span>
                    </div>
                </div>
            </div>

            <div className="detail-content">
                <div className="info-grid">
                    <div className="info-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faUsers} className="me-2" />Información del Grupo
                        </h3>
                        <div className="info-items">
                            <div className="info-item">
                                <span className="info-label">ID del Grupo</span>
                                <span className="info-value">
                                    <FontAwesomeIcon icon={faUsers} className="me-2 text-primary" />
                                    {grupo.id_grupo}
                                </span>
                            </div>
                            <div className="info-item">
                                <span className="info-label">Creado Por</span>
                                <span className="info-value">
                                    <FontAwesomeIcon icon={faUsers} className="me-2 text-success" />
                                    {grupo.creado_por}
                                </span>
                            </div>
                            {grupo.modificado_por && (
                                <div className="info-item">
                                    <span className="info-label">Modificador Por</span>
                                    <span className="info-value">
                                        <FontAwesomeIcon icon={faUsers} className="me-2 text-info" />
                                        {grupo.modificado_por}
                                    </span>
                                </div>
                            )}
                            <div className="info-item">
                                <span className="info-label">Fecha de Creación</span>
                                <span className="info-value">
                                    <FontAwesomeIcon icon={faUsers} className="me-2 text-warning" />
                                    {grupo.fecha_creacion}
                                </span>
                            </div>
                            {grupo.fecha_modificacion && (
                                <div className="info-item">
                                    <span className="info-label">Última Modificación</span>
                                    <span className="info-value">
                                        <FontAwesomeIcon icon={faUsers} className="me-2 text-info" />
                                        {grupo.fecha_modificacion}
                                    </span>
                                </div>
                            )}
                        </div>
                    </div>

                    <div className="info-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faChartBar} className="me-2" />Estadísticas
                        </h3>
                        <div className="stats-cards">
                            <div className="mini-stat-card">
                                <div className="mini-stat-icon students-icon">
                                    <FontAwesomeIcon icon={faUserGraduate} className="text-white" />
                                </div>
                                <div className="mini-stat-info">
                                    <h4>{grupo.estudiantes.length}</h4>
                                    <p>Estudiantes</p>
                                </div>
                            </div>
                            <div className="mini-stat-card">
                                <div className="mini-stat-icon status-icon">
                                    {grupo.estado ? (
                                        <FontAwesomeIcon icon={faCheckCircle} className="text-success" />
                                    ) : (
                                        <FontAwesomeIcon icon={faTimesCircle} className="text-danger" />
                                    )}

                                </div>
                                <div className="mini-stat-info">
                                    <p>Estado</p>
                                    <h4>{grupo.estado ? "Activo" : "Inactivo"}</h4>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="info-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faQrcode} className="me-2" />Reporte Digital
                        </h3>
                        <div className="qr-section">
                            <a href="/Grupos/GenerarReportePDF?id=@Model.grupo.id_grupo" className="qr-link" target="_blank">
                                <div className="qr-container">
                                    <img src="/Grupos/GenerarReporteQR?id=@Model.grupo.id_grupo" alt="QR Code" className="qr-code" />
                                    <div className="qr-overlay">
                                        <FontAwesomeIcon icon={faDownload} className="text-white" />
                                        <span>Descargar Reporte</span>
                                    </div>
                                </div>
                            </a>
                            <p className="qr-description">Haz click en el QR o escanéalo para descargar el reporte completo del grupo</p>
                        </div>
                    </div>
                </div>

                <div className="students-section">
                    <div className="section-header">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faUserGraduate} className="me-2" />Estudiantes del Grupo
                        </h3>
                        <span className="students-count">{grupo.estudiantes.length} estudiantes</span>
                    </div>

                    {grupo.estudiantes.length === 0 ? (
                        <div className="empty-state">
                            <FontAwesomeIcon icon={faUserSlash} className="fa-3x mb-3 text-muted" />
                            <h4>No hay estudiantes en este grupo</h4>
                            <p className="text-muted">Agrega estudiantes al grupo para verlos listados aquí</p>
                        </div>
                    ) : (<div className="table-container">
                        <table className="premium-table students-table">
                            <thead>
                                <tr>
                                    <th className="student-col">Estudiante</th>
                                    <th className="email-col">Email</th>
                                    <th className="id-col">Identificacion</th>
                                </tr>
                            </thead>
                            <tbody>
                                {grupo.estudiantes.map((estudiante) => {
                                    const iniciales = estudiante.nombre && estudiante.apellido ? `${estudiante.nombre[0]}${estudiante.apellido[0]}` : "ES";
                                    return (
                                        <tr className="student-row" key={estudiante.idUsuario}>
                                            <td className="student-cell">
                                                <div className="student-info">
                                                    <div className="student-avatar">

                                                        <span className="avatar-text">{iniciales}</span>
                                                    </div>
                                                    <div className="student-details">
                                                        <span className="student-name">{estudiante.nombre} {estudiante.apellido}</span>
                                                    </div>
                                                </div>
                                            </td>
                                            <td className="email-cell">
                                                <div className="email-content">
                                                    <FontAwesomeIcon icon={faEnvelope} className="email-icon" />
                                                    <span>{estudiante.email}</span>
                                                </div>
                                            </td>
                                            <td className="id-cell">
                                                <span className="id-badge">{estudiante.identificacion}</span>
                                            </td>
                                        </tr>
                                    )
                                })}
                            </tbody>
                        </table>
                    </div>)}
                </div>
            </div>

            <div className="detail-footer">
                <button type="button" className="btn btn-secondary" onClick={onClose}>
                    <FontAwesomeIcon icon={faArrowLeft} className="me-2" />Volver a la lista
                </button>
            </div>
        </div >
    );

}