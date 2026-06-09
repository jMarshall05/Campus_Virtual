import "../../content/announcements/addAnnouncement.css";
import "../../content/announcements/announcementDetails.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBullhorn, faCalendarAlt, faTimes } from "../../content/icons.js";

      


const formatDate = (dateStr) =>
    dateStr  ? new Date(dateStr).toLocaleDateString("es-ES", { day: "2-digit", month: "short", year: "numeric" })
        : "N/A";

export default function AnnouncementDetails({ anuncio, onClose }) {
    return (
        <div className="edit-group-modal">
            <div className="edit-header">
                <div className="header-content">
                    <div className="header-icon">
                        <FontAwesomeIcon icon={faBullhorn} />
                    </div>
                    <div>
                        <h2 className="modal-title">{anuncio.titulo}</h2>
                        <p className="modal-subtitle">
                            <span className={`status-badge-inline ${anuncio.estado ? "active" : "inactive"}`}>
                                {anuncio.estado ? "Activo" : "Inactivo"}
                            </span>
                        </p>
                    </div>
                </div>
            </div>

            <div className="edit-content">

                <div className="form-section">
                    <div className="date-cards-row">
                        <div className="date-card">
                            <div className="date-icon">
                                <FontAwesomeIcon icon={faCalendarAlt} />
                            </div>
                            <div className="date-info">
                                <span className="date-label">Fecha del Evento</span>
                                <span className="date-value">{formatDate(anuncio.fechaEvento)}</span>
                            </div>
                        </div>
                        <div className="date-card">
                            <div className="date-icon">
                                <FontAwesomeIcon icon={faCalendarAlt} />
                            </div>
                            <div className="date-info">
                                <span className="date-label">Publicado el</span>
                                <span className="date-value">{formatDate(anuncio.fechaPublicacion)}</span>
                            </div>
                        </div>
                    </div>
                </div>

                <div className="form-section">
                    <h3 className="section-title">Descripción</h3>
                    <p className="description-content">{anuncio.descripcion}</p>
                </div>

                {anuncio.imagenRuta && (
                    <div className="form-section">
                        <h3 className="section-title">Imagen del Evento</h3>
                        <div className="ad-image-container">
                            <img src={anuncio.imagenRuta} alt={`Imagen del evento ${anuncio.titulo}`} />
                        </div>
                    </div>
                )}

            </div>

            <div className="edit-footer">
                <button type="button" className="btn btn-secondary" onClick={onClose}>
                    <FontAwesomeIcon icon={faTimes} className="me-2" />Cerrar
                </button>
            </div>
        </div>
    );
}
