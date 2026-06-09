import "../../content/users/usuarios.css";
import { useEffect, useState } from "react";
import { exportUsersPdf, getUsers } from "../../api/userService.js";
import Loader from "../../components/Loader.jsx";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUserShield, faUserCheck, faUsers, faPlusCircle, faSearch, faDownload, faEnvelope, faEye, faPen, faChevronLeft, faChevronRight } from "../../content/icons.js"
import UserDetails from "../../components/users/UserDetails.jsx";
import UserEdit from "../../components/users/UserEdit.jsx";
import { Link } from "react-router-dom";

const handleExportUsersPdf = async () => {
    try {
        const response = await exportUsersPdf();
        const url = URL.createObjectURL(response);
        window.open(url, "_blank");
    } catch (error) {
        console.error("Error al generar el PDF:", error);
    }
};

export default function Users() {
    const [loading, setLoading] = useState(true);
    const [usuarios, setUsuarios] = useState([]);
    const [search, setSearch] = useState("");
    const [modalhidden, setModalHidden] = useState(true);
    const [userModal, setUserModal] = useState(null);
    const [modalType, setModalType] = useState("");
    const [pagina, setPagina] = useState(1);
    const porPagina = 10;

    const cargarDatos = async () => {
        try {
            var users = await getUsers();
            setUsuarios(users);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        cargarDatos();
    }, []);

    const usuariosFiltrados = usuarios.filter(u => {
        const texto = search.toLowerCase();
        const fullName = `${u.nombre} ${u.apellido}`;
        return (
            fullName.toLowerCase().includes(texto) ||
            u.nombre.toLowerCase().includes(texto) ||
            u.apellido.toLowerCase().includes(texto) ||
            u.email.toLowerCase().includes(texto) ||
            u.identificacion.toLowerCase().includes(texto) ||
            u.rol.toLowerCase().includes(texto))
    }
    );
    const usuariosActivosFiltrados = usuariosFiltrados.filter(u => u.estado === true);
    const totalPaginas = Math.ceil(usuariosFiltrados.length / porPagina);
    const inicio = (pagina - 1) * porPagina;
    const fin = inicio + porPagina;
    const usuariosPaginados = usuariosFiltrados.slice(inicio, fin);
    useEffect(() => {
        setPagina(1);
    }, [search]);

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
                </div>

                <div className="stats-grid">
                    <div className="stat-card">
                        <div className="stat-icon users-icon">
                            <FontAwesomeIcon icon={faUsers} />
                        </div>
                        <div className="stat-info">
                            <h3>{usuariosFiltrados.length}</h3>
                            <p>Total Usuarios</p>
                        </div>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon active-icon">
                            <FontAwesomeIcon icon={faUserCheck} />
                        </div>
                        <div className="stat-info">
                            <h3>{usuariosActivosFiltrados.length}</h3>
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
                            <input
                                type="text"
                                className="search-input" id="searchInput" aria-label="Buscar"
                                placeholder="Buscar usuarios..."
                                value={search}
                                onChange={(e) => setSearch(e.target.value)}
                            />
                        </div>
                        <div className="filter-actions">
                            <Link to="/addUser" className="btn-premium">
                                <FontAwesomeIcon icon={faPlusCircle} />
                                Nuevo Usuario
                            </Link>
                            <button type="button" onClick={handleExportUsersPdf} className="btn-export" >
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
                                {usuariosFiltrados.length === 0 ? (
                                    <tr>
                                        <td colSpan="6" className="text-center text-muted py-4">
                                            <FontAwesomeIcon icon={faUsers} />
                                            <p>
                                                No hay usuarios registrados con el dato especificado
                                            </p>
                                        </td>
                                    </tr>
                                ) : (
                                    usuariosPaginados.map((usuario) => (
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
                                                    <span className="status-badge activo">Activo</span>
                                                ) : (
                                                    <span className="status-badge inactivo">Inactivo</span>
                                                )}
                                            </td>
                                            <td className="actions-cell" data-label="Acciones">
                                                <div className="actions-container">

                                                    <button type="button" className="btn btn-outline-dark btn-sm btn-Detalles"
                                                        data-bs-toggle="tooltip"
                                                        title="Ver Detalles"
                                                        onClick={() => {
                                                            setUserModal(usuario);
                                                            setModalType("details");
                                                            setModalHidden(false);
                                                        }}>
                                                        <FontAwesomeIcon icon={faEye} />
                                                        <span className="action-text">Detalles</span>
                                                    </button>

                                                    <button type="button" className="btn btn-outline-primary btn-sm btn-Editar"
                                                        data-bs-toggle="tooltip"
                                                        title="Editar"
                                                        onClick={() => {
                                                            setUserModal(usuario);
                                                            setModalType("edit");
                                                            setModalHidden(false);
                                                        }}>
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
                    Mostrando <strong>{usuariosPaginados.length}</strong> de <strong>{usuariosFiltrados.length}</strong> usuarios
                </div>

                <div className="pagination-controls">
                    <button type="button"
                        className={`pagination-btn ${pagina === 1 ? "disabled" : ""}`}
                        disabled={pagina === 1}
                        onClick={() => setPagina(p => Math.max(p - 1, 1))}
                    >
                        <FontAwesomeIcon icon={faChevronLeft} />
                    </button>

                    <span className="pagination-btn active">{pagina}</span>

                    <button type="button"
                        className={`pagination-btn ${pagina === totalPaginas ? "disabled" : ""}`}
                        disabled={pagina === totalPaginas}
                        onClick={() => setPagina(p => Math.min(p + 1, totalPaginas))}
                    >
                        <FontAwesomeIcon icon={faChevronRight} />
                    </button>
                </div>
            </div>
            {!modalhidden && (
                <>
                    <div className="modal fade show d-block" tabIndex="-1">
                        <div className="modal-dialog modal-dialog-centered modal-lg ">
                            <div className="modal-content">
                                <div className="modal-body">
                                    {
                                        modalType === "details" && <UserDetails usuario={userModal} onClose={() => setModalHidden(true)} />
                                    }
                                    {
                                        modalType === "edit" && <UserEdit usuario={userModal} onClose={() => { setModalHidden(true); cargarDatos(); }} />
                                    }
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="modal-backdrop fade show"></div>
                </>
            )}

        </div>
    );
};