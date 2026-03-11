import { useEffect, useState } from "react";
import "../../content/users/profileManage.css"
import Loader from "../../components/Loader";
import { leerToken, disable2Factor, cambiarToken } from "../../utils/auth";
import { getUserById } from "../../api/userService";
import { Link } from "react-router-dom";
import EnableTwoFa from "../../components/users/EnableTwoFa";
import { jwtDecode } from "jwt-decode";

export default function Profile() {
    const [userId, setId] = useState("");
    const [usuario, setUsuario] = useState({
        nombre: "",
        apellido: "",
        telefonos: [{
            id: null,
            idUsuario: null,
            codigo: "",
            telefono: "",
            tipo: "",
            estado: true
        }]
    });
    const [loading, setLoading] = useState(true)
    const [form, setForm] = useState({
        nombre: "",
        apellido: "",
        telefonos: [{
            id: null,
            idUsuario: null,
            codigo: "",
            telefono: "",
            tipo: "",
            estado: true
        }]
    })
    const [error, setError] = useState("");
    const [twoFactorEnabled, setTwoFactor] = useState(false)
    const [modalhidden, setModalHidden] = useState(true)


    const inactivar2fa = async () => {
        try {
            const response = await disable2Factor(userId);

            if (response?.token) {
                cambiarToken(response.token);
            }
            setTwoFactor(false);

        } catch (error) {
            console.error(error);
        }
    };
    const activar2fa = async () => {
        setModalHidden(false)
    }

    useEffect(() => {
        const cargarDatos = async () => {
            const localToken = leerToken()
            setId(localToken.sub)
            setTwoFactor(localToken.twoFactorEnabled === true || localToken.twoFactorEnabled === "true");

            const user = await getUserById(localToken.sub);
            setUsuario(user);

            setLoading(false)
        };
        cargarDatos();
    }, []);


    if (loading) return <Loader />;


    const handleTelefonoChange = (index, field, value) => {
        const nuevosTelefonos = [...form.telefonos];
        nuevosTelefonos[index][field] = value;

        setForm({
            ...form,
            telefonos: nuevosTelefonos
        });
    };
    const handleChange = (e) => {
        const { name, value, type, checked } = e.target;

        setForm({
            ...form,
            [name]: type === "checkbox" ? checked : value
        });
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");
        setLoading(true);

        try {
            const payload = {
                nombre: form.nombre,
                apellido: form.apellido,
                telefonos: form.telefonos
            }
            await editUser(userId, payload);
        } catch (err) {
            setError(`${err.message}`);
        } finally {
            setLoading(false)
        }
    };



    return (
        <div className="container-fluid">
            <div className="row justify-content-center">
                <div className="col-12 col-md-10 col-lg-9 col-xl-8">

                    <h2 className="container-title mb-4">
                        <i className="bi bi-person-badge me-2"></i>
                        Mi cuenta
                    </h2>

                    {error && (
                        <div className="alert alert-danger alert-dismissible fade show shadow-sm" role="alert">
                            <i className="bi bi-x-circle-fill me-2"></i>
                            {error}
                            <button type="button" className="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                        </div>
                    )}

                    <div className="card shadow-lg mb-4 border-0 rounded-4">
                        <div className="card-header border-0 rounded-top-4 d-flex align-items-center">
                            <h5 className="mb-0 fw-bold text-dark">
                                <i className="bi bi-info-circle me-2"></i>
                                Información General
                            </h5>
                        </div>
                        <div className="card-body p-4 p-sm-3 p-md-4">
                            <form onSubmit={handleSubmit}>
                                {/* Vista de tabla para pantallas medianas y grandes */}
                                <div className="d-none d-md-block">
                                    <table className="table table-bordered table-striped align-middle premium-table">
                                        <tbody>
                                            <tr>
                                                <th className="fw-semibold" style={{ width: "30%" }}>
                                                    <i className="bi bi-person me-2 text-dark"></i>
                                                    Nombre
                                                </th>
                                                <td className="valor" data-campo="Nombre">{usuario.nombre}</td>
                                            </tr>
                                            <tr>
                                                <th className="fw-semibold">
                                                    <i className="bi bi-person me-2 text-dark"></i>
                                                    Apellido
                                                </th>
                                                <td className="valor" data-campo="Apellido">{usuario.apellido}</td>
                                            </tr>
                                            <tr>
                                                <th className="fw-semibold">
                                                    <i className="bi bi-phone me-2 text-dark"></i>
                                                    Teléfonos
                                                </th>
                                                <td className="valor" data-campo="Telefonos">
                                                    {usuario.telefonos.map((telefono) => (
                                                        <div
                                                            key={telefono.id}
                                                            className="telefono-item d-flex align-items-center justify-content-between py-2 px-3 mb-2 rounded shadow-sm bg-light"
                                                            data-id={telefono.id}
                                                        >
                                                            <span>
                                                                (+{telefono.codigo}){" "}
                                                                {String(telefono.telefono).slice(0, 4)}-{String(telefono.telefono).slice(4)}:{" "}
                                                                {telefono.tipo}
                                                            </span>
                                                            {telefono.estado ? (
                                                                <span className="badge bg-success fw-bold">Activo</span>
                                                            ) : (
                                                                <span className="badge bg-danger fw-bold">Inactivo</span>
                                                            )}
                                                        </div>
                                                    ))}
                                                </td>
                                            </tr>
                                            <tr>
                                                <th className="fw-semibold">
                                                    <i className="bi bi-envelope me-2 text-dark"></i>
                                                    Email
                                                </th>
                                                <td>{usuario.email}</td>
                                            </tr>
                                            <tr>
                                                <th className="fw-semibold">
                                                    <i className="bi bi-card-text me-2 text-dark"></i>
                                                    Identificación
                                                </th>
                                                <td>{usuario.identificacion}</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>

                                {/* Vista de cards para pantallas pequeñas */}
                                <div className="d-md-none">
                                    <div className="info-mobile-container">
                                        <div className="info-mobile-item">
                                            <div className="info-mobile-label">
                                                <i className="bi bi-person me-2"></i>
                                                Nombre
                                            </div>
                                            <div className="info-mobile-value">{usuario.nombre}</div>
                                        </div>

                                        <div className="info-mobile-item">
                                            <div className="info-mobile-label">
                                                <i className="bi bi-person me-2"></i>
                                                Apellido
                                            </div>
                                            <div className="info-mobile-value">{usuario.apellido}</div>
                                        </div>

                                        <div className="info-mobile-item">
                                            <div className="info-mobile-label">
                                                <i className="bi bi-phone me-2"></i>
                                                Teléfonos
                                            </div>
                                            <div className="info-mobile-value">
                                                {usuario.telefonos.map((telefono) => (
                                                    <div
                                                        key={telefono.id}
                                                        className="telefono-item-mobile mb-2 p-2 rounded shadow-sm bg-light"
                                                        data-id={telefono.id}
                                                    >
                                                        <div className="d-flex justify-content-between align-items-center flex-wrap gap-2">
                                                            <span className="telefono-number">
                                                                (+{telefono.codigo}) {String(telefono.telefono).slice(0, 4)}-{String(telefono.telefono).slice(4)}
                                                            </span>
                                                            {telefono.estado ? (
                                                                <span className="badge bg-success fw-bold">Activo</span>
                                                            ) : (
                                                                <span className="badge bg-danger fw-bold">Inactivo</span>
                                                            )}
                                                        </div>
                                                        <small className="text-muted">{telefono.tipo}</small>
                                                    </div>
                                                ))}
                                            </div>
                                        </div>

                                        <div className="info-mobile-item">
                                            <div className="info-mobile-label">
                                                <i className="bi bi-envelope me-2"></i>
                                                Email
                                            </div>
                                            <div className="info-mobile-value text-break">{usuario.email}</div>
                                        </div>

                                        <div className="info-mobile-item">
                                            <div className="info-mobile-label">
                                                <i className="bi bi-card-text me-2"></i>
                                                Identificación
                                            </div>
                                            <div className="info-mobile-value">{usuario.identificacion}</div>
                                        </div>
                                    </div>
                                </div>
                            </form>

                            <div className="d-flex justify-content-between mt-4 gap-2 flex-wrap flex-md-nowrap">
                                <div className="w-100 w-md-auto">
                                    <Link to="/edit-password" className="btn btn-gradient-warning fw-semibold shadow-sm w-100">
                                        <i className="bi bi-key me-2"></i>
                                        Cambiar contraseña
                                    </Link>
                                </div>
                                <div className="w-100 w-md-auto">
                                    <button type="button" id="btnEditar" className="btn btn-gradient-primary fw-semibold shadow-sm w-100">
                                        <i className="bi bi-pencil-square me-2"></i>
                                        Editar datos
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="card shadow-lg border-0 rounded-4">
                        <div className="card-header bg-success bg-opacity-10 border-0 rounded-top-4 d-flex align-items-center">
                            <h5 className="mb-0 fw-bold text-success">
                                <i className="bi bi-shield-lock me-2"></i>
                                Autenticación en Dos Pasos (2FA)
                            </h5>
                        </div>
                        <div className="card-body p-4 p-sm-3 p-md-4">
                            <dl className="row mb-0">
                                <dd className="col-12">
                                    {twoFactorEnabled ? (
                                        <div className="d-flex align-items-start align-items-md-center justify-content-between flex-column flex-md-row gap-3">
                                            <div className="flex-grow-1">
                                                <span className="badge bg-success fs-6 me-2 shadow-sm">
                                                    <i className="bi bi-check-circle-fill me-1"></i>
                                                    Habilitado
                                                </span>
                                                <small className="text-muted d-block mt-2">
                                                    Tu cuenta está protegida con verificación en dos pasos
                                                </small>
                                            </div>

                                            <button
                                                type="button"
                                                onClick={inactivar2fa}
                                                className="btn btn-outline-danger btn-sm fw-semibold shadow-sm w-100 w-md-auto"
                                            >
                                                <i className="bi bi-shield-x me-1"></i>
                                                Deshabilitar
                                            </button>
                                        </div>
                                    ) : (
                                        <div className="alert alert-warning mb-0 shadow-sm" role="alert">
                                            <div className="d-flex align-items-start align-items-md-center justify-content-between flex-column flex-md-row gap-3">
                                                <div className="flex-grow-1">
                                                    <div className="d-flex align-items-center mb-1">
                                                        <i className="bi bi-exclamation-triangle-fill me-2"></i>
                                                        <strong>Deshabilitado</strong>
                                                    </div>
                                                    <p className="mb-0 small">
                                                        Te recomendamos activar 2FA para mayor seguridad
                                                    </p>
                                                </div>

                                                <button
                                                    type="button"
                                                    onClick={activar2fa}
                                                    className="btn btn-outline-success btn-sm fw-semibold shadow-sm w-100 w-md-auto"
                                                >
                                                    <i className="bi bi-shield-check me-1"></i>
                                                    Habilitar
                                                </button>
                                            </div>
                                        </div>
                                    )}
                                </dd>
                            </dl>
                        </div>
                    </div>
                </div>
            </div>
            {!modalhidden && (
                <>
                    <div className="modal fade show d-block" tabIndex="-1">
                        <div className="modal-dialog modal-dialog-centered twofa-modal">
                            <div className="modal-content">
                                <div className="modal-body">
                                    <EnableTwoFa onClose={() => setModalHidden(true)}
                                        onEnabled={(newToken) => {
                                            const token = jwtDecode(newToken.token);
                                            cambiarToken(newToken.token);
                                            setTwoFactor(token.twoFactorEnabled === true || token.twoFactorEnabled === "true")
                                            setModalHidden(true);
                                        }} />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div className="modal-backdrop fade show"></div>
                </>
            )}
        </div>

    );
}