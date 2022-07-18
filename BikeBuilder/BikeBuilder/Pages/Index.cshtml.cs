using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data;
using ProductsAPI.Data.Enum;
using ProductsAPI.Data.Models;

namespace BikeBuilder.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ProductsDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public List<Product> Frames { get; set; }
        public List<Product> Wheels { get; set; }

        public Dictionary<string, List<string>> colors = new Dictionary<string, List<string>>();

        public IndexModel(ProductsDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task OnGetAsync()
        {
            var products = await _context.Products.ToListAsync();

            foreach (var product in products)
            {
                var bundleManifest = $"{Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "Assets", product.AssetBundle.Split("/").Last())}.manifest";
                string[] lines = System.IO.File.ReadAllLines(bundleManifest);
                List<string> colorsInBundle = new List<string>();
                foreach (var line in lines)
                {
                    if (line.StartsWith("- Assets/"))
                    {
                        colorsInBundle.Add(ParseColor(line));
                    }
                }

                colors.Add(product.Name, colorsInBundle.Distinct().ToList());
            }

            Frames = products.Where(p => p.ProductType == ProductType.Frame).ToList();
            Wheels = products.Where(p => p.ProductType == ProductType.Wheels).ToList();
        }

        private string ParseColor(string line)
        {
            var parts = line.Split('/');

            return parts[parts.Length-2];
        }
    }
}
