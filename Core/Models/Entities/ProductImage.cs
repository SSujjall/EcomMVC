using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcomSiteMVC.Core.Models.Entities
{
    public class ProductImage
    {
        [Key]
        public int ImageId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public string ImageUrl { get; set; }

        // Navigation property
        public virtual Product Product { get; set; }
    }
}
