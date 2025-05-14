using ECommerceASP.Data;
using ECommerceASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;

namespace ECommerceASP.Controllers
{
    public class ProductController : Controller
    {

        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListeProduct()
        {
            var Products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();
            return View(Products);
        }

        public async Task<IActionResult> CreateProduct()
        {
            var categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.name
                })
                .ToListAsync();

            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ListeProduct));
            }

            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.name
                })
                .ToListAsync();

            return View(product);
        }

        // GET: Product/EditProduct/5
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _context.Categories
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.name
                })
                .ToListAsync();

            return View(product);
        }

        // POST: Product/EditProduct/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _context.Categories
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.name
                    })
                    .ToListAsync();

                return View(product);
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ListeProduct));
        }
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ListeProduct));
        }
        public async Task<IActionResult> GeneratePdf()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .ToListAsync();

            var document = new ProductPdfDocument(products);
            var pdfBytes = document.GeneratePdf();

            return File(pdfBytes, "application/pdf", "Liste_Produits.pdf");
        }

    }
}