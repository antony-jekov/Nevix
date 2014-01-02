namespace Jekov.Nevix.Desktop.TestClient
{
    using Jekov.Nevix.Desktop.Common;
    using System;

    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            string userEmail = "troty@abv.bg";
            string userPass = "pass";

            NevixLocalDbContext db = new NevixLocalDbContext();
            PersisterManager persister = new PersisterManager();

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
                persister.UpdateChannelName(Environment.MachineName);
            }

            FileManager fileManager = new FileManager();

            if (string.IsNullOrEmpty(db.LocalDb.BsPlayerLocation))
            {
                string playerLocation = fileManager.FindBsPlayerLocation();
                if (string.IsNullOrEmpty(playerLocation))
                {
                    playerLocation = fileManager.AskBsPlayerLocation();
                }

                if (string.IsNullOrEmpty(playerLocation) || !playerLocation.EndsWith("bsplayer.exe"))
                {
                    throw new ArgumentNullException("bsplayer", "Make sure BSPlayer is installed on this system and restart the application!");
                }

                db.LocalDb.BsPlayerLocation = playerLocation;
                db.SaveChanges();
            }

            PlayerManager player = new PlayerManager(db.LocalDb.BsPlayerLocation);

            CommunicationsManager listener = new CommunicationsManager(Environment.MachineName, player);

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}