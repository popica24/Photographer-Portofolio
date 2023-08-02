using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace MVCCore.Models
{
    public class ReviewModel
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string? DbPhoto { get; set; }

        [Required]
        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
