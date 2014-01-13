namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    using Jekov.Nevix.Common.Data;
    using Jekov.Nevix.Common.Data.Contracts;
    using Jekov.Nevix.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web.Http;

    public class BaseApiController : ApiController
    {
        private const string SessionKeyName = "X-SessionKey";

        protected string GetSessionKey()
        {
            string sessionKey = string.Empty;

            IEnumerable<string> sessionKeys;

            if (Request.Headers.TryGetValues(SessionKeyName, out sessionKeys))
            {
                if (sessionKeys.Any())
                {
                    sessionKey = sessionKeys.First().Trim(new char[]{' ', '"'});
                }
            }

            return sessionKey;
        }

        protected NevixUser GetCurrentUser()
        {
            string sessionKey = GetSessionKey();
            return Data.Users.All().FirstOrDefault(u => u.SessionKey == sessionKey);
        }

        protected bool IsCurrentUserAuthorized()
        {
            return GetCurrentUser() != null;
        }

        protected HttpResponseMessage NullModelErrorMessage()
        {
            var error = new ArgumentNullException("model", "Model cannot be null!");
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
        }

        protected INevixData Data { get; set; }

        public BaseApiController()
            : this(new NevixData())
        {
        }

        public BaseApiController(INevixData data)
        {
            Data = data;
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

        protected HttpResponseMessage UnauthorizedErrorMessage()
        {
            var error = new UnauthorizedAccessException("You must login to perform this operation!");
            return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, error);
        }
    }
}