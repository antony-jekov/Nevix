namespace Jekov.Nevix.Desktop.TestClient
{
    using Jekov.Nevix.Common.ViewModels;
    using Jekov.Nevix.Desktop.Common;
    using Jekov.Nevix.Desktop.Common.Contracts;
    using Jekov.Nevix.Desktop.Common.Players;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal class Program
    {
        private static void Main()
        {
            string userEmail = "troty@abv.bg";
            string userPass = "pass";

            NevixLocalDbContext db = NevixLocalDbContext.Instance();
            PersisterManager persister = PersisterManager.Instance();
            FileManager fileManager = FileManager.Instance();

            if (!string.IsNullOrEmpty(db.LocalDb.SessionKey))
            {
                persister.SessionKey = db.LocalDb.SessionKey;
            }
            else
            {
                db.LocalDb.SessionKey = persister.Login(userEmail, userPass);
                db.SaveChanges();
            }

            string channelName = persister.GetChannelName();

            if (string.IsNullOrEmpty(channelName) || channelName != Environment.MachineName)
            {
                var allDownloadFolders = fileManager.GetAllDownloadDirectories(fileManager.GetAllDrives());
                List<MediaFolderViewModel> foldersConverted = new List<MediaFolderViewModel>();
                MediaFolderViewModel rootFolder;
                foreach (var folder in allDownloadFolders)
                {
                    rootFolder = fileManager.ConvertToMediaFolderViewModel(folder);

                    if (rootFolder.Files.Any() || rootFolder.Folders.Any())
                    {
                        foldersConverted.Add(rootFolder);
                    }
                }

                foreach (var convertedFolder in foldersConverted)
                {
                    persister.AddMediaFolderToDatabase(convertedFolder);
                }

                persister.UpdateChannelName(Environment.MachineName);
            }
            else
            {
                DateTime lastDatabaseUpdate = persister.LastMediaUpdate();
            }

            if (string.IsNullOrEmpty(db.LocalDb.BsPlayerLocation))
            {
                string playerLocation = fileManager.FindBsPlayerLocation();
                if (string.IsNullOrEmpty(playerLocation))
                {
                }

                if (string.IsNullOrEmpty(playerLocation) || !playerLocation.EndsWith("bsplayer.exe"))
                {
                    throw new ArgumentNullException("bsplayer", "Make sure BSPlayer is installed on this system and restart the application!");
                }

                db.LocalDb.BsPlayerLocation = playerLocation;
                db.SaveChanges();
            }

            IPlayer player = new BsPlayer(db.LocalDb.BsPlayerLocation);

            CommunicationsManager listener = new CommunicationsManager(Environment.MachineName, new CommandExecutor(player, db.LocalDb.Files));

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}