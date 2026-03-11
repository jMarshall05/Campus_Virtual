import '../../content/docs/editDoc.css'
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit, faAlignLeft, faFilePdf, faFileUpload, faHeading, faSave, faTag, faTimes } from "../../content/icons.js";
import { editDoc } from '../../api/docsService.js';
import { useState } from 'react';
import Loader from '../Loader.jsx';
import Swal from 'sweetalert2';
export default function EditDoc({ doc, onClose }) {
    const [loading, setLoading] = useState(false);
    const handleSubmit = async (e) => {
        e.preventDefault()
        setLoading(true)
        try {
            const form = e.currentTarget
            const formData = new FormData()
            formData.append('Titulo', form.tituloDocumento.value)
            formData.append('Descripcion', form.descripcionDocumento.value)
            formData.append('Categoria', form.categoriaDocumento.value)
            formData.append('Doc', form.archivoDocumento.files[0])
            const response = await editDoc(doc.id, formData);
            if (response) {
                setLoading(false)
                Swal.fire({
                    title: 'Archivo editado con éxito',
                    icon: 'success',
                    confirmButtonText: 'OK'
                })
                onClose();
            }

        } catch (error) {
            setLoading(false)
            Swal.fire({
                title: '¡Error!',
                text: 'Algo a fallado intente de nuevo, para detalles revise la consola',
                icon: 'error',
                confirmButtonText: 'OK'
            })
            console.error(error)
        }
    }
    if (loading) return <Loader />
    return (
        <>
            <div className="modal-header">
                <h5 className="modal-title" id="editarDocumentoLabel">
                    <FontAwesomeIcon icon={faEdit} className="me-2" />
                    Editar Documento
                </h5>
                <button type="button" className="btn-close" onClick={onClose} aria-label="Close"></button>
            </div>

            <form id="formEditarDocumento" onSubmit={handleSubmit}>
                <div className="modal-body">

                    <div className="form-field-wrap mb-3">
                        <label htmlFor="tituloDocumento" className="form-label">
                            <FontAwesomeIcon icon={faHeading} /> Título del Documento
                        </label>
                        <input
                            type="text"
                            className="form-control"
                            id="tituloDocumento"
                            name="Titulo"
                            required
                            defaultValue={doc.titulo}
                            placeholder="Ej: Manual de Convivencia"
                        />
                    </div>

                    <div className="form-field-wrap mb-3">
                        <label htmlFor="descripcionDocumento" className="form-label">
                            <FontAwesomeIcon icon={faAlignLeft} /> Descripción
                        </label>
                        <textarea
                            className="form-control"
                            id="descripcionDocumento"
                            name="Descripcion"
                            rows="3"
                            required
                            defaultValue={doc.descripcion}
                            placeholder="Describe brevemente el contenido del documento"
                        ></textarea>
                    </div>

                    <div className="form-field-wrap mb-3">
                        <label htmlFor="archivoDocumento" className="form-label">
                            <FontAwesomeIcon icon={faFileUpload} /> Reemplazar Archivo
                            <span className="badge-opcional">Opcional</span>
                        </label>
                        <input
                            type="file"
                            className="form-control"
                            id="archivoDocumento"
                            name="Archivo"
                            accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx"
                            style={{ height: 'fit-content' }}
                        />
                        {doc.nombreArchivo && (
                            <div className="file-current mt-2">
                                <FontAwesomeIcon icon={faFilePdf} className="file-current-icon" />
                                <span className="file-current-name">{doc.nombreArchivo}</span>
                                <span className="file-current-badge">Archivo actual</span>
                            </div>
                        )}
                        <div className="form-text">
                            Formatos permitidos: PDF, Word, Excel, PowerPoint (Máx. 10MB)
                        </div>
                    </div>

                    <div className="form-field-wrap mb-3">
                        <label htmlFor="categoriaDocumento" className="form-label">
                            <FontAwesomeIcon icon={faTag} /> Categoría
                        </label>
                        <select
                            className="form-select"
                            id="categoriaDocumento"
                            name="Categoria"
                            defaultValue={doc.categoria}
                        >
                            <option value="Institucional">Institucional</option>
                            <option value="Normativa">Normativa</option>
                            <option value="Reglamento">Reglamento</option>
                            <option value="Manual">Manual</option>
                            <option value="Guía">Guía</option>
                            <option value="Circular">Circular</option>
                            <option value="Formato">Formato</option>
                            <option value="Otro">Otro</option>
                        </select>
                    </div>

                </div>

                <div className="modal-footer">
                    <button type="button" onClick={onClose} className="btn btn-secondary">
                        <FontAwesomeIcon icon={faTimes} /> Cancelar
                    </button>
                    <button type="submit" className="btn btn-primary btn-edit">
                        <FontAwesomeIcon icon={faSave} /> Guardar Cambios
                    </button>
                </div>
            </form>
        </>
    );
}