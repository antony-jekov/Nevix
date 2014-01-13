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

        public string GetMd5HashCode()
        {
            return CalculateMd5HashCode(GetAllLocations());
        }

        private string CalculateMd5HashCode(string text)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(text);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0, len = hash.Length; i < len; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private string GetAllLocations()
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