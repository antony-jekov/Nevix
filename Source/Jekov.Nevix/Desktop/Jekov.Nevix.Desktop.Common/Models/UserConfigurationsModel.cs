namespace Jekov.Nevix.Desktop.Common.Models
{
    using Jekov.Nevix.Common.ViewModels;
    using System;
    using System.Collections.Generic;

    public class UserConfigurationsModel
    {
        public string SessionKey { get; set; }

        public string Email { get; set; }

        public string PreferredPlayerLocation { get; set; }

        public bool Remember { get; set; }

        public string Password { get; set; }

        public ICollection<MediaFolderViewModel> MediaFolders { get; set; }

        public ICollection<String> MediaFolderLocations { get; set; }

        public IDictionary<int, string> Filesz { get; set; }

        public bool StartWithWindows { get; set; }

        public ICollection<PlayerEntry> CustomlyAddedPlayers { get; set; }

        public UserConfigurationsModel()
        {
            ClearMedia();
        }

        public void ClearMedia()
        {
            this.MediaFolders = new List<MediaFolderViewModel>();
            this.Filesz = new Dictionary<int, string>();
            this.MediaFolderLocations = new List<string>();
            this.CustomlyAddedPlayers = new List<PlayerEntry>();
        }
    }
}