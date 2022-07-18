using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductsAPI.Data;
using ProductsAPI.Data.Models;

namespace BikeBuilder.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly ProductsDbContext _context;

        public CreateModel(ProductsDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        [BindProperty]
        public IFormFile AssetBundlde { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }



            Product.PublicId = Guid.NewGuid();

            await _context.Products.AddAsync(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
