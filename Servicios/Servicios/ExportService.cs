using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Abstracciones.Modelos.ModelosDto;
using Abstracciones.Servicios;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Mapster;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QRCoder;
using Border = iText.Layout.Borders.Border;
using Color = System.Drawing.Color;
using Image = iText.Layout.Element.Image;
using Paragraph = iText.Layout.Element.Paragraph;
using Rectangle = System.Drawing.Rectangle;

namespace Servicios.Servicios
{
    public class ExportService : IExportService
    {
        private static readonly Color[] PieColors =
        {
            Color.FromArgb(52, 152, 219), Color.FromArgb(46, 204, 113),
            Color.FromArgb(241, 196, 15), Color.FromArgb(231, 76, 60),
            Color.FromArgb(155, 89, 182), Color.FromArgb(52,  73,  94),
            Color.FromArgb(26, 188, 156), Color.FromArgb(230, 126,  34)
        };

        // Colores de marca reutilizados en PDF (mismos que la clase Exportar)
        private static readonly DeviceRgb ColorHeader = new DeviceRgb(41, 128, 185);
        private static readonly DeviceRgb ColorHeaderTabla = new DeviceRgb(52, 73, 94);
        private static readonly DeviceRgb ColorMetaBg = new DeviceRgb(236, 240, 241);
        private static readonly DeviceRgb ColorFilaPar = new DeviceRgb(245, 245, 245);
        private static readonly DeviceRgb ColorBorde = new DeviceRgb(220, 220, 220);
        private static readonly DeviceRgb ColorFooterBg = new DeviceRgb(250, 250, 250);
        private static readonly DeviceRgb ColorFooterBorde = new DeviceRgb(200, 200, 200);

        public ExportService()
        {
            ExcelPackage.License.SetNonCommercialOrganization("Jamal Marshall");
        }

        public byte[] ExportarListaAExcel<T>(
            IEnumerable<T> datos,
            string nombreHoja = "Datos",
            string titulo = null,
            Dictionary<string, string> encabezados = null)
        {
            var lista = datos?.ToList() ?? new List<T>();
            var propiedades = ObtenerPropiedades<T>(encabezados);

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add(nombreHoja);
            int fila = 1;

            // Título opcional con fondo azul
            if (!string.IsNullOrWhiteSpace(titulo))
            {
                ws.Cells[fila, 1, fila, propiedades.Count].Merge = true;
                EstilarTituloExcel(ws.Cells[fila, 1], titulo);
                fila += 2;
            }

            // Encabezados automáticos (PascalCase → "Pascal Case")
            for (int col = 0; col < propiedades.Count; col++)
            {
                var c = ws.Cells[fila, col + 1];
                c.Value = propiedades[col].Encabezado;
                c.Style.Font.Bold = true;
                c.Style.Font.Size = 12;
                c.Style.Font.Color.SetColor(Color.White);
                c.Style.Fill.PatternType = ExcelFillStyle.Solid;
                c.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(68, 114, 196));
                c.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                c.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                c.Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
            fila++;

            // Filas de datos
            foreach (var item in lista)
            {
                for (int col = 0; col < propiedades.Count; col++)
                {
                    var valor = propiedades[col].Propiedad.GetValue(item);
                    var c = ws.Cells[fila, col + 1];
                    c.Value = FormatearValor(valor);
                    c.Style.WrapText = true;
                    c.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    c.Style.Border.BorderAround(ExcelBorderStyle.Thin, Color.LightGray);

                    // Filas alternas
                    if (fila % 2 == 0)
                    {
                        c.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        c.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(217, 225, 242));
                    }

                    // Bool → semáforo
                    if (valor is bool b)
                    {
                        c.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        c.Style.Fill.BackgroundColor.SetColor(
                            b ? Color.FromArgb(212, 239, 223) : Color.FromArgb(250, 219, 216));
                    }
                }
                fila++;
            }

            ws.Cells.AutoFitColumns(10, 50);
            for (int r = 2; r < fila; r++) ws.Row(r).CustomHeight = false;
            ws.View.FreezePanes(2, 1);

            return package.GetAsByteArray();
        }

        public byte[] ExportarObjetoAExcel<T>(
            T dato,
            string nombreHoja = "Detalle",
            string titulo = null,
            Dictionary<string, string> encabezados = null)
        {
            var propiedades = ObtenerPropiedades<T>(encabezados);

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add(nombreHoja);
            int fila = 1;

            if (!string.IsNullOrWhiteSpace(titulo))
            {
                ws.Cells[fila, 1, fila, 2].Merge = true;
                EstilarTituloExcel(ws.Cells[fila, 1], titulo);
                fila += 2;
            }

            foreach (var prop in propiedades)
            {
                var cLabel = ws.Cells[fila, 1];
                cLabel.Value = prop.Encabezado;
                cLabel.Style.Font.Bold = true;
                cLabel.Style.Fill.PatternType = ExcelFillStyle.Solid;
                cLabel.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(189, 195, 199));
                cLabel.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                var cValor = ws.Cells[fila, 2];
                cValor.Value = FormatearValor(prop.Propiedad.GetValue(dato));
                cValor.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                fila++;
            }

            ws.Column(1).Width = 30;
            ws.Column(2).AutoFit();
            return package.GetAsByteArray();
        }

        public byte[] ExportarListaAPdf<T>(
            IEnumerable<T> datos,
            string titulo = "Reporte",
            string subtituloTabla = null,
            string rutaLogo = null,
            bool incluirGrafico = false,
            string propiedadGrafico = null,
            string tituloGrafico = "Distribución",
            Dictionary<string, string> encabezados = null)
        {
            var lista = datos?.ToList() ?? new List<T>();
            var propiedades = ObtenerPropiedades<T>(encabezados);

            using var ms = new MemoryStream();
            var pdf = new PdfDocument(new PdfWriter(ms));
            var document = new Document(pdf, PageSize.A4.Rotate());
            document.SetMargins(36, 36, 36, 36);

            var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            var regular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

            AgregarLogo(document, rutaLogo);
            AgregarBannerTitulo(document, titulo, bold);
            AgregarMetadatos(document, bold, regular, lista.Count, subtituloTabla);

            // Gráfico opcional
            if (incluirGrafico && !string.IsNullOrWhiteSpace(propiedadGrafico))
            {
                var img = new Image(ImageDataFactory.Create(
                    GenerarGraficoPastelGenerico(lista, propiedadGrafico, tituloGrafico)));
                img.ScaleToFit(380, 260)
                   .SetHorizontalAlignment(HorizontalAlignment.CENTER)
                   .SetMarginBottom(20);
                document.Add(img);
            }

            // Tabla con anchos heurísticos automáticos
            float[] anchos = propiedades.Select(p => AnchoHeuristico(p.Propiedad.Name)).ToArray();
            var tabla = new Table(UnitValue.CreatePercentArray(anchos))
                .UseAllAvailableWidth().SetMarginBottom(20);

            // Encabezados de tabla (auto)
            foreach (var prop in propiedades)
                tabla.AddHeaderCell(CeldaEncabezadoTabla(prop.Encabezado, bold));

            // Datos
            int rowIdx = 0;
            foreach (var item in lista)
            {
                bool esPar = ++rowIdx % 2 == 0;
                foreach (var prop in propiedades)
                {
                    var valor = prop.Propiedad.GetValue(item);
                    tabla.AddCell(CeldaDato(valor, regular, esPar));
                }
            }

            document.Add(tabla);
            AgregarPiePagina(document, regular);
            document.Close();
            return ms.ToArray();
        }
        public byte[] ExportarObjetoAPdf<T>(
            T dato,
            string titulo = "Reporte de Detalle",
            string rutaLogo = null,
            Dictionary<string, string> encabezados = null)
        {
            if (titulo == "Reporte de Grupo" && dato.GetType().Name == "GruposDto")
            {
                var file = reporteGrupo(dato.Adapt<GruposDto>());
                return file;

            }
            var propiedades = ObtenerPropiedades<T>(encabezados);

            using var ms = new MemoryStream();
            try
            {
                var pdf = new PdfDocument(new PdfWriter(ms));


                var document = new Document(pdf, PageSize.A4);
                document.SetMargins(40, 40, 40, 40);

                var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
                var regular = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);

                AgregarLogo(document, rutaLogo);
                AgregarBannerTitulo(document, titulo, bold);
                AgregarMetadatos(document, bold, regular, -1, null);

                var tabla = new Table(2, false).UseAllAvailableWidth();
                int idx = 0;
                foreach (var prop in propiedades)
                {
                    var valor = prop.Propiedad.GetValue(dato);
                    bool esPar = idx++ % 2 == 0;

                    tabla.AddCell(new Cell()
                        .Add(new Paragraph(prop.Encabezado).SetFont(bold).SetFontSize(10))
                        .SetBackgroundColor(ColorMetaBg)
                        .SetPadding(8)
                        .SetBorder(new SolidBorder(ColorBorde, 0.5f)));

                    tabla.AddCell(CeldaDato(valor, regular, esPar));
                }

                document.Add(tabla);
                AgregarPiePagina(document, regular);
                document.Close();
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                throw new Exception("ERROR REAL iText: " + ex.ToString());

            }
        }

        private byte[] reporteGrupo(GruposDto grupo)
        {
            using var ms = new MemoryStream();
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, iText.Kernel.Geom.PageSize.A4);
                document.SetMargins(40, 40, 40, 40);

                PdfFont bold = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
                Paragraph titulo = new Paragraph("Reporte del Grupo")
                    .SetFont(bold)
                    .SetFontSize(20)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.BLUE)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20);
                document.Add(titulo);

                // Tabla con datos del grupo
                Table infoTable = new Table(2, false).SetWidth(UnitValue.CreatePercentValue(100));

                void AddRow(string label, string value)
                {
                    infoTable.AddCell(new Cell().Add(new Paragraph(label)).SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
                    infoTable.AddCell(new Cell().Add(new Paragraph(value ?? "")));
                }

                AddRow("Nombre del Grupo", grupo.Nombre);
                AddRow("Descripción del Grupo", grupo.Descripcion);
                AddRow("Creador", grupo.creado_por ?? "");
                if (grupo.modificado_por != null)
                {
                    AddRow("Modificado por", grupo.modificado_por);

                }
                AddRow("Fecha de Creación", grupo.FechaDeCreacion.ToString("dd/MM/yyyy HH:mm"));
                if (grupo.modificado_por != null)
                {
                    AddRow("Ultima Modificacion", grupo.FechaDeModificacion?.ToString("dd/MM/yyyy HH:mm"));

                }
                AddRow("Estado", grupo.Estado ? "Activo" : "Inactivo");

                document.Add(infoTable);

                document.Add(new Paragraph("\n"));

                //Tabla con miembros

                Paragraph subtitulo = new Paragraph("Miembros del Grupo")
                    .SetFont(bold)
                    .SetFontSize(14)
                    .SetFontColor(iText.Kernel.Colors.ColorConstants.BLACK)
                    .SetTextAlignment(TextAlignment.LEFT)
                    .SetMarginBottom(10);
                document.Add(subtitulo);

                Table cursosTable = new Table(new float[] { 2, 2, 4, 3, 3 });
                cursosTable.SetWidth(UnitValue.CreatePercentValue(100));

                // Encabezados
                string[] headers = { "Nombre", "Apellido", "Email", "Teléfonos", "Identificacion" };
                foreach (var header in headers)
                {
                    cursosTable.AddHeaderCell(new Cell()
                        .Add(new Paragraph(header).SetFont(bold))
                        .SetBackgroundColor(iText.Kernel.Colors.ColorConstants.LIGHT_GRAY));
                }

                foreach (var u in grupo.Estudiantes)
                {
                    cursosTable.AddCell(new Paragraph(u.Nombre));
                    cursosTable.AddCell(new Paragraph(u.Apellido));
                    cursosTable.AddCell(new Paragraph(u.Email));
                    cursosTable.AddCell(new Paragraph(u.Identificacion.ToString()));
                }

                document.Add(cursosTable);

                document.Close();
                return ms.ToArray();
            }
        }

        public byte[] ExportarReporteQr(string contenido)
        {
            using var gen = new QRCodeGenerator();
            var data = gen.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);
            using var qr = new QRCode(data);
            using var bitmap = qr.GetGraphic(20);
            using var ms = new MemoryStream();
            bitmap.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public byte[] GenerarGraficoPastelGenerico<T>(
            IEnumerable<T> datos,
            string nombrePropiedad,
            string tituloPastel = "Distribución")
        {
            var lista = datos?.ToList() ?? new List<T>();
            var prop = typeof(T).GetProperty(nombrePropiedad,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            Dictionary<string, int> grupos;

            if (prop?.PropertyType == typeof(bool))
            {
                grupos = new Dictionary<string, int>
                {
                    { "Activo",   lista.Count(i => (bool)(prop.GetValue(i) ?? false)) },
                    { "Inactivo", lista.Count(i => !(bool)(prop.GetValue(i) ?? false)) }
                };
            }
            else if (prop != null)
            {
                grupos = lista
                    .GroupBy(i => prop.GetValue(i)?.ToString() ?? "N/A")
                    .ToDictionary(g => g.Key, g => g.Count());
            }
            else
            {
                grupos = new Dictionary<string, int> { { "Sin datos", 1 } };
            }

            return DibujarGraficoPastel(grupos, tituloPastel, lista.Count);
        }
        private static void AgregarLogo(Document doc, string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta) || !File.Exists(ruta)) return;
            try
            {
                var logo = new Image(ImageDataFactory.Create(File.ReadAllBytes(ruta)));
                logo.ScaleToFit(100, 100).SetHorizontalAlignment(HorizontalAlignment.CENTER);
                doc.Add(logo);
            }
            catch { }
        }

        private static void AgregarBannerTitulo(Document doc, string titulo, PdfFont bold)
        {
            var t = new Table(1).UseAllAvailableWidth();
            t.AddCell(new Cell()
                .Add(new Paragraph(titulo).SetFont(bold).SetFontSize(22)
                    .SetFontColor(ColorConstants.WHITE)
                    .SetTextAlignment(TextAlignment.CENTER))
                .SetBackgroundColor(ColorHeader).SetPadding(15).SetBorder(Border.NO_BORDER));
            doc.Add(t);
        }

        private static void AgregarMetadatos(Document doc, PdfFont bold, PdfFont regular,
            int totalRegistros, string seccion)
        {
            var meta = new Table(2).UseAllAvailableWidth().SetMarginTop(15).SetMarginBottom(15);

            void Fila(string label, string value)
            {
                meta.AddCell(new Cell()
                    .Add(new Paragraph(label).SetFont(bold).SetFontSize(10))
                    .SetBorder(Border.NO_BORDER).SetBackgroundColor(ColorMetaBg).SetPadding(8));
                meta.AddCell(new Cell()
                    .Add(new Paragraph(value).SetFont(regular).SetFontSize(10))
                    .SetBorder(Border.NO_BORDER).SetBackgroundColor(ColorMetaBg).SetPadding(8));
            }

            Fila("Fecha de generación:", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            if (totalRegistros >= 0)
                Fila("Total de registros:", totalRegistros.ToString());
            if (!string.IsNullOrWhiteSpace(seccion))
                Fila("Sección:", seccion);

            doc.Add(meta);
        }

        private static void AgregarPiePagina(Document doc, PdfFont regular)
        {
            var footer = new Table(1).UseAllAvailableWidth().SetMarginTop(20);
            footer.AddCell(new Cell()
                .Add(new Paragraph("Documento generado automáticamente")
                    .SetFont(regular).SetFontSize(9)
                    .SetFontColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER))
                .SetBorder(new SolidBorder(ColorFooterBorde, 1))
                .SetBackgroundColor(ColorFooterBg).SetPadding(10));
            doc.Add(footer);
        }

        private static Cell CeldaEncabezadoTabla(string texto, PdfFont bold) =>
            new Cell()
                .Add(new Paragraph(texto).SetFont(bold).SetFontSize(9)
                    .SetFontColor(ColorConstants.WHITE))
                .SetBackgroundColor(ColorHeaderTabla)
                .SetTextAlignment(TextAlignment.CENTER)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetPadding(6)
                .SetBorder(new SolidBorder(ColorConstants.WHITE, 1));

        private static Cell CeldaDato(object valor, PdfFont regular, bool esPar)
        {
            string texto = FormatearValor(valor)?.ToString() ?? "";

            var celda = new Cell()
                .Add(new Paragraph(texto).SetFont(regular).SetFontSize(9))
                .SetTextAlignment(TextAlignment.LEFT)
                .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                .SetPadding(5)
                .SetBorder(new SolidBorder(ColorBorde, 0.5f));

            if (esPar) celda.SetBackgroundColor(ColorFilaPar);

            if (valor is bool b)
            {
                celda.SetBackgroundColor(b ? new DeviceRgb(212, 239, 223) : ColorConstants.RED);
                celda.SetFontColor(b ? ColorConstants.BLACK : ColorConstants.WHITE);
            }

            return celda;
        }
        private static float AnchoHeuristico(string nombrePropiedad)
        {
            string n = nombrePropiedad.ToLower();
            if (n.Contains("id") && n != "identificacion") return 2f;
            if (n.Contains("nombre") || n.Contains("apellido")) return 1.5f;
            if (n.Contains("identificacion") || n.Contains("tipo")) return 1.2f;
            return 1f;
        }
        private static void EstilarTituloExcel(ExcelRange celda, string titulo)
        {
            celda.Value = titulo;
            celda.Style.Font.Bold = true;
            celda.Style.Font.Size = 16;
            celda.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            celda.Style.Fill.PatternType = ExcelFillStyle.Solid;
            celda.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(41, 128, 185));
            celda.Style.Font.Color.SetColor(Color.White);
        }

        private byte[] DibujarGraficoPastel(
            Dictionary<string, int> grupos, string titulo, int totalOriginal)
        {
            int w = 640, h = 420;
            using var bmp = new Bitmap(w, h);
            using var g = Graphics.FromImage(bmp);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.White);

            using var tFont = new Font("Arial", 14, FontStyle.Bold);
            g.DrawString(titulo, tFont, Brushes.Black, new PointF(w / 2f - 80, 12));

            var pieRect = new Rectangle(30, 60, 280, 280);
            float total = grupos.Values.Sum();
            float start = 0;
            int lY = 80, i = 0;

            foreach (var kv in grupos)
            {
                float sweep = total > 0 ? kv.Value / total * 360 : 0;
                var color = PieColors[i % PieColors.Length];
                using (var br = new SolidBrush(color)) g.FillPie(br, pieRect, start, sweep);
                g.DrawPie(Pens.White, pieRect, start, sweep);

                int lx = 340;
                using (var br = new SolidBrush(color)) g.FillRectangle(br, lx, lY, 18, 18);
                g.DrawRectangle(Pens.Gray, lx, lY, 18, 18);

                using var lFont = new Font("Arial", 9);
                string pct = total > 0 ? $"{kv.Value / total * 100:F1}%" : "0%";
                g.DrawString($"{kv.Key}: {kv.Value} ({pct})", lFont, Brushes.Black, lx + 25, lY + 1);

                lY += 26; start += sweep; i++;
            }

            using var sf = new Font("Arial", 10, FontStyle.Bold);
            g.DrawString($"Total: {totalOriginal}", sf, Brushes.Black, 340, lY + 10);

            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        private record PropiedadColumna(PropertyInfo Propiedad, string Encabezado);

        private static List<PropiedadColumna> ObtenerPropiedades<T>(
            Dictionary<string, string> encabezados = null)
        {
            return typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(p =>
                {
                    string label = encabezados != null && encabezados.TryGetValue(p.Name, out var alias)
                        ? alias
                        : SepararPascalCase(p.Name);
                    return new PropiedadColumna(p, label);
                })
                .ToList();
        }

        private static string SepararPascalCase(string nombre)
        {
            if (string.IsNullOrEmpty(nombre)) return nombre;
            var sb = new StringBuilder();
            foreach (char c in nombre)
            {
                if (char.IsUpper(c) && sb.Length > 0) sb.Append(' ');
                sb.Append(c);
            }
            return sb.ToString();
        }

        private static object FormatearValor(object valor)
        {
            if (valor == null)
                return "N/A";

            if (valor is System.Collections.IEnumerable enumerable && valor is not string)
            {
                var items = new List<string>();

                foreach (var item in enumerable)
                {
                    if (item == null) continue;

                    items.Add(FormatearObjeto(item));
                }

                return string.Join("\n", items);
            }

            return valor switch
            {
                bool b => b ? "Activo" : "Inactivo",
                DateTime d => d.ToString("dd/MM/yyyy"),
                Guid gu => gu.ToString()[..8] + "...",
                _ => valor.ToString()
            };
        }
        private static string FormatearObjeto(object obj)
        {
            if (obj == null) return "";

            var tipo = obj.GetType();

            if (tipo.Name == "TelefonoDto")
            {
                var codigo = tipo.GetProperty("Codigo")?.GetValue(obj);
                var numero = tipo.GetProperty("Telefono")?.GetValue(obj)?.ToString();
                var tipoTel = tipo.GetProperty("Tipo")?.GetValue(obj);
                var estado = tipo.GetProperty("Estado")?.GetValue(obj) as bool?;

                if (!string.IsNullOrEmpty(numero) && numero.Length >= 8)
                    numero = numero.Insert(4, "-");

                return $"(+{codigo}) {numero} - {tipoTel} {(estado == true ? "(Activo)" : "(Inactivo)")}";
            }

            // Genérico: intenta mostrar propiedades simples
            var props = tipo.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return string.Join(", ", props.Select(p =>
            {
                var val = p.GetValue(obj);
                return $"{p.Name}: {FormatearValor(val)}";
            }));
        }

    }
}