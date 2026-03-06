using Abstracciones.Modelos.ModelosDto;
using DA.Entidades;
using DA.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace DA.Implementaciones
{
    public class DocumentosDA : IDocumentosAD
    {
        private readonly ApplicationDbContext _elContexto;
        public DocumentosDA(ApplicationDbContext elContexto)
        {
            _elContexto = elContexto;
        }
        public async Task<int> AgregarDocumento(DocumentosAD documento)
        {
            var entidad = await _elContexto.Documentos.AddAsync(documento);
            await _elContexto.SaveChangesAsync();
            return entidad.Entity.Id;
        }

        public async Task BorrarDocumento(int idDocumento)
        {
            await _elContexto.Documentos.Where(d => d.Id == idDocumento).ExecuteDeleteAsync();
            await _elContexto.SaveChangesAsync();
        }

        public async Task<bool> EditarDocumento(int id, DocumentosAD documento)
        {
            var documentoExistente = await _elContexto.Documentos.FindAsync(id);

            documentoExistente.Titulo = documento.Titulo;
            documentoExistente.Descripcion = documento.Descripcion;
            documentoExistente.RutaArchivo = documento.RutaArchivo;
            documentoExistente.Categoria = documento.Categoria;
            documentoExistente.FechaRegistro = documento.FechaRegistro;
            var result = await _elContexto.SaveChangesAsync();
            return result != 0;

        }

        public async Task<IEnumerable<DocumentosDto>> ListarDocumentos()
        {
            var documentos = await _elContexto.Documentos.Select(doc => new DocumentosDto
            {
                Id = doc.Id,
                Titulo = doc.Titulo,
                Descripcion = doc.Descripcion,
                RutaArchivo = doc.RutaArchivo,
                Categoria = doc.Categoria,
                FechaRegistro = doc.FechaRegistro
            }).ToListAsync();
            return documentos;
        }

        public async Task<DocumentosDto> ObtenerDocumento(int Id)
        {
            var doc = await _elContexto.Documentos.FindAsync(Id);
            return doc.Adapt<DocumentosDto>();
        }
    }
}
