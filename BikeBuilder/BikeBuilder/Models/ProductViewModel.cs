using System.ComponentModel.DataAnnotations;

namespace BikeBuilder.Models
{
    public class ProductViewModel
    {
        public Guid PublicId { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string AssetBundle { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
