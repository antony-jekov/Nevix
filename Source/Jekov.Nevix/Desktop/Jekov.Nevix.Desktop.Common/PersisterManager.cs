namespace Jekov.Nevix.Desktop.Common
{
    using Jekov.Nevix.Common.ViewModels;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;

    public class PersisterManager
    {
        //private const string RootAddress = "http://localhost:50906/api/";
        private const string RootAddress = "http://nevix.apphb.com/api/";
        private const string HttpPost = "POST";
        private const string HttpPut = "PUT";
        private const string HttpGet = "GET";
        private readonly static char[] trimCharsForRequest = { '"', '\\', ' ' };

        public string SessionKey { get; set; }

        public PersisterManager () : this (string.Empty)
        {
        }

        public PersisterManager(string sessionKey)
        {
            this.SessionKey = sessionKey;
        }

        public DateTime LastMediaUpdate()
        {
            string time = GetRequest(RootAddress + "User/LastMediaUpdate");
            return DateTime.Parse(time);
        }

        public string Login(string email, string password)
        {
            UserLoginViewModel loginData = new UserLoginViewModel
            {
                Email = email.ToLower(),
                Password = StringToSha1(password)
            };

            string serializedRequestBody = JsonConvert.SerializeObject(loginData);

            SessionKey = HttpRequest(RootAddress + "user/login", HttpPut, serializedRequestBody);

            return SessionKey;
        }

        public void LogOff()
        {
            HttpRequest(RootAddress + "user/logoff", HttpPut);
        }

        public string Register(string email, string password, string confirmPassword)
        {
            UserRegisterViewModel registerData = new UserRegisterViewModel
            {
                Email = email.ToLower(),
                Password = StringToSha1(password),
                ConfirmPassword = StringToSha1(confirmPassword)
            };

            string serializedRequestBody = JsonConvert.SerializeObject(registerData);

            SessionKey = HttpRequest(RootAddress + "user/register", HttpPost, serializedRequestBody);
            return SessionKey;
        }

        public MediaFolderViewModel AddMediaFolderToDatabase(MediaFolderViewModel rootFolder)
        {
            string requestBody = JsonConvert.SerializeObject(rootFolder);

            string result = HttpRequest(RootAddress + "mediafolders/addfolder", HttpPost, requestBody);

            return JsonConvert.DeserializeObject<MediaFolderViewModel>(result);
        }

        protected string HttpRequest(string url, string method, string Parameters = "")
        {
            WebRequest req = System.Net.WebRequest.Create(url);
            req.ContentType = "application/json";
            req.Method = method;
            req.Headers.Add(string.Format("X-SessionKey:{0}", SessionKey));
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            WebResponse resp = null;
            try
            {
                resp = req.GetResponse();
            }
            catch (WebException e)
            {
                string err = e.Message;
                if (err.EndsWith("Bad Request."))
                {
                    throw new ArgumentException();
                }
                else if (err.EndsWith("Conflict."))
                {
                    throw new InvalidOperationException("There is already an user with that email.");
                }
                else if (err.EndsWith("Unauthorized."))
                {
                    throw new UnauthorizedAccessException();
                }
            }

            if (resp == null) return null;

            StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim(trimCharsForRequest);
        }

        protected string GetRequest(string url)
        {
            WebRequest req = System.Net.WebRequest.Create(url);
            req.Headers.Add(string.Format("X-SessionKey:{0}", SessionKey));
            WebResponse resp = req.GetResponse();
            StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim(trimCharsForRequest);
        }

        protected string StringToSha1(string text)
        {
            System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();

            byte[] buffer = encoder.GetBytes(text);
            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }

        public void UpdateChannelName(string computerName)
        {
            HttpRequest(RootAddress + "User/UpdateChannel", HttpPut, JsonConvert.SerializeObject(new ChannelViewModel { Name = computerName }));
        }

        public string GetChannelName()
        {
            return GetRequest(RootAddress + "User/GetChannel");
        }

        public void ClearAllMedia()
        {
            HttpRequest(RootAddress + "mediafolders/clear", HttpPut);
        }

        public void DeleteFolder(string folderName)
        {
            HttpRequest(RootAddress + "mediafolders/remove?location=" + folderName, HttpPut);
        }

        public string GetAllFolders()
        {
            return GetRequest(RootAddress + "mediafolders/getfolders");
        }

        public string GetMediaHash()
        {
            return GetRequest(RootAddress + "mediafolders/GetMediaHash");
        }
    }
}