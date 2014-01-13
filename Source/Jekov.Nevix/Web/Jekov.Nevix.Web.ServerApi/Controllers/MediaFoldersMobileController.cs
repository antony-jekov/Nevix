namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    using Jekov.Nevix.Common.Models;
    using Jekov.Nevix.Common.ViewModels;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;

    public class MediaFoldersMobileController : BaseApiController
    {
        public HttpResponseMessage GetFolders()
        {
            var currentUser = GetCurrentUser();

            if (currentUser == null)
            {
                return UnauthorizedErrorMessage();
            }

            IEnumerable<MediaFolderMobileViewModel> folders = GetUserFoldersToMobile(currentUser.Id);

            return Request.CreateResponse(HttpStatusCode.OK, folders);
        }

        private IEnumerable<MediaFolderMobileViewModel> GetUserFoldersToMobile(int userId)
        {
            List<MediaFolderMobileViewModel> foldersConverted = new List<MediaFolderMobileViewModel>();
            var folders = Data.Folders.All().Where(f => f.ParentFolderId == null && f.UserId == userId).ToList();

            foreach (var folder in folders)
            {
                foldersConverted.Add(FolderToMobileViewModel(folder));
            }

            return foldersConverted;
        }

        private MediaFolderMobileViewModel FolderToMobileViewModel(MediaFolder folder)
        {
            MediaFolderMobileViewModel root = new MediaFolderMobileViewModel
            {
                Name = folder.Name
            };

            foreach (var file in folder.Folders)
            {
                root.Files.Add(new MediaFileMobileViewModel
                    {
                        Id = file.Id,
                        Name = file.Name
                    });
            }

            foreach (var subFolder in folder.Folders)
            {
                root.Folders.Add(FolderToMobileViewModel(subFolder));
            }

            return root;
        }
    }
}