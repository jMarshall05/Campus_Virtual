import { useEffect, useState } from "react"
import { getGroups } from "../../api/groupService.js";
import Loader from "../../components/Loader.jsx";
import "../../content/groups/groups.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUsers, faUserCheck, faSearch, faSort, faEdit, faEye, faPlusCircle, faChevronLeft, faChevronRight } from "../../content/icons.js";
import GroupDetails from "../../components/groups/GroupDetails.jsx";
import GroupEdit from "../../components/groups/GroupEdit.jsx";
import AddGroup from "../../components/groups/AddGroup.jsx";

export default function Groups() {
    const [groups, setGroups] = useState([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState("");
    const [modalhidden, setModalHidden] = useState(true);
    const [modalType, setModalType] = useState("");
    const [groupModal, setGroupModal] = useState(null);
    const [pagina, setPagina] = useState(1);
    const porPagina = 10;

    const cargarDatos = async () => {
        const response = await getGroups();
        setGroups(response);
        setLoading(false);
    };
    useEffect(() => {
        cargarDatos();
    }, []);
    const filteredGroups = groups.filter(group => {
        const searchTerm = search.toLowerCase();
        return (
            group?.nombre?.toLowerCase().includes(searchTerm) ||
            group?.descripcion?.toLowerCase().includes(searchTerm) ||
            group?.creado_por?.toLowerCase().includes(searchTerm)
        )
    }
    );
    const activeFilteredGroups = filteredGroups.filter(u => u.estado === true);
    const totalPaginas = Math.ceil(filteredGroups.length / porPagina);
    const inicio = (pagina - 1) * porPagina;
    const fin = inicio + porPagina;
    const gruposPaginados = filteredGroups.slice(inicio, fin);
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
                            <FontAwesomeIcon icon={faUsers} />
                        </div>
                        <div>
                            <h1>Gestion de Grupos</h1>
                            <p className="header-subtitle">Administra todos los grupos del sistema</p>
                        </div>
                    </div>
                    <button className="btn-premium btn-Agregar-Grupo" onClick={() => { setModalType('add'); setModalHidden(false) }}>
                        <FontAwesomeIcon icon={faPlusCircle} className="me-2" />
                        Nuevo Grupo
                    </button>
                </div>

                <div className="stats-grid">
                    <div className="stat-card">
                        <div className="stat-icon groups-icon">
                            <FontAwesomeIcon icon={faUsers} />
                        </div>
                        <div className="stat-info">
                            <h3>{filteredGroups.length}</h3>
                            <p>Total Grupos</p>
                        </div>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon active-icon">
                            <FontAwesomeIcon icon={faUserCheck} />
                        </div>
                        <div className="stat-info">
                            <h3>{activeFilteredGroups.length}</h3>
                            <p>Grupos Activos</p>
                        </div>
                    </div>
                </div>
            </div>

            <div className="premium-card">
                <div className="card-header-premium">
                    <div className="header-actions">
                        <div className="search-container">
                            <FontAwesomeIcon icon={faSearch} className="search-icon" />
                            <input type="text"
                                className="search-input"
                                placeholder="Buscar grupos..."
                                id="searchInput"
                                onChange={(e) => setSearch(e.target.value)} />
                        </div>
                        <div className="filter-actions">

                        </div>
                    </div>
                </div>

                <div className="card-body-premium">
                    <div className="table-container">
                        <table className="premium-table" id="TablaDeGrupos">
                            <thead>
                                <tr>
                                    <th className="name-col">
                                        <div className="th-content">
                                            <span>Nombre del Grupo</span>
                                            <FontAwesomeIcon icon={faSort} className="ms-2" />
                                        </div>
                                    </th>
                                    <th className="desc-col">
                                        <div className="th-content">
                                            <span>Descripción</span>
                                            <FontAwesomeIcon icon={faSort} className="ms-2" />
                                        </div>
                                    </th>
                                    <th className="creator-col">
                                        <div className="th-content">
                                            <span>Creado Por</span>
                                            <FontAwesomeIcon icon={faSort} className="ms-2" />
                                        </div>
                                    </th>
                                    <th className="status-col">
                                        <div className="th-content">
                                            <span>Estado</span>
                                        </div>
                                    </th>
                                    <th className="actions-col">
                                        <div className="th-content">
                                            <span>Acciones</span>
                                        </div>
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                {filteredGroups && filteredGroups.length === 0 ? (
                                    <tr>
                                        <td colSpan="5" className="text-center text-muted py-4">
                                            <FontAwesomeIcon icon={faUsers} className="fa-2x mb-2" />
                                            <br />
                                            No hay grupos registrados con el dato especificado
                                        </td>
                                    </tr>
                                ) : (
                                    gruposPaginados.map((group) => {
                                        const iniciales = group.creado_por
                                            ? group.creado_por
                                                .split(" ")
                                                .map(p => p[0])
                                                .join("")
                                                .toUpperCase()
                                            : "AD";

                                        return (
                                            <tr className="group-row" key={group.idGrupo}>
                                                <td className="name-cell">
                                                    <div className="group-info">
                                                        <div className="group-icon">
                                                            <FontAwesomeIcon icon={faUsers} />
                                                        </div>
                                                        <div className="group-details">
                                                            <span className="group-name">{group.nombre}</span>
                                                            <span className="group-id">ID: {group.idGrupo}</span>
                                                        </div>
                                                    </div>
                                                </td>

                                                <td className="desc-cell">
                                                    <span className="group-description">{group.descripcion}</span>
                                                </td>

                                                <td className="creator-cell">
                                                    <div className="creator-info">
                                                        <div className="creator-avatar">
                                                            <span className="avatar-text">{iniciales}</span>
                                                        </div>
                                                        <span className="creator-name">{group.creado_por || "Administrador"}</span>
                                                    </div>
                                                </td>

                                                <td className="status-cell">
                                                    <span className={`status-badge ${group.estado ? "active" : "inactive"}`}>
                                                        {group.estado ? "Activo" : "Inactivo"}
                                                    </span>
                                                </td>

                                                <td className="actions-cell">
                                                    <div className="d-flex justify-content-center gap-2">
                                                        <button
                                                            className="btn btn-outline-primary btn-sm btn-Editar-Grupo"
                                                            data-id={group.idGrupo}
                                                            title="Editar Grupo"
                                                            onClick={() => {
                                                                setModalType("edit");
                                                                setGroupModal(group);
                                                                setModalHidden(false);
                                                            }}
                                                        >
                                                            <FontAwesomeIcon icon={faEdit} />
                                                        </button>

                                                        <button
                                                            className="btn btn-outline-dark btn-sm btn-Detalles-Grupo"
                                                            data-id={group.idGrupo}
                                                            title="Ver Detalles"
                                                            onClick={() => {
                                                                setModalType("details");
                                                                setGroupModal(group);
                                                                setModalHidden(false);
                                                            }}
                                                        >
                                                            <FontAwesomeIcon icon={faEye} />
                                                        </button>
                                                    </div>
                                                </td>
                                            </tr>
                                        );
                                    })
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>

                <div className="card-footer-premium">
                <div className="pagination-info">
                    Mostrando <strong>{gruposPaginados.length}</strong> de <strong>{filteredGroups.length}</strong> grupos
                </div>

                <div className="pagination-controls">
                    <button
                        className={`pagination-btn ${pagina === 1 ? "disabled" : ""}`}
                        disabled={pagina === 1}
                        onClick={() => setPagina(p => Math.max(p - 1, 1))}
                    >
                        <FontAwesomeIcon icon={faChevronLeft} />
                    </button>

                    <span className="pagination-btn active">{pagina}</span>

                    <button
                        className={`pagination-btn ${pagina === totalPaginas ? "disabled" : ""}`}
                        disabled={pagina === totalPaginas}
                        onClick={() => setPagina(p => Math.min(p + 1, totalPaginas))}
                    >
                        <FontAwesomeIcon icon={faChevronRight} />
                    </button>
                </div>
            </div>
            </div>

            {!modalhidden && (
                <>
                    <div className="modal fade show d-block" tabIndex="-1">
                        <div className="modal-dialog modal-dialog-centered modal-lg ">
                            <div className="modal-content">
                                <div className="modal-body">
                                    {
                                        modalType === "details" && <GroupDetails grupo={groupModal} onClose={() => setModalHidden(true)} />
                                    }
                                    {
                                        modalType === "edit" && <GroupEdit grupo={groupModal} onClose={() => { setModalHidden(true); cargarDatos(); }} />
                                    }
                                    {
                                        modalType === "add" && <AddGroup onClose={() => { setModalHidden(true); cargarDatos(); }} />
                                    }
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="modal-backdrop fade show"></div>
                </>
            )}
        </div >
    )
}