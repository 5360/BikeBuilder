using BikeBuilder.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductsAPI.Data;
using ProductsAPI.Data.Models;

namespace BikeBuilder.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly ProductsDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(ProductsDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public CreateProductViewModel ProductVM { get; set; }

        private Product Product { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        [BindProperty]
        public IFormFile[] AssetBundle { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (AssetBundle.Length != 2)
            {
                ModelState.AddModelError("AssetBundle", "Please upload the asset bundle and manifest");
                return Page();
            }

            IFormFile? bundle = null;
            IFormFile? manifest = null;

            foreach (var file in AssetBundle)
            {
                if (file.FileName.Split('.').Count() == 1)
                {
                    bundle = file;
                    continue;
                }
                else if (file.FileName.Split('.').Last().Equals("manifest"))
                {
                    manifest = file;
                }
                else
                {
                    ModelState.AddModelError("AssetBundle", "Please upload the asset bundle and manifest");
                    return Page();
                }
            }

            if (bundle == null || manifest == null)
            {
                ModelState.AddModelError("AssetBundle", "Please upload the asset bundle and manifest");
                return Page();
            }

            Product = new Product()
            {
                PublicId = Guid.NewGuid(),
                Picture = Image.FileName,
                AssetBundle = bundle.FileName,
                Quantity = 100,
                Name = ProductVM.Name,
                ProductType = ProductVM.ProductType,
                Price = ProductVM.Price,
            };

            var imageUploadPath = Path.Combine(_environment.WebRootPath, "uploads/Images", Image.FileName);
            using (var stream = new FileStream(imageUploadPath, FileMode.Create))
            {
                await Image.CopyToAsync(stream);
            }


            var bundlePath = Path.Combine(_environment.WebRootPath, "uploads/Assets", bundle.FileName);
            using (var stream = new FileStream(bundlePath, FileMode.Create))
            {
                await bundle.CopyToAsync(stream);
            }

            var manifestPath = Path.Combine(_environment.WebRootPath, "uploads/Assets", manifest.FileName);
            using (var stream = new FileStream(manifestPath, FileMode.Create))
            {
                await manifest.CopyToAsync(stream);
            }


            await _context.Products.AddAsync(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
