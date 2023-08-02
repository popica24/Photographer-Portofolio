using MVCCore.Models.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCore.Models
{
    public class AlbumModel
    {
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        public string Name { get; set; }    

        public Category Category { get; set; }

        [NotMapped]
        public IFormFile Thumbnail { get; set; }

        [NotMapped]
        public ICollection<IFormFile> Images { get; set; }

        [ScaffoldColumn(false)]
        public ICollection<PhotoModel> DbPhotos { get; set; }
    }
}
