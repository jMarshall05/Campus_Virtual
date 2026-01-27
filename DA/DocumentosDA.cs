using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstracciones.DA;
using Abstracciones.Modelos.ModelosDA;
using Abstracciones.Modelos.ModelosDto;
using Microsoft.EntityFrameworkCore;

namespace DA
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

        public async Task EditarDocumento(DocumentosAD documento)
        {
            var documentoExistente = await _elContexto.Documentos.FindAsync(documento);

            documentoExistente.Titulo = documento.Titulo;
            documentoExistente.Descripcion = documento.Descripcion;
            documentoExistente.RutaArchivo = documento.RutaArchivo;
            documentoExistente.Categoria = documento.Categoria;
            documentoExistente.FechaRegistro = documento.FechaRegistro;
            await _elContexto.SaveChangesAsync();

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
    }
}
