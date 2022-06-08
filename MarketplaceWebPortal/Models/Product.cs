using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        [Required(ErrorMessage = "Please enter file name")]
        public string FileName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please select file")]
        public IFormFile File { get; set; }
    }
}
