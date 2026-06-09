import { useState, useEffect } from "react";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTimes, faTasks, faSave } from "../../content/icons.js";
import { editTarea } from "../../api/tareasService.js";
import { getMaterias } from "../../api/materiasService.js";
import { getGroups } from "../../api/groupService.js";
import Swal from "sweetalert2";

export default function EditTask({ tarea, onClose }) {
    const [loading, setLoading] = useState(false);
    const [materias, setMaterias] = useState([]);
    const [grupos, setGrupos] = useState([]);
    const [form, setForm] = useState({
        Titulo: tarea.titulo || "",
        Descripcion: tarea.descripcion || "",
        IdMateria: tarea.idMateria || "",
        IdGrupo: tarea.idGrupo || "",
        FechaEntrega: tarea.fechaEntrega
            ? new Date(tarea.fechaEntrega).toISOString().slice(0, 16)
            : "",
    });

    useEffect(() => {
        const cargarDatos = async () => {
            try {
                const [materiasData, gruposData] = await Promise.all([
                    getMaterias(),
                    getGroups(),
                ]);
                setMaterias(materiasData);
                setGrupos(gruposData);
            } catch (error) {
                console.error("Error al cargar datos:", error);
            }
        };
        cargarDatos();
    }, []);

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);

        try {
            await editTarea(tarea.idTarea, {
                Titulo: form.Titulo,
                Descripcion: form.Descripcion,
                IdMateria: parseInt(form.IdMateria),
                IdGrupo: parseInt(form.IdGrupo),
                FechaEntrega: new Date(form.FechaEntrega).toISOString(),
            });

            Swal.fire({
                title: "Tarea actualizada exitosamente",
                icon: "success",
                timer: 2000,
            });
            onClose();
        } catch (error) {
            console.error(error);
            Swal.fire({
                title: "Error al actualizar tarea",
                text: error.message,
                icon: "error",
                confirmButtonText: "OK",
            });
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="modal-content" style={{ borderRadius: "16px", border: "none" }}>
            <div
                className="d-flex justify-content-between align-items-center p-3"
                style={{
                    background: "linear-gradient(135deg, #3b82f6, #2563eb)",
                    borderRadius: "16px 16px 0 0",
                    color: "white",
                }}
            >
                <div className="d-flex align-items-center gap-2">
                    <FontAwesomeIcon icon={faTasks} />
                    <h5 className="mb-0 fw-bold">Editar Tarea</h5>
                </div>
                <button type="button"
                    className="btn"
                    onClick={onClose}
                    style={{ color: "white", background: "rgba(255,255,255,0.2)", borderRadius: "8px" }}
                >
                    <FontAwesomeIcon icon={faTimes} />
                </button>
            </div>

            <form onSubmit={handleSubmit} className="p-4">
                <div className="mb-3">
                    <label htmlFor="título" className="form-label fw-semibold">Título</label>
                    <input id="título"
                        type="text"
                        className="form-control"
                        name="Titulo"
                        value={form.Titulo}
                        onChange={handleChange}
                        placeholder="Ej: Presentación final del proyecto"
                        required
                        style={{ borderRadius: "10px" }}
                    />
                </div>

                <div className="mb-3">
                    <label htmlFor="descripción" className="form-label fw-semibold">Descripción</label>
                    <textarea id="descripción"
                        className="form-control"
                        name="Descripcion"
                        value={form.Descripcion}
                        onChange={handleChange}
                        rows="3"
                        placeholder="Describa los detalles de la tarea..."
                        required
                        style={{ borderRadius: "10px" }}
                    />
                </div>

                <div className="row">
                    <div className="col-md-6 mb-3">
                        <label htmlFor="materia" className="form-label fw-semibold">Materia</label>
                        <select id="materia"
                            className="form-select"
                            name="IdMateria"
                            value={form.IdMateria}
                            onChange={handleChange}
                            required
                            style={{ borderRadius: "10px" }}
                        >
                            <option value="">Seleccione una materia</option>
                            {materias.map((m) => (
                                <option key={m.id_Materia} value={m.id_Materia}>
                                    {m.nombre}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div className="col-md-6 mb-3">
                        <label htmlFor="grupo" className="form-label fw-semibold">Grupo</label>
                        <select id="grupo"
                            className="form-select"
                            name="IdGrupo"
                            value={form.IdGrupo}
                            onChange={handleChange}
                            required
                            style={{ borderRadius: "10px" }}
                        >
                            <option value="">Seleccione un grupo</option>
                            {grupos.map((g) => (
                                <option key={g.idGrupo} value={g.idGrupo}>
                                    {g.nombre}
                                </option>
                            ))}
                        </select>
                    </div>
                </div>

                <div className="mb-4">
                    <label htmlFor="fechadeentrega" className="form-label fw-semibold">Fecha de Entrega</label>
                    <input id="fechaDeEntrega"
                        type="datetime-local"
                        className="form-control"
                        name="FechaEntrega"
                        value={form.FechaEntrega}
                        onChange={handleChange}
                        min={new Date().toISOString().slice(0, 16)}
                        required
                        style={{ borderRadius: "10px" }}
                    />
                </div>

                <div className="d-flex justify-content-end gap-2">
                    <button
                        type="button"
                        className="btn btn-outline-secondary"
                        onClick={onClose}
                        style={{ borderRadius: "10px", fontWeight: 600 }}
                    >
                        Cancelar
                    </button>
                    <button
                        type="submit"
                        className="btn"
                        disabled={loading}
                        style={{
                            background: "linear-gradient(135deg, #3b82f6, #2563eb)",
                            color: "white",
                            borderRadius: "10px",
                            fontWeight: 600,
                        }}
                    >
                        <FontAwesomeIcon icon={faSave} className="me-1" />
                        {loading ? "Guardando..." : "Guardar Cambios"}
                    </button>
                </div>
            </form>
        </div>
    );
}
