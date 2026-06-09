import "../../content/announcements/addAnnouncement.css";
import "../../content/announcements/announcementEdit.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faInfoCircle, faTag, faAlignLeft, faPen, faCalendarAlt, faFileUpload, faTimes, faSave, faTrash } from "../../content/icons.js";
import { useState, useEffect, useRef } from "react";
import { editAnnouncement } from "../../api/announcementsService.js";
import Swal from "sweetalert2";

export default function AnnouncementEdit({ anuncio, onClose }) {
    const [estado, setEstado] = useState(anuncio.estado);
    const [imgUrl, setImgUrl] = useState(null);
    const [previewUrl, setPreviewUrl] = useState(null);
    const [quitarImagen, setQuitarImagen] = useState(false);

    const imgUrlRef = useRef(null);
    const previewUrlRef = useRef(null);

    useEffect(() => {
        if (!anuncio.imagenRuta) return;
        setImgUrl(anuncio.imagenRuta);
        //  getImage(anuncio.idAnuncio)
        //    .then((blob) => {
        //      const url = URL.createObjectURL(blob);
        //    imgUrlRef.current = url;
        //   setImgUrl(url);
        //})
        //.catch((err) => console.error("Error al cargar imagen:", err));

        //        return () => {
        //          if (imgUrlRef.current) URL.revokeObjectURL(imgUrlRef.current);
        //        if (previewUrlRef.current) URL.revokeObjectURL(previewUrlRef.current);
        //  };
    }, [anuncio]);

    const handleFileChange = (e) => {
        if (previewUrlRef.current) URL.revokeObjectURL(previewUrlRef.current);
        const file = e.target.files[0];
        if (file) {
            const url = URL.createObjectURL(file);
            previewUrlRef.current = url;
            setPreviewUrl(url);
        } else {
            previewUrlRef.current = null;
            setPreviewUrl(null);
        }
    };

    const handleQuitarImagen = () => {
        if (imgUrlRef.current) URL.revokeObjectURL(imgUrlRef.current);
        if (previewUrlRef.current) URL.revokeObjectURL(previewUrlRef.current);
        imgUrlRef.current = null;
        previewUrlRef.current = null;
        setImgUrl(null);
        setPreviewUrl(null);
        setQuitarImagen(true);
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        const form = e.currentTarget;
        const formData = new FormData();
        formData.append('idAnuncio', anuncio.idAnuncio);
        formData.append('titulo', form.titulo.value);
        formData.append('descripcion', form.descripcion.value);
        formData.append('fechaEvento', form.fechaEvento.value);
        formData.append('estado', estado);
        formData.append('quitarImagen', quitarImagen);
        if (form.imagen.files[0] != null) {
            formData.append('imagen', form.imagen.files[0]);
        }
        const response = await editAnnouncement(formData);
        if (response) {
            Swal.fire({ title: 'Anuncio editado con éxito', icon: 'success', confirmButtonText: 'OK' });
            onClose();
        } else {
            Swal.fire({ title: '¡Error!', text: 'Algo ha fallado, intente de nuevo', icon: 'error', confirmButtonText: 'OK' });
        }
    };

    const imagenActual = imgUrl || previewUrl;

    return (
        <div className="edit-group-modal">
            <div className="edit-header">
                <div className="header-content">
                    <div className="header-icon">
                        <FontAwesomeIcon icon={faEdit} />
                    </div>
                    <div>
                        <h2 className="modal-title">Editar Anuncio</h2>
                        <p className="modal-subtitle">Actualiza la información del anuncio</p>
                    </div>
                </div>
            </div>

            <form onSubmit={handleSubmit}>
                <input type="hidden" name="idAnuncio" value={anuncio.idAnuncio} />

                <div className="edit-content">

                    <div className="form-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faInfoCircle} className="me-2" />Información del Anuncio
                        </h3>
                        <div className="form-grid">
                            <div className="form-group">
                                <label className="form-label" htmlFor="titulo">Título</label>
                                <div className="input-group">
                                    <span className="input-icon"><FontAwesomeIcon icon={faTag} /></span>
                                    <input
                                        type="text"
                                        className="form-control"
                                        id="titulo"
                                        name="titulo"
                                        defaultValue={anuncio.titulo}
                                        placeholder="Ingrese el título"
                                        required
                                    />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="form-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faAlignLeft} className="me-2" />Descripción
                        </h3>
                        <div className="form-group">
                            <label className="form-label" htmlFor="descripcion">Descripción</label>
                            <div className="input-group">
                                <span className="input-icon"><FontAwesomeIcon icon={faPen} /></span>
                                <textarea
                                    className="form-control"
                                    id="descripcion"
                                    name="descripcion"
                                    rows={4}
                                    defaultValue={anuncio.descripcion}
                                    placeholder="Ingrese la descripción..."
                                    required
                                />
                            </div>
                        </div>
                    </div>

                    <div className="form-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faCalendarAlt} className="me-2" />Fecha del Evento
                        </h3>
                        <div className="form-group">
                            <label className="form-label" htmlFor="fechaEvento">Fecha del Evento</label>
                            <div className="input-group">
                                <span className="input-icon"><FontAwesomeIcon icon={faCalendarAlt} /></span>
                                <input
                                    type="datetime-local"
                                    className="form-control"
                                    id="fechaEvento"
                                    name="fechaEvento"
                                    defaultValue={anuncio.fechaEvento?.slice(0, 16)}
                                    min={new Date(new Date().setSeconds(0, 0)).toISOString().slice(0, 16)}
                                    required
                                />
                            </div>
                        </div>
                    </div>

                    <div className="form-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faFileUpload} className="me-2" />Imagen del Anuncio
                        </h3>

                        {imagenActual && (
                            <div className="form-group mb-3">
                                <div className="d-flex align-items-center justify-content-between mb-2">
                                    <label className="form-label mb-0">
                                        {previewUrl ? "Vista previa" : "Imagen actual"}
                                    </label>
                                    <button
                                        type="button"
                                        className="btn btn-sm btn-outline-danger"
                                        onClick={handleQuitarImagen}
                                    >
                                        <FontAwesomeIcon icon={faTrash} className="me-1" />Quitar imagen
                                    </button>
                                </div>
                                <div className="current-image-container">
                                    <img src={imagenActual} className="img-fluid rounded shadow-sm" alt="Imagen del anuncio" />
                                </div>
                            </div>
                        )}

                        
                            <div className="form-group">
                                <label className="form-label" htmlFor="imagen">
                                  {!imagenActual ? "Subir imagen" : "Remplazar imagen (opcional)"}
                                </label>
                                <div className="input-group">
                                    <span className="input-icon"><FontAwesomeIcon icon={faFileUpload} /></span>
                                    <input
                                        type="file"
                                        className="form-control"
                                        id="imagen"
                                        name="imagen"
                                        accept=".jpg,.jpeg,.png,.gif"
                                        onChange={handleFileChange}
                                    />
                                </div>
                                <small className="text-muted d-block mt-1">Formatos permitidos: .jpg / .jpeg / .png / .gif</small>
                            </div>
                        
                    </div>

                    <div className="form-group">
                        <label htmlFor="estado" className="form-label">Estado</label>
                        <div className="toggle-group">
                            <label htmlFor="setestadoestadoestadoactivoinactivo" className="toggle-label">
                                <input id="estadoToggle"
                                    type="checkbox"
                                    className="toggle-input"
                                    checked={estado}
                                    onChange={() => setEstado(!estado)}
                                />
                                <span className="toggle-slider"></span>
                                <span className="toggle-text">{estado ? "Activo" : "Inactivo"}</span>
                            </label>
                        </div>
                    </div>

                </div>

                <div className="edit-footer">
                    <button type="button" className="btn btn-secondary" onClick={onClose}>
                        <FontAwesomeIcon icon={faTimes} className="me-2" />Cancelar
                    </button>
                    <button type="submit" className="btn btn-primary">
                        <FontAwesomeIcon icon={faSave} className="me-2" />Guardar Cambios
                    </button>
                </div>
            </form>
        </div>
    );
}
