namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    using Jekov.Nevix.Common.Models;
    using Jekov.Nevix.Common.ViewModels;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http;

    public class MediaFoldersController : BaseApiController
    {
        public static IList<MediaFolderViewModel> MediaToList(string media)
        {
            return JsonConvert.DeserializeObject<IList<MediaFolderViewModel>>(media);
        }

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

            IList<MediaFolderViewModel> currentUserFolders = MediaToList(currentUser.Media);
            currentUserFolders.Add(model);
            currentUser.Media = JsonConvert.SerializeObject(currentUserFolders);
            currentUser.LastFilesUpdate = DateTime.UtcNow;

            Data.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created);
        }

        [HttpPut]
        public HttpResponseMessage Remove(string location)
        {
            NevixUser user = GetCurrentUser();

            if (user == null)
            {
                return UnauthorizedErrorMessage();
            }

            IList<MediaFolderViewModel> currentUserFolders = MediaToList(user.Media);
            MediaFolderViewModel requestedFolder = currentUserFolders.FirstOrDefault(f => f.Location == location);
            if (requestedFolder != null)
            {
                currentUserFolders.Remove(requestedFolder);
                user.Media = JsonConvert.SerializeObject(currentUserFolders);

                user.LastFilesUpdate = DateTime.UtcNow;
                Data.SaveChanges();
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage GetMediaHash()
        {
            var currentUser = GetCurrentUser();

            if (currentUser == null)
            {
                return UnauthorizedErrorMessage();
            }

            string hash = string.Empty;

            var folders = JsonConvert.DeserializeObject<IList<MediaFolderViewModel>>(currentUser.Media);

            if (folders.Any())
            {
                StringBuilder sb = new StringBuilder();
                foreach (var folder in folders)
                {
                    sb.Append(folder.GetAllLocations());
                }

                hash = CalculateMd5HashCode(sb.ToString());
            }

            return Request.CreateResponse(HttpStatusCode.OK, hash);
        }

        [HttpGet]
        public HttpResponseMessage GetFolders()
        {
            NevixUser currentUser = GetCurrentUser();

            if (currentUser == null)
            {
                return UnauthorizedErrorMessage();
            }

            return Request.CreateResponse(HttpStatusCode.OK, MediaToList(currentUser.Media));
        }

        [HttpPut]
        public HttpResponseMessage Clear()
        {
            NevixUser user = GetCurrentUser();
            if (user == null)
            {
                return UnauthorizedErrorMessage();
            }

            user.Media = string.Empty;
            user.LastFilesUpdate = DateTime.UtcNow;
            Data.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}