import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTimes, faTasks, faCalendarAlt, faPaperclip, faUsers, faBook } from "../../content/icons.js";


    const formatDate = (dateStr) =>
        dateStr    ? new Date(dateStr).toLocaleDateString("es-CR", {
                  day: "2-digit",
                  month: "2-digit",
                  year: "numeric",
                  hour: "2-digit",
                  minute: "2-digit",
              })
            : "N/A";

export default function TaskDetails({ tarea, onClose }) {
        

    return (
        <div className="modal-content" style={{ borderRadius: "16px", border: "none" }}>
            <div
                className="d-flex justify-content-between align-items-center p-3"
                style={{
                    background: "linear-gradient(135deg, #3b82f6, #2563eb)",
                    borderRadius: "16px 16px 0 0",
                    color: "white",
                }}
            >
                <div className="d-flex align-items-center gap-2">
                    <FontAwesomeIcon icon={faTasks} />
                    <h5 className="mb-0 fw-bold">Detalles de Tarea</h5>
                </div>
                <button type="button"
                    className="btn"
                    onClick={onClose}
                    style={{ color: "white", background: "rgba(255,255,255,0.2)", borderRadius: "8px" }}
                >
                    <FontAwesomeIcon icon={faTimes} />
                </button>
            </div>

            <div className="p-4">
                <table className="table table-bordered" style={{ borderRadius: "12px", overflow: "hidden" }}>
                    <tbody>
                        <tr>
                            <th style={{ width: "35%", background: "#f8fafc" }}>
                                <FontAwesomeIcon icon={faTasks} className="me-2" />
                                Título
                            </th>
                            <td className="fw-bold">{tarea.titulo}</td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>
                                <FontAwesomeIcon icon={faBook} className="me-2" />
                                Descripción
                            </th>
                            <td>{tarea.descripcion || "Sin descripción"}</td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>
                                <FontAwesomeIcon icon={faUsers} className="me-2" />
                                Grupo
                            </th>
                            <td>
                                <span className="id-badge">{tarea.nombreGrupo || "N/A"}</span>
                            </td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>
                                <FontAwesomeIcon icon={faCalendarAlt} className="me-2" />
                                Fecha de Entrega
                            </th>
                            <td>
                                <span
                                    className={`badge ${
                                        new Date(tarea.fechaEntrega) < new Date()
                                            ? "bg-danger"
                                            : "bg-primary"
                                    }`}
                                >
                                    {formatDate(tarea.fechaEntrega)}
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>
                                <FontAwesomeIcon icon={faPaperclip} className="me-2" />
                                Archivo Adjunto
                            </th>
                            <td>
                                {tarea.archivoAdjunto ? (
                                    <a
                                        href={tarea.archivoAdjunto}
                                        target="_blank"
                                        rel="noopener noreferrer"
                                        className="btn btn-sm btn-outline-primary"
                                        style={{ borderRadius: "8px" }}
                                    >
                                        <FontAwesomeIcon icon={faPaperclip} className="me-1" />
                                        Ver Archivo
                                    </a>
                                ) : (
                                    <span className="text-muted">Sin archivo adjunto</span>
                                )}
                            </td>
                        </tr>
                        <tr>
                            <th style={{ background: "#f8fafc" }}>Estado</th>
                            <td>
                                <span className={`status-badge ${tarea.estado ? "active" : "inactive"}`}>
                                    {tarea.estado ? "Activo" : "Inactivo"}
                                </span>
                            </td>
                        </tr>
                    </tbody>
                </table>

                <div className="d-flex justify-content-end">
                    <button
                        type="button"
                        className="btn btn-outline-secondary"
                        onClick={onClose}
                        style={{ borderRadius: "10px", fontWeight: 600 }}
                    >
                        Cerrar
                    </button>
                </div>
            </div>
        </div>
    );
}
