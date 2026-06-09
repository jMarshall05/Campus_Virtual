import "../../content/announcements/announcements.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faBullhorn, faUsers, faUserCheck, faSearch, faSort,
    faCalendarAlt, faPlusCircle, faEdit, faEye,
    faChevronLeft, faChevronRight
} from "../../content/icons.js";
import { getAnnouncements } from "../../api/announcementsService.js";
import { useEffect, useState } from "react";
import Loader from "../../components/Loader.jsx";
import AnnouncementDetails from "../../components/announcements/AnnouncementDetails.jsx";
import AnnouncementEdit from "../../components/announcements/AnnouncementEdit.jsx";
import AddAnnouncement from "../../components/announcements/AddAnnouncement.jsx";

const formatDate = (dateStr) =>
    dateStr
        ? new Date(dateStr).toLocaleDateString("es-ES", { day: "2-digit", month: "2-digit", year: "numeric" })
        : "N/A";

export default function Announcements() {
    const [loading, setLoading] = useState(true);
    const [announcements, setAnnouncements] = useState([]);
    const [search, setSearch] = useState("");
    const [modalhidden, setModalHidden] = useState(true);
    const [modalType, setModalType] = useState("");
    const [announcementModal, setAnnouncementModal] = useState(null);
    const [pagina, setPagina] = useState(1);
    const porPagina = 10;


    const handleSearchChange = (e) => {
        setSearch(e.target.value);
        setPagina(1);
    };

    const cargarDatos = async () => {
        try {
            const data = await getAnnouncements();
            setAnnouncements(data);
        } catch (error) {
            console.error("Error al cargar los anuncios:", error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        cargarDatos();
    }, []);

    const filtrados = announcements.filter(a => {
        const texto = search.toLowerCase();
        return (
            a.titulo?.toLowerCase().includes(texto) ||
            a.descripcion?.toLowerCase().includes(texto)
        );
    });

    const activos = filtrados.filter(a => a.estado === true);
    const totalPaginas = Math.ceil(filtrados.length / porPagina);
    const paginados = filtrados.slice((pagina - 1) * porPagina, pagina * porPagina);

    if (loading) return <Loader />;
    return (
        <div className="admin-container-fluid">
            <div className="admin-header">
                <div className="header-content">
                    <div className="header-title">
                        <div className="title-icon">
                            <FontAwesomeIcon icon={faBullhorn} />
                        </div>
                        <div>
                            <h1>Lista de Anuncios</h1>
                            <p className="header-subtitle">Gestiona y visualiza todos los anuncios del sistema</p>
                        </div>
                    </div>

                </div>

                <div className="stats-grid">
                    <div className="stat-card">
                        <div className="stat-icon announcements-icon">
                            <FontAwesomeIcon icon={faUsers} />
                        </div>
                        <div className="stat-info">
                            <h3>{filtrados.length}</h3>
                            <p>Total Anuncios</p>
                        </div>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon announcements-active-icon">
                            <FontAwesomeIcon icon={faUserCheck} />
                        </div>
                        <div className="stat-info">
                            <h3>{activos.length}</h3>
                            <p>Anuncios Activos</p>
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
                                placeholder="Buscar anuncios..."
                                value={search}
                                onChange={handleSearchChange}
                            />
                        </div>
                        <div className="filter-actions">
                            <button type="button" className="btn-premium btn-Crear-Anuncio" onClick={() => { setModalType("add"); setModalHidden(false); }}>
                                <FontAwesomeIcon icon={faPlusCircle} className="me-2" />
                                Nuevo Anuncio
                            </button></div>
                    </div>
                </div>

                <div className="card-body-premium">
                    <div className="table-container">
                        <table className="premium-table" id="TablaDeAnuncios">
                            <thead>
                                <tr>
                                    <th className="name-col">
                                        <div className="th-content">
                                            <span>Título del Anuncio</span>
                                            <FontAwesomeIcon icon={faSort} />
                                        </div>
                                    </th>
                                    <th className="desc-col">
                                        <div className="th-content">
                                            <span>Descripción</span>
                                            <FontAwesomeIcon icon={faSort} />
                                        </div>
                                    </th>
                                    <th className="creator-col">
                                        <div className="th-content">
                                            <span>Fecha Evento</span>
                                            <FontAwesomeIcon icon={faCalendarAlt} className="me-1" />
                                        </div>
                                    </th>
                                    <th className="status-col">
                                        <div className="th-content">
                                            <span>Publicación</span>
                                            <FontAwesomeIcon icon={faCalendarAlt} className="me-1" />
                                        </div>
                                    </th>
                                    <th className="actions-col">
                                        <div className="th-content">
                                            <span>Acciones</span>
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                {filtrados.length === 0 ? (
                                    <tr>
                                        <td colSpan="5" className="text-center text-muted py-4">
                                            <FontAwesomeIcon icon={faBullhorn} className="fa-2x mb-2" />
                                            <br />
                                            No hay anuncios publicados actualmente.
                                        </td>
                                    </tr>
                                ) : (
                                    paginados.map((anuncio) => (
                                        <tr className="group-row" key={anuncio.idAnuncio}>
                                            <td className="name-cell" data-label="Título">
                                                <div className="group-info">
                                                    <div className="group-icon">
                                                        <FontAwesomeIcon icon={faBullhorn} />
                                                    </div>
                                                    <div className="group-details">
                                                        <span className="group-name">{anuncio.titulo}</span>
                                                    </div>
                                                </div>
                                            </td>
                                            <td className="desc-cell" data-label="Descripción">
                                                <span className="group-description">{anuncio.descripcion}</span>
                                            </td>
                                            <td className="desc-cell" data-label="Fecha evento">
                                                <span className="badge-date">{formatDate(anuncio.fechaEvento)}</span>
                                            </td>
                                            <td className="desc-cell" data-label="Publicación">
                                                <span className="badge-date">{formatDate(anuncio.fechaPublicacion)}</span>
                                            </td>
                                            <td className="actions-cell" data-label="Acciones">
                                                <div className="d-flex justify-content-center gap-2">
                                                    <button type="button"
                                                        className="btn btn-outline-primary btn-sm"
                                                        title="Editar anuncio"
                                                        onClick={() => { setAnnouncementModal(anuncio); setModalType("edit"); setModalHidden(false); }}
                                                    >
                                                        <FontAwesomeIcon icon={faEdit} />
                                                    </button>
                                                    <button type="button"
                                                        className="btn btn-outline-dark btn-sm"
                                                        title="Ver detalles"
                                                        onClick={() => { setAnnouncementModal(anuncio); setModalType("details"); setModalHidden(false); }}
                                                    >
                                                        <FontAwesomeIcon icon={faEye} />
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
                        Mostrando <strong>{paginados.length}</strong> de <strong>{filtrados.length}</strong> anuncio(s)
                    </div>
                    <div className="pagination-controls">
                        <button type="button"
                            className={`pagination-btn ${pagina === 1 ? "disabled" : ""}`}
                            disabled={pagina === 1}
                            onClick={() => setPagina(p => Math.max(p - 1, 1))}
                        >
                            <FontAwesomeIcon icon={faChevronLeft} />
                        </button>
                        <span className="pagination-btn active">{pagina}</span>
                        <button type="button"
                            className={`pagination-btn ${pagina >= totalPaginas ? "disabled" : ""}`}
                            disabled={pagina >= totalPaginas}
                            onClick={() => setPagina(p => Math.min(p + 1, totalPaginas))}
                        >
                            <FontAwesomeIcon icon={faChevronRight} />
                        </button>
                    </div>
                </div>
            </div>

            {!modalhidden && (
                <>
                    <div className="modal fade show d-block" tabIndex="-1">
                        <div className="modal-dialog modal-dialog-centered ">
                            <div className="modal-content">
                                {
                                    modalType === "details" && <AnnouncementDetails anuncio={announcementModal} onClose={() => setModalHidden(true)} />
                                }
                                {
                                    modalType === "edit" && <AnnouncementEdit anuncio={announcementModal} onClose={() => { setModalHidden(true); cargarDatos(); }} />
                                }
                                {
                                    modalType === "add" && <AddAnnouncement onClose={() => { setModalHidden(true); cargarDatos(); }} />
                                }
                            </div>
                        </div>
                    </div>
                    <div className="modal-backdrop fade show"></div>
                </>
            )}
        </div>
    );
}
