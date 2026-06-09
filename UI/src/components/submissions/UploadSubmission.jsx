import { useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTimes, faFileUpload, faSave } from "../../content/icons.js";
import { subirEntrega } from "../../api/entregasService.js";
import Swal from "sweetalert2";

export default function UploadSubmission({ onClose }) {
    const [loading, setLoading] = useState(false);
    const [archivo, setArchivo] = useState(null);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        try {
            const formData = new FormData();
            formData.append("archivo", archivo);
            await subirEntrega(formData);
            Swal.fire({ title: "Entrega subida exitosamente", icon: "success", timer: 2000 });
            onClose();
        } catch (error) {
            Swal.fire({ title: "Error", text: error.message, icon: "error", confirmButtonText: "OK" });
        } finally { setLoading(false); }
    };

    return (
        <div className="modal-content" style={{ borderRadius: "16px", border: "none" }}>
            <div className="d-flex justify-content-between align-items-center p-3"
                style={{ background: "linear-gradient(135deg, #8b5cf6, #7c3aed)", borderRadius: "16px 16px 0 0", color: "white" }}>
                <div className="d-flex align-items-center gap-2">
                    <FontAwesomeIcon icon={faFileUpload} />
                    <h5 className="mb-0 fw-bold">Subir Entrega</h5>
                </div>
                <button type="button" className="btn" onClick={onClose} aria-label="Cerrar" style={{color: "white", background: "rgba(255,255,255,0.2)", borderRadius: "8px"}}><FontAwesomeIcon icon={faTimes} /></button>
            </div>
            <form onSubmit={handleSubmit} className="p-4">
                <div className="mb-4">
                    <label htmlFor="archivo" className="form-label fw-semibold">Archivo</label>
                    <input type="file" id="archivo" className="form-control" onChange={(e) => setArchivo(e.target.files[0])}
                        required style={{ borderRadius: "10px" }} />
                    <small className="text-muted">Formatos aceptados: PDF, DOCX, PPTX, XLSX, ZIP</small>
                </div>
                <div className="d-flex justify-content-end gap-2">
                    <button type="button" className="btn btn-outline-secondary" onClick={onClose}
                        style={{ borderRadius: "10px", fontWeight: 600 }}>Cancelar</button>
                    <button type="submit" className="btn" disabled={loading || !archivo}
                        style={{ background: "linear-gradient(135deg, #8b5cf6, #7c3aed)", color: "white", borderRadius: "10px", fontWeight: 600 }}>
                        <FontAwesomeIcon icon={faSave} className="me-1" />
                        {loading ? "Subiendo..." : "Subir Entrega"}
                    </button>
                </div>
            </form>
        </div>
    );
}
