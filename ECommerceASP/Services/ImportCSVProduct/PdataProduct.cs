using ECommerceASP.Data;
using ECommerceASP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.FileIO;

namespace ECommerceASP.Services.ImportCSVProduct
{
    public class PdataProduct
    {
        private readonly ApplicationDbContext _context;

        public PdataProduct(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> ImportProductsFromCsvAsync(string filePath)
        {
            var insertedProducts = new List<Product>();

            using var parser = new TextFieldParser(filePath);
            parser.SetDelimiters(",");
            parser.HasFieldsEnclosedInQuotes = true;

            parser.ReadLine(); // skip headers

            while (!parser.EndOfData)
            {
                var fields = parser.ReadFields();
                if (fields == null || fields.Length < 3)
                    continue;

                var name = fields[0].Trim('"');
                var priceText = fields[1].Trim('"');
                var categoryName = fields[2].Trim('"');

                if (!decimal.TryParse(priceText, out var price))
                    continue;

                // Check or create category
                var category = await _context.Categories.FirstOrDefaultAsync(c => c.name == categoryName);
                if (category == null)
                {
                    category = new Category { name = categoryName };
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync(); // get category ID
                }

                // Avoid duplicates
                if (await _context.Products.AnyAsync(p => p.Name == name))
                    continue;

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
            return insertedProducts;
        }
    }
}
