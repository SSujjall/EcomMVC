using System.ComponentModel.DataAnnotations;

namespace EcomSiteMVC.Models.Entities
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }

        // Navigation properties
        public ICollection<Product> Products { get; set; }
    }
}
