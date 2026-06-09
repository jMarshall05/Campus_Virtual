import { useEffect, useState } from "react";
// TODO: Descomentar cuando EventosController esté listo en el backend
// import { getEventos } from "../../api/eventsService.js";
import Loader from "../../components/Loader.jsx";
import "../../content/calendar/calendar.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faCalendarAlt, faSearch,
    faChevronLeft, faChevronRight, faCheckCircle,
    faSort
} from "../../content/icons.js";
// TODO: Descomentar cuando EventosController esté listo
// import AddEvent from "../../components/calendar/AddEvent.jsx";
// import EditEvent from "../../components/calendar/EditEvent.jsx";
// import EventDetails from "../../components/calendar/EventDetails.jsx";
import FullCalendar from "@fullcalendar/react";
import dayGridPlugin from "@fullcalendar/daygrid";
import timeGridPlugin from "@fullcalendar/timegrid";
import interactionPlugin from "@fullcalendar/interaction";
import esLocale from "@fullcalendar/core/locales/es";


const formatDate = (dateStr) =>
    dateStr ? new Date(dateStr).toLocaleDateString("es-CR", {
        day: "2-digit",
        month: "2-digit",
        year: "numeric",
        hour: "2-digit",
        minute: "2-digit",
    })
        : "N/A";

export default function Calendar() {
    const [loading, setLoading] = useState(true);
    const [eventos, setEventos] = useState([]);
    const [search, setSearch] = useState("");
    const [vistaActual, setVistaActual] = useState("calendar"); // "calendar" | "list"
    // TODO: Descomentar modal states cuando EventosController esté listo
    // const [modalhidden, setModalHidden] = useState(true);
    // const [modalType, setModalType] = useState("");
    // const [eventModal, setEventModal] = useState(null);
    const [pagina, setPagina] = useState(1);
    const porPagina = 10;

    const cargarDatos = async () => {
        try {
            // TODO: Descomentar cuando EventosController esté listo
            // const data = await getEventos();
            // setEventos(data);
            setEventos([]);
        } catch (error) {
            console.error("Error al cargar eventos:", error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        cargarDatos();
    }, []);

    useEffect(() => {
        setPagina(1);
    }, [search]);

    // Transformar eventos para FullCalendar
    const calendarEvents = eventos.map((e) => ({
        id: e.id,
        title: e.titulo,
        start: e.fechaInicio,
        end: e.fechaFin,
        backgroundColor: e.estado ? "#3b82f6" : "#94a3b8",
        borderColor: e.estado ? "#2563eb" : "#64748b",
        extendedProps: {
            estado: e.estado,
            idUsuario: e.idUsuario,
        },
    }));

    // Filtrado para vista de tabla
    const filtrados = eventos.filter((e) => {
        const texto = search.toLowerCase();
        return e.titulo?.toLowerCase().includes(texto);
    });

    const activos = filtrados.filter((e) => e.estado === true);
    const totalPaginas = Math.ceil(filtrados.length / porPagina);
    const paginados = filtrados.slice(
        (pagina - 1) * porPagina,
        pagina * porPagina
    );

    // TODO: Descomentar cuando EventosController esté listo
    // const toggleEstado = async (id) => {
    //     try {
    //         await toggleEventoEstado(id);
    //         Swal.fire({ title: "Estado cambiado", icon: "success", timer: 1500 });
    //         cargarDatos();
    //     } catch (error) {
    //         console.error(error);
    //         Swal.fire({ title: "Error", icon: "error", confirmButtonText: "OK" });
    //     }
    // };



    if (loading) return <Loader />;

    return (
        <div className="admin-container-fluid">
            <div className="admin-header">
                <div className="header-content">
                    <div className="header-title">
                        <div className="title-icon calendar-icon">
                            <FontAwesomeIcon icon={faCalendarAlt} />
                        </div>
                        <div>
                            <h1>Calendario</h1>
                            <p className="header-subtitle">
                                Eventos y fechas importantes
                            </p>
                        </div>
                    </div>
                </div>

                <div className="stats-grid">
                    <div className="stat-card">
                        <div className="stat-icon calendar-icon">
                            <FontAwesomeIcon icon={faCalendarAlt} />
                        </div>
                        <div className="stat-info">
                            <h3>{filtrados.length}</h3>
                            <p>Total Eventos</p>
                        </div>
                    </div>
                    <div className="stat-card">
                        <div className="stat-icon active-icon">
                            <FontAwesomeIcon icon={faCheckCircle} />
                        </div>
                        <div className="stat-info">
                            <h3>{activos.length}</h3>
                            <p>Eventos Activos</p>
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
                                placeholder="Buscar eventos..."
                                value={search}
                                onChange={(e) => setSearch(e.target.value)}
                            />
                        </div>
                        <div className="filter-actions">
                            {/* Toggle vista calendario / tabla */}
                            <div className="btn-group me-2" role="group">
                                <button
                                    type="button"
                                    className={`btn btn-sm ${vistaActual === "calendar" ? "btn-primary" : "btn-outline-primary"}`}
                                    onClick={() => setVistaActual("calendar")}
                                >
                                    <FontAwesomeIcon icon={faCalendarAlt} className="me-1" />
                                    Calendario
                                </button>
                                <button
                                    type="button"
                                    className={`btn btn-sm ${vistaActual === "list" ? "btn-primary" : "btn-outline-primary"}`}
                                    onClick={() => setVistaActual("list")}
                                >
                                    <FontAwesomeIcon icon={faSort} className="me-1" />
                                    Lista
                                </button>
                            </div>
                            {/* TODO: Descomentar cuando EventosController esté listo */}
                            {/* <button type="button" className="btn-premium" onClick={() => { setModalType("add"); setModalHidden(false); }}>
                                <FontAwesomeIcon icon={faPlusCircle} className="me-2" />
                                Nuevo Evento
                            </button> */}
                        </div>
                    </div>
                </div>

                <div className="card-body-premium">
                    {vistaActual === "calendar" ? (
                        /* === VISTA CALENDARIO === */
                        <div className="calendar-view">
                            {eventos.length === 0 ? (
                                <div className="calendar-empty-state">
                                    <div className="empty-icon">
                                        <FontAwesomeIcon icon={faCalendarAlt} />
                                    </div>
                                    <h4>Sin eventos programados</h4>
                                    <p>
                                        {/* TODO: Descomentar cuando EventosController esté listo */}
                                        {/* Cuando se creen eventos, aparecerán en este calendario. */}
                                        El calendario mostrará eventos cuando el backend esté disponible.
                                    </p>
                                </div>
                            ) : (
                                <FullCalendar
                                    plugins={[dayGridPlugin, timeGridPlugin, interactionPlugin]}
                                    initialView="dayGridMonth"
                                    locale={esLocale}
                                    headerToolbar={{
                                        left: "prev,next today",
                                        center: "title",
                                        right: "dayGridMonth,timeGridWeek,timeGridDay",
                                    }}
                                    events={calendarEvents}
                                    editable={false}
                                    selectable={true}
                                    dayMaxEvents={3}
                                    eventDisplay="block"
                                    height="auto"
                                    contentHeight={600}
                                // TODO: Descomentar cuando EventosController esté listo
                                // eventClick={(info) => {
                                //     setEventModal(info.event.extendedProps);
                                //     setModalType("details");
                                //     setModalHidden(false);
                                // }}
                                />
                            )}
                        </div>
                    ) : (
                        /* === VISTA TABLA === */
                        <div className="table-container">
                            <table className="premium-table">
                                <thead>
                                    <tr>
                                        <th>
                                            <div className="th-content">
                                                <span>Evento</span>
                                                <FontAwesomeIcon icon={faSort} />
                                            </div>
                                        </th>
                                        <th>
                                            <div className="th-content">
                                                <span>Fecha Inicio</span>
                                                <FontAwesomeIcon icon={faSort} />
                                            </div>
                                        </th>
                                        <th>
                                            <div className="th-content">
                                                <span>Fecha Fin</span>
                                                <FontAwesomeIcon icon={faSort} />
                                            </div>
                                        </th>
                                        <th>
                                            <div className="th-content">
                                                <span>Estado</span>
                                            </div>
                                        </th>
                                        <th>
                                            <div className="th-content">
                                                <span>Acciones</span>
                                            </div>
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {filtrados.length === 0 ? (
                                        <tr>
                                            <td colSpan="5" className="text-center text-muted py-4">
                                                <FontAwesomeIcon icon={faCalendarAlt} className="fa-2x mb-2" />
                                                <br />
                                                No hay eventos registrados
                                            </td>
                                        </tr>
                                    ) : (
                                        paginados.map((evento) => (
                                            <tr className="event-row" key={evento.id}>
                                                <td data-label="Evento">
                                                    <div className="event-info">
                                                        <div className="event-icon">
                                                            <FontAwesomeIcon icon={faCalendarAlt} />
                                                        </div>
                                                        <div className="event-details">
                                                            <span className="event-name">{evento.titulo}</span>
                                                            <span className="event-id">ID: {evento.id}</span>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td data-label="Fecha Inicio">
                                                    <span className="badge-date">{formatDate(evento.fechaInicio)}</span>
                                                </td>
                                                <td data-label="Fecha Fin">
                                                    <span className="badge-date">{formatDate(evento.fechaFin)}</span>
                                                </td>
                                                <td data-label="Estado">
                                                    <span className={`status-badge ${evento.estado ? "active" : "inactive"}`}>
                                                        {evento.estado ? "Activo" : "Inactivo"}
                                                    </span>
                                                </td>
                                                <td data-label="Acciones">
                                                    <div className="actions-container">
                                                        {/* TODO: Descomentar acciones cuando EventosController esté listo */}
                                                        {/* <button type="button" className="btn btn-outline-dark btn-sm" title="Ver Detalles"
                                                            onClick={() => { setEventModal(evento); setModalType("details"); setModalHidden(false); }}>
                                                            <FontAwesomeIcon icon={faEye} />
                                                        </button>
                                                        <button type="button" className="btn btn-outline-primary btn-sm" title="Editar"
                                                            onClick={() => { setEventModal(evento); setModalType("edit"); setModalHidden(false); }}>
                                                            <FontAwesomeIcon icon={faEdit} />
                                                        </button>
                                                        <button type="button" className={`btn btn-sm ${evento.estado ? "btn-outline-danger" : "btn-outline-success"}`}
                                                            title={evento.estado ? "Desactivar" : "Activar"}
                                                            onClick={() => toggleEstado(evento.id)}>
                                                            <FontAwesomeIcon icon={evento.estado ? faTrash : faCheckCircle} />
                                                        </button> */}
                                                        <span className="text-muted small">Próximamente</span>
                                                    </div>
                                                </td>
                                            </tr>
                                        ))
                                    )}
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>

                {vistaActual === "list" && (
                    <div className="card-footer-premium">
                        <div className="pagination-info">
                            Mostrando <strong>{paginados.length}</strong> de{" "}
                            <strong>{filtrados.length}</strong> eventos
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
                )}
            </div>

            {/* TODO: Descomentar modal cuando EventosController esté listo */}
            {/* {!modalhidden && (
                <>
                    <div className="modal fade show d-block" tabIndex="-1">
                        <div className="modal-dialog modal-dialog-centered modal-lg">
                            <div className="modal-content">
                                <div className="modal-body">
                                    {modalType === "details" && <EventDetails evento={eventModal} onClose={() => setModalHidden(true)} />}
                                    {modalType === "edit" && <EditEvent evento={eventModal} onClose={() => { setModalHidden(true); cargarDatos(); }} />}
                                    {modalType === "add" && <AddEvent onClose={() => { setModalHidden(true); cargarDatos(); }} />}
                                </div>
                            </div>
                        </div>
                    </div>
                    <div className="modal-backdrop fade show"></div>
                </>
            )} */}
        </div>
    );
}
