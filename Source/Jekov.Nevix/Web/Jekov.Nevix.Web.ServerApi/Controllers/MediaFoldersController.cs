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

            MediaFolder rootFolder = ConvertMediaFolderViewModel(model, currentUser.Id);
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

        private MediaFolder ConvertMediaFolderViewModel(MediaFolderViewModel folder, int userId)
        {
            MediaFolder model = new MediaFolder
            {
                Name = folder.Name,
                UserId = userId,
                Location = folder.Location
            };

            foreach (var file in folder.Files)
            {
                model.MediaFiles.Add(ConvertMediaFileViewModel(file));
            }

            foreach (var subFolder in folder.Folders)
            {
                model.MediaFolders.Add(ConvertMediaFolderViewModel(subFolder, userId));
            }

            return model;
        }

        [HttpPut]
        public HttpResponseMessage Remove(string location)
        {
            NevixUser user = GetCurrentUser();

            if (user == null)
            {
                return UnauthorizedErrorMessage();
            }

            MediaFolder requestedFolder = user.Folders.FirstOrDefault(f => f.Location == location);
            if (requestedFolder != null)
            {
                List<MediaFolder> foldersToRemove = new List<MediaFolder>();
                GetChildrenFolders(foldersToRemove, requestedFolder);

                foreach (var folder in foldersToRemove)
                {
                    List<MediaFile> files = new List<MediaFile>(folder.MediaFiles);

                    foreach (var file in files)
                    {
                        Data.Files.Delete(file.Id);
                    }
                }

                foreach (var folder in foldersToRemove)
                {
                    Data.Folders.Delete(folder.Id);
                }

                user.LastFilesUpdate = DateTime.UtcNow;
                Data.SaveChanges();
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private void GetChildrenFolders(ICollection<MediaFolder> folders, MediaFolder root)
        {
            folders.Add(root);

            foreach (var subFolder in root.MediaFolders)
            {
                GetChildrenFolders(folders, subFolder);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetFolders()
        {
            NevixUser currentUser = GetCurrentUser();

            if (currentUser == null)
            {
                return UnauthorizedErrorMessage();
            }

            IEnumerable<MediaFolder> rootMediaFolders = Data.Folders.All().Where(f => f.UserId == currentUser.Id && f.ParentFolderId == null).ToList();

            List<MediaFolderViewModel> userFolders = new List<MediaFolderViewModel>();

            foreach (var currentRootFolder in rootMediaFolders)
            {
                userFolders.Add(ConvertFolderToViewModel(currentRootFolder));
            }

            return Request.CreateResponse(HttpStatusCode.OK, userFolders);
        }

        private MediaFileViewModel ConvertFileToViewModel(MediaFile file)
        {
            return new MediaFileViewModel
            {
                Length = file.Length,
                Location = file.Location,
                Name = file.Name
            };
        }

        private MediaFolderViewModel ConvertFolderToViewModel(MediaFolder folder)
        {
            MediaFolderViewModel model = new MediaFolderViewModel
            {
                Name = folder.Name,
                Location = folder.Location
            };

            foreach (var file in folder.MediaFiles)
            {
                model.Files.Add(ConvertFileToViewModel(file));
            }

            foreach (var subFolder in folder.MediaFolders)
            {
                model.Folders.Add(ConvertFolderToViewModel(subFolder));
            }

            return model;
        }

        [HttpPut]
        public HttpResponseMessage Clear()
        {
            NevixUser user = GetCurrentUser();
            if (user == null)
            {
                return UnauthorizedErrorMessage();
            }

            foreach (var folder in user.Folders.Where(f => f.ParentFolderId == null).ToList())
            {
                Remove(folder.Location);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}