namespace Jekov.Nevix.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class MediaFile : MediaEntity
    {
        [Required]
        public long Length { get; set; }
    }
}