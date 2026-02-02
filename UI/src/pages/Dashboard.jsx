import { useState } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faTachometerAlt } from "@fortawesome/free-solid-svg-icons";
import '../content/dashboard.css';

export default function Dashboard() {
    return(
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
    </div>
)
}
