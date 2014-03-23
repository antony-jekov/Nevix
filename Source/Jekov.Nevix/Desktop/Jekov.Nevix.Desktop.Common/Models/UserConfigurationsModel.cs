﻿namespace Jekov.Nevix.Desktop.Common.Models
{
    using Jekov.Nevix.Common.ViewModels;
    using System;
    using System.Collections.Generic;

    public class UserConfigurationsModel
    {
        public string SessionKey { get; set; }

        public string Email { get; set; }

        public string BsPlayerLocation { get; set; }

        public bool Remember { get; set; }

        public string Password { get; set; }

        public ICollection<MediaFolderViewModel> MediaFolders { get; set; }

        public ICollection<String> MediaFolderLocations { get; set; }

        public IDictionary<int, string> Files { get; set; }

        public UserConfigurationsModel()
        {
            ClearMedia();
        }

        public void ClearMedia()
        {
            this.MediaFolderLocations = new List<String>();
            this.MediaFolders = new List<MediaFolderViewModel>();
            this.Files = new Dictionary<int, string>();
        }
    }
}