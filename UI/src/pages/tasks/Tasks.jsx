import { useEffect, useState, useMemo } from "react";
import { getTareas, toggleTareaState } from "../../api/tareasService.js";
import Loader from "../../components/Loader.jsx";
import "../../content/tasks/tasks.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faTasks, faSearch, faSort, faEdit, faEye,
    faChevronLeft, faChevronRight, faPlusCircle,
    faCalendarAlt, faPaperclip, faCheckCircle, faTrash
} from "../../content/icons.js";
import TaskDetails from "../../components/tasks/TaskDetails.jsx";
import AddTask from "../../components/tasks/AddTask.jsx";
import EditTask from "../../components/tasks/EditTask.jsx";
import Swal from "sweetalert2";


const formatDate = (dateStr) =>
    dateStr
        ? new Date(dateStr).toLocaleDateString("es-CR", {
              day: "2-digit",
              month: "2-digit",
              year: "numeric",
          })
        : "N/A";

export default function Tasks() {
    const [loading, setLoading] = useState(true);
    const [tareas, setTareas] = useState([]);
    const [search, setSearch] = useState("");
    const [modalhidden, setModalHidden] = useState(true);
    const [modalType, setModalType] = useState("");
    const [tareaModal, setTareaModal] = useState(null);
    const [pagina, setPagina] = useState(1);
    const porPagina = 10;
    const nowDate = useMemo(() => new Date(), []);


    const handleSearchChange = (e) => {
        setSearch(e.target.value);
        setPagina(1);
    };

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

    useEffect(() => {
        cargarDatos();
    }, []);

    const filtradas = tareas.filter((t) => {
        const texto = search.toLowerCase();
        return (
            t.titulo?.toLowerCase().includes(texto) ||
            t.descripcion?.toLowerCase().includes(texto) ||
            t.nombreGrupo?.toLowerCase().includes(texto)
        );
    });

    const activas = filtradas.filter((t) => t.estado === true);
    const totalPaginas = Math.ceil(filtradas.length / porPagina);
    const paginadas = filtradas.slice(
        (pagina - 1) * porPagina,
        pagina * porPagina
    );

    const toggleEstado = async (id) => {
        try {
            await toggleTareaState(id);
            Swal.fire({
                title: "Estado cambiado",
                icon: "success",
                timer: 1500,
            });
            cargarDatos();
        } catch (error) {
            console.error(error);
            Swal.fire({ title: "Error", icon: "error", confirmButtonText: "OK" });
        }
    };

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
                            <h1>Gestión de Tareas</h1>
                            <p className="header-subtitle">
                                Administra todas las tareas del sistema
                            </p>
                        </div>
                    </div>
                </div>

                <div className="stats-grid">
                    <div className="stat-card">
                        <div className="stat-icon tasks-icon">
                            <FontAwesomeIcon icon={faTasks} />
                        </div>
                        <div className="stat-info">
                            <h3>{filtradas.length}</h3>
                            <p>Total Tareas</p>
                        </div>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon active-icon">
                            <FontAwesomeIcon icon={faCheckCircle} />
                        </div>
                        <div className="stat-info">
                            <h3>{activas.length}</h3>
                            <p>Tareas Activas</p>
                        </div>
                    </div>
                </div>
            </div>

            <div className="premium-card">
                <div className="card-header-premium">
                    <div className="header-actions">
                        <div className="search-container">
                            <FontAwesomeIcon icon={faSearch} className="search-icon" />
                            <input
                                type="text"
                                className="search-input" id="searchInput" aria-label="Buscar"
                                placeholder="Buscar tareas..."
                                value={search}
                                onChange={handleSearchChange}
                            />
                        </div>
                        <div className="filter-actions">
                            <button type="button"
                                className="btn-premium"
                                onClick={() => {
                                    setModalType("add");
                                    setModalHidden(false);
                                }}
                            >
                                <FontAwesomeIcon icon={faPlusCircle} className="me-2" />
                                Nueva Tarea
                            </button>
                        </div>
                    </div>
                </div>

                <div className="card-body-premium">
                    <div className="table-container">
                        <table className="premium-table">
                            <thead>
                                <tr>
                                    <th>
                                        <div className="th-content">
                                            <span>Título</span>
                                            <FontAwesomeIcon icon={faSort} />
                                        </div>
                                    </th>
                                    <th>
                                        <div className="th-content">
                                            <span>Grupo</span>
                                            <FontAwesomeIcon icon={faSort} />
                                        </div>
                                    </th>
                                    <th>
                                        <div className="th-content">
                                            <span>Fecha Entrega</span>
                                            <FontAwesomeIcon icon={faCalendarAlt} className="me-1" />
                                        </div>
                                    </th>
                                    <th>
                                        <div className="th-content">
                                            <span>Archivo</span>
                                        </div>
                                    </th>
                                    <th>
                                        <div className="th-content">
                                            <span>Estado</span>
                                        </div>
                                    </th>
                                    <th>
                                        <div className="th-content">
                                            <span>Acciones</span>
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                {filtradas.length === 0 ? (
                                    <tr>
                                        <td colSpan="6" className="text-center text-muted py-4">
                                            <FontAwesomeIcon icon={faTasks} className="fa-2x mb-2" />
                                            <br />
                                            No hay tareas registradas
                                        </td>
                                    </tr>
                                ) : (
                                    paginadas.map((tarea) => (
                                        <tr className="task-row" key={tarea.idTarea}>
                                            <td data-label="Título">
                                                <div className="task-info">
                                                    <div className="task-icon">
                                                        <FontAwesomeIcon icon={faTasks} />
                                                    </div>
                                                    <div className="task-details">
                                                        <span className="task-name">{tarea.titulo}</span>
                                                        <span className="task-id">ID: {tarea.idTarea}</span>
                                                    </div>
                                                </div>
                                            </td>
                                            <td data-label="Grupo">
                                                <span className="id-badge">{tarea.nombreGrupo || "N/A"}</span>
                                            </td>
                                            <td data-label="Fecha Entrega">
                                                <span className={`badge-date ${new Date(tarea.fechaEntrega) < nowDate ? "overdue" : ""}`}>
                                                    {formatDate(tarea.fechaEntrega)}
                                                </span>
                                            </td>
                                            <td data-label="Archivo">
                                                <span className={`badge-attachment ${tarea.archivoAdjunto ? "has-file" : "no-file"}`}>
                                                    <FontAwesomeIcon icon={faPaperclip} />
                                                    {tarea.archivoAdjunto ? "Sí" : "No"}
                                                </span>
                                            </td>
                                            <td data-label="Estado">
                                                <span className={`status-badge ${tarea.estado ? "active" : "inactive"}`}>
                                                    {tarea.estado ? "Activo" : "Inactivo"}
                                                </span>
                                            </td>
                                            <td data-label="Acciones">
                                                <div className="actions-container">
                                                    <button type="button"
                                                        className="btn btn-outline-dark btn-sm"
                                                        title="Ver Detalles"
                                                        onClick={() => {
                                                            setTareaModal(tarea);
                                                            setModalType("details");
                                                            setModalHidden(false);
                                                        }}
                                                    >
                                                        <FontAwesomeIcon icon={faEye} />
                                                    </button>
                                                    <button type="button"
                                                        className="btn btn-outline-primary btn-sm"
                                                        title="Editar"
                                                        onClick={() => {
                                                            setTareaModal(tarea);
                                                            setModalType("edit");
                                                            setModalHidden(false);
                                                        }}
                                                    >
                                                        <FontAwesomeIcon icon={faEdit} />
                                                    </button>
                                                    <button type="button"
                                                        className={`btn btn-sm ${tarea.estado ? "btn-outline-danger" : "btn-outline-success"}`}
                                                        title={tarea.estado ? "Desactivar" : "Activar"}
                                                        onClick={() => toggleEstado(tarea.idTarea)}
                                                    >
                                                        <FontAwesomeIcon icon={tarea.estado ? faTrash : faCheckCircle} />
                                                    </button>
                                                </div>
                                            </td>
                                        </tr>
                                    ))
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>

                <div className="card-footer-premium">
                    <div className="pagination-info">
                        Mostrando <strong>{paginadas.length}</strong> de{" "}
                        <strong>{filtradas.length}</strong> tareas
                    </div>
                    <div className="pagination-controls">
                        <button type="button"
                            className={`pagination-btn ${pagina === 1 ? "disabled" : ""}`}
                            disabled={pagina === 1}
                            onClick={() => setPagina((p) => Math.max(p - 1, 1))}
                        >
                            <FontAwesomeIcon icon={faChevronLeft} />
                        </button>
                        <span className="pagination-btn active">{pagina}</span>
                        <button type="button"
                            className={`pagination-btn ${pagina >= totalPaginas ? "disabled" : ""}`}
                            disabled={pagina >= totalPaginas}
                            onClick={() => setPagina((p) => Math.min(p + 1, totalPaginas))}
                        >
                            <FontAwesomeIcon icon={faChevronRight} />
                        </button>
                    </div>
                </div>
            </div>

            {!modalhidden && (
                <>
                    <div className="modal fade show d-block" tabIndex="-1">
                        <div className="modal-dialog modal-dialog-centered modal-lg">
                            <div className="modal-content">
                                <div className="modal-body">
                                    {modalType === "details" && (
                                        <TaskDetails
                                            tarea={tareaModal}
                                            onClose={() => setModalHidden(true)}
                                        />
                                    )}
                                    {modalType === "edit" && (
                                        <EditTask
                                            tarea={tareaModal}
                                            onClose={() => {
                                                setModalHidden(true);
                                                cargarDatos();
                                            }}
                                        />
                                    )}
                                    {modalType === "add" && (
                                        <AddTask
                                            onClose={() => {
                                                setModalHidden(true);
                                                cargarDatos();
                                            }}
                                        />
                                    )}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="modal-backdrop fade show"></div>
                </>
            )}
        </div>
    );
}
