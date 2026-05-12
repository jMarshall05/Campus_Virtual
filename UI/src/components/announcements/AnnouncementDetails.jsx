import "../../content/announcements/addAnnouncement.css";
import "../../content/announcements/announcementDetails.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBullhorn, faCalendarAlt, faTimes } from "../../content/icons.js";
import Loader from "../Loader.jsx";
import { getImage } from "../../api/announcementsService.js";
import { useEffect, useState } from "react";

const formatDate = (dateStr) =>
    dateStr
        ? new Date(dateStr).toLocaleDateString("es-ES", { day: "2-digit", month: "short", year: "numeric" })
        : "N/A";

export default function AnnouncementDetails({ anuncio, onClose }) {
    const [loading, setLoading] = useState(false);
    const [imgUrl, setImgUrl] = useState(null);

    useEffect(() => {
        if (!anuncio.imagenRuta) return;
        setLoading(true);
        setImgUrl(anuncio.imagenRuta);
        setLoading(false);
        // getImage(anuncio.idAnuncio)
        //   .then((blob) => {
        //     objectUrl = URL.createObjectURL(blob);
        //   setImgUrl(objectUrl);
        //}
        //)
        //.catch((err) => console.error("Error al cargar la imagen del anuncio:", err))
        //.finally(() => setLoading(false));

        //  return () => {
        //    if (objectUrl) URL.revokeObjectURL(objectUrl);
        //};
    }, [anuncio]);
    if (loading) return <Loader />;

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
                            <img src={imgUrl} alt={`Imagen del evento ${anuncio.titulo}`} />
                        </div>
                    </div>
                )}

            </div>

            <div className="edit-footer">
                <button className="btn btn-secondary" onClick={onClose}>
                    <FontAwesomeIcon icon={faTimes} className="me-2" />Cerrar
                </button>
            </div>
        </div>
    );
}
