import { useEffect, useState } from "react";
import { getUsers } from "../../api/userService";
import { getGroups } from "../../api/groupService";
import { getMaterias } from "../../api/materiasService";
import '../../content/courses/addcourse.css';
import Loader from "../Loader";
import { addCourse } from "../../api/coursesService";
import Swal from 'sweetalert2';
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
      idMateria: e.target[0].value,
      idGrupo: e.target[1].value,
      idProfesor: e.target[2].value
    };
    try {
      const response = await addCourse(data);
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
      <>
        <form id="add-course-form" onSubmit={handleSubmit}>
          <div className="modal-body">
            <div className="field-group">
              <label className="field-label">Materia</label>
              <select className="field-select">
                <option value="">Seleccione una materia</option>
                {materias.map(m => (
                  <option key={m.idMateria} value={m.idMateria}>{m.nombre}</option>
                ))}
              </select>
            </div>

            <div className="field-group">
              <label className="field-label">Grupo</label>
              <select className="field-select">
                <option value="">Seleccione un grupo</option>
                {grupos.map(g => (
                  <option key={g.idGrupo} value={g.idGrupo}>{g.nombre}</option>
                ))}
              </select>
            </div>

            <div className="field-group">
              <label className="field-label">Profesor</label>
              <select className="field-select">
                <option value="">Seleccione un profesor</option>
                {profesores.map(p => (
                  <option key={p.idUsuario} value={p.idUsuario}>{p.nombre} {p.apellido}</option>
                ))}
              </select>
            </div>
          </div>

          <div className="modal-footer">
            <button className="btn-cancel" onClick={onClose}>Cancelar</button>
            <button className="btn-save" type="submit">
              <i className="fas fa-save"></i> Guardar
            </button>
          </div>
        </form>
      </>
    );
};