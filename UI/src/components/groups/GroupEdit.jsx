import { leerToken } from "../../utils/auth";
import { editGroup } from "../../api/groupService.js";
import { useState } from "react";
import '../../content/groups/groupEdit.css';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faEdit, faInfoCircle, faUsers,
    faAlignLeft, faPen, faTimes, faSave
} from "../../content/icons.js";
import Loader from "../Loader";

export default function GroupEdit({ grupo, onClose }) {
    const [loading, setLoading] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);

        const token = leerToken();
        const data = {
            idGrupo:     grupo.idGrupo,
            nombre:      e.target.nombre.value,
            descripcion: e.target.descripcion.value,
            estado:      e.target.estado.checked,
        };

        const response = await editGroup(token.sub, data);

        if (response.error) {
            alert("Error al actualizar el grupo");
            console.error(response.error);
            setLoading(false);
            return;
        }

        setLoading(false);
        alert("Grupo actualizado exitosamente");
        onClose();
    };

    if (loading) return <Loader />;
    return (
        <div className="edit-group-modal">
            {/* ── Header ── */}
            <div className="edit-header">
                <div className="header-content">
                    <div className="header-icon">
                        <FontAwesomeIcon icon={faEdit} />
                    </div>
                    <div>
                        <h2 className="modal-title">Editar Grupo</h2>
                        <p className="modal-subtitle">Actualiza la información del grupo</p>
                    </div>
                </div>
            </div>

            <form className="edit-form" onSubmit={handleSubmit} id="groupEditForm">
                <div className="edit-content">

                    {/* ── Nombre ── */}
                    <div className="form-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faInfoCircle} />
                            Información del Grupo
                        </h3>
                        <div className="form-grid">
                            <div className="form-group">
                                <label className="form-label" htmlFor="nombre">Nombre del Grupo</label>
                                <div className="input-group">
                                    <span className="input-icon">
                                        <FontAwesomeIcon icon={faUsers} />
                                    </span>
                                    <input
                                        type="text"
                                        className="form-control"
                                        name="nombre"
                                        id="nombre"
                                        defaultValue={grupo.nombre}
                                        placeholder="Ingrese el nombre del grupo"
                                        required
                                    />
                                </div>
                            </div>
                        </div>
                    </div>

                    {/* ── Descripción ── */}
                    <div className="form-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faAlignLeft} />
                            Descripción
                        </h3>
                        <div className="form-group">
                            <label className="form-label" htmlFor="descripcion">Detalles del Grupo</label>
                            <div className="input-group">
                                <span className="input-icon" style={{ top: '0.75rem', alignSelf: 'flex-start' }}>
                                    <FontAwesomeIcon icon={faPen} />
                                </span>
                                {/* Fixed: was <input>, should be <textarea> */}
                                <textarea
                                    className="form-control"
                                    name="descripcion"
                                    id="descripcion"
                                    rows={4}
                                    defaultValue={grupo.descripcion}
                                    placeholder="Detalles del grupo..."
                                    required
                                />
                            </div>
                        </div>
                    </div>

                    {/* ── Estado ── */}
                    <div className="form-group">
                        <label className="form-label">Estado del Grupo</label>
                        <div className="toggle-group">
                            <label className="toggle-label">
                                <input
                                    type="checkbox"
                                    className="toggle-input"
                                    name="estado"
                                    id="estado"
                                    defaultChecked={grupo.estado}
                                />
                                <span className="toggle-slider" />
                                <span className="toggle-text">
                                    {grupo.estado ? "Activo" : "Inactivo"}
                                </span>
                            </label>
                        </div>
                    </div>

                </div>

                {/* ── Footer ── */}
                <div className="edit-footer">
                    <button
                        type="button"
                        className="btn btn-secondary"
                        onClick={onClose}
                        disabled={loading}
                    >
                        <FontAwesomeIcon icon={faTimes} />
                        Cancelar
                    </button>
                    <button type="submit" className="btn btn-primary" disabled={loading}>
                        <FontAwesomeIcon icon={faSave} />
                        {loading ? "Guardando…" : "Guardar Cambios"}
                    </button>
                </div>
            </form>
        </div>
    );
}