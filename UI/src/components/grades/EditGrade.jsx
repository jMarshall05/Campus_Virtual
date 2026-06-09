import { useState } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTimes, faStar, faSave } from "../../content/icons.js";
import { editCalificacion } from "../../api/calificacionesService.js";
import Swal from "sweetalert2";

export default function EditGrade({ calificacion, onClose }) {
    const [loading, setLoading] = useState(false);
    const [form, setForm] = useState({
        calificacion: calificacion.calificacion || "",
        comentario: calificacion.comentario || "",
    });

    const handleChange = (e) => setForm({ ...form, [e.target.name]: e.target.value });

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        try {
            await editCalificacion(calificacion.idCalificacion, {
                calificacion: parseFloat(form.calificacion),
                comentario: form.comentario,
            });
            Swal.fire({ title: "Calificación actualizada", icon: "success", timer: 2000 });
            onClose();
        } catch (error) {
            Swal.fire({ title: "Error", text: error.message, icon: "error", confirmButtonText: "OK" });
        } finally { setLoading(false); }
    };

    return (
        <div className="modal-content" style={{ borderRadius: "16px", border: "none" }}>
            <div className="d-flex justify-content-between align-items-center p-3"
                style={{ background: "linear-gradient(135deg, #f59e0b, #d97706)", borderRadius: "16px 16px 0 0", color: "white" }}>
                <div className="d-flex align-items-center gap-2">
                    <FontAwesomeIcon icon={faStar} />
                    <h5 className="mb-0 fw-bold">Editar Calificación</h5>
                </div>
                <button type="button" className="btn" onClick={onClose} aria-label="Cerrar" style={{color: "white", background: "rgba(255,255,255,0.2)", borderRadius: "8px"}}><FontAwesomeIcon icon={faTimes} /></button>
            </div>
            <form onSubmit={handleSubmit} className="p-4">
                <div className="mb-3">
                    <label htmlFor="calificación" className="form-label fw-semibold">Calificación (0-100)</label>
                    <input type="number" id="calificación0100" className="form-control" name="calificacion"
                        value={form.calificacion} onChange={handleChange}
                        min="0" max="100" step="0.01" required style={{ borderRadius: "10px" }} />
                </div>
                <div className="mb-4">
                    <label htmlFor="comentario" className="form-label fw-semibold">Comentario</label>
                    <textarea id="comentario" className="form-control" name="comentario" value={form.comentario}
                        onChange={handleChange} rows="3" placeholder="Ingrese un comentario..."
                        style={{ borderRadius: "10px" }} />
                </div>
                <div className="d-flex justify-content-end gap-2">
                    <button type="button" className="btn btn-outline-secondary" onClick={onClose}
                        style={{ borderRadius: "10px", fontWeight: 600 }}>Cancelar</button>
                    <button type="submit" className="btn" disabled={loading}
                        style={{ background: "linear-gradient(135deg, #f59e0b, #d97706)", color: "white", borderRadius: "10px", fontWeight: 600 }}>
                        <FontAwesomeIcon icon={faSave} className="me-1" />
                        {loading ? "Guardando..." : "Guardar Cambios"}
                    </button>
                </div>
            </form>
        </div>
    );
}
