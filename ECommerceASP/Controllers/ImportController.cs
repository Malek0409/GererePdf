using System.Globalization;
using ECommerceASP.Data;
using ECommerceASP.Models;
using ECommerceASP.Services.ImportCSVProduct;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceASP.Controllers
{
    public class ImportController : Controller
    {
        private readonly PdataProduct _importer;
        private readonly ApplicationDbContext _context;


        public ImportController(PdataProduct importer, ApplicationDbContext context)
        {
            _importer = importer;
            _context = context;

        }

        [HttpGet("/import/products")]
        public async Task<IActionResult> Import()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "ProductsData.csv");

            if (!System.IO.File.Exists(filePath))
                return NotFound("Fichier CSV non trouvé.");

            var insertedProducts = new List<Product>();
            var skippedRows = new List<string>();

            using var reader = new StreamReader(filePath);
            int lineIndex = 0;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                lineIndex++;

                if (lineIndex == 1 && line.Contains("Name")) // skip header
                    continue;

                var values = line.Split(',');

                if (values.Length != 3)
                {
                    skippedRows.Add($"Ligne {lineIndex} mal formée");
                    continue;
                }

                string name = values[0].Trim('"').Trim();
                string priceStr = values[1].Trim('"').Trim();
                string categoryName = values[2].Trim('"').Trim();

                if (!decimal.TryParse(priceStr, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal price))
                {
                    skippedRows.Add($"Ligne {lineIndex} : prix invalide → '{priceStr}'");
                    continue;
                }

                var category = await _context.Categories.FirstOrDefaultAsync(c => c.name == categoryName);
                if (category == null)
                {
                    skippedRows.Add($"Ligne {lineIndex} : catégorie '{categoryName}' introuvable");
                    continue;
                }

                // Vérifie si le produit existe déjà (même nom ET même catégorie)
                var exists = await _context.Products.AnyAsync(p => p.Name == name && p.CategoryId == category.Id);
                if (exists)
                {
                    skippedRows.Add($"Ligne {lineIndex} : produit '{name}' déjà existant");
                    continue;
                }

                var product = new Product
                {
                    Name = name,
                    Price = price,
                    CategoryId = category.Id
                };

                _context.Products.Add(product);
                insertedProducts.Add(product);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Import terminé",
                Ajoutés = insertedProducts.Count,
                Ignorés = skippedRows.Count,
                DétailIgnorés = skippedRows
            });
        }
    }
}