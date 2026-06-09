import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import "../../content/docs/addDoc.css"
import { faAlignLeft,faFilePdf, faFileUpload, faHeading, faSave, faTag, faTimes } from "../../content/icons.js";
import { useState } from "react";
import { addDoc } from "../../api/docsService.js";
import Swal from "sweetalert2";

export default function AddDoc({ onClose }) {
    const [, setLoading] = useState(false);
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
            const response = await addDoc(formData);
            if (response) {
                setLoading(false)
                Swal.fire({
                    title: 'Archivo subido con éxito',
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

    return (
        <>
            <div className="modal-header">
                <h5 className="modal-title" id="agregarDocumentoLabel">
                    <FontAwesomeIcon icon={faFileUpload} className="me-2" />
                    Agregar Nuevo Documento
                </h5>
                <button type="button" className="btn-close" aria-label="Cerrar" onClick={onClose}></button>
            </div>
            <form id="formAgregarDocumento" onSubmit={handleSubmit}>
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
                            placeholder="Describe brevemente el contenido del documento"
                        ></textarea>
                    </div>

                    <div className="form-field-wrap mb-3">
                        <label htmlFor="archivoDocumento" className="form-label">
                            <FontAwesomeIcon icon={faFileUpload} /> Archivo del Documento
                        </label>
                        <input
                            type="file"
                            className="form-control"
                            id="archivoDocumento"
                            name="Archivo"
                            required
                            accept=".pdf,.doc,.docx,.xls,.xlsx,.ppt,.pptx"
                            style={{ height: 'fit-content' }}
                        />
                        <div className="form-text">
                            Formatos permitidos: PDF, Word, Excel, PowerPoint (Máx. 10MB)
                        </div>
                        <div id="archivoPreview" className="file-preview mt-2" style={{ display: "none" }}>
                            <div className="file-preview-item">
                                <FontAwesomeIcon icon={faFilePdf} className="file-preview-icon" />
                                <div className="file-preview-info">
                                    <span className="file-preview-name"></span>
                                    <span className="file-preview-size"></span>
                                </div>
                                <button type="button" className="btn-remove-file">
                                    <FontAwesomeIcon icon={faTimes} />
                                </button>
                            </div>
                        </div>
                    </div>

                    <div className="form-field-wrap mb-3">
                        <label htmlFor="categoriaDocumento" className="form-label">
                            <FontAwesomeIcon icon={faTag} /> Categoría
                        </label>
                        <select className="form-select" id="categoriaDocumento" name="Categoria">
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
                    <button type="button" onClick={onClose}  className="btn btn-secondary" >
                        <FontAwesomeIcon icon={faTimes} /> Cancelar
                    </button>
                    <button type="submit" className="btn btn-primary">
                        <FontAwesomeIcon icon={faSave} /> Guardar Documento
                    </button>
                </div>
            </form>
        </>
    );
}