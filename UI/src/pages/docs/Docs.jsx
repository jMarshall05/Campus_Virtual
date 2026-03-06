import { useEffect, useState } from "react"
import { getDoc, getDocs } from "../../api/docsService";
import Loader from "../../components/Loader";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faTrash, faCalendar, faCertificate, faCirclePlus, faEdit, faEllipsisV, faExternalLinkAlt, faFileAlt, faFileContract, faFolder, faFolderOpen, faStar } from "../../content/icons.js";
import "../../content/docs/docs.css"
import AddDoc from "../../components/docs/AddDoc.jsx";
import Swal from "sweetalert2";


export default function Docs() {
	const [loading, setLoading] = useState(true);
	const [error, setError] = useState();
	const [docs, setDocs] = useState([null]);
	const [modalhidden, setModalHidden] = useState(true);
	const [modalType, setModalType] = useState('');

	const cargarDatos = async () => {
		try {
			const documentos = await getDocs();
			setDocs(documentos);
		} catch (err) {
			setError("Error al cargar los documentos");
			console.error(err);
		} finally {
			setLoading(false);
		}
	};

	useEffect(() => {
		cargarDatos();
	}, []);

	const Download = async (id) => {
		setLoading(true)
		try {
			const url = getDoc(id);
			window.open(url, "_blank");
		} catch (error) {
			Swal.fire({
				title: "Algo a fallado intente de nuevo",
				icon: 'error',
				confirmButtonText: "OK"
			})
		}
		finally {
			setLoading(false);
		}
	}
	if (loading) return <Loader />;

	const docsInstitucionales = docs.filter(doc => doc.categoria === 'Institucional');

	return (
		<>
			<div className="documentos-main">
				<div className="page-header">
					<div className="header-content">
						<div className="header-text" style={{ display: 'flex', alignItems: 'center', gap: '1rem' }}>
							<div className="header-icon-wrap">
								<FontAwesomeIcon icon={faFolderOpen} />
							</div>
							<div>
								<h1 className="page-title">Documentos y Normativas</h1>
								<p className="page-subtitle">Gestiona y consulta documentos importantes del centro educativo</p>
							</div>
						</div>
						<div className="header-actions">
							<button
								className="btn-add-document"
								onClick={() => { setModalType("add"); setModalHidden(false); }}
							>
								<FontAwesomeIcon icon={faCirclePlus} />
								<span>Agregar Documento</span>
							</button>
						</div>
					</div>
				</div>

				<div className="docs-content">

					<div className="documents-section">
						<div className="section-header">
							<div className="section-title-wrapper">
								<FontAwesomeIcon icon={faStar} className="section-icon" />
								<h2 className="section-title">Documentos Institucionales</h2>
							</div>
							<span className="section-badge">{docsInstitucionales.length}</span>
						</div>

						<div className="row g-4 mb-2">
							{docsInstitucionales.length === 0 ? (
								<div className="col-12">
									<div className="empty-state">
										<div className="empty-icon">
											<FontAwesomeIcon icon={faFolderOpen} />
										</div>
										<h3>No hay documentos institucionales</h3>
										<p>Los documentos institucionales que se agreguen aparecerán aquí</p>
									</div>
								</div>
							) : (
								docsInstitucionales.map((doc) => (
									<div key={doc.id} className="col-12 col-lg-6">
										<div className="document-card official-doc">
											<div className="document-menu">
												<button className="btn-menu">
													<FontAwesomeIcon icon={faEllipsisV} />
												</button>
												<div className="menu-dropdown">
													<a href="#" onClick={() => editarDocumento(doc.id)}>
														<FontAwesomeIcon icon={faEdit} />
														Editar
													</a>
													<a href="#" onClick={() => editarDocumento(doc.id)}>
														<FontAwesomeIcon icon={faTrash} />
														Eliminar
													</a>
												</div>
											</div>
											<div className="document-header">
												<div className="document-icon normativa">
													<FontAwesomeIcon icon={faFileContract} />
												</div>
												<div className="document-info">
													<h3 className="document-title">{doc.titulo}</h3>
													<p className="document-meta">
														<i className="far fa-calendar-alt"></i>
														<span>{new Date(doc.fechaRegistro).toLocaleDateString("es-CR")}</span>
													</p>
												</div>
											</div>
											<div className="document-body">
												<p className="document-description">{doc.descripcion}</p>
											</div>
											<div className="document-footer">
												<div className="document-actions full-width">
													<a
														onClick={() => Download(doc.id)}
														className="btn-document btn-primary"

													>
														<FontAwesomeIcon icon={faExternalLinkAlt} />
														Ver Documento
													</a>
												</div>
											</div>
										</div>
									</div>
								))
							)}
						</div>
					</div>

					<div className="documents-section">
						<div className="section-header">
							<div className="section-title-wrapper">
								<FontAwesomeIcon icon={faFolder} className="section-icon" />
								<h2 className="section-title">Documentos Adicionales</h2>
							</div>
							<span className="section-badge">{docs.length}</span>
						</div>

						<div className="row g-4" id="documentosAdicionalesContainer">
							{docs.length === 0 ? (
								<div className="col-12">
									<div className="empty-state">
										<div className="empty-icon">
											<FontAwesomeIcon icon={faFolderOpen} />
										</div>
										<h3>No hay documentos adicionales</h3>
										<p>Los documentos que se agreguen aparecerán aquí</p>
										<button
											className="btn-add-document"
											style={{ margin: '0 auto' }}
											onClick={() => { setModalType("add"); setModalHidden(false); }}
										>
											<FontAwesomeIcon icon={faCirclePlus} />
											<span>Agregar Primer Documento</span>
										</button>
									</div>
								</div>
							) : (
								docs.map((doc) => (
									<div key={doc.id} className="col-12 col-md-6 col-xl-4">
										<div className="document-card">
											<div className="document-header">
												<div className="document-icon adicional">
													<FontAwesomeIcon icon={faFileAlt} />
												</div>
												<div className="document-info" style={{ paddingRight: '0.5rem' }}>
													<h3 className="document-title">{doc.titulo}</h3>
													<p className="document-meta">
														<FontAwesomeIcon icon={faCalendar} />
														<span>{new Date(doc.fechaRegistro).toLocaleDateString("es-CR")}</span>
													</p>
												</div>
												<div className="document-menu">
													<button className="btn-menu">
														<FontAwesomeIcon icon={faEllipsisV} />
													</button>
													<div className="menu-dropdown">
														<a href="#" onClick={() => editarDocumento(doc.id)}>
															<FontAwesomeIcon icon={faEdit} />
															Editar
														</a>
														<a href="#" onClick={() => editarDocumento(doc.id)}>
															<FontAwesomeIcon icon={faTrash} />
															Eliminar
														</a>
													</div>
												</div>
											</div>
											<div className="document-body">
												<p className="document-description">{doc.descripcion}</p>
											</div>
											<div className="document-footer">
												<div className="document-actions full-width">
													<a
														className="btn-document btn-primary"
														onClick={() => Download(doc.id)}
													>
														<FontAwesomeIcon icon={faExternalLinkAlt} />
														Obtener Documento
													</a>
												</div>
											</div>
										</div>
									</div>
								))
							)}
						</div>
					</div>
				</div>

				{!modalhidden && (
					<>
						<div className="modal fade show d-block" tabIndex="-1">
							<div className="modal-dialog modal-dialog-centered modal-md">
								<div className="modal-content">
									<div className="modal-body">
										{modalType === "add" && (
											<AddDoc onClose={() => { setModalHidden(true); cargarDatos(); }} />
										)}
										{modalType === "edit" && (
											<EditDoc doc={docModal} onClose={() => { setModalHidden(true); cargarDatos(); }} />
										)}
									</div>
								</div>
							</div>
						</div>
						<div className="modal-backdrop fade show"></div>
					</>
				)}
			</div>
		</>
	);
}