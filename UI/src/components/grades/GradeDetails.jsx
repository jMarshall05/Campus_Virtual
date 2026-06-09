import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTimes, faStar, faUser, faBook, faComments } from "../../content/icons.js";

export default function GradeDetails({ calificacion, onClose }) {
    return (
        <div className="modal-content" style={{ borderRadius: "16px", border: "none" }}>
            <div className="d-flex justify-content-between align-items-center p-3"
                style={{ background: "linear-gradient(135deg, #f59e0b, #d97706)", borderRadius: "16px 16px 0 0", color: "white" }}>
                <div className="d-flex align-items-center gap-2">
                    <FontAwesomeIcon icon={faStar} />
                    <h5 className="mb-0 fw-bold">Detalles de Calificación</h5>
                </div>
                <button type="button" className="btn" onClick={onClose} aria-label="Cerrar" style={{color: "white", background: "rgba(255,255,255,0.2)", borderRadius: "8px"}}><FontAwesomeIcon icon={faTimes} /></button>
            </div>
            <div className="p-4">
                <table className="table table-bordered" style={{ borderRadius: "12px", overflow: "hidden" }}>
                    <tbody>
                        <tr>
                            <th style={{ width: "35%", background: "#f8fafc" }}>
                                <FontAwesomeIcon icon={faUser} className="me-2" /> Estudiante
                            </th>
                            <td className="fw-bold">{calificacion.nombreEstudiante || "N/A"}</td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>
                                <FontAwesomeIcon icon={faBook} className="me-2" /> Tarea
                            </th>
                            <td>{calificacion.nombreTarea || "N/A"}</td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>
                                <FontAwesomeIcon icon={faStar} className="me-2" /> Calificación
                            </th>
                            <td className="fw-bold" style={{ fontSize: "1.2rem" }}>
                                {parseFloat(calificacion.calificacion).toFixed(1)}
                            </td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>
                                <FontAwesomeIcon icon={faComments} className="me-2" /> Comentario
                            </th>
                            <td>{calificacion.comentario || "Sin comentario"}</td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>Estado</th>
                            <td>
                                <span className={`status-badge ${calificacion.estado ? "active" : "inactive"}`}>
                                    {calificacion.estado ? "Activo" : "Inactivo"}
                                </span>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <div className="d-flex justify-content-end">
                    <button type="button" className="btn btn-outline-secondary" onClick={onClose}
                        style={{ borderRadius: "10px", fontWeight: 600 }}>Cerrar</button>
                </div>
            </div>
        </div>
    );
}
