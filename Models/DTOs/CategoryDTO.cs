namespace EcomSiteMVC.Models.DTOs
{
    public class AddCategoryDTO
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }

    public class UpdateCategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
