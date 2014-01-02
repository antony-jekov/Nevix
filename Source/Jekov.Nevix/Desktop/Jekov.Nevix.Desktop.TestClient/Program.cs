namespace Jekov.Nevix.Desktop.TestClient
{
    using Jekov.Nevix.Common.ViewModels;
    using Jekov.Nevix.Desktop.Common;
    using System;
    using System.Collections.Generic;

    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            string userEmail = "troty@abv.bg";
            string userPass = "pass";

            int width = Console.LargestWindowWidth >> 1;
            int height = Console.LargestWindowHeight >> 1;

            Console.WindowHeight = height;
            Console.BufferHeight = height;

            Console.WindowWidth = width;
            Console.BufferWidth = width;

            Console.CursorVisible = false;

            Console.WriteLine("Nevix Remote Application v 0.3");

            CommunicationsManager comManager = new CommunicationsManager("nevena");
            PersisterManager persister = new PersisterManager();
            persister.Register(userEmail, userPass, userPass);
            persister.Login(userEmail, userPass);
            FileManager fileManager = new FileManager();
            var dd = fileManager.GetAllDownloadDirectories(fileManager.GetAllDrives());

            List<MediaFolderViewModel> folders = new List<MediaFolderViewModel>();

            foreach (var folder in dd)
            {
                folders.Add(fileManager.ConvertToMediaFolderViewModel(folder));
            }

            Console.WriteLine();
            foreach (var folder in folders)
            {
                persister.SendMediaDatabase(folder);
            }

            while (true)
            {
                Console.ReadLine();
            }
        }
    }
}