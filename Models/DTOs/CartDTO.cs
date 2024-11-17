namespace EcomSiteMVC.Models.DTOs
{
    public class AddToCartDTO
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
