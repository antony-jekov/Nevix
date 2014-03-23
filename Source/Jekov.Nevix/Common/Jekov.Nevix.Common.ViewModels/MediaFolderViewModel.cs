namespace Jekov.Nevix.Common.ViewModels
{
    using Jekov.Nevix.Common.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Security.Cryptography;
    using System.Text;

    [DataContract]
    public class MediaFolderViewModel
    {
        public MediaFolderViewModel()
        {
            this.Folders = new List<MediaFolderViewModel>();
            this.Files = new List<MediaFileViewModel>();
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
        
        public string GetAllLocations()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.Location);
            foreach (var file in this.Files)
            {
                sb.Append(file.Location);
            }
            foreach (var subFolder in this.Folders)
            {
                sb.Append(subFolder.GetAllLocations());
            }

            return sb.ToString();
        }
    }
}