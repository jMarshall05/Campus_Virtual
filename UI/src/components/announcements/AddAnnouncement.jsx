import "../../content/announcements/addAnnouncement.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBullhorn, faTag, faPen, faCalendarAlt, faFileUpload, faTimes, faSave } from "../../content/icons.js";
import { addAnnouncement } from "../../api/announcementsService.js";
import Swal from "sweetalert2";
import { useState, useEffect, useRef, useMemo } from "react";

export default function AddAnnouncement({ onClose }) {
    const [previewUrl, setPreviewUrl] = useState(null);
    const previewUrlRef = useRef(null);
    const nowDate = useMemo(() => new Date(), []);

    useEffect(() => {
        return () => {
            if (previewUrlRef.current) URL.revokeObjectURL(previewUrlRef.current);
        };
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, []);

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

    const handleSubmit = async (e) => {
        e.preventDefault();
        const form = e.currentTarget;
        const formData = new FormData();
        formData.append('titulo', form.titulo.value);
        formData.append('descripcion', form.descripcion.value);
        formData.append('fechaEvento', form.fechaEvento.value);
        if (form.imagen.files[0]) {
            formData.append('imagen', form.imagen.files[0]);
        }
       const response = await addAnnouncement(formData);
       if(response){
        Swal.fire({
            title: 'Anuncio creado con éxito',
            icon: 'success',
            confirmButtonText: 'OK'
        })
        onClose();
       }else{
        Swal.fire({
            title: '¡Error!',
            text: 'Algo a fallado intente de nuevo, para detalles revise la consola',
            icon: 'error',
            confirmButtonText: 'OK'
        })
       }    

    };
    return (
        <div className="edit-group-modal">
            <div className="edit-header">
                <div className="header-content">
                    <div className="header-icon">
                        <FontAwesomeIcon icon={faBullhorn} />
                    </div>
                    <div>
                        <h2 className="modal-title">Crear Nuevo Anuncio</h2>
                        <p className="modal-subtitle">Completa la información del anuncio</p>
                    </div>
                </div>
            </div>

            <form onSubmit={handleSubmit}>
                <div className="edit-content">

                    <div className="form-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faBullhorn} className="me-2" />Información del Anuncio
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
                                        placeholder="Ej: Festival de Ciencias"
                                        maxLength={100}
                                        required
                                    />
                                </div>
                            </div>

                            <div className="form-group">
                                <label className="form-label" htmlFor="descripcion">Descripción</label>
                                <div className="input-group">
                                    <span className="input-icon"><FontAwesomeIcon icon={faPen} /></span>
                                    <textarea
                                        className="form-control"
                                        id="descripcion"
                                        name="descripcion"
                                        rows={4}
                                        maxLength={500}
                                        placeholder="Describe los detalles del evento..."
                                        required
                                    />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="form-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faCalendarAlt} className="me-2" />Fecha y Hora del Evento
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
                                    min={(() => { const d = new Date(nowDate); d.setSeconds(0, 0); return d.toISOString().slice(0, 16); })()}
                                    required
                                />
                            </div>
                        </div>
                    </div>

                    <div className="form-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faFileUpload} className="me-2" />Imagen del Anuncio (Opcional)
                        </h3>
                        <div className="form-group">
                            <label className="form-label" htmlFor="imagen">Subir Imagen</label>
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
                            <small className="form-text">Formatos permitidos: .jpg / .jpeg / .png / .gif</small>
                        </div>
                        {previewUrl && (
                            <div className="image-preview-container mt-3">
                                <span className="form-label">Vista previa</span>
                                <img src={previewUrl} className="img-fluid rounded shadow-sm" alt="Vista previa" />
                            </div>
                        )}
                    </div>

                </div>

                <div className="edit-footer">
                    <button type="button" className="btn btn-secondary" onClick={onClose}>
                        <FontAwesomeIcon icon={faTimes} className="me-2" />Cancelar
                    </button>
                    <button type="submit" className="btn btn-primary">
                        <FontAwesomeIcon icon={faSave} className="me-2" />Crear Anuncio
                    </button>
                </div>
            </form>
        </div>
    );
}
