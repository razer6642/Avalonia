using MyAvaloniaApp.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Linq;

namespace MyAvaloniaApp.Services;

public class PdfService
{
    static PdfService()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public string ExportarFicha(Personaje p)
    {
        var carpeta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var nombreArchivo = $"Ficha_{LimpiarNombre(p.Nombre)}.pdf";
        var ruta = Path.Combine(carpeta, nombreArchivo);

        Document.Create(documento =>
        {
            documento.Page(pagina =>
            {
                pagina.Size(PageSizes.A4);
                pagina.Margin(40);
                pagina.DefaultTextStyle(x => x.FontSize(12));

                pagina.Header().Column(col =>
                {
                    col.Item().Text("Compendio del Aventurero")
                        .FontSize(14).FontColor("#B8860B");
                    col.Item().PaddingTop(4).LineHorizontal(1).LineColor("#B8860B");
                });

                pagina.Content().PaddingVertical(20).Column(col =>
                {
                    col.Spacing(10);

                    col.Item().Text(p.Nombre).FontSize(28).Bold().FontColor("#5A3E1B");
                    col.Item().Text($"{p.Raza} · {p.Clase}").FontSize(14);
                    col.Item().Text($"Nivel {p.Nivel}").FontSize(14).FontColor("#555555");

                    col.Item().PaddingTop(10).Text("Habilidades")
                        .FontSize(16).Bold().FontColor("#5A3E1B");
                    AgregarLista(col, p.Habilidades, "Sin habilidades cargadas.");

                    col.Item().PaddingTop(10).Text("Hechizos")
                        .FontSize(16).Bold().FontColor("#5A3E1B");
                    AgregarLista(col, p.Hechizos, "Sin hechizos cargados.");
                });

                pagina.Footer().AlignCenter().Text(texto =>
                {
                    texto.Span($"Generado el {DateTime.Now:dd/MM/yyyy}")
                        .FontSize(9).FontColor("#555555");
                });
            });
        }).GeneratePdf(ruta);

        return ruta;
    }

    private static void AgregarLista(ColumnDescriptor col, System.Collections.Generic.List<string> items, string vacio)
    {
        if (!items.Any())
        {
            col.Item().Text(vacio).Italic().FontColor("#777777");
            return;
        }

        foreach (var item in items)
            col.Item().Text($"• {item}");
    }

    private static string LimpiarNombre(string nombre)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
            nombre = nombre.Replace(c, '_');
        return nombre.Replace(' ', '_');
    }
}