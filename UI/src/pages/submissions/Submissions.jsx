import { useEffect, useState } from "react";
import { getEntregas, toggleEntregaEstado } from "../../api/entregasService.js";
import Loader from "../../components/Loader.jsx";
import "../../content/submissions/submissions.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faFileUpload, faSearch, faSort, faEye,
    faChevronLeft, faChevronRight, faPlusCircle,
    faCheckCircle, faTrash, faPaperclip
} from "../../content/icons.js";
import SubmissionDetails from "../../components/submissions/SubmissionDetails.jsx";
import UploadSubmission from "../../components/submissions/UploadSubmission.jsx";
import Swal from "sweetalert2";


    const formatDate = (dateStr) =>
        dateStr ? new Date(dateStr).toLocaleDateString("es-CR", { day: "2-digit", month: "2-digit", year: "numeric" }) : "N/A";

export default function Submissions() {
    const [loading, setLoading] = useState(true);
    const [entregas, setEntregas] = useState([]);
    const [search, setSearch] = useState("");
    const [modalhidden, setModalHidden] = useState(true);
    const [modalType, setModalType] = useState("");
    const [submissionModal, setSubmissionModal] = useState(null);
    const [pagina, setPagina] = useState(1);
    const porPagina = 10;


    const handleSearchChange = (e) => {
        setSearch(e.target.value);
        setPagina(1);
    };

    const cargarDatos = async () => {
        try {
            const data = await getEntregas();
            setEntregas(data);
        } catch (error) {
            console.error("Error al cargar entregas:", error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => { cargarDatos(); }, []);

    const filtradas = entregas.filter((e) => {
        const texto = search.toLowerCase();
        return (
            e.nombreEstudiante?.toLowerCase().includes(texto) ||
            e.nombreTarea?.toLowerCase().includes(texto)
        );
    });

    const activas = filtradas.filter((e) => e.estado === true);
    const totalPaginas = Math.ceil(filtradas.length / porPagina);
    const paginadas = filtradas.slice((pagina - 1) * porPagina, pagina * porPagina);

    const toggleEstado = async (id) => {
        try {
            await toggleEntregaEstado(id);
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
                            <FontAwesomeIcon icon={faFileUpload} />
                        </div>
                        <div>
                            <h1>Gestión de Entregas</h1>
                            <p className="header-subtitle">Administra todas las entregas del sistema</p>
                        </div>
                    </div>
                </div>
                <div className="stats-grid">
                    <div className="stat-card">
                        <div className="stat-icon submissions-icon">
                            <FontAwesomeIcon icon={faFileUpload} />
                        </div>
                        <div className="stat-info">
                            <h3>{filtradas.length}</h3>
                            <p>Total Entregas</p>
                        </div>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon active-icon">
                            <FontAwesomeIcon icon={faCheckCircle} />
                        </div>
                        <div className="stat-info">
                            <h3>{activas.length}</h3>
                            <p>Entregas Activas</p>
                        </div>
                    </div>
                </div>
            </div>

            <div className="premium-card">
                <div className="card-header-premium">
                    <div className="header-actions">
                        <div className="search-container">
                            <FontAwesomeIcon icon={faSearch} className="search-icon" />
                            <input type="text" className="search-input" id="searchInput" aria-label="Buscar" placeholder="Buscar entregas..."
                                value={search} onChange={handleSearchChange} />
                        </div>
                        <div className="filter-actions">
                            <button type="button" className="btn-premium" onClick={() => { setModalType("add"); setModalHidden(false); }}>
                                <FontAwesomeIcon icon={faPlusCircle} className="me-2" />
                                Nueva Entrega
                            </button>
                        </div>
                    </div>
                </div>
                <div className="card-body-premium">
                    <div className="table-container">
                        <table className="premium-table">
                            <thead>
                                <tr>
                                    <th><div className="th-content"><span>Estudiante</span><FontAwesomeIcon icon={faSort} /></div></th>
                                    <th><div className="th-content"><span>Tarea</span><FontAwesomeIcon icon={faSort} /></div></th>
                                    <th><div className="th-content"><span>Fecha Entrega</span><FontAwesomeIcon icon={faSort} /></div></th>
                                    <th><div className="th-content"><span>Archivo</span></div></th>
                                    <th><div className="th-content"><span>Estado</span></div></th>
                                    <th><div className="th-content"><span>Acciones</span></div></th>
                                </tr>
                            </thead>
                            <tbody>
                                {filtradas.length === 0 ? (
                                    <tr>
                                        <td colSpan="6" className="text-center text-muted py-4">
                                            <FontAwesomeIcon icon={faFileUpload} className="fa-2x mb-2" />
                                            <br />No hay entregas registradas
                                        </td>
                                    </tr>
                                ) : paginadas.map((entrega) => (
                                    <tr className="submission-row" key={entrega.idEntrega}>
                                        <td data-label="Estudiante">
                                            <div className="submission-info">
                                                <div className="submission-icon"><FontAwesomeIcon icon={faFileUpload} /></div>
                                                <div className="submission-details">
                                                    <span className="submission-name">{entrega.nombreEstudiante || "N/A"}</span>
                                                    <span className="submission-id">ID: {entrega.idEntrega}</span>
                                                </div>
                                            </div>
                                        </td>
                                        <td data-label="Tarea"><span className="id-badge">{entrega.nombreTarea || "N/A"}</span></td>
                                        <td data-label="Fecha Entrega"><span className="badge-date">{formatDate(entrega.fechaEntrega)}</span></td>
                                        <td data-label="Archivo">
                                            <span className={`badge-attachment ${entrega.archivoEntregado ? "has-file" : "no-file"}`}>
                                                <FontAwesomeIcon icon={faPaperclip} />
                                                {entrega.archivoEntregado ? "Sí" : "No"}
                                            </span>
                                        </td>
                                        <td data-label="Estado">
                                            <span className={`status-badge ${entrega.estado ? "active" : "inactive"}`}>
                                                {entrega.estado ? "Activo" : "Inactivo"}
                                            </span>
                                        </td>
                                        <td data-label="Acciones">
                                            <div className="actions-container">
                                                <button type="button" className="btn btn-outline-dark btn-sm" title="Ver Detalles"
                                                    onClick={() => { setSubmissionModal(entrega); setModalType("details"); setModalHidden(false); }}>
                                                    <FontAwesomeIcon icon={faEye} />
                                                </button>
                                                <button type="button" className={`btn btn-sm ${entrega.estado ? "btn-outline-danger" : "btn-outline-success"}`}
                                                    title={entrega.estado ? "Desactivar" : "Activar"}
                                                    onClick={() => toggleEstado(entrega.idEntrega)}>
                                                    <FontAwesomeIcon icon={entrega.estado ? faTrash : faCheckCircle} />
                                                </button>
                                            </div>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
                <div className="card-footer-premium">
                    <div className="pagination-info">
                        Mostrando <strong>{paginadas.length}</strong> de <strong>{filtradas.length}</strong> entregas
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
                                    {modalType === "details" && <SubmissionDetails entrega={submissionModal} onClose={() => setModalHidden(true)} />}
                                    {modalType === "add" && <UploadSubmission onClose={() => { setModalHidden(true); cargarDatos(); }} />}
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
