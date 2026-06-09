# Migration Log: Razor Views → React UI

> **Fecha de inicio:** 8 de junio, 2026  
> **Proyecto fuente:** `Campus_SantaAna/Campus.UI` (ASP.NET MVC + Razor)  
> **Proyecto destino:** `Campus_Virtual/UI` (React 19 + Vite)  
> **Última actualización:** 9 de junio, 2026  
> **Último fix:** react-doctor — score 37→59, issues 276→35 (-241), accessibility 102→5 (-97)

---

## 📊 Estado General de Migración

| Módulo | Razor Views | React Status | Notas |
|--------|------------|--------------|-------|
| Auth (Login, 2FA) | Account/Login, LoginWith2FA, Solicitud2FA | ✅ Completo | `pages/auth/` |
| Dashboard | Home/Index (Admin) | ✅ Completo | `pages/users/AdminDashboard.jsx` |
| Usuarios | ListarUsuarios, _Detalles, _Editar | ✅ Completo | `pages/users/`, `components/users/` |
| Registro | Account/Register | ✅ Completo | `pages/users/AddUser.jsx` |
| Perfil | Manage/Index | ✅ Completo | `pages/users/ProfileManage.jsx` |
| 2FA Setup | Account/EnableAuthenticator | ✅ Completo | `components/users/EnableTwoFa.jsx` |
| Grupos | ListarGrupos, _Agregar, _Detalles, _Editar | ✅ Completo | `pages/groups/`, `components/groups/` |
| Cursos | ListarCursos, _AgregarCurso | ✅ Completo | `pages/courses/`, `components/courses/` |
| Documentos | Home/Documentos | ✅ Completo | `pages/docs/`, `components/docs/` |
| Anuncios | ListarAnuncios, _Create, _Edit, _Details | ✅ Completo | `pages/announcements/`, `components/announcements/` |
| **Tareas** | ListarTareas, Create, Edit, Details | ✅ **Migrado** | Admin CRUD + Estudiante vista cards |
| **Contactos** | Usuarios/VerDocentesAdministrativos | ✅ **Migrado** | Lista docentes + administrativos |
| **Calificaciones** | Create, Edit, Delete, MisCalificaciones | 🔄 **Stub creado** | UI completa, backend pendiente |
| **Entregas** | Index, SubirEntrega, MisEntregas, Edit | 🔄 **Stub creado** | UI completa, backend pendiente |
| **Calendario** | Home/About (FullCalendar) | 🔄 **Stub creado** | UI completa con FullCalendar v6, backend pendiente |
| **Cambiar Contraseña** | Manage/ChangePassword | 🔄 **Stub creado** | UI completa, backend pendiente |

---

## 🔧 Patrón de Borrado Lógico con `estado`

El proyecto utiliza **borrado lógico** en lugar de borrado físico. Cada entidad tiene un campo `estado` (boolean) que se alterna con `PATCH /api/{entity}/{id}/estado`.

### Análisis del Backend (.NET Core)

| Controller | Endpoint de "Delete" | Tipo | Método Backend |
|-----------|---------------------|------|----------------|
| **TareasController** | `PATCH /{idTarea}/estado` | ✅ Borrado Lógico | `CambiarEstadoTarea()` |
| **AnunciosController** | `PATCH /{anuncioId}` | ✅ Borrado Lógico | `CambiarEstadoAnuncio()` |
| **CursosController** | `PATCH /{idCurso}` | ✅ Borrado Lógico | `ModificarEstadoCurso()` |
| **DocumentosController** | `DELETE /{idDocumento}` | ⚠️ Borrado Físico | `BorrarDocumento()` |
| **GruposController** | — | Sin delete | Estado vía `EditarGrupo()` |
| **MateriasController** | — | Sin delete | Estado vía `EditarMateria()` |
| **UsuariosController** | — | Sin delete | Estado vía `EditarUsuarioAdmin()` |
| **CalificacionesController** | — | No existe aún | Asumimos `PATCH /{id}/estado` |
| **EntregasController** | — | No existe aún | Asumimos `PATCH /{id}/estado` |
| **EventosController** | — | No existe aún | Asumimos `PATCH /{id}/estado` |

**Regla:** Si el backend no tiene implementado un delete específico, se asume borrado lógico con `PATCH /{entity}/{id}/estado`.

### Patrón en la UI (consistente en todas las vistas):

```jsx
// Botón toggle: rojo cuando activo → verde cuando inactivo
<button
    className={`btn btn-sm ${item.estado ? "btn-outline-danger" : "btn-outline-success"}`}
    title={item.estado ? "Desactivar" : "Activar"}
    onClick={() => toggleEstado(item.id)}
>
    <FontAwesomeIcon icon={item.estado ? faTrash : faCheckCircle} />
</button>

// Badge de estado
<span className={`status-badge ${item.estado ? "active" : "inactive"}`}>
    {item.estado ? "Activo" : "Inactivo"}
</span>

// Contador de activos en stats
const activos = filtrados.filter(item => item.estado === true);
```

### API endpoint esperado (varía por controller):
```
PATCH /api/{entity}/{id}       →  Cambia estado (true↔false) [Anuncios, Cursos]
PATCH /api/{entity}/{id}/estado →  Cambia estado (true↔false) [Tareas]
```
> **Nota:** La inconsistencia en el sufijo `/estado` existe en el backend. Al implementar nuevos controllers, seguir el patrón `PATCH /{id}/estado` para mayor claridad.

---

## 📁 Archivos Creados/Modificados (Diff Completo)

### API Services (NUEVOS)
| Archivo | Estado | Descripción |
|---------|--------|-------------|
| `UI/src/api/tareasService.js` | ✅ Activo | CRUD de tareas (GET, POST, PUT, PATCH estado) |
| `UI/src/api/calificacionesService.js` | 🔄 Stub | Endpoints comentados con TODO, retorna `[]` |
| `UI/src/api/entregasService.js` | 🔄 Stub | Endpoints comentados con TODO, retorna `[]` |
| `UI/src/api/eventsService.js` | 🔄 Stub | Endpoints comentados con TODO, retorna `[]` |

### Pages (NUEVOS)
| Archivo | Estado | Descripción |
|---------|--------|-------------|
| `UI/src/pages/tasks/Tasks.jsx` | ✅ Activo | Lista tareas admin con CRUD, toggle estado |
| `UI/src/pages/tasks/MyTasks.jsx` | ✅ Activo | Vista cards para estudiantes |
| `UI/src/pages/contacts/Contacts.jsx` | ✅ Activo | Lista docentes y administrativos |
| `UI/src/pages/grades/Grades.jsx` | 🔄 Stub | Lista calificaciones con toggle estado |
| `UI/src/pages/submissions/Submissions.jsx` | 🔄 Stub | Lista entregas con toggle estado |
| `UI/src/pages/calendar/Calendar.jsx` | 🔄 Stub | Calendario FullCalendar v6 + vista tabla, API comentada |
| `UI/src/pages/users/ChangePassword.jsx` | 🔄 Stub | Formulario cambio contraseña, API comentada |

### Components (NUEVOS)
| Archivo | Estado | Descripción |
|---------|--------|-------------|
| `UI/src/components/tasks/AddTask.jsx` | ✅ Activo | Formulario crear tarea |
| `UI/src/components/tasks/TaskDetails.jsx` | ✅ Activo | Modal detalles tarea |
| `UI/src/components/tasks/EditTask.jsx` | ✅ Activo | Formulario editar tarea |
| `UI/src/components/grades/GradeDetails.jsx` | 🔄 Stub | Modal detalles calificación |
| `UI/src/components/grades/AddGrade.jsx` | 🔄 Stub | Formulario crear calificación |
| `UI/src/components/grades/EditGrade.jsx` | 🔄 Stub | Formulario editar calificación |
| `UI/src/components/submissions/SubmissionDetails.jsx` | 🔄 Stub | Modal detalles entrega |
| `UI/src/components/submissions/UploadSubmission.jsx` | 🔄 Stub | Formulario subir entrega |

### Styles (NUEVOS)
| Archivo | Estado | Descripción |
|---------|--------|-------------|
| `UI/src/content/tasks/tasks.css` | ✅ Activo | Estilos módulo tareas + hover-card |
| `UI/src/content/contacts/contacts.css` | ✅ Activo | Estilos módulo contactos |
| `UI/src/content/grades/grades.css` | ✅ Activo | Estilos módulo calificaciones |
| `UI/src/content/submissions/submissions.css` | ✅ Activo | Estilos módulo entregas |
| `UI/src/content/calendar/calendar.css` | 🔄 Stub | Estilos calendario FullCalendar + empty state |

### Modified
| Archivo | Cambios |
|---------|---------|
| `UI/src/App.jsx` | +65 líneas: rutas `/tasks`, `/my-tasks`, `/contacts`, `/grades`, `/submissions`, `/calendar`, `/change-password` |
| `UI/src/layouts/adminLayout.jsx` | +35/-7 líneas: sidebar con enlaces Tareas, Contactos, Calificaciones, Entregas, Calendario; role-based Mis Tareas; ChangePassword link; `Logout` → `handleLogout` con `useCallback` |
| `UI/src/api/authService.js` | +7 líneas: stub `changePassword()` comentado |
| `UI/src/content/icons.js` | +1 línea: export `faShieldAlt` |
| `UI/src/pages/courses/Courses.jsx` | `Pdf` movida a scope del módulo como `handleExportCoursesPdf` |
| `UI/src/pages/users/Users.jsx` | `Pdf` movida a scope del módulo como `handleExportUsersPdf` |
| `UI/src/components/users/UserDetails.jsx` | `UserPdf` movida a scope del módulo como `handleExportUserPdf(idUsuario)` |
| `UI/src/pages/docs/Docs.jsx` | `Delete` envuelta con `useCallback` → `handleDeleteDoc` |
| `UI/src/components/announcements/AnnouncementDetails.jsx` | Eliminado `useState`/`useEffect` innecesarios; se usa `anuncio.imagenRuta` directamente |
| `UI/src/components/users/UserEdit.jsx` | `telefonos` se inicializa desde prop, eliminado sync en useEffect |

### Documentation (NUEVO)
| Archivo | Descripción |
|---------|-------------|
| `UI/MIGRATION_LOG.md` | Este archivo de documentación |

### NPM Packages (NUEVOS)
| Paquete | Versión | Descripción |
|---------|---------|-------------|
| `@fullcalendar/react` | ^6.x | Wrapper React para FullCalendar |
| `@fullcalendar/daygrid` | ^6.x | Plugin de vista de cuadrícula de días |
| `@fullcalendar/timegrid` | ^6.x | Plugin de vista de cuadrícula de tiempo |
| `@fullcalendar/interaction` | ^6.x | Plugin de interacción (click, drag) |
| `react-doctor` | ^0.4.2 | Auditoría de código React (devDependency) |

---

## 🔍 React Doctor — Historial de Fixes

**Herramienta:** `react-doctor` v0.4.2 (millionco)  
**Score final:** 37/100 → 58/100 (+21)  
**Issues finales:** 276 → 52 (-224 total)

### Resumen por categoría

| Categoría | Inicio | R3 | R5 | R7 | Final | Reducción |
|-----------|--------|----|----|-----|-------|-----------|
| Bugs | 136 | 57 | 47 | 30 | 19 | -117 |
| Accessibility | 102 | 63 | 21 | 16 | 5 | -97 |
| Maintainability | 32 | 24 | 23 | 20 | 8 | -24 |
| Performance | 6 | 3 | 3 | 3 | 2 | -4 |
| **Total** | **276** | **147** | **94** | **69** | **35** | **-241** |

### Ronda 1: Errores críticos (8 de 8)

| # | Tipo | Archivo | Fix |
|---|------|---------|-----|
| 1 | Componente dentro de componente | `Courses.jsx` | `Pdf` → `handleExportCoursesPdf` (scope del módulo) |
| 2 | Componente dentro de componente | `Users.jsx` | `Pdf` → `handleExportUsersPdf` (scope del módulo) |
| 3 | Componente dentro de componente | `UserDetails.jsx` | `UserPdf` → `handleExportUserPdf(idUsuario)` (scope del módulo) |
| 4 | Componente dentro de componente | `adminLayout.jsx` | `Logout` → `handleLogout` (`useCallback`) |
| 5 | Componente dentro de componente | `Docs.jsx` | `Delete` → `handleDeleteDoc` (`useCallback`) |
| 6 | State sincronizado a prop | `AnnouncementDetails.jsx` | Eliminado `useState`/`useEffect`; se usa `anuncio.imagenRuta` directamente |
| 7 | State sincronizado a prop | `UserEdit.jsx` | `useState(usuario?.telefonos || [])`; eliminado sync en useEffect |
| 8 | Imports muertos | `AnnouncementDetails.jsx` | Eliminado `Loader`/`getImage`/`useEffect`/`useState` |

### Ronda 2: Fixes de accesibilidad y botones

| Fix | Archivos | Issues |
|-----|----------|--------|
| `type="button"` agregado | ~30 JSX files | ~54 |
| Duplicados `type="button"` eliminados | ~15 JSX files | ~20 |
| `htmlFor`/`id` en labels/controls | 16+ JSX files | ~35 |
| `aria-label` en botones/iconos | 10+ JSX files | ~15 |
| Anchor-as-button → `<button>` | Docs.jsx, UserDetails.jsx, GroupDetails.jsx | 12 |
| `autoFocus` eliminado | EnableTwoFa.jsx, Login2fa.jsx | 2 |
| Sidebar overlay `role`/`tabIndex`/`onKeyDown` | adminLayout.jsx | 3 |
| `cargarDatos` → `useCallback` | Docs.jsx | 1 |
| `inputStyle` → module constant | ChangePassword.jsx | 1 |
| `colSpan` (React correcto) | Users.jsx | 1 |
| Duplicate `id="estado"` corregido | AnnouncementEdit.jsx | 4 |

### Ronda 3: Performance, mantenibilidad y más

| Fix | Archivo | Descripción |
|-----|---------|-------------|
| `formatDate` movido fuera del componente | 8 archivos (Tasks.jsx, MyTasks.jsx, TaskDetails.jsx, Calendar.jsx, Announcements.jsx, Submissions.jsx, SubmissionDetails.jsx, AnnouncementDetails.jsx) | Pure function rebuilt every render |
| `Promise.all` reemplaza awaits secuenciales | AddCourse.jsx | Sequential independent awaits |
| `getUserRole`/`validarToken` reusan `leerToken` | auth.js | Repeated localStorage reads |
| `getToken()` helper centralizado | apiClient.js | Repeated localStorage reads |
| `disable2FA` import restaurado | auth.js | Import was incorrectly removed |
| Unused `logOut` comentado | authService.js | Unused export |
| Unused `getCourse` comentado | coursesService.js | Unused export |
| Unused `getDoc` comentado | docsService.js | Unused export |
| Duplicate props eliminados | Users.jsx, Groups.jsx, AddUser.jsx | Duplicate id attributes |

### Ronda 4: Bugs y patrones react (5 de 5)

| Fix | Archivo | Descripción |
|-----|---------|-------------|
| Duplicate `aria-label` eliminado | ProfileManage.jsx | `aria-label="Close"` duplicado removido |
| Duplicate `aria-label` eliminado | AddDoc.jsx | `aria-label="Close"` duplicado removido |
| Duplicate `aria-label` eliminado | EditDoc.jsx | `aria-label="Close"` duplicado removido |
| Derived state in effect → handler | Contacts.jsx | `useEffect(() => setPagina(1), [search])` → `handleSearchChange()` |
| IIFE destructuring → helper function | UserDetails.jsx | `getTipoIdInfo()` retorna `{icon, text, color}` |
| Setter renombrado para claridad | GroupEdit.jsx | `setState` → `setGroupState` |
| Setter renombrado para claridad | UserEdit.jsx | `setState` → `setUserState` |

### Ronda 5: Performance (3 intentos, react-doctor detecta patrones distintos)

| Fix | Archivo | Descripción |
|-----|---------|-------------|
| `useMemo` para derivación de props | `UserDetails.jsx` | `getTipoIdInfo(usuario.tipoIdentificacion)` envuelto en `useMemo` |
| Dead state eliminado | `ProfileManage.jsx` | Removido `useState` de `form` (nunca se actualiza), removido `handleSubmit` muerto, removido bloque `error` JSX |
| Pure functions a module scope | `AddUser.jsx` | `soloNumeros` y `alfaNumerico` movidos fuera del componente |

> **Nota:** react-doctor sigue reportando 3 warnings de performance — el tool puede estar detectando patrones diferentes (e.g., `qr` state via useEffect en UserDetails.jsx, wrapper `tipoIdentificacionChange` en AddUser.jsx). Los fixes son válidos independientemente.

### Ronda 6: Derived-state-in-effect + Accessibility + Maintainabilidad (94→69, 46→51/100)

| Fix | Archivos | Issues |
|-----|----------|--------|
| `useEffect(() => setPagina(1), [search])` → `handleSearchChange` | Users.jsx, Groups.jsx, Courses.jsx, Announcements.jsx, Tasks.jsx, Grades.jsx, Submissions.jsx, Calendar.jsx | 16 (derived-state + state-chained-through-effects) |
| Label `htmlFor` ↔ input `id` associations corregidos | AddUser.jsx, UserEdit.jsx | 6 |
| `aria-label` agregado a inputs/selects sin label | AddUser.jsx, UserEdit.jsx, EditTask.jsx, AddTask.jsx, AddGrade.jsx, EditGrade.jsx | 10 |
| `role="button"` → `<button>` semántico | adminLayout.jsx | 1 |
| Pure functions movidas a module scope | UserEdit.jsx (`soloNumeros`/`alfaNumerico`), Grades.jsx (`getScoreClass`) | 2 |
| `<input type="submit">` → `<button type="submit">` | AddUser.jsx | 1 |

### Ronda 7: Accessibility + Performance (69→63, 51→52/100)

| Fix | Archivos | Issues |
|-----|----------|--------|
| `aria-label` agregado a inputs DIMEX/Pasaporte/Fecha/Rol | AddUser.jsx, UserEdit.jsx | 5 |
| `aria-label` agregado a datetime-local inputs | AddTask.jsx, EditTask.jsx | 2 |
| `htmlFor` corregido (capitalización `númeroDeIdentificación`) | AddUser.jsx | 1 |
| `quitarImagen` useState → useRef (solo usado en handlers) | AnnouncementEdit.jsx | 1 |

### Ronda 10: Array index as key — stable keys (54→52, 56→58/100)

| Fix | Archivos | Issues |
|-----|----------|--------|
| `key={index}` → `key={telefono.id}` + `visibles[telefono.id]` | UserDetails.jsx | 1 |
| `key={i}` → `key={tel.id \|\| 'new-' + i}` | UserEdit.jsx | 1 |
| `key={i}` → `key={tel._key \|\| 'new-' + i}` con `crypto.randomUUID()` | AddUser.jsx | 1 |

### Ronda 9: new Date() hydration — useMemo (63→54, 52→56/100)

| Fix | Archivos | Issues |
|-----|----------|--------|
| `useMemo(() => new Date(), [])` como `nowDate` reemplaza `new Date()` en JSX | AddUser.jsx, UserEdit.jsx, AddTask.jsx, EditTask.jsx, AddAnnouncement.jsx, AnnouncementEdit.jsx, Tasks.jsx, MyTasks.jsx, TaskDetails.jsx | 9 |

### Ronda 8: Accessibility final — stale htmlFor + role (63→~57, 52→53/100)

| Fix | Archivos | Issues |
|-----|----------|--------|
| `aria-label="Eliminar teléfono"` agregado al botón de borrar teléfono | AddUser.jsx | 1 |
| `id="númeroDeIdentificación"` agregado al input disabled para match label | AddUser.jsx | 1 |
| Typo corregido `Nómero`→`Número` en aria-label | AddUser.jsx | 1 |
| Stale `htmlFor` removido de label "Estado" (apuntaba a checkbox, no form control) | UserEdit.jsx | 1 |
| Stale `htmlFor` removido de label "remove phone" (apuntaba a button, no form control) | UserEdit.jsx | 1 |
| `role="group"` → `aria-label="Vista"` en btn-group | Calendar.jsx | 1 |

### Ronda 11: Unused exports commented out (52→41, maintainability -11)

| Fix | Archivo | Issues |
|-----|---------|--------|
| `editUser` comentado (reemplazado por `editUserAdmin`) | `userService.js` | 1 |
| `getTareaById`, `getTareasByGrupo` comentados | `tareasService.js` | 2 |
| `getAnnouncementById`, `toggleAnnouncementStatus`, `getImage` comentados | `announcementsService.js` | 3 |
| `getCalificacionById`, `getMisCalificaciones` comentados (stub) | `calificacionesService.js` | 2 |
| `getEntregaById`, `getMisEntregas`, `editEntrega` comentados (stub) | `entregasService.js` | 3 |

### Pendiente (pre-existente, requiere refactor mayor)
- 13 archivos con `Many related useState calls` (candidatos a `useReducer`) — 13 issues
- 3 archivos con `Prop derived into useState` (GroupEdit, UserEdit, AnnouncementEdit) — 3 issues
- 2 archivos con `State only used in handlers` → `useRef` (EnableTwoFa `copied`, ProfileManage `modalhidden` — ambos usados en JSX, no convertibles) — 2 issues
- ✅ Resueltos en Ronda 10: 3 archivos con `Array index as key` — corregidos con keys estables (`telefono.id`, `crypto.randomUUID()`)
- ✅ Resueltos en Ronda 11: 11 exports no utilizados en servicios API — comentados con notas TODO/UNUSED
- 1 archivo con `Missing effect dependencies` (AddAnnouncement.jsx) — 1 issue
- 1 archivo con `Uncontrolled input value` (AddUser.jsx DIMEX/Pasaporte) — 1 issue
- 2 archivos sin alcance (`eventsService.js`, `stress-test.js`) — 2 issues

> **Nota:** `EnableTwoFa.copied` y `ProfileManage.modalhidden` se mantienen como `useState` porque se usan en JSX para renderizado condicional/estilos. react-doctor los reporta erróneamente como "solo en handlers".

---

## ⏳ Backend Pendiente

| Controller/Endpoint | Archivo Frontend | Acción requerida |
|--------------------|------------------|------------------|
| `CalificacionesController.cs` | `calificacionesService.js` | Descomentar endpoints |
| `EntregasController.cs` | `entregasService.js` | Descomentar endpoints |
| `EventosController.cs` | `eventsService.js` | Descomentar endpoints + descomentar lógica en `Calendar.jsx` |
| `POST /api/auth/change-password` | `authService.js` | Descomentar función `changePassword()` |

### Descomentar en Calendar.jsx cuando EventosController esté listo:
1. Import de `getEventos` desde `eventsService.js`
2. Import de componentes `AddEvent`, `EditEvent`, `EventDetails` (pendientes de crear en `components/calendar/`)
3. States: `modalhidden`, `modalType`, `eventModal`
4. Lógica de `cargarDatos()` con API real
5. `toggleEstado()` para borrado lógico
6. Modal JSX completo
7. `eventClick` handler en FullCalendar

### Descomentar en ChangePassword.jsx cuando endpoint esté listo:
1. Import de `changePassword` desde `authService.js`
2. Llamada real en `handleSubmit()`
3. Quitar simulación Swal de éxito

---

## 📐 Patrones de UI Establecidos

### Estructura de Página Admin
```jsx
<div className="admin-container-fluid">
  <div className="admin-header">
    <div className="header-content">...</div>
    <div className="stats-grid">
      {/* Total + Activos */}
    </div>
  </div>
  <div className="premium-card">
    <div className="card-header-premium">
      <div className="header-actions">
        <div className="search-container">...</div>
        <div className="filter-actions">...</div>
      </div>
    </div>
    <div className="card-body-premium">
      <div className="table-container">
        <table className="premium-table">...</table>
      </div>
    </div>
    <div className="card-footer-premium">
      {/* Paginación */}
    </div>
  </div>
</div>
```

### Modal Pattern (detalles/editar/crear)
```jsx
{!modalhidden && (
  <>
    <div className="modal fade show d-block" tabIndex="-1">
      <div className="modal-dialog modal-dialog-centered modal-lg">
        <div className="modal-content">
          <div className="modal-body">
            {modalType === "details" && <DetailsComponent />}
            {modalType === "edit" && <EditComponent />}
            {modalType === "add" && <AddComponent />}
          </div>
        </div>
      </div>
    </div>
    <div className="modal-backdrop fade show"></div>
  </>
)}
```

### Logical Delete Toggle Pattern
```jsx
<button
    className={`btn btn-sm ${item.estado ? "btn-outline-danger" : "btn-outline-success"}`}
    title={item.estado ? "Desactivar" : "Activar"}
    onClick={() => toggleEstado(item.id)}
>
    <FontAwesomeIcon icon={item.estado ? faTrash : faCheckCircle} />
</button>
```

---

## 🗂️ Estructura de Directorios React

```
UI/src/
├── api/                    # Servicios API por dominio
│   ├── announcementsService.js
│   ├── apiClient.js
│   ├── authService.js
│   ├── calificacionesService.js    # NUEVO (stub)
│   ├── coursesService.js
│   ├── docsService.js
│   ├── entregasService.js          # NUEVO (stub)
│   ├── eventsService.js            # NUEVO (stub)
│   ├── groupService.js
│   ├── materiasService.js
│   ├── tareasService.js            # NUEVO
│   └── userService.js
├── components/             # Componentes reutilizables
│   ├── announcements/
│   ├── auth/
│   ├── courses/
│   ├── docs/
│   ├── grades/                     # NUEVO
│   ├── groups/
│   ├── submissions/                # NUEVO
│   ├── tasks/                      # NUEVO
│   └── users/
├── content/                # Estilos CSS
│   ├── announcements/
│   ├── auth/
│   ├── calendar/                   # NUEVO
│   ├── contacts/                   # NUEVO
│   ├── courses/
│   ├── docs/
│   ├── grades/                     # NUEVO
│   ├── groups/
│   ├── submissions/                # NUEVO
│   ├── tasks/                      # NUEVO
│   └── users/
├── layouts/
│   └── adminLayout.jsx
├── pages/
│   ├── announcements/
│   ├── auth/
│   ├── calendar/                   # NUEVO
│   ├── contacts/                   # NUEVO
│   ├── courses/
│   ├── docs/
│   ├── grades/                     # NUEVO
│   ├── groups/
│   ├── submissions/                # NUEVO
│   ├── tasks/                      # NUEVO
│   └── users/
└── utils/
    └── auth.js
```
