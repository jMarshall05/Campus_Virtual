using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstracciones.Servicios
{
    public interface IExportService
    {
        byte[] ExportarListaAExcel<T>(
            IEnumerable<T> datos,
            string nombreHoja = "Datos",
            string titulo = null,
            Dictionary<string, string> encabezados = null); byte[] ExportarObjetoAExcel<T>(
            T dato,
            string nombreHoja = "Detalle",
            string titulo = null,
            Dictionary<string, string> encabezados = null); byte[] ExportarListaAPdf<T>(
            IEnumerable<T> datos,
            string titulo = "Reporte",
            string subtituloTabla = "Listado",
            string rutaLogo = null,
            bool incluirGrafico = false,
            string propiedadGrafico = null,
            string tituloGrafico = "Distribución",
            Dictionary<string, string> encabezados = null);
        byte[] ExportarObjetoAPdf<T>(
            T dato,
            string titulo = "Reporte de Detalle",
            string rutaLogo = null,
            Dictionary<string, string> encabezados = null);
        byte[] ExportarReporteQr(string contenido);
        byte[] GenerarGraficoPastelGenerico<T>(
            IEnumerable<T> datos,
            string nombrePropiedad,
            string tituloPastel = "Distribución");
    }
}

