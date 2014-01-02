namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    using Jekov.Nevix.Common.Models;
    using Jekov.Nevix.Common.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class MediaFoldersController : BaseController
    {
        [HttpPost]
        public HttpResponseMessage AddFolder(MediaFolderViewModel model)
        {
            NevixUser currentUser = GetCurrentUser();

            if (currentUser == null)
            {
                return UnauthorizedErrorMessage();
            }

            if (model == null)
            {
                return NullModelErrorMessage();
            }

            MediaFolder rootFolder = new MediaFolder
            {
                Name = model.Name,
                User = currentUser,
                Location = model.Location
            };

            foreach (var mediaFile in model.Files)
            {
                rootFolder.MediaFiles.Add(ConvertMediaFileViewModel(mediaFile));
            }

            CreateSubFoldersAndFiles(model.Folders, rootFolder);

            currentUser.Folders.Add(rootFolder);

            currentUser.LastFilesUpdate = DateTime.UtcNow;

            Data.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        private MediaFile ConvertMediaFileViewModel(MediaFileViewModel mediaFile)
        {
            return new MediaFile
            {
                Length = mediaFile.Length,
                Location = mediaFile.Location,
                Name = mediaFile.Name
            };
        }

        private void CreateSubFoldersAndFiles(IEnumerable<MediaFolderViewModel> folders, MediaFolder parentFolder)
        {
            MediaFolder currentFolderEntity;
            MediaFolder subFolderEntity;

            foreach (var folder in folders)
            {
                currentFolderEntity = ConvertMediaFolderViewModel(folder, parentFolder);
                parentFolder.MediaFolders.Add(currentFolderEntity);

                foreach (var file in folder.Files)
                {
                    currentFolderEntity.MediaFiles.Add(ConvertMediaFileViewModel(file));
                }

                foreach (var subFolder in folder.Folders)
                {
                    subFolderEntity = ConvertMediaFolderViewModel(subFolder, currentFolderEntity);
                    currentFolderEntity.MediaFolders.Add(subFolderEntity);

                    CreateSubFoldersAndFiles(subFolder.Folders, subFolderEntity);
                }
            }
        }

        private MediaFolder ConvertMediaFolderViewModel(MediaFolderViewModel folder, MediaFolder parentFolder)
        {
            return new MediaFolder
            {
                Name = folder.Name,
                User = parentFolder.User,
                Location = folder.Location
            };
        }

        [HttpGet]
        public HttpResponseMessage GetFolders()
        {
            NevixUser currentUser = GetCurrentUser();

            if (currentUser == null)
            {
                return UnauthorizedErrorMessage();
            }

            IEnumerable<MediaFolder> rootMediaFolders = Data.Folders.All().Where(f => f.UserId == currentUser.Id && f.ParentFolderId == 0).ToList();

            List<MediaFolderViewModel> userFolders = new List<MediaFolderViewModel>();
            Queue<MediaFolder> folders = new Queue<MediaFolder>();
            MediaFolder currentFolder;
            MediaFolderViewModel newFolder;
            MediaFolderViewModel currentFolderViewModel;
            MediaFileViewModel currentFileViewModel;

            foreach (var currentRootFolder in rootMediaFolders)
            {
                folders.Enqueue(currentRootFolder);
                currentFolderViewModel = ConvertFolderToViewModel(currentRootFolder);

                while (folders.Count > 0)
                {
                    currentFolder = folders.Dequeue();

                    foreach (var folder in currentFolder.MediaFolders)
                    {
                        newFolder = ConvertFolderToViewModel(folder);
                        currentFolderViewModel.Folders.Add(newFolder);
                        folders.Enqueue(folder);
                    }

                    foreach (var file in currentFolder.MediaFiles)
                    {
                        currentFileViewModel = ConvertFileToViewModel(file);
                        currentFolderViewModel.Files.Add(currentFileViewModel);
                    }
                }

                userFolders.Add(currentFolderViewModel);
            }

            return Request.CreateResponse(HttpStatusCode.OK, userFolders);
        }

        private MediaFileViewModel ConvertFileToViewModel(MediaFile file)
        {
            return new MediaFileViewModel
            {
                Length = file.Length,
                Location = file.Location,
                Name = file.Name,
                ParentDirectoryId = file.ParentFolderId ?? 0
            };
        }

        private MediaFolderViewModel ConvertFolderToViewModel(MediaFolder folder)
        {
            return new MediaFolderViewModel
            {
                Name = folder.Name
            };
        }
    }
}