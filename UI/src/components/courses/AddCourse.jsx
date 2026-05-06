import { useEffect, useState } from "react";
import { getUsers } from "../../api/userService";
import { getGroups } from "../../api/groupService";
import { getMaterias } from "../../api/materiasService";
import '../../content/courses/addcourse.css';
import Loader from "../Loader";
import { addCourse } from "../../api/coursesService";
import Swal from 'sweetalert2';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBook, faBookOpen, faChalkboardTeacher, faSave, faTimes, faUsers } from "../../content/icons.js";

export default function AddCourse({ onClose }) {
  const [profesores, setProfesores] = useState([]);
  const [materias, setMaterias] = useState([]);
  const [grupos, setGrupos] = useState([]);
  const [loading, setLoading] = useState(false);
  const cargarDatos = async () => {
    const users = await getUsers();
    const materiasData = await getMaterias();
    const gruposData = await getGroups();
    setMaterias(materiasData);
    setGrupos(gruposData);
    const teachers = users.filter(user => user.rol === "Profesores")
    setProfesores(teachers);
  }
  useEffect(() => {
    cargarDatos();
  }, [])
  const handleSubmit = async (e) => {
    e.preventDefault();

    setLoading(true);

    const data = {
      materiaId: e.target[0].value,
      GrupoId: e.target[1].value,
      idProfesor: e.target[2].value
    };
    try {
      await addCourse(data);
      setLoading(false);
      Swal.fire({
        title: 'Curso creado con éxito!',
        icon: 'success',
        confirmButtonText: 'OK'
      });
      onClose();


    } catch (error) {
      setLoading(false);
      Swal.fire({
        title: 'Error',
        icon: 'error',
        confirmButtonText: 'OK'
      });
      console.log(error);
    }};
    if (loading) return <Loader />;
    return (
      <div className="ac-modal">
        <div className="ac-header">
          <div className="ac-header__content">
            <div className="ac-header__icon">
              <FontAwesomeIcon icon={faBookOpen} />
            </div>
            <div>
              <h2 className="ac-header__title">Crear Nuevo Curso</h2>
              <p className="ac-header__subtitle">Completa la información del curso</p>
            </div>
          </div>
        </div>

        <form id="add-course-form" onSubmit={handleSubmit}>
          <div className="ac-body">

            <div className="ac-section">
              <h3 className="ac-section__title">
                <FontAwesomeIcon icon={faBook} />
                Materia
              </h3>
              <div className="ac-field">
                <label className="ac-field__label" htmlFor="idMateria">Materia del curso</label>
                <div className="ac-select-wrap">
                  <span className="ac-select-icon">
                    <FontAwesomeIcon icon={faBook} />
                  </span>
                  <select className="ac-select" id="idMateria" required>
                    <option value="">Seleccione una materia</option>
                    {materias.map(m => (
                      <option key={m.idMateria} value={m.idMateria}>{m.nombre}</option>
                    ))}
                  </select>
                </div>
              </div>
            </div>

            <div className="ac-section">
              <h3 className="ac-section__title">
                <FontAwesomeIcon icon={faUsers} />
                Grupo
              </h3>
              <div className="ac-field">
                <label className="ac-field__label" htmlFor="idGrupo">Grupo asignado</label>
                <div className="ac-select-wrap">
                  <span className="ac-select-icon">
                    <FontAwesomeIcon icon={faUsers} />
                  </span>
                  <select className="ac-select" id="idGrupo" required>
                    <option value="">Seleccione un grupo</option>
                    {grupos.map(g => (
                      <option key={g.idGrupo} value={g.idGrupo}>{g.nombre}</option>
                    ))}
                  </select>
                </div>
              </div>
            </div>

            <div className="ac-section">
              <h3 className="ac-section__title">
                <FontAwesomeIcon icon={faChalkboardTeacher} />
                Profesor
              </h3>
              <div className="ac-field">
                <label className="ac-field__label" htmlFor="idProfesor">Profesor a cargo</label>
                <div className="ac-select-wrap">
                  <span className="ac-select-icon">
                    <FontAwesomeIcon icon={faChalkboardTeacher} />
                  </span>
                  <select className="ac-select" id="idProfesor" required>
                    <option value="">Seleccione un profesor</option>
                    {profesores.map(p => (
                      <option key={p.idUsuario} value={p.idUsuario}>{p.nombre} {p.apellido}</option>
                    ))}
                  </select>
                </div>
              </div>
            </div>

          </div>

          <div className="ac-footer">
            <button type="button" className="ac-btn ac-btn--secondary" onClick={onClose}>
              <FontAwesomeIcon icon={faTimes} />
              Cancelar
            </button>
            <button type="submit" className="ac-btn ac-btn--primary">
              <FontAwesomeIcon icon={faSave} />
              Guardar
            </button>
          </div>
        </form>
      </div>
    );
};