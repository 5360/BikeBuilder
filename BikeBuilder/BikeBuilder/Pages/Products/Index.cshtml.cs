using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data;
using ProductsAPI.Data.Models;

namespace BikeBuilder.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly ProductsDbContext _context;

        public IndexModel(ProductsDbContext context)
        {
            _context = context;
        }

        public List<Product> Products { get; set; }

        public async Task OnGetAsync()
        {
            Products = await _context.Products.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var productToDelete = Products.Single(p => p.PublicId == id);

            _context.Products.Remove(productToDelete);
            await _context.SaveChangesAsync();

            Products.Remove(productToDelete);

            return Page();
        }
    }
}
