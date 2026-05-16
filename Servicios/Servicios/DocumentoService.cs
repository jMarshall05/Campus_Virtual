using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using DA.Entidades;
using DA.Interfaces;
using Mapster;
using Reglas;

namespace Servicios.Servicios
{
    public class DocumentoService : IDocumentoService
    {
        private readonly IDocumentosAD _documentos;
        private readonly IFileStorageService _fileStorage;
        public DocumentoService(IDocumentosAD documentos, IFileStorageService fileStorage)
        {
            _documentos = documentos;
            _fileStorage = fileStorage;
        }
        public async Task<int> AgregarDocumento(DocumentosDto documento)
        {
            return await _documentos.AgregarDocumento(documento.Adapt<DocumentosAD>());
        }

        public async Task BorrarDocumento(int idDocumento, DocumentosDto doc)
        {
            DocumentosReglas.ExisteDoc(doc != null);
            if (doc.RutaArchivo != null)
                await _fileStorage.DeleteAsync(doc.RutaArchivo);
            await _documentos.BorrarDocumento(idDocumento);
        }

        public async Task<DocumentosDto> DescargarDocumento(int Id)
        {
            var doc = await _documentos.ObtenerDocumento(Id);
            DocumentosReglas.ExisteDoc(doc != null);
            return doc;
        }

        public async Task<bool> EditarDocumento(int idDocumento, DocumentosDto documento)
        {
            var doc = await _documentos.ObtenerDocumento(idDocumento);
            DocumentosReglas.ExisteDoc(doc != null);
            if (documento.Doc != null)
            {
                var url = await _fileStorage.SaveAsync(documento.Doc, "Docs",true);
                documento.RutaArchivo = url;
            }
            return await _documentos.EditarDocumento(idDocumento, documento.Adapt<DocumentosAD>());
        }

        public async Task<IEnumerable<DocumentosDto>> ListarDocumentos()
        {
            return await _documentos.ListarDocumentos();
        }
        public async Task<DocumentosDto> ObtenerDocumento(int Id)
        {
            var doc = await _documentos.ObtenerDocumento(Id);
            return doc;
        }

    }
}
