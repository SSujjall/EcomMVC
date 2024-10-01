using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcomSiteMVC.Models.Entities
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        [ForeignKey("User")]
        public int CustomerId { get; set; }

        public int TotalAmount { get; set; }

        // Navigation properties
        public User Customer { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
