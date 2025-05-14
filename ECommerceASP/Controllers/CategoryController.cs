using ECommerceASP.Data;
using ECommerceASP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceASP.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> ListeCategorie()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {

                await _context.Categories.AddAsync(category);
                await _context.SaveChangesAsync();

                return RedirectToAction("ListeCategorie");
            }

            return View(category);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("ListeCategorie");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Categories.Any(c => c.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(category);
        }
        // GET: Category/Delete/5
public async Task<IActionResult> Delete(int id)
{
    var category = await _context.Categories.FindAsync(id);
    if (category == null)
    {
        return NotFound();
    }
    return View(category);
}

// POST: Category/Delete/5
[HttpPost, ActionName("Delete")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> DeleteConfirmed(int id)
{
    var category = await _context.Categories.FindAsync(id);
    if (category != null)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }

    return RedirectToAction(nameof(ListeCategorie));
}
     
    }
}
