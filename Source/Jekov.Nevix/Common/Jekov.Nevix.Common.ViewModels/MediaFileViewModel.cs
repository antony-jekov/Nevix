namespace Jekov.Nevix.Common.ViewModels
{
    using Jekov.Nevix.Common.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    public class MediaFileViewModel
    {
        [Required]
        [DataMember(Name = "name")]
        [StringLength(ModelConstants.NameLength)]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "location")]
        [StringLength(ModelConstants.LocationLength)]
        public string Location { get; set; }

        [Required]
        [DataMember(Name = "length")]
        public long Length { get; set; }
    }
}