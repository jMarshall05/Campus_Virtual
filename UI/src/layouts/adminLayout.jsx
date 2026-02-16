import React, { useEffect, useState } from "react";
import { Link, Outlet, useNavigate } from "react-router-dom";
import { EliminarToken, leerToken, validarToken } from "../utils/auth";
import logo from '../assets/LogoInstitucion.png';


export default function Layout (){

    const navigate = useNavigate();
    const [username, setUsername] = useState('');

    useEffect(() => {

        const token = leerToken();
        if (!token || !validarToken()) {
            navigate('/login');
            return;
        }
        setUsername(token.name)
    

}, [navigate]);


const Logout = () => {
    EliminarToken();
    navigate('/login');
};

return (
    <div className="admin-wrapper">
        <aside className="sidebar-admin">
            <div className="logo">
                <Link to="/Dashboard">
                    <img src={logo} alt="Logo" height='90px' />
                </Link>
                <h4>Panel Administrador</h4>
            </div>

            <ul className="menu">
                <li>
                    <Link to="/users">
                        <i className="fas fa-user"></i> Usuarios
                    </Link>
                </li>
                <li>
                    <Link to="/cursos">
                        <i className="fas fa-book"></i> Cursos
                    </Link>
                </li>
                <li>
                    <Link to="/grupos">
                        <i className="fas fa-users"></i> Grupos
                    </Link>
                </li>
                <li>
                    <Link to="/anuncios">
                        <i className="fas fa-bullhorn"></i> Anuncios
                    </Link>
                </li>
                <li>
                    <Link to="/documentos">
                        <i className="fas fa-file-alt"></i> Documentos
                    </Link>
                </li>
                <li>
                    <Link to="/contactos">
                        <i className="fas fa-phone"></i> Contactos
                    </Link>
                </li>
                <li>
                    <Link to="/calendario">
                        <i className="fas fa-calendar"></i> Calendario
                    </Link>
                </li>
                <li>
                    <button type="button" onClick={Logout} className="logout-btn">
                        <i className="fas fa-sign-out-alt"></i> Cerrar sesión
                    </button>
                </li>
            </ul>
        </aside>

        <div className="main-content">
            <header className="user-header bg-white shadow-sm">
                <div className="container-fluid py-3">
                    <div className="row align-items-center">
                        <div className="col">
                            <div className="d-flex align-items-center">
                                <div className="user-avatar me-3">
                                    <div className="avatar-circle bg-primary text-white d-flex align-items-center justify-content-center">
                                        <i className="bi bi-person-fill fs-4"></i>
                                    </div>
                                </div>

                                <div>
                                    <p className="text-muted small mb-0">Bienvenido de nuevo</p>
                                    <h4 className="mb-0 text-dark fw-semibold">
                                        {username}
                                    </h4>
                                </div>
                            </div>
                        </div>

                        <div className="col-auto">
                            <div className="dropdown">
                                <button className="btn btn-outline-secondary btn-sm dropdown-toggle" type="button" id="userMenuDropdown" data-bs-toggle="dropdown" aria-expanded="false">
                                    <i className="bi bi-gear-fill me-1"></i>
                                    <span className="d-none d-md-inline">Cuenta</span>
                                </button>
                                <ul className="dropdown-menu dropdown-menu-end" aria-labelledby="userMenuDropdown">
                                    <li>
                                        <Link to='/profile' className="dropdown-item">
                                            <i className="bi bi-person-circle me-2"></i>
                                            Mi perfil
                                        </Link>
                                    </li>
                                    <li>
                                        <Link to='/cambiar-contrasena' className="dropdown-item">
                                            <i className="bi bi-key-fill me-2"></i>
                                            Cambiar contraseña
                                        </Link>
                                    </li>
                                    <li><hr className="dropdown-divider" /></li>
                                    <li>
                                        <button type="button" className="dropdown-item text-danger" onClick={Logout}>
                                            <i className="bi bi-box-arrow-right me-2"></i>
                                            Cerrar sesión
                                        </button>
                                    </li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </header>

            <main className="content-admin">
                <Outlet />
            </main>
        </div>
    </div>
);
}
