namespace Jekov.Nevix.Common.Models
{
    using System.Collections.Generic;

    public class MediaFolder : MediaEntity
    {
        public MediaFolder()
        {
            this.MediaFolders = new HashSet<MediaFolder>();
            this.MediaFiles = new HashSet<MediaFile>();
        }

        public virtual ICollection<MediaFolder> MediaFolders { get; set; }

        public virtual ICollection<MediaFile> MediaFiles { get; set; }

        public int UserId { get; set; }

        public virtual NevixUser User { get; set; }
    }
}