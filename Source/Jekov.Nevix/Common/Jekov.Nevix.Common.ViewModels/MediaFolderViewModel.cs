namespace Jekov.Nevix.Common.ViewModels
{
    using Jekov.Nevix.Common.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class MediaFolderViewModel
    {
        public MediaFolderViewModel()
        {
            this.Folders = new HashSet<MediaFolderViewModel>();
            this.Files = new HashSet<MediaFileViewModel>();
        }

        [Required]
        [DataMember(Name = "name")]
        [StringLength(ModelConstants.NameLength)]
        public string Name { get; set; }

        [StringLength(ModelConstants.LocationLength)]
        [DataMember(Name = "location")]
        public string Location { get; set; }

        [DataMember(Name = "folders")]
        public ICollection<MediaFolderViewModel> Folders { get; set; }

        [DataMember(Name = "files")]
        public ICollection<MediaFileViewModel> Files { get; set; }
    }
}