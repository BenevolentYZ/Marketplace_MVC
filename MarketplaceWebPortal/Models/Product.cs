using System.ComponentModel.DataAnnotations;

namespace MarketplaceWebPortal.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Manufacture { get; set; }

        public string Type { get; set; }

        [Required]
        [Display(Name="Model Year")]
        [Range(1989,2022)]
        public int ModelYear { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        public int Price { get; set; }
    }
}
