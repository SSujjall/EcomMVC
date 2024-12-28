using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcomSiteMVC.Core.Models.Entities
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }

        [ForeignKey("Cart")]
        public int CartId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }

        // Navigation properties
        public virtual Cart Cart { get; set; }
        public virtual Product Product { get; set; }
    }
}
