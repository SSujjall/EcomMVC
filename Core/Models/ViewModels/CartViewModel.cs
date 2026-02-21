namespace EcomSiteMVC.Core.Models.ViewModels
{
    public class CartViewModel
    {
        public int CustomerId { get; set; }
        public List<CartItemViewModel> CartItems { get; set; } = new();
    }

    public class CartProductImageViewModel
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
    }

    public class CartProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Price { get; set; }

        public List<CartProductImageViewModel> Images { get; set; } = new();
    }

    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int UnitPrice { get; set; }

        public CartProductViewModel Product { get; set; }
    }
}
