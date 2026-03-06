using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public DocumentoService(IDocumentosAD documentos)
        {
            _documentos = documentos;
        }
        public async Task<int> AgregarDocumento(DocumentosDto documento)
        {
            return await _documentos.AgregarDocumento(documento.Adapt<DocumentosAD>());
        }

        public async Task BorrarDocumento(int idDocumento)
        {
           await _documentos.BorrarDocumento(idDocumento);
        }

        public async Task<DocumentosDto> DescargarDocumento(int Id)
        {
            var doc =await _documentos.ObtenerDocumento(Id);
            DocumentosReglas.ExisteDoc(doc!=null);
            return doc;
        }

        public async Task<bool> EditarDocumento(int idDocumento, DocumentosDto documento)
        {
            return await _documentos.EditarDocumento(idDocumento, documento.Adapt<DocumentosAD>());
        }

        public async Task<IEnumerable<DocumentosDto>> ListarDocumentos()
        {
            return await _documentos.ListarDocumentos();
        }
    }
}
