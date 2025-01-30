using EcomSiteMVC.Core.Enums;
using EcomSiteMVC.Core.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomSiteMVC.Core.DTOs
{
    public class PlaceOrderDTO
    {
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int TotalAmount { get; set; }
        public Status Status { get; set; } = Status.Pending; // Pending, Shipped, Delivered, Cancelled
        public string ShippingAddress { get; set; }
    }
}
