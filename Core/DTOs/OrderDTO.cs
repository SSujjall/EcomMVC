using EcomSiteMVC.Core.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomSiteMVC.Core.DTOs
{
    public class PlaceOrderDTO
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string ZipCode { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class OrderDetailsResponseDTO
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string FullName { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string ShippingAddress { get; set; }
        public int TotalOrderAmount { get; set; }
        public List<OrderItemDTO> OrderDetails { get; set; }
    }

    public class OrderItemDTO
    {
        public string ProductName { get; set; }
        public string Quantity { get; set; }
        public int UnitPrice { get; set; }
        public string TotalAmount { get; set; }
    }
}
