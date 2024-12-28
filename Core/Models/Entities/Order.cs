using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using EcomSiteMVC.Core.Enums;

namespace EcomSiteMVC.Core.Models.Entities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey("User")]
        public int CustomerId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int TotalAmount { get; set; }
        public Status Status { get; set; } // Pending, Shipped, Delivered, Cancelled
        public string ShippingAddress { get; set; }

        // Navigation properties
        public virtual User Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
