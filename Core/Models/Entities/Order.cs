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

        public string FullName { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int TotalOrderAmount { get; set; }
        public string OrderStatus { get; set; } // Pending, Shipped, Delivered, Cancelled
        public string PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual User Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
