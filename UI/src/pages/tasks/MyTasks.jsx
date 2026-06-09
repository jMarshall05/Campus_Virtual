import { useEffect, useState, useMemo } from "react";
import "../../content/tasks/tasks.css";
import Loader from "../../components/Loader.jsx";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faTasks, faPaperclip,
    faUpload, faStar
} from "../../content/icons.js";
import { getTareas } from "../../api/tareasService.js";


const formatDate = (dateStr) =>
    dateStr
        ? new Date(dateStr).toLocaleDateString("es-CR", {
              day: "2-digit",
              month: "2-digit",
              year: "numeric",
          })
        : "N/A";

export default function MyTasks() {
    const [loading, setLoading] = useState(true);
    const [tareas, setTareas] = useState([]);

    useEffect(() => {
        const cargarDatos = async () => {
            try {
                const data = await getTareas();
                setTareas(data);
            } catch (error) {
                console.error("Error al cargar tareas:", error);
            } finally {
                setLoading(false);
            }
        };
        cargarDatos();
    }, []);

    if (loading) return <Loader />;

    return (
        <div className="admin-container-fluid">
            <div className="admin-header">
                <div className="header-content">
                    <div className="header-title">
                        <div className="title-icon">
                            <FontAwesomeIcon icon={faTasks} />
                        </div>
                        <div>
                            <h1>Mis Tareas Asignadas</h1>
                            <p className="header-subtitle">
                                Visualiza y gestiona tus tareas
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            {tareas.length === 0 ? (
                <div className="premium-card">
                    <div className="card-body-premium p-4 text-center">
                        <FontAwesomeIcon icon={faTasks} className="fa-3x text-muted mb-3" />
                        <h5 className="text-muted">No tienes tareas asignadas</h5>
                        <p className="text-muted small">
                            Cuando se te asignen tareas, aparecerán aquí
                        </p>
                    </div>
                </div>
            ) : (
                <div className="row g-4">
                    {tareas.map((tarea) => {
                        const fechaVencida = new Date(tarea.fechaEntrega) < nowDate;
                        const tieneEntrega = tarea.calificacion?.entrega != null && 
                            tarea.calificacion.entrega.archivo_entregado;

                        return (
                            <div className="col-12 col-md-6 col-lg-4" key={tarea.idTarea}>
                                <div
                                    className="card shadow-sm h-100 border-0 hover-card"
                                    style={{
                                        borderRadius: "12px",
                                        background: "linear-gradient(135deg, #ffffff, #f8f9fa)",
                                    }}
                                >
                                    <div className="card-body d-flex flex-column p-4">
                                        <div className="d-flex justify-content-between align-items-start mb-2">
                                            <h5
                                                className="card-title fw-bold text-truncate"
                                                style={{ fontSize: "1.1rem", color: "#343a40" }}
                                                title={tarea.titulo}
                                            >
                                                {tarea.titulo}
                                            </h5>
                                            <span
                                                className={`badge ${fechaVencida ? "bg-warning text-dark" : "bg-primary text-white"}`}
                                                style={{ fontSize: "0.75rem", fontWeight: 600 }}
                                            >
                                                {formatDate(tarea.fechaEntrega)}
                                            </span>
                                        </div>

                                        <p
                                            className="card-text text-truncate mb-3"
                                            style={{ maxHeight: "3rem", fontSize: "0.9rem", color: "#495057" }}
                                            title={tarea.descripcion}
                                        >
                                            {tarea.descripcion}
                                        </p>

                                        <div className="mt-auto d-flex flex-wrap gap-2">
                                            {tarea.archivoAdjunto && (
                                                <a
                                                    href={tarea.archivoAdjunto}
                                                    target="_blank"
                                                    rel="noopener noreferrer"
                                                    className="btn btn-sm btn-outline-primary"
                                                    style={{ borderRadius: "8px", fontWeight: 500 }}
                                                >
                                                    <FontAwesomeIcon icon={faPaperclip} className="me-1" />
                                                    Ver Tarea
                                                </a>
                                            )}

                                            {tieneEntrega ? (
                                                <a
                                                    href={tarea.calificacion.entrega.archivo_entregado}
                                                    target="_blank"
                                                    rel="noopener noreferrer"
                                                    className="btn btn-sm btn-outline-success"
                                                    style={{ borderRadius: "8px", fontWeight: 500 }}
                                                >
                                                    <FontAwesomeIcon icon={faUpload} className="me-1" />
                                                    Ver Entrega
                                                </a>
                                            ) : fechaVencida ? (
                                                <span
                                                    className="badge"
                                                    style={{
                                                        background: "#e53935",
                                                        color: "#fff",
                                                        borderRadius: "999px",
                                                        fontSize: "0.8rem",
                                                        fontWeight: 600,
                                                    }}
                                                >
                                                    Entrega caducada
                                                </span>
                                            ) : (
                                                <span
                                                    className="badge"
                                                    style={{
                                                        background: "#f39c12",
                                                        color: "#fff",
                                                        borderRadius: "999px",
                                                        fontSize: "0.8rem",
                                                        fontWeight: 600,
                                                    }}
                                                >
                                                    Pendiente
                                                </span>
                                            )}

                                            {tarea.calificacion?.calificacion != null && (
                                                <div className="d-flex align-items-center fw-bold">
                                                    <span className="text-dark me-1">Calificación:</span>
                                                    <FontAwesomeIcon
                                                        icon={faStar}
                                                        className="me-1"
                                                        style={{ color: "#f59e0b" }}
                                                    />
                                                    <span>
                                                        {parseFloat(tarea.calificacion.calificacion).toFixed(1)}
                                                    </span>
                                                </div>
                                            )}
                                        </div>

                                        {/* TODO: Agregar navegación a detalles de tarea cuando se implemente la vista de detalles */}
                                    </div>
                                </div>
                            </div>
                        );
                    })}
                </div>
            )}
        </div>
    );
}
