namespace Jekov.Nevix.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MediaFile : MediaEntity
    {
        [StringLength(ModelConstants.LocationLength)]
        public string Location { get; set; }

        [Required]
        public long Length { get; set; }
    }
}