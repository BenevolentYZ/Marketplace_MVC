using System.ComponentModel.DataAnnotations;

namespace MarketplaceWebPortal.Models
{
    public class SearchViewModel
    {
        [Required]
        public string Keyword { get; set; }

        [Required]
        public string Type { get; set; }
    }
}
