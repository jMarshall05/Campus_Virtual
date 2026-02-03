import { useEffect, useState } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import '../content/dashboard.css';
import { getUsersByRole } from "../api/userService.js";
import { Link } from "react-router-dom";
import Loader from "../components/Loader.jsx";
import {faTachometerAlt,
  faUserGraduate,
  faChalkboardTeacher,
  faBolt,
  faTasks,
  faInbox,
  faUsersCog,
  faBook,
  faCalendarAlt,
  faFileAlt,
  faBullhorn }from'../content/icons.js'


export default function Dashboard() {
    const [loading, setLoading] = useState(true);
    const [Estudiantes,setEstudiantes] = useState([]);
    const [Profesores,setProfesores]= useState([]);

    useEffect(() => {
        const cargarDatos = async () => {
            try {
                const Est = await getUsersByRole(`Estudiantes`)
                const Prof = await getUsersByRole(`Profesores`)

                setEstudiantes(Est);
                setProfesores(Prof);
            } finally {
                setLoading(false);
            }
        };
        cargarDatos();
    }, []);
    if (loading) return <Loader />;

    return (
        <div className="Dashboard">
            <div className="admin-Dasboard-wrapper">
                <div className="welcome-section mb-4">
                    <div className="welcome-card">
                        <div className="welcome-icon">
                            <FontAwesomeIcon icon={faTachometerAlt} />
                        </div>
                        <div className="welcome-content">
                            <h2 className="welcome-title">Panel de Administración</h2>
                            <p className="welcome-subtitle">Bienvenido al sistema de gestión de Santa Ana</p>
                        </div>
                    </div>
                </div>
            </div><div className="metrics-section mb-4">
                <div className="row g-3">
                    <div className="col-12 col-sm-6 col-lg-6">
                        <div className="metric-card metric-students">
                            <div className="metric-icon-wrapper">
                                <FontAwesomeIcon icon={faUserGraduate}/>
                            </div>
                            <div className="metric-details">
                                <h3 className="metric-title">Total de Estudiantes</h3>
                                <p className="metric-value">{Estudiantes.length}</p>
                            </div>
                        </div>
                    </div>
                    <div className="col-12 col-sm-6 col-lg-6">
                        <div className="metric-card metric-teachers">
                            <div className="metric-icon-wrapper">
                                <FontAwesomeIcon icon={faChalkboardTeacher}/>
                            </div>
                            <div className="metric-details">
                                <h3 className="metric-title">Total de Profesores</h3>
                                <p className="metric-value">{Profesores.length}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div className="actions-section">
                <h4 className="section-heading mb-3">
                    <FontAwesomeIcon icon={faBolt}/>
                    Acciones Rápidas
                </h4>
                <div className="row g-3 mb-4">
                    <div className="col-12 col-sm-6 col-lg-3">
                        <Link to="/#" className="action-link">
                            <div className="action-card-modern action-primary">
                                <div className="action-icon-circle">
                                    <FontAwesomeIcon icon={faTasks}/>
                                </div>
                                <h5 className="action-label">Asignar Tarea</h5>
                                <p className="action-desc">Crear y asignar tareas</p>
                            </div>
                        </Link>
                    </div>
                    <div className="col-12 col-sm-6 col-lg-3">
                        <Link to="/#" className="action-link">
                            <div className="action-card-modern action-info">
                                <div className="action-icon-circle">
                                    <FontAwesomeIcon icon={faInbox}/>
                                </div>
                                <h5 className="action-label">Tareas Recibidas</h5>
                                <p className="action-desc">Revisar entregas</p>
                            </div>
                        </Link>
                    </div>
                    <div className="col-12 col-sm-6 col-lg-3">
                    <Link to='/#' className="action-link">
                        <div className="action-card-modern action-warning">
                            <div className="action-icon-circle">
                                <FontAwesomeIcon icon={faUsersCog}/>
                            </div>
                            <h5 className="action-label">Gestionar Usuarios</h5>
                            <p className="action-desc">Administrar usuarios</p>
                        </div>
                    </Link>
                </div>
                <div className="col-12 col-sm-6 col-lg-3">
                    <Link to='/#' className="action-link">
                        <div className="action-card-modern action-success">
                            <div className="action-icon-circle">
                                <FontAwesomeIcon icon={faBook}/>
                            </div>
                            <h5 className="action-label">Configurar Cursos</h5>
                            <p className="action-desc">Administrar cursos</p>
                        </div>
                    </Link>
                </div>
                </div>
                
                <div className="row g-3 justify-content-center">

                    <div className="col-12 col-sm-6 col-lg-3">
                        <Link to='/#' className="action-link">
                            <div className="action-card-modern action-pink">
                                <div className="action-icon-circle">
                                    <FontAwesomeIcon icon={faCalendarAlt}/>
                                </div>
                                <h5 className="action-label">Calendario</h5>
                                <p className="action-desc">Eventos y fechas</p>
                            </div>
                        </Link>
                    </div>
                    <div className="col-12 col-sm-6 col-lg-3">
                        <Link to='/#' className="action-link">
                            <div className="action-card-modern action-orange">
                                <div className="action-icon-circle">
                                    <FontAwesomeIcon icon={faFileAlt}/>
                                </div>
                                <h5 className="action-label">Documentos</h5>
                                <p className="action-desc">Gestionar archivos</p>
                            </div>
                        </Link>
                    </div>
                    <div className="col-12 col-sm-6 col-lg-3">
                        <Link to='/#' className="action-link">
                            <div className="action-card-modern action-red">
                                <div className="action-icon-circle">
                                    <FontAwesomeIcon icon={faBullhorn}/>
                                </div>
                                <h5 className="action-label">Anuncios</h5>
                                <p className="action-desc">Publicar comunicados</p>
                            </div>
                        </Link>
                    </div>
                </div >
            </div >
        </div >


    )
}
