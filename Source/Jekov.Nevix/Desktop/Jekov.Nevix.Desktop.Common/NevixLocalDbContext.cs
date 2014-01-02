namespace Jekov.Nevix.Desktop.Common
{
    using Jekov.Nevix.Desktop.Common.Models;
    using Newtonsoft.Json;
    using System;
    using System.IO;

    public class NevixLocalDbContext
    {
        public UserConfigurationsModel LocalDb { get; protected set; }

        private const string ConfigFileName = "NevixConfig.dat";
        private string configFilePath;

        public NevixLocalDbContext()
        {
            string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            configFilePath = string.Format("{0}\\{1}", personalFolder, ConfigFileName);

            if (DatabaseExists())
            {
                LocalDb = JsonConvert.DeserializeObject<UserConfigurationsModel>(File.ReadAllText(configFilePath));
            }
            else
            {
                LocalDb = new UserConfigurationsModel();
                SaveChanges();
            }
        }

        private bool DatabaseExists()
        {
            return File.Exists(configFilePath);
        }

        public void SaveChanges()
        {
            if (DatabaseExists())
            {
                WriteDataToDatabase();
            }
        }

        private void WriteDataToDatabase()
        {
            File.WriteAllText(configFilePath, JsonConvert.SerializeObject(LocalDb));
        }
    }
}