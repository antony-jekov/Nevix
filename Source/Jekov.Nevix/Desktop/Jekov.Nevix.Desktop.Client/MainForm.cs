using Jekov.Nevix.Common.ViewModels;
using Jekov.Nevix.Desktop.Common;
using Jekov.Nevix.Desktop.Common.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Jekov.Nevix.Desktop.Client
{
    public partial class MainForm : Form
    {
        private readonly string userEmail;
        private readonly string sessionKey;
        private NevixLocalDbContext db;
        private PersisterManager persister;
        private CommunicationsManager listener;
        private FileManager fileManager;
        private string bsPlayerLocation;

        private IEnumerable<MediaFolderViewModel> serverFolders;

        public MainForm(string email, string sessionKey)
        {
            this.userEmail = email;
            this.sessionKey = sessionKey;
            this.db = new NevixLocalDbContext();
            this.fileManager = new FileManager();
            this.persister = new PersisterManager(sessionKey);

            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            connect.Select();
            email.Text = this.userEmail;

            if (!string.IsNullOrEmpty(db.LocalDb.BsPlayerLocation))
            {
                bsPlayerLocation = db.LocalDb.BsPlayerLocation;
            }
            else
            {
                bsPlayerLocation = fileManager.FindBsPlayerLocation();
                if (string.IsNullOrEmpty(bsPlayerLocation))
                {
                    MessageBox.Show("BsPlayer could not be found.\n\nPlease choose the location of the bsplayer.exe file manually.");
                }
            }

            playerLocation.Text = bsPlayerLocation;

            string channelName = persister.GetChannelName();

            if (channelName != Environment.MachineName)
            {
                persister.UpdateChannelName(Environment.MachineName);
            }

            SyncMedia();
        }

        private void SyncMedia()
        {
            string serverFolders = persister.GetAllFolders();

            var locals = LoadMediaFolders(serverFolders);

            string localFolders = fileManager.Serialize(locals);//.Replace("\\\\", "\\");
            
            if (serverFolders.Length > 2 && serverFolders.Equals(localFolders))
            {
                mediaDirectories.Items.Clear();
                mediaDirectories.Items.AddRange(db.LocalDb.MediaFolderLocations.ToArray());
            }
            else
            {
                if (!db.LocalDb.MediaFolderLocations.Any())
                {
                    if (MessageBox.Show("There are no media folders currently set.\n\nWould you like to add them automatically?", "Media folders not set", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Thread initThread = new Thread(() => InitMedia());
                        initThread.Start();
                    }
                    else
                    {
                        MessageBox.Show("Please add folders manually.");
                    }
                }
                else
                {
                    persister.ClearAllMedia();
                    db.LocalDb.ClearMedia();
                    mediaDirectories.Items.Clear();

                    MediaFolderViewModel createdFolder;
                    foreach (var folder in locals)
                    {
                        createdFolder = persister.AddMediaFolderToDatabase(folder);

                        db.LocalDb.MediaFolderLocations.Add(createdFolder.Location);
                        db.LocalDb.MediaFolders.Add(createdFolder);
                        mediaDirectories.Items.Add(createdFolder.Location);

                        AddFilesToLocalDb(createdFolder);
                    }

                    db.SaveChanges();
                }
            }
        }

        private void AddFilesToLocalDb(MediaFolderViewModel createdFolder)
        {
            foreach (var file in createdFolder.Files)
            {
                db.LocalDb.Files.Add(file.Id, file.Location);
            }

            foreach (var subFolder in createdFolder.Folders)
            {
                AddFilesToLocalDb(subFolder);
            }
        }

        private IEnumerable<MediaFolderViewModel> LoadMediaFolders(string serverFolders)
        {
            List<MediaFolderViewModel> locals = new List<MediaFolderViewModel>();
            foreach (var folder in db.LocalDb.MediaFolderLocations)
            {
                MediaFolderViewModel folderConverted = fileManager.GetFolder(folder);
                locals.Add(folderConverted);
            }

            return locals;
        }

        private void InitMedia()
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

            persister.ClearAllMedia();
            db.LocalDb.ClearMedia();

            foreach (var folder in foldersConverted)
            {
                persister.AddMediaFolderToDatabase(folder);
                db.LocalDb.MediaFolderLocations.Add(folder.Location);
                db.LocalDb.MediaFolders.Add(folder);
                mediaDirectories.Items.Add(folder.Location);
            }

            db.SaveChanges();
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            int selectedIndex = mediaDirectories.SelectedIndex;
            if (selectedIndex == -1 || mediaDirectories.Items.Count == 0)
            {
                return;
            }

            string folderName = mediaDirectories.Items[selectedIndex].ToString();

            persister.DeleteFolder(folderName);

            db.LocalDb.MediaFolderLocations.Remove(folderName);
            db.LocalDb.MediaFolders.Remove(db.LocalDb.MediaFolders.FirstOrDefault(f => f.Location == folderName));
            db.SaveChanges();

            mediaDirectories.Items.RemoveAt(selectedIndex);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select media folder. All folders inside will be added automatticaly.";
            dialog.ShowNewFolderButton = false;

            dialog.ShowDialog();
            String path = dialog.SelectedPath;

            if (db.LocalDb.MediaFolders.Any(f => f.Equals(path)))
            {
                MessageBox.Show("This folder is already included.");
            }
            else
            {
                MediaFolderViewModel folder = fileManager.GetFolder(path);
                persister.AddMediaFolderToDatabase(folder);
                db.LocalDb.MediaFolderLocations.Add(folder.Location);
                db.LocalDb.MediaFolders.Add(folder);
                db.SaveChanges();

                mediaDirectories.Items.Add(path);
            }
        }

        private void syncBtn_Click(object sender, EventArgs e)
        {
            SyncMedia();
        }

        private void ChangePlayerBtn_Click(object sender, EventArgs e)
        {
            string newLocation = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;
            dialog.Filter = "BSPlayer (bsplayer.exe)|bsplayer.exe";
            dialog.ShowDialog();
            newLocation = dialog.FileName;
            if (!newLocation.EndsWith("bsplayer.exe"))
            {
                MessageBox.Show("The chosen file is not a valid bsplayer.exe file.");
            }
            else
            {
                bsPlayerLocation = newLocation;
                db.LocalDb.BsPlayerLocation = bsPlayerLocation;
                db.SaveChanges();

                playerLocation.Text = bsPlayerLocation;
            }
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            IPlayer player = new BsPlayer(bsPlayerLocation);

            CommandExecutor cmdExec = new CommandExecutor(player, db.LocalDb.Files);
            listener = new CommunicationsManager(Environment.MachineName, cmdExec);
        }
    }
}