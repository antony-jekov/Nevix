namespace Jekov.Nevix.Desktop.Common
{
    using Jekov.Nevix.Desktop.Common.Models;
    using Newtonsoft.Json;
    using System;
    using System.IO;

    public class NevixLocalDbContext
    {
        private UserConfigurationsModel configFile;
        private const string ConfigFileName = "NevixConfig.dat";
        private string configFilePath;

        public NevixLocalDbContext()
        {
            string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            configFilePath = string.Format("{0}\\{1}", personalFolder, ConfigFileName);

            if (File.Exists(configFilePath))
            {
                configFile = JsonConvert.DeserializeObject<UserConfigurationsModel>(File.ReadAllText(configFilePath));
            }
            else
            {
                File.WriteAllText(configFilePath, JsonConvert.SerializeObject(new UserConfigurationsModel()));
            }
        }
    }
}