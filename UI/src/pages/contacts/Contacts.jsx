import { useEffect, useState } from "react";
import { getUsers } from "../../api/userService.js";
import Loader from "../../components/Loader.jsx";
import "../../content/contacts/contacts.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faUsers, faUserCheck, faSearch,
    faChevronLeft, faChevronRight, faEnvelope
} from "../../content/icons.js";

export default function Contacts() {
    const [loading, setLoading] = useState(true);
    const [usuarios, setUsuarios] = useState([]);
    const [search, setSearch] = useState("");
    const [pagina, setPagina] = useState(1);
    const porPagina = 10;

    useEffect(() => {
        const cargarDatos = async () => {
            try {
                const allUsers = await getUsers();
                // Filtrar solo docentes y administrativos
                const filtered = allUsers.filter(
                    (u) =>
                        u.rol === "Profesores" ||
                        u.rol === "Administradores"
                );
                setUsuarios(filtered);
            } catch (error) {
                console.error("Error al cargar contactos:", error);
            } finally {
                setLoading(false);
            }
        };
        cargarDatos();
    }, []);

    const handleSearchChange = (value) => {
        setSearch(value);
        setPagina(1);
    };

    const filtrados = usuarios.filter((u) => {
        const texto = search.toLowerCase();
        const fullName = `${u.nombre} ${u.apellido}`;
        return (
            fullName.toLowerCase().includes(texto) ||
            u.email?.toLowerCase().includes(texto) ||
            u.identificacion?.toLowerCase().includes(texto) ||
            u.rol?.toLowerCase().includes(texto)
        );
    });

    const activos = filtrados.filter((u) => u.estado === true);
    const totalPaginas = Math.ceil(filtrados.length / porPagina);
    const paginados = filtrados.slice(
        (pagina - 1) * porPagina,
        pagina * porPagina
    );

    if (loading) return <Loader />;

    return (
        <div className="admin-container-fluid">
            <div className="admin-header">
                <div className="header-content">
                    <div className="header-title">
                        <div className="title-icon" style={{ background: "linear-gradient(135deg, #06b6d4, #0891b2)" }}>
                            <FontAwesomeIcon icon={faUsers} />
                        </div>
                        <div>
                            <h1>Contactos</h1>
                            <p className="header-subtitle">
                                Docentes y personal administrativo
                            </p>
                        </div>
                    </div>
                </div>

                <div className="stats-grid">
                    <div className="stat-card">
                        <div className="stat-icon contacts-icon">
                            <FontAwesomeIcon icon={faUsers} />
                        </div>
                        <div className="stat-info">
                            <h3>{filtrados.length}</h3>
                            <p>Total Contactos</p>
                        </div>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon active-icon">
                            <FontAwesomeIcon icon={faUserCheck} />
                        </div>
                        <div className="stat-info">
                            <h3>{activos.length}</h3>
                            <p>Contactos Activos</p>
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
                                placeholder="Buscar contactos..."
                                value={search}
                                onChange={(e) => handleSearchChange(e.target.value)}
                            />
                        </div>
                    </div>
                </div>

                <div className="card-body-premium">
                    <div className="table-container">
                        <table className="premium-table">
                            <thead>
                                <tr>
                                    <th>Nombre</th>
                                    <th>Email</th>
                                    <th>Rol</th>
                                    <th>Estado</th>
                                </tr>
                            </thead>
                            <tbody>
                                {filtrados.length === 0 ? (
                                    <tr>
                                        <td colSpan="4" className="text-center text-muted py-4">
                                            <FontAwesomeIcon icon={faUsers} className="fa-2x mb-2" />
                                            <br />
                                            No hay contactos registrados
                                        </td>
                                    </tr>
                                ) : (
                                    paginados.map((contacto) => (
                                        <tr className="contact-row" key={contacto.idUsuario}>
                                            <td data-label="Nombre">
                                                <div className="contact-info">
                                                    <div className="contact-avatar">
                                                        {contacto.nombre?.[0]}
                                                        {contacto.apellido?.[0]}
                                                    </div>
                                                    <div className="contact-details">
                                                        <span className="contact-name">
                                                            {contacto.nombre} {contacto.apellido}
                                                        </span>
                                                        <span className="contact-role">
                                                            {contacto.identificacion}
                                                        </span>
                                                    </div>
                                                </div>
                                            </td>
                                            <td data-label="Email">
                                                <div className="contact-email-content">
                                                    <FontAwesomeIcon icon={faEnvelope} style={{ color: "#94a3b8", width: 16 }} />
                                                    <span>{contacto.email}</span>
                                                </div>
                                            </td>
                                            <td data-label="Rol">
                                                <span
                                                    className={`role-badge ${
                                                        contacto.rol === "Profesores" ? "profesor" : "administrador"
                                                    }`}
                                                >
                                                    {contacto.rol === "Profesores" ? "Profesor" : "Administrador"}
                                                </span>
                                            </td>
                                            <td data-label="Estado">
                                                <span
                                                    className={`status-badge ${
                                                        contacto.estado ? "active" : "inactive"
                                                    }`}
                                                >
                                                    {contacto.estado ? "Activo" : "Inactivo"}
                                                </span>
                                            </td>
                                        </tr>
                                    ))
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>

                <div className="card-footer-premium">
                    <div className="pagination-info">
                        Mostrando <strong>{paginados.length}</strong> de{" "}
                        <strong>{filtrados.length}</strong> contactos
                    </div>
                    <div className="pagination-controls">
                        <button type="button"
                            className={`pagination-btn ${pagina === 1 ? "disabled" : ""}`}
                            disabled={pagina === 1}
                            onClick={() => setPagina((p) => Math.max(p - 1, 1))}
                        >
                            <FontAwesomeIcon icon={faChevronLeft} />
                        </button>
                        <span className="pagination-btn active">{pagina}</span>
                        <button type="button"
                            className={`pagination-btn ${pagina >= totalPaginas ? "disabled" : ""}`}
                            disabled={pagina >= totalPaginas}
                            onClick={() => setPagina((p) => Math.min(p + 1, totalPaginas))}
                        >
                            <FontAwesomeIcon icon={faChevronRight} />
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
}
