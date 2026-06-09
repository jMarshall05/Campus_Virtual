import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTimes, faFileUpload, faUser, faBook, faPaperclip, faCalendar } from "../../content/icons.js";


    const formatDate = (dateStr) =>
        dateStr ? new Date(dateStr).toLocaleDateString("es-CR", { day: "2-digit", month: "2-digit", year: "numeric" }) : "N/A";

export default function SubmissionDetails({ entrega, onClose }) {

    return (
        <div className="modal-content" style={{ borderRadius: "16px", border: "none" }}>
            <div className="d-flex justify-content-between align-items-center p-3"
                style={{ background: "linear-gradient(135deg, #8b5cf6, #7c3aed)", borderRadius: "16px 16px 0 0", color: "white" }}>
                <div className="d-flex align-items-center gap-2">
                    <FontAwesomeIcon icon={faFileUpload} />
                    <h5 className="mb-0 fw-bold">Detalles de Entrega</h5>
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
                            <td className="fw-bold">{entrega.nombreEstudiante || "N/A"}</td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>
                                <FontAwesomeIcon icon={faBook} className="me-2" /> Tarea
                            </th>
                            <td>{entrega.nombreTarea || "N/A"}</td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>
                                <FontAwesomeIcon icon={faCalendar} className="me-2" /> Fecha de Entrega
                            </th>
                            <td>{formatDate(entrega.fechaEntrega)}</td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>
                                <FontAwesomeIcon icon={faPaperclip} className="me-2" /> Archivo
                            </th>
                            <td>
                                {entrega.archivoEntregado ? (
                                    <a href={entrega.archivoEntregado} target="_blank" rel="noopener noreferrer"
                                        className="btn btn-sm btn-outline-primary" style={{ borderRadius: "8px" }}>
                                        <FontAwesomeIcon icon={faPaperclip} className="me-1" /> Ver Archivo
                                    </a>
                                ) : <span className="text-muted">Sin archivo</span>}
                            </td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>Estado</th>
                            <td>
                                <span className={`status-badge ${entrega.estado ? "active" : "inactive"}`}>
                                    {entrega.estado ? "Activo" : "Inactivo"}
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
