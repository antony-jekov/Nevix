namespace Jekov.Nevix.Common.ViewModels
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class MediaFolderMobileViewModel
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "folders")]
        public ICollection<MediaFolderMobileViewModel> Folders { get; set; }

        [DataMember(Name = "files")]
        public ICollection<MediaFileMobileViewModel> Files { get; set; }
    }
}