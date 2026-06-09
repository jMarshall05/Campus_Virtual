import { useEffect, useState } from "react";
import { getCalificaciones, toggleCalificacionEstado } from "../../api/calificacionesService.js";
import Loader from "../../components/Loader.jsx";
import "../../content/grades/grades.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faStar, faSearch, faSort, faEdit, faEye,
    faChevronLeft, faChevronRight, faPlusCircle,
    faCheckCircle, faTrash
} from "../../content/icons.js";
import GradeDetails from "../../components/grades/GradeDetails.jsx";
import AddGrade from "../../components/grades/AddGrade.jsx";
import EditGrade from "../../components/grades/EditGrade.jsx";
import Swal from "sweetalert2";

const getScoreClass = (score) => {
    if (score >= 80) return "high";
    if (score >= 60) return "medium";
    return "low";
};

export default function Grades() {
    const [loading, setLoading] = useState(true);
    const [calificaciones, setCalificaciones] = useState([]);
    const [search, setSearch] = useState("");
    const [modalhidden, setModalHidden] = useState(true);
    const [modalType, setModalType] = useState("");
    const [gradeModal, setGradeModal] = useState(null);
    const [pagina, setPagina] = useState(1);
    const porPagina = 10;


    const handleSearchChange = (e) => {
        setSearch(e.target.value);
        setPagina(1);
    };

    const cargarDatos = async () => {
        try {
            const data = await getCalificaciones();
            setCalificaciones(data);
        } catch (error) {
            console.error("Error al cargar calificaciones:", error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        cargarDatos();
    }, []);

    const filtradas = calificaciones.filter((c) => {
        const texto = search.toLowerCase();
        return (
            c.nombreEstudiante?.toLowerCase().includes(texto) ||
            c.nombreTarea?.toLowerCase().includes(texto) ||
            c.comentario?.toLowerCase().includes(texto)
        );
    });

    const activas = filtradas.filter((c) => c.estado === true);
    const totalPaginas = Math.ceil(filtradas.length / porPagina);
    const paginadas = filtradas.slice(
        (pagina - 1) * porPagina,
        pagina * porPagina
    );

    const toggleEstado = async (id) => {
        try {
            await toggleCalificacionEstado(id);
            Swal.fire({ title: "Estado cambiado", icon: "success", timer: 1500 });
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
                            <FontAwesomeIcon icon={faStar} />
                        </div>
                        <div>
                            <h1>Gestión de Calificaciones</h1>
                            <p className="header-subtitle">Administra todas las calificaciones del sistema</p>
                        </div>
                    </div>
                </div>

                <div className="stats-grid">
                    <div className="stat-card">
                        <div className="stat-icon grades-icon">
                            <FontAwesomeIcon icon={faStar} />
                        </div>
                        <div className="stat-info">
                            <h3>{filtradas.length}</h3>
                            <p>Total Calificaciones</p>
                        </div>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon active-icon">
                            <FontAwesomeIcon icon={faCheckCircle} />
                        </div>
                        <div className="stat-info">
                            <h3>{activas.length}</h3>
                            <p>Calificaciones Activas</p>
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
                                placeholder="Buscar calificaciones..."
                                value={search}
                                onChange={handleSearchChange}
                            />
                        </div>
                        <div className="filter-actions">
                            <button type="button" className="btn-premium" onClick={() => { setModalType("add"); setModalHidden(false); }}>
                                <FontAwesomeIcon icon={faPlusCircle} className="me-2" />
                                Nueva Calificación
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
                                            <span>Estudiante</span>
                                            <FontAwesomeIcon icon={faSort} />
                                        </div>
                                    </th>
                                    <th>
                                        <div className="th-content">
                                            <span>Tarea</span>
                                            <FontAwesomeIcon icon={faSort} />
                                        </div>
                                    </th>
                                    <th>
                                        <div className="th-content">
                                            <span>Calificación</span>
                                            <FontAwesomeIcon icon={faSort} />
                                        </div>
                                    </th>
                                    <th>
                                        <div className="th-content">
                                            <span>Comentario</span>
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
                                            <FontAwesomeIcon icon={faStar} className="fa-2x mb-2" />
                                            <br />
                                            No hay calificaciones registradas
                                        </td>
                                    </tr>
                                ) : (
                                    paginadas.map((cal) => (
                                        <tr className="grade-row" key={cal.idCalificacion}>
                                            <td data-label="Estudiante">
                                                <div className="grade-info">
                                                    <div className="grade-icon">
                                                        <FontAwesomeIcon icon={faStar} />
                                                    </div>
                                                    <div className="grade-details">
                                                        <span className="grade-name">{cal.nombreEstudiante || "N/A"}</span>
                                                        <span className="grade-id">ID: {cal.idCalificacion}</span>
                                                    </div>
                                                </div>
                                            </td>
                                            <td data-label="Tarea">
                                                <span className="id-badge">{cal.nombreTarea || "N/A"}</span>
                                            </td>
                                            <td data-label="Calificación">
                                                <span className={`grade-score ${getScoreClass(cal.calificacion)}`}>
                                                    {parseFloat(cal.calificacion).toFixed(1)}
                                                </span>
                                            </td>
                                            <td data-label="Comentario">
                                                <span className="grade-comment" title={cal.comentario}>
                                                    {cal.comentario || "Sin comentario"}
                                                </span>
                                            </td>
                                            <td data-label="Estado">
                                                <span className={`status-badge ${cal.estado ? "active" : "inactive"}`}>
                                                    {cal.estado ? "Activo" : "Inactivo"}
                                                </span>
                                            </td>
                                            <td data-label="Acciones">
                                                <div className="actions-container">
                                                    <button type="button" className="btn btn-outline-dark btn-sm" title="Ver Detalles"
                                                        onClick={() => { setGradeModal(cal); setModalType("details"); setModalHidden(false); }}>
                                                        <FontAwesomeIcon icon={faEye} />
                                                    </button>
                                                    <button type="button" className="btn btn-outline-primary btn-sm" title="Editar"
                                                        onClick={() => { setGradeModal(cal); setModalType("edit"); setModalHidden(false); }}>
                                                        <FontAwesomeIcon icon={faEdit} />
                                                    </button>
                                                    <button type="button" className={`btn btn-sm ${cal.estado ? "btn-outline-danger" : "btn-outline-success"}`}
                                                        title={cal.estado ? "Desactivar" : "Activar"}
                                                        onClick={() => toggleEstado(cal.idCalificacion)}>
                                                        <FontAwesomeIcon icon={cal.estado ? faTrash : faCheckCircle} />
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
                        Mostrando <strong>{paginadas.length}</strong> de <strong>{filtradas.length}</strong> calificaciones
                    </div>
                    <div className="pagination-controls">
                        <button type="button" className={`pagination-btn ${pagina === 1 ? "disabled" : ""}`}
                            disabled={pagina === 1} onClick={() => setPagina((p) => Math.max(p - 1, 1))}>
                            <FontAwesomeIcon icon={faChevronLeft} />
                        </button>
                        <span className="pagination-btn active">{pagina}</span>
                        <button type="button" className={`pagination-btn ${pagina >= totalPaginas ? "disabled" : ""}`}
                            disabled={pagina >= totalPaginas} onClick={() => setPagina((p) => Math.min(p + 1, totalPaginas))}>
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
                                    {modalType === "details" && <GradeDetails calificacion={gradeModal} onClose={() => setModalHidden(true)} />}
                                    {modalType === "edit" && <EditGrade calificacion={gradeModal} onClose={() => { setModalHidden(true); cargarDatos(); }} />}
                                    {modalType === "add" && <AddGrade onClose={() => { setModalHidden(true); cargarDatos(); }} />}
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
