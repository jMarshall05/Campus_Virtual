import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faAlignLeft, faInfoCircle, faPen, faSave, faTimes, faUsers } from "../../content/icons.js"
import { createGroup } from "../../api/groupService.js";
import { leerToken } from "../../utils/auth.js";
import { useState } from "react";
import Loader from "../Loader.jsx";
import Swal from "sweetalert2";

export default function AddGroup({ onClose }) {
    const [loading, setLoading] = useState(false);
    const handleSubmit = async (e) => {
        e.preventDefault()

        setLoading(true)
        const data = {
            nombre: e.target.nombre.value,
            descripcion: e.target.descripcion.value
        }
        const token = leerToken();
        try {
            await createGroup(token.sub, data)
            setLoading(false);
            Swal.fire({
                title: 'Grupo creado con exito!',
                icon: 'success',
                confirmButtonText: 'OK'
            })
            onClose()
        } catch (error) {
            setLoading(false); 
            Swal.fire({
                title: 'Error',
                text : 'Algo a fallado intente de nuevo',
                icon: 'error',
                confirmButtonText: 'OK'
            })
            console.log(error);
        }
    }
    if (loading) return <Loader />
    return (
        <>
            <div className="edit-header">
                <div className="header-content">
                    <div className="header-icon">
                        <FontAwesomeIcon icon={faUsers} />
                    </div>
                    <div>
                        <h2 className="modal-title">Crear Nuevo Grupo</h2>
                        <p className="modal-subtitle">Completa la información del grupo</p>
                    </div>
                </div>
            </div>

            <form id="Group-Form" onSubmit={handleSubmit}>
                <div className="edit-content">

                    <div className="form-section">
                        <h3 className="section-title">
                            <FontAwesomeIcon icon={faInfoCircle} className="me-2" />Información del Grupo
                        </h3>
                        <div className="form-grid">
                            <div className="form-group">
                                <label className="form-label" htmlFor="nombre">Nombre del Grupo</label>
                                <div className="input-group">
                                    <span className="input-icon">
                                        <i className="fas fa-users"></i>
                                    </span>
                                    <input
                                        className="form-control"
                                        id="nombre"
                                        placeholder="Ingrese el nombre del grupo"
                                        required="required"
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
                            <label className="form-label" htmlFor="descripcion">Descripcion</label>
                            <div className="input-group">
                                <span className="input-icon">
                                    <FontAwesomeIcon icon={faPen} />
                                </span>
                                <textarea
                                    className="form-control"
                                    id="descripcion"
                                    rows={4}
                                    placeholder="Detalles del grupo..."
                                    required="required"
                                />
                            </div>
                        </div>
                    </div>
                </div>

                <div className="edit-footer">
                    <button type="button" className="btn btn-secondary " data-bs-dismiss="modal" onClick={onClose}>
                        <FontAwesomeIcon icon={faTimes} className="me-2" />Cancelar
                    </button>
                    <button type="submit" className="btn btn-primary btn-submit-grupo" data-id="@ViewBag.Id">
                        <FontAwesomeIcon icon={faSave} className="me-2" />Crear Grupo
                    </button>
                </div>
            </form>

        </>
    );
}