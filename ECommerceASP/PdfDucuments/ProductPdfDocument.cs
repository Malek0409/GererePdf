using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ECommerceASP.Models;
using System;
using System.Collections.Generic;

public class ProductPdfDocument : IDocument
{
    private readonly List<Product> _products;

    public ProductPdfDocument(List<Product> products)
    {
        _products = products;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Margin(30);
            page.Size(PageSizes.A4);

            // ✅ Utilisation correcte de FontSize avec Element
            page.Header().Element(e => e.Text("Liste des produits")
                                        .FontSize(20)
                                        .Bold()
                                        .FontColor(Colors.Blue.Medium));

            page.Content().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                // ✅ En-têtes
                table.Header(header =>
                {
                    header.Cell().Element(CellStyle).Text("Nom");
                    header.Cell().Element(CellStyle).Text("Prix (€)");
                    header.Cell().Element(CellStyle).Text("Catégorie");
                });

                // ✅ Données
                foreach (var product in _products)
                {
                    table.Cell().Element(CellStyle).Text(product.Name);
                    table.Cell().Element(CellStyle).Text($"{product.Price:0.00}");
                    table.Cell().Element(CellStyle).Text(product.Category?.name ?? "Aucune");
                }

                // ✅ Style cellule
                IContainer CellStyle(IContainer container) => container
                    .Border(1)
                    .Padding(5)
                    .AlignLeft()
                    .DefaultTextStyle(x => x.FontSize(12)); // <== correct ici
            });

            // ✅ Footer bien formaté
            page.Footer().AlignCenter().Text(txt =>
            {
                txt.Span("Document généré le ").FontSize(10);
                txt.Span($"{DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(10).SemiBold();
            });
        });
    }
}
