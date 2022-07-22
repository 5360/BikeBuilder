using ProductsAPI.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace BikeBuilder.Models
{
    public class CreateProductViewModel
    {
        [Required]
        public ProductType ProductType { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(30)]
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
