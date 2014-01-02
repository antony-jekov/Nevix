namespace Jekov.Nevix.Desktop.Common
{
    using Jekov.Nevix.Common.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    public class FileManager
    {
        private static string[] MediaTypes = { ".avi", ".mov", ".mpeg", ".mkv", ".mp3", ".mp4", ".wav" };
        private static string[] SkipDownloadDirs = { "appdata", "windows", "program files", "program files (x86)", "programdata", "all users" };

        public MediaFolderViewModel ConvertToMediaFolderViewModel(DirectoryInfo rootDirectory)
        {
            MediaFolderViewModel subFolder;
            MediaFolderViewModel rootMediaFolder = new MediaFolderViewModel
            {
                Location = rootDirectory.FullName,
                Name = rootDirectory.Name
            };

            foreach (var file in rootDirectory.GetFiles())
            {
                rootMediaFolder.Files.Add(ConvertToMediaFileViewModel(file));
            }

            foreach (var subDir in rootDirectory.GetDirectories())
            {
                subFolder = new MediaFolderViewModel
                {
                    Location = subDir.FullName,
                    Name = subDir.Name
                };

                foreach (var file in subDir.GetFiles())
                {
                    subFolder.Files.Add(ConvertToMediaFileViewModel(file));
                }

                rootMediaFolder.Folders.Add(ConvertToMediaFolderViewModel(subDir));
            }

            return rootMediaFolder;
        }

        private MediaFileViewModel ConvertToMediaFileViewModel(FileInfo file)
        {
            return new MediaFileViewModel
                        {
                            Length = file.Length,
                            Location = file.FullName,
                            Name = file.Name
                        };
        }

        public IEnumerable<DirectoryInfo> GetAllDownloadDirectories(IEnumerable<DriveInfo> drives)
        {
            List<DirectoryInfo> downloadDirs = new List<DirectoryInfo>();
            HashSet<string> visitedDirs = new HashSet<string>();
            Queue<DirectoryInfo> dirs = new Queue<DirectoryInfo>();

            foreach (var drive in drives)
            {
                DirectoryInfo dir = new DirectoryInfo(drive.Name);
                var directories = dir.GetDirectories();
                foreach (var d in directories)
                {
                    if (!SkipDownloadDirs.Contains(d.Name.ToLower()))
                    {
                        dirs.Enqueue(d);
                    }
                }
            }

            DirectoryInfo[] subDirs;
            string nameToLower;

            while (dirs.Count > 0)
            {
                DirectoryInfo currentDir = dirs.Dequeue();
                if (visitedDirs.Contains(currentDir.FullName))
                {
                    continue;
                }

                visitedDirs.Add(currentDir.FullName);
                nameToLower = currentDir.Name.ToLower();

                if (SkipDownloadDirs.Contains(nameToLower))
                {
                    continue;
                }

                if (nameToLower.Contains("download"))
                {
                    downloadDirs.Add(currentDir);
                }
                else
                {
                    try
                    {
                        subDirs = currentDir.GetDirectories();

                        if (!subDirs.Any())
                        {
                            continue;
                        }

                        foreach (var d in subDirs)
                        {
                            dirs.Enqueue(d);
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }

            return downloadDirs;
        }

        public IEnumerable<FileInfo> GetMediaFiles(IEnumerable<DirectoryInfo> downloadDirectories)
        {
            List<FileInfo> mediaFiles = new List<FileInfo>();

            Queue<DirectoryInfo> queue = new Queue<DirectoryInfo>(downloadDirectories);
            DirectoryInfo currentDir;
            FileInfo[] files;
            IEnumerable<DirectoryInfo> subDirs;

            while (queue.Count > 0)
            {
                currentDir = queue.Dequeue();
                try
                {
                    files = currentDir.GetFiles();
                    if (files.Any())
                    {
                        mediaFiles.AddRange(files.Where(f => MediaTypes.Contains(f.Extension.ToLower())).ToArray());
                    }

                    subDirs = currentDir.GetDirectories();
                    if (subDirs.Any())
                    {
                        foreach (var d in subDirs)
                        {
                            queue.Enqueue(d);
                        }
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }

            return mediaFiles.OrderBy(f => f.Name);
        }

        public string AskBsPlayerLocation()
        {
            string location = string.Empty;

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.ShowDialog();
            location = dialog.FileName;

            return location;
        }

        public string FindBsPlayerLocation()
        {
            string location = string.Empty;

            Queue<DirectoryInfo> queue = new Queue<DirectoryInfo>();

            DirectoryInfo dir = new DirectoryInfo("c:\\");
            DirectoryInfo[] dirs = dir.GetDirectories("Program Files*");
            FileInfo[] files;

            foreach (var d in dirs)
            {
                queue.Enqueue(d);
            }

            DirectoryInfo currentDir;

            while (queue.Count > 0)
            {
                currentDir = queue.Dequeue();
                try
                {
                    files = currentDir.GetFiles();
                }
                catch (Exception)
                {
                    continue;
                }

                FileInfo bsplayer = files.FirstOrDefault(f => f.Name == "bsplayer.exe");

                if (bsplayer != null)
                {
                    location = bsplayer.FullName;
                    break;
                }

                dirs = currentDir.GetDirectories();

                foreach (var d in dirs)
                {
                    queue.Enqueue(d);
                }
            }

            return location;
        }

        public IEnumerable<DriveInfo> GetAllDrives()
        {
            return DriveInfo.GetDrives().Where(d => d.IsReady).ToList();
        }
    }
}