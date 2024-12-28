namespace EcomSiteMVC.Core.DTOs
{
    public class AddProductDTO
    {
        public int CategoryId { get; set; }
        public string? ProductName { get; set; }
        public string? Description { get; set; }
        public int Price { get; set; }
        public int StockQuantity { get; set; }
        public List<IFormFile>? Images { get; set; }
    }

    public class UpdateProductDTO
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public int StockQuantity { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
