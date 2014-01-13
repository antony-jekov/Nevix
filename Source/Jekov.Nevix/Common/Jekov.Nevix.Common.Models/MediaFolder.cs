namespace Jekov.Nevix.Common.Models
{
    using System.Collections.Generic;
    using System.Text;

    public class MediaFolder : MediaEntity
    {
        public MediaFolder()
        {
            this.Folders = new HashSet<MediaFolder>();
            this.Files = new HashSet<MediaFile>();
        }

        public virtual ICollection<MediaFolder> Folders { get; set; }

        public virtual ICollection<MediaFile> Files { get; set; }

        public int UserId { get; set; }

        public virtual NevixUser User { get; set; }

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