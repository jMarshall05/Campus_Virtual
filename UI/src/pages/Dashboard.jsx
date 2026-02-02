import { useState } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTachometerAlt } from "@fortawesome/free-solid-svg-icons";
import '../content/dashboard.css';
import { getUsersByRole } from "../api/userService";

const Estudiantes =await getUsersByRole(`Estudiantes`)
const Profesores = await getUsersByRole(`Profesores`)

console.log(Estudiantes)
console.log(Profesores)

export default function Dashboard() {
    return (
        <><div className="admin-Dasboard-wrapper">
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
                                <i className="fas fa-user-graduate"></i>
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
                                <i className="fas fa-chalkboard-teacher"></i>
                            </div>
                            <div className="metric-details">
                                <h3 className="metric-title">Total de Profesores</h3>
                                <p className="metric-value">{Profesores.length}</p>
                            </div>
                        </div>
                    </div>
                </div>
            </div></>


    )
}
