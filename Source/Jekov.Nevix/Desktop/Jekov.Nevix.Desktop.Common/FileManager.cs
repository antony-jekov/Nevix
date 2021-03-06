﻿namespace Jekov.Nevix.Desktop.Common
{
    using Jekov.Nevix.Common.ViewModels;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    public class FileManager
    {
        private static readonly FileManager instance = new FileManager();

        public static FileManager Instance()
        {
            return instance;
        }

        private FileManager()
        {
            directoriesWithMediaInside = new HashSet<string>();
        }

        private readonly static HashSet<string> MediaTypes = new HashSet<string> { ".avi", ".mov", ".mpeg", ".mkv", ".mp3", ".mp4", ".wav", ".wmv" };
        private readonly static HashSet<string> SkipDownloadDirs = new HashSet<string> { "appdata", "windows", "program files", "program files (x86)", "programdata", "all users" };

        private readonly HashSet<string> directoriesWithMediaInside;

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
                if (MediaTypes.Contains(file.Extension.ToLower()))
                {
                    rootMediaFolder.Files.Add(ConvertToMediaFileViewModel(file));
                }
            }

            foreach (var subDir in rootDirectory.GetDirectories())
            {
                if (DirectoryHasMediaInside(subDir))
                {
                    subFolder = new MediaFolderViewModel
                    {
                        Location = subDir.FullName,
                        Name = subDir.Name
                    };

                    foreach (var file in subDir.GetFiles())
                    {
                        if (MediaTypes.Contains(file.Extension.ToLower()))
                        {
                            subFolder.Files.Add(ConvertToMediaFileViewModel(file));
                        }
                    }

                    rootMediaFolder.Folders.Add(ConvertToMediaFolderViewModel(subDir));
                }
            }

            return rootMediaFolder;
        }

        private bool DirectoryHasMediaInside(DirectoryInfo subDir)
        {
            if (directoriesWithMediaInside.Contains(subDir.FullName))
            {
                return true;
            }

            bool hasFiles = false;
            try
            {
                if (subDir.GetFiles().Any(f => MediaTypes.Contains(f.Extension.ToLower())))
                {
                    directoriesWithMediaInside.Add(subDir.FullName);
                    return true;
                }
            }
            catch (UnauthorizedAccessException)
            {
                //MessageBox.Show("You don't have access to one or more folders inside the selected location!", "Restricted Access", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            

            var subDirs = subDir.GetDirectories();

            if (subDirs.Any())
            {
                foreach (var innerSubDir in subDirs)
                {
                    hasFiles = DirectoryHasMediaInside(innerSubDir);
                    if (hasFiles)
                    {
                        directoriesWithMediaInside.Add(subDir.FullName);
                        return hasFiles;
                    }
                }
            }

            return false;
        }

        private MediaFileViewModel ConvertToMediaFileViewModel(FileInfo file)
        {
            return new MediaFileViewModel
                        {
                            Location = file.FullName,
                            Name = file.Name
                        };
        }

        public IEnumerable<DirectoryInfo> GetAllDownloadDirectories(IEnumerable<DriveInfo> drives)
        {
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


            List<DirectoryInfo> downloadDirs = GetDirecory(dirs);

            return downloadDirs;
        }

        private List<DirectoryInfo> GetDirecory(Queue<DirectoryInfo> dirs)
        {
            List<DirectoryInfo> folders = new List<DirectoryInfo>();

            DirectoryInfo[] subDirs;
            string nameToLower;

            while (dirs.Count > 0)
            {
                DirectoryInfo currentDir = dirs.Dequeue();
                nameToLower = currentDir.Name.ToLower();

                if (SkipDownloadDirs.Contains(nameToLower))
                {
                    continue;
                }

                if (nameToLower.Contains("download"))
                {
                    folders.Add(currentDir);
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

            return folders;
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

        public MediaFolderViewModel GetFolder(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            MediaFolderViewModel folder = ConvertToMediaFolderViewModel(dir);

            return folder;
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}