﻿namespace Jekov.Nevix.Desktop.Client
{
    using Jekov.Nevix.Common.ViewModels;
    using Jekov.Nevix.Desktop.Common;
    using Jekov.Nevix.Desktop.Common.Contracts;
    using Jekov.Nevix.Desktop.Common.Players;
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    public partial class MainForm : Form
    {
        private bool playerChangeScheduled;
        private readonly string userEmail;
        private readonly string sessionKey;
        private NevixLocalDbContext db;
        private PersisterManager persister;
        private CommunicationsManager listener;
        private FileManager fileManager;
        private string preferredPlayerLocation;
        private ICollection<PlayerEntry> playerEntries;

        public MainForm(string email, string sessionKey)
        {
            this.userEmail = email;
            this.sessionKey = sessionKey;
            this.db = NevixLocalDbContext.Instance();
            this.fileManager = FileManager.Instance();
            this.persister = PersisterManager.Instance();
            this.persister.SessionKey = sessionKey;

            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //this.notifyIcon1.ContextMenu = new System.Windows.Forms.ContextMenu(new MenuItem[] {
            //new MenuItem("Exit", (s, d) => {Application.Exit();})
            //});
            playerChangeScheduled = false;
            email.Text = this.userEmail;
            RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"Software", false);
            playerEntries = new List<PlayerEntry>();
            playerEntries.Add(new PlayerEntry("System Player", string.Empty));

            UpdatePlayerEntries(regKey, playerEntries);

            foreach (var playerEntry in playerEntries)
            {
                playerSelect.Items.Add(playerEntry.Name);
            }

            if (!string.IsNullOrEmpty(db.LocalDb.PreferredPlayerLocation))
            {
                preferredPlayerLocation = db.LocalDb.PreferredPlayerLocation;
            }

            UpdatePlayerComboBox();

            SyncMedia();
            Connect();

            playerChangeScheduled = true;
        }

        private void UpdatePlayerEntries(RegistryKey parentKey, ICollection<PlayerEntry> entries)
        {
            string[] nameList = parentKey.GetSubKeyNames();
            string[] subNameList;
            RegistryKey subKey;
            string location;
            if (nameList.Contains("BST"))
            {
                subKey = parentKey.OpenSubKey("BST", false);
                subNameList = subKey.GetSubKeyNames();
                if (subNameList.Contains("bsplayer"))
                {
                    subKey = subKey.OpenSubKey("bsplayer", false);
                    location = subKey.GetValue("AppPath").ToString();

                    entries.Add(new PlayerEntry("BSPlayer", location));
                }
                if (subNameList.Contains("bsplayerv1"))
                {
                    subKey = parentKey.OpenSubKey("BST", false);
                    subKey = subKey.OpenSubKey("bsplayerv1", false);
                    location = subKey.GetValue("AppPath").ToString();

                    entries.Add(new PlayerEntry("BSPlayer Pro", location));
                }
            }
        }

        private void UpdatePlayerComboBox()
        {
            playerSelect.SelectedIndex = 0;
            if (!string.IsNullOrEmpty(preferredPlayerLocation))
            {
                byte i = 0;
                foreach (var item in playerEntries)
                {
                    if (item.Location == preferredPlayerLocation)
                    {
                        playerSelect.SelectedIndex = i;
                        break;
                    }

                    i++;
                }
            }
        }

        protected string CalculateMd5HashCode(string text)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(text);
            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0, len = hash.Length; i < len; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private async void SyncMedia()
        {
            string serverHash = string.Empty;

            try
            {
                serverHash = await persister.GetMediaHash();
            }
            catch (Exception ex)
            {
                if (ex is UnauthorizedAccessException)
                {
                    AbortSession(ex);
                }
                else if (ex is ApplicationException)
                {
                    MessageBox.Show(ex.Message, "Server down", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
            }

            var localFolders = LoadMediaFolders();

            StringBuilder sb = new StringBuilder();
            foreach (var folder in localFolders)
            {
                sb.Append(folder.GetAllLocations());
            }

            string localMediaHash = CalculateMd5HashCode(sb.ToString());

            if (serverHash.Length > 0 && serverHash.Equals(localMediaHash))
            {
                mediaDirectories.Items.Clear();
                mediaDirectories.Items.AddRange(db.LocalDb.MediaFolderLocations.ToArray());
            }
            else
            {
                persister.ClearAllMedia();
                var locals = db.LocalDb.MediaFolderLocations.ToArray();
                db.LocalDb.ClearMedia();
                mediaDirectories.Items.Clear();

                foreach (var folder in locals)
                {
                    AddFolder(folder);
                }

                db.SaveChanges();
            }


            progressIndicator.Visible = false;
        }

        private void AbortSession(Exception ex)
        {
            MessageBox.Show(ex.Message, "Unauthorized error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            db.LocalDb.SessionKey = string.Empty;
            db.SaveChanges();
            Application.Restart();
        }

        private void UpdateFileIndexes(MediaFolderViewModel folder)
        {
            int id = db.LocalDb.Files.Count;
            foreach (var file in folder.Files)
            {
                file.Id = id;
                db.LocalDb.Files.Add(id, file.Location);
                id++;
            }

            foreach (var subFolder in folder.Folders)
            {
                UpdateFileIndexes(subFolder);
            }
        }

        //private string RemoveIdFromFiles(string serverFolders)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0, len = serverFolders.Length; i < len; i++)
        //    {
        //        if (serverFolders[i] == 'I' && serverFolders[i + 1] == 'd' &&
        //            serverFolders[i + 2] == '"' && serverFolders[i + 3] == ':')
        //        {
        //            i = serverFolders.IndexOf(',', i + 3) + 1;
        //            sb.Append("Id\":0,\"");
        //        }
        //        else
        //        {
        //            sb.Append(serverFolders[i]);
        //        }
        //    }

        //    return sb.ToString();
        //}

        private IEnumerable<MediaFolderViewModel> LoadMediaFolders()
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
            progressIndicator.Visible = true;
            RemoveFolder();
        }

        private void RemoveFolder()
        {
            int selectedIndex = mediaDirectories.SelectedIndex;
            if (selectedIndex == -1 || mediaDirectories.Items.Count == 0)
            {
                progressIndicator.Visible = false;
                return;
            }

            string folderName = mediaDirectories.Items[selectedIndex].ToString();
            persister.DeleteFolder(folderName);

            db.LocalDb.MediaFolderLocations.Remove(folderName);
            db.LocalDb.MediaFolders.Remove(db.LocalDb.MediaFolders.FirstOrDefault(f => f.Location == folderName));
            db.SaveChanges();
            progressIndicator.Visible = false;
            mediaDirectories.Items.RemoveAt(selectedIndex);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select media folder. All folders inside will be added automatticaly.";
            dialog.ShowNewFolderButton = false;

            dialog.ShowDialog();
            progressIndicator.Visible = true;
            AddFolder(dialog.SelectedPath);
        }

        private void AddFolder(string path)
        {
            if (db.LocalDb.MediaFolders.Any(f => f.Equals(path)))
            {
                MessageBox.Show("This folder is already included.");
            }
            else
            {
                MediaFolderViewModel folder = fileManager.GetFolder(path);
                UpdateFileIndexes(folder);
                persister.AddMediaFolderToDatabase(folder);
                db.LocalDb.MediaFolderLocations.Add(folder.Location);
                db.LocalDb.MediaFolders.Add(folder);
                db.SaveChanges();

                mediaDirectories.Items.Add(path);
            }

            progressIndicator.Visible = false;
        }

        private void syncBtn_Click(object sender, EventArgs e)
        {
            progressIndicator.Visible = true;
            SyncMedia();
        }

        //private void ChangePlayerBtn_Click(object sender, EventArgs e)
        //{
        //    string newLocation = string.Empty;

        //    OpenFileDialog dialog = new OpenFileDialog();
        //    dialog.Multiselect = false;
        //    dialog.Filter = "BSPlayer (bsplayer.exe)|bsplayer.exe";
        //    dialog.ShowDialog();
        //    newLocation = dialog.FileName;
        //    if (!newLocation.EndsWith("bsplayer.exe"))
        //    {
        //        MessageBox.Show("The chosen file is not a valid bsplayer.exe file.");
        //    }
        //    else
        //    {
        //        preferredPlayerLocation = newLocation;
        //        db.LocalDb.PreferredPlayerLocation = preferredPlayerLocation;
        //        db.SaveChanges();
        //    }
        //}

        private void Connect()
        {
            CommandExecutor cmdExec = new CommandExecutor(GetPlayer(), db.LocalDb.Files);
            listener = new CommunicationsManager(sessionKey, cmdExec);
        }

        private IPlayer GetPlayer()
        {
            string player = playerEntries.ElementAt(playerSelect.SelectedIndex).Name;
            if (player == "BSPlayer" || player == "BSPlayer Pro")
            {
                return new BsPlayer(preferredPlayerLocation);
            }

            return new SystemPlayer();
        }

        private void playerSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!playerChangeScheduled)
            {
                return;
            }
            string newLocation = playerEntries.ElementAt(playerSelect.SelectedIndex).Location;
            db.LocalDb.PreferredPlayerLocation = newLocation;
            db.SaveChanges();

            preferredPlayerLocation = newLocation;

            if (listener != null)
                listener.executor.player = GetPlayer();
        }

        private void logoff_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            db.LocalDb.SessionKey = string.Empty;
            db.SaveChanges();
            Program.logOutScheduled = true;
            Program.LoginForm().Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!Program.logOutScheduled)
            {
                Application.Exit();
            }
        }

        //private void checkBox1_CheckedChanged(object sender, EventArgs e)
        //{
        //    RegistryKey rk = Registry.CurrentUser.OpenSubKey
        //    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        //    if (startWithWindows.Checked)
        //        rk.SetValue("Nevix Desktop Client", Application.ExecutablePath.ToString());
        //    else
        //        rk.DeleteValue("Nevix Desktop Client", false);

        //    db.LocalDb.StartWithWindows = startWithWindows.Checked;
        //    db.SaveChanges();
        //}

        //private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        //{
        //    if (!minimized)
        //    {
        //        Minimize(e);
        //    }
        //}

        //private void Minimize(FormClosingEventArgs e)
        //{
        //    if (e != null)
        //        e.Cancel = true;
        //    Hide();
        //    notifyIcon1.Visible = true;
        //    notifyIcon1.ShowBalloonTip(3000, "Nevix", "Nevix Desktop Client is working in the background.", ToolTipIcon.Info);

        //    minimized = true;
        //}



        //private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        //{
        //    Show();
        //    notifyIcon1.Visible = false;
        //    minimized = false;
        //}

        //private void MainForm_FormClosing_1(object sender, FormClosingEventArgs e)
        //{
        //    if (!minimized)
        //    {
        //        e.Cancel = true;
        //        Hide();
        //        notifyIcon1.Visible = true;
        //        notifyIcon1.ShowBalloonTip(
        //            3000, "Nevix Desktop Client", "Nevix is working in background mode.", ToolTipIcon.Info);
        //        minimized = true;
        //    }
        //}

        //private void notifyIcon1_MouseDoubleClick_1(object sender, MouseEventArgs e)
        //{
        //    minimized = false;
        //    notifyIcon1.Visible = false;
        //    Show();
        //}

        //private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        //{
        //    minimized = false;
        //    notifyIcon1.Visible = false;
        //    Show();
        //}
    }
}