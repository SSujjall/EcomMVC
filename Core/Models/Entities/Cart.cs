using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcomSiteMVC.Core.Models.Entities
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [ForeignKey("User")]
        public int CustomerId { get; set; }

        // Navigation properties
        public virtual User Customer { get; set; }
        public virtual List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
