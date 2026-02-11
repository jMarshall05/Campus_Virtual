import "../content/usuarios.css";
import { useEffect, useState } from "react";
import { getUsers } from "../api/userService";
import Loader from "../components/Loader";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUserShield, faUserCheck, faUsers, faPlusCircle, faSearch, faDownload, faEnvelope, faEye, faPen, faChevronLeft, faChevronRight } from "../content/icons.js"

export default function () {
    const [loading, setLoading] = useState(true);
    const [usuarios, setUsuarios] = useState([]);
    const [usuariosActivos, setUsuariosActivos] = useState([])

    useEffect(() => {
        const cargarDatos = async () => {
            try {
                var users = await getUsers();
                setUsuarios(users);

                const activeUsers = users.filter(u => u.estado === true);
                setUsuariosActivos(activeUsers)
            } finally {
                setLoading(false);
            }
        };
        cargarDatos();
    }, [])
    if (loading) return <Loader />;
    return (
        <div className="admin-container-fluid">
            <div className="admin-header">
                <div className="header-content">
                    <div className="header-title">
                        <div className="title-icon">
                            <FontAwesomeIcon icon={faUserShield} />
                        </div>
                        <div>
                            <h1>Gestión de Usuarios</h1>
                            <p className="header-subtitle">Administra todos los usuarios del sistema</p>
                        </div>
                    </div>
                    <button /*onClick={Register}*/ className="btn-premium">
                        <FontAwesomeIcon icon={faPlusCircle} />
                        Nuevo Usuario
                    </button>
                </div>

                <div className="stats-grid">
                    <div className="stat-card">
                        <div className="stat-icon users-icon">
                            <FontAwesomeIcon icon={faUsers} />
                        </div>
                        <div className="stat-info">
                            <h3>{usuarios.length}</h3>
                            <p>Total Usuarios</p>
                        </div>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon active-icon">
                            <FontAwesomeIcon icon={faUserCheck} />
                        </div>
                        <div className="stat-info">
                            <h3>{usuariosActivos.length}</h3>
                            <p>Usuarios Activos</p>
                        </div>
                    </div>
                </div>
            </div>
            <div className="premium-card">
                <div className="card-header-premium">
                    <div className="header-actions">
                        <div className="search-container">
                            <FontAwesomeIcon icon={faSearch} className="search-icon" />
                            <input type="text" className="search-input" placeholder="Buscar usuarios..." id="searchInput" />
                        </div>
                        <div className="filter-actions">

                            <button /*onClick={exportGeneral}*/ className="btn-export" >
                                <FontAwesomeIcon icon={faDownload} />
                                Exportar
                            </button>
                        </div>
                    </div>
                </div>

                <div className="card-body-premium">
                    <div className="table-container">
                        <table className="premium-table" id="TablaDeUsuarios">
                            <thead>
                                <tr>
                                    <th>Nombre Completo</th>
                                    <th>Email</th>
                                    <th>Identificacion</th>
                                    <th>Estado</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                {usuarios.length === 0 ? (
                                    <tr>
                                        <td colspan="6" className="text-center text-muted py-4">
                                            <FontAwesomeIcon icon={faUsers} />
                                            <br>
                                                No hay usuarios registrados
                                            </br>
                                        </td>
                                    </tr>
                                ) : (
                                    usuarios.map((usuario) => (
                                        <tr key={usuario.idUsuario} className="user-row">
                                            <td className="user-info-cell" data-label="Usuario">
                                                <div className="user-info">
                                                    <div className="user-avatar-table">
                                                        <span className="avatar-text">{usuario.nombre[0]}{usuario.apellido[0]}</span>
                                                    </div>
                                                    <div className="user-details">
                                                        <span className="user-name">{usuario.nombre} {usuario.apellido}</span>
                                                        <span className="user-role">{usuario.rol}</span>
                                                    </div>
                                                </div>
                                            </td>
                                            <td className="email-cell" data-label="Email">
                                                <div className="email-content">
                                                    <FontAwesomeIcon icon={faEnvelope} />
                                                    <span className="email-text">{usuario.email}</span>
                                                </div>
                                            </td>
                                            <td className="id-cell" data-label="Cédula">
                                                <span className="id-badge">{usuario.identificacion}</span>
                                            </td>
                                            <td className="status-cell" data-label="Estado">
                                                {usuario.estado ? (
                                                    <span className="status-badge active">Activo</span>
                                                ) : (
                                                    <span className="status-badge inactive">Inactivo</span>
                                                )}
                                            </td>
                                            <td className="actions-cell" data-label="Acciones">
                                                <div className="actions-container">

                                                    <button className="btn btn-outline-dark btn-sm btn-Detalles" data-id="@item.IdUsuario" data-bs-toggle="tooltip" title="Ver Detalles">
                                                        <FontAwesomeIcon icon={faEye} />
                                                        <span className="action-text">Detalles</span>
                                                    </button>

                                                    <button className="btn btn-outline-primary btn-sm btn-Editar" data-id="@item.IdUsuario" data-bs-toggle="tooltip" title="Editar">
                                                        <FontAwesomeIcon icon={faPen} />
                                                        <span className="action-text">Editar</span>
                                                    </button>
                                                </div>
                                            </td>
                                        </tr>
                                    ))
                                )
                                }

                            </tbody>
                        </table>
                    </div>
                </div>
            </div >
            <div className="card-footer-premium">
                <div className="pagination-info">
                    Mostrando <strong>{usuarios.length}</strong> usuarios
                </div>
                <div className="pagination-controls">
                    <button className="pagination-btn disabled">
                        <FontAwesomeIcon icon={faChevronLeft}/>
                    </button>
                    <button className="pagination-btn active">1</button>
                    <button className="pagination-btn">
                        <FontAwesomeIcon icon={faChevronRight}/>
                    </button>
                </div>
            </div>
        </div>

    );
};