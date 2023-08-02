using MVCCore.Models.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCore.Models
{
    public class CoverModel
    {
        [ScaffoldColumn(false)]
        public string ID { get; set; }
        
        public Category Category { get; set; }
        public string DbPath { get; set; }

        [NotMapped]
        public IFormFile Thumbnail { get; set; }
    }
}
