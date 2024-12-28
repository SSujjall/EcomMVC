using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcomSiteMVC.Core.Models.Entities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int StockQuantity { get; set; }
        public DateOnly CreatedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly LastUpdatedDate { get; set; }

        // Navigation properties
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
    }
}
