using Microsoft.EntityFrameworkCore;
using ProductsAPI.Data.Enum;
using System.ComponentModel.DataAnnotations;

namespace ProductsAPI.Data.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [EnumDataType(typeof(ProductType))]
        public ProductType ProductType { get; set; }
        [Required]
        public Guid PublicId { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(2048)]
        [DataType(DataType.ImageUrl)]
        public string Picture { get; set; }
        [Required]
        [MaxLength(2048)]
        [DataType(DataType.Url)]
        public string AssetBundle { get; set; }
        [Required]
        [Precision(10,2)]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
