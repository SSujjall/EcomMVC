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
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public virtual List<CartItem> CartItems { get; set; }
    }
}
