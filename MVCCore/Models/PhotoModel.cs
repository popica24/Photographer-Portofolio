using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCore.Models
{
    public class PhotoModel
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        [NotMapped]
        [Required]
        public IFormFile? Image { get; set; }

        [ScaffoldColumn(false)]
        public string DbPath { get; set; }

        [ScaffoldColumn(false)]
        public AlbumModel? Album { get; set; }

        [ScaffoldColumn(false)]
        public string AlbumId { get; set; }

        [ScaffoldColumn(false)]
        public int Position { get; set; }

        public bool IsThumbnail { get; set; }
    }
}
