namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    using Jekov.Nevix.Common.Models;
    using Jekov.Nevix.Common.ViewModels;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class MediaFoldersMobileController : BaseApiController
    {
        [HttpGet]
        public HttpResponseMessage GetFolders()
        {
            var currentUser = GetCurrentUser();

            if (currentUser == null)
            {
                return UnauthorizedErrorMessage();
            }

            IEnumerable<MediaFolderMobileViewModel> folders = GetUserFoldersToMobile(currentUser.Media);

            return Request.CreateResponse(HttpStatusCode.OK, folders);
        }

        private IEnumerable<MediaFolderMobileViewModel> GetUserFoldersToMobile(string media)
        {
            List<MediaFolderMobileViewModel> foldersConverted = new List<MediaFolderMobileViewModel>();
            var folders = MediaFoldersController.MediaToList(media);

            foreach (var folder in folders)
            {
                foldersConverted.Add(FolderToMobileViewModel(folder));
            }

            return foldersConverted;
        }

        private MediaFolderMobileViewModel FolderToMobileViewModel(MediaFolderViewModel folder)
        {
            MediaFolderMobileViewModel root = new MediaFolderMobileViewModel
            {
                Name = folder.Name
            };

            foreach (var file in folder.Files)
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