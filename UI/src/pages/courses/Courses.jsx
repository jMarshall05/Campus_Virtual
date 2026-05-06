import { useEffect, useState } from "react";
import { changeCourseState, exportCoursesPdf, getCourses } from "../../api/coursesService";
import Loader from "../../components/Loader";
import "../../content/courses/courses.css"
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";

import { faBook, faBookOpen, faChalkboardTeacher, faCheckCircle, faChevronLeft, faChevronRight, faFileAlt, faPlusCircle, faSearch, faSort, faTrash } from "../../content/icons.js"
import Swal from "sweetalert2";
import AddCourse from "../../components/courses/AddCourse.jsx";

export default function Courses() {
    const [courses, setCourses] = useState([]);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('')
    const [pagina, setPagina] = useState(1);
    const [modalhidden,setModalHidden] =useState(true);
    const porPagina = 10;

    const cargarDatos = async () => {
        const data = await getCourses();
        setCourses(data);
        setLoading(false);
    }
    useEffect(() => {
        cargarDatos();
    }, [])
    const changeState = async (Id) => {
        try {
            setLoading(true);
            await changeCourseState(Id);
            Swal.fire({
                title: "Se a cambiado el estado del curso",
                icon: 'success',
                timer: 2000
            })
            cargarDatos();
        } catch (error) {
            console.error(error);
            Swal.fire({
                title: "Algo a fallado",
                icon: 'error',
                confirmButtonText: 'OK'
            })
        } finally {
            setLoading(false);
        }
    }
    const Pdf = async () => {
            try {
                const response = await exportCoursesPdf();
                const url = URL.createObjectURL(response);
                window.open(url, "_blank");
            } catch (error) {
                console.error("Error al generar el PDF:", error);
            }
        };

    const filteredCourses = courses.filter(course => {
        const searchterm = search.toLowerCase();
        return (
            course?.nombreGrupo?.toLowerCase().includes(searchterm) ||
            course?.nombreMateria?.toLowerCase().includes(searchterm) ||
            course?.nombreProfesor?.toLowerCase().includes(searchterm)

        )
    })
    const ActiveFilteredCourses = filteredCourses.filter(c => c.estado === true);
    const totalPaginas = Math.ceil(filteredCourses.length / porPagina);
    const inicio = (pagina - 1) * porPagina;
    const fin = inicio + porPagina;
    const cursosPaginados = filteredCourses.slice(inicio, fin);
    useEffect(() => {
        setPagina(1);
    }, [search]);

    if (loading) return <Loader />

    return (
        <div className="admin-container-fluid">
            <div className="admin-header">
                <div className="header-content">
                    <div className="header-title">
                        <div className="title-icon">
                            <FontAwesomeIcon icon={faBookOpen} />
                        </div>
                        <div>
                            <h1>Gestión de Cursos</h1>
                            <p className="header-subtitle">Administra todos los cursos del sistema</p>
                        </div>
                    </div>

                </div>

                <div className="stats-grid">
                    <div className="stat-card">
                        <div className="stat-icon courses-icon">
                            <FontAwesomeIcon icon={faBook} />
                        </div>
                        <div className="stat-info">
                            <h3>{courses ? (
                                courses.length
                            ) : (0)}</h3>
                            <p>Total Cursos</p>
                        </div>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon teachers-icon">
                            <FontAwesomeIcon icon={faChalkboardTeacher} />
                        </div>
                        <div className="stat-info">
                            <h3>
                                {courses ? ([... new Set(ActiveFilteredCourses.map(c => c.profesorId))].length) : (0)}</h3>
                            <p>Profesores Activos</p>
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
                                className="search-input"
                                placeholder="Buscar cursos..."
                                onChange={(e) => setSearch(e.target.value)}
                            />
                        </div>

                        <div className="buttons-group">
                            <button className="btn-premium btn-Agregar-Curso" onClick={()=> setModalHidden(false)}>
                                <FontAwesomeIcon icon={faPlusCircle} className="me-2" />
                                Nuevo Curso
                            </button>

                            <button className="btn-export btn-GenerarReporte" onClick={Pdf}>
                                <FontAwesomeIcon icon={faFileAlt} className="me-2" />
                                Generar Reporte
                            </button>
                        </div>

                    </div>
                </div>

                <div className="card-body-premium">
                    <div className="table-container">
                        <table className="premium-table" id="TablaDeCursos">
                            <thead>
                                <tr>
                                    <th className="course-id-col">
                                        <div className="th-content">
                                            <span>ID</span>
                                            <FontAwesomeIcon icon={faSort} />
                                        </div>
                                    </th>
                                    <th className="materia-col">
                                        <div className="th-content">
                                            <span>Materia</span>
                                            <FontAwesomeIcon icon={faSort} />
                                        </div>
                                    </th>
                                    <th className="grupo-col">
                                        <div className="th-content">
                                            <span>Grupo</span>
                                            <FontAwesomeIcon icon={faSort} />
                                        </div>
                                    </th>
                                    <th className="profesor-col">
                                        <div className="th-content">
                                            <span>Profesor</span>
                                            <FontAwesomeIcon icon={faSort} />
                                        </div>
                                    </th>
                                    <th className="profesor-col">
                                        <div className="th-content">
                                            <span>Estado</span>
                                            <FontAwesomeIcon icon={faSort} />
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
                                {courses && courses.length === 0 || courses === null ? (
                                    <tr>
                                        <td colSpan="6" className="text-center text-muted py-4">
                                            <FontAwesomeIcon icon={faBook} className="fa-2x mb-2" />

                                            <br />
                                            No hay cursos registrados
                                        </td>
                                    </tr>
                                ) : (
                                    cursosPaginados.map((course) => {
                                        let iniciales = "PR";
                                        if (course.nombreProfesor && course.nombreProfesor.trim()) {
                                            const nombres = course.nombreProfesor.trim().split(/\s+/);
                                            if (nombres.length >= 2) {
                                                iniciales = nombres[0][0].toUpperCase() + nombres[1][0].toUpperCase();
                                            } else if (nombres.length === 1 && nombres[0].length > 0) {
                                                iniciales = nombres[0][0].toUpperCase() + "P";
                                            }
                                        }

                                        return (
                                            <tr key={course.idCurso} className="course-row">
                                                <td className="course-id-cell" data-label="ID">
                                                    <span className="id-badge">#{course.idCurso}</span>
                                                </td>
                                                <td className="materia-cell" data-label="Materia">
                                                    <div className="materia-info">
                                                        <div className="materia-icon">
                                                            <FontAwesomeIcon icon={faBook} />
                                                        </div>
                                                        <div className="materia-details">
                                                            <span className="materia-name">{course.nombreMateria}</span>
                                                            <span className="materia-id">ID: {course.materiaId}</span>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td className="grupo-cell" data-label="Grupo">
                                                    <div className="grupo-info">
                                                        <div className="grupo-icon">
                                                            <i className="fas fa-users"></i>
                                                        </div>
                                                        <div className="grupo-details">
                                                            <span className="grupo-name">{course.nombreGrupo}</span>
                                                            <span className="grupo-id">ID: {course.grupoId}</span>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td className="profesor-cell" data-label="Profesor">
                                                    <div className="profesor-info">
                                                        <div className="profesor-avatar">
                                                            <span className="avatar-text">{iniciales}</span>
                                                        </div>
                                                        <div className="profesor-details">
                                                            <span className="profesor-name">{course.nombreProfesor}</span>
                                                            <span className="profesor-id">ID: {course.profesorId}</span>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td className="actions-cell" data-label="Estado">
                                                    <div className="d-flex justify-content-center gap-2">
                                                        {course.estado ? (
                                                            <span className="status-badge active">Activo</span>
                                                        ) : (
                                                            <span className="status-badge inactive">Inactivo</span>
                                                        )}
                                                    </div>
                                                </td>
                                                <td className="actions-cell" data-label="Acciones">
                                                    <div className="d-flex justify-content-center gap-2">
                                                        {course.estado ? (
                                                            <button
                                                                type="button"
                                                                className="btn btn-outline-danger btn-sm"
                                                                title="Desactivar Curso"
                                                                onClick={() => { changeState(course.idCurso) }}

                                                            >
                                                                <FontAwesomeIcon icon={faTrash} />
                                                            </button>
                                                        ) : (
                                                            <button
                                                                type="button"
                                                                className="btn btn-outline-success btn-sm"
                                                                title="Activar Curso"
                                                                onClick={() => { changeState(course.idCurso) }}
                                                            >
                                                                <FontAwesomeIcon icon={faCheckCircle} />
                                                            </button>
                                                        )}
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
                        Mostrando <strong>{cursosPaginados.length}</strong> de <strong>{filteredCourses.length}</strong> grupos
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
                        <div className="modal-dialog modal-dialog-centered">
                            <AddCourse onClose={() => { setModalHidden(true); cargarDatos(); }} />
                        </div>
                    </div>
                    <div className="modal-backdrop fade show"></div>
                </>
            )}
        </div>
    );
}