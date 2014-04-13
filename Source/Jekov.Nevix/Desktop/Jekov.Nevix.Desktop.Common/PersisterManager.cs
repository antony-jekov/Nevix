namespace Jekov.Nevix.Desktop.Common
{
    using Jekov.Nevix.Common.ViewModels;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Threading.Tasks;

    public class PersisterManager
    {
        //private const string RootAddress = "http://localhost:50906/api/";
        //private const string RootAddress = "http://nevix.apphb.com/api/";
        private const string RootAddress = "http://nevix.antonyjekov.com/api/";
        private const string HttpPost = "POST";
        private const string HttpPut = "PUT";
        private const string HttpGet = "GET";
        private readonly static char[] trimCharsForRequest = { '"', '\\', ' ' };

        private static readonly PersisterManager instance = new PersisterManager();

        public static PersisterManager Instance()
        {
            return instance;
        }

        public string SessionKey { get; set; }

        private PersisterManager () : this (string.Empty)
        {
        }

        private PersisterManager(string sessionKey)
        {
            this.SessionKey = sessionKey;
        }

        public async Task<DateTime> LastMediaUpdate()
        {
            string time = await GetRequest(RootAddress + "User/LastMediaUpdate");
            return DateTime.Parse(time);
        }

        public async Task<string> Login(string email, string password)
        {
            UserLoginViewModel loginData = new UserLoginViewModel
            {
                Email = email.ToLower(),
                Password = StringToSha1(password)
            };

            string serializedRequestBody = JsonConvert.SerializeObject(loginData);

            SessionKey = await HttpRequest(RootAddress + "user/login", HttpPut, serializedRequestBody);

            return SessionKey;
        }
        public async void LogOff()
        {
            await HttpRequest(RootAddress + "user/logoff", HttpPut);
        }

        public async Task Register(string email, string password, string confirmPassword)
        {
            UserRegisterViewModel registerData = new UserRegisterViewModel
            {
                Email = email.ToLower(),
                Password = StringToSha1(password),
                ConfirmPassword = StringToSha1(confirmPassword)
            };

            string serializedRequestBody = JsonConvert.SerializeObject(registerData);

            await HttpRequest(RootAddress + "user/register", HttpPost, serializedRequestBody);
        }

        public async void AddMediaFolderToDatabase(MediaFolderViewModel rootFolder)
        {
            string requestBody = JsonConvert.SerializeObject(rootFolder);
            await HttpRequest(RootAddress + "mediafolders/addfolder", HttpPost, requestBody);
        }

        protected async Task<string> HttpRequest(string url, string method, string Parameters = "")
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
                resp = await req.GetResponseAsync();
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
                else if (err.EndsWith("Server Error."))
                {
                    throw new ApplicationException("The server appears to be down... Sorry for the inconvenience!");
                }

                throw e;
            }

            if (resp == null) return null;

            StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim(trimCharsForRequest);
        }

        protected async Task<string> GetRequest(string url)
        {
            WebRequest req = System.Net.WebRequest.Create(url);
            req.Headers.Add(string.Format("X-SessionKey:{0}", SessionKey));
            WebResponse resp = null;
            try
            {
                resp = await req.GetResponseAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                if (e is WebException)
                {
                    WebException ex = e as WebException;
                    if (true)
                    {
                        throw new UnauthorizedAccessException("Your session is not valid! Please log in again to renew it!");
                    }
                }
            }
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

        public async void ClearAllMedia()
        {
            await HttpRequest(RootAddress + "mediafolders/clear", HttpPut);
        }

        public async void DeleteFolder(string folderName)
        {
            await HttpRequest(RootAddress + "mediafolders/remove?location=" + folderName, HttpPut);
        }

        public async Task<string> GetAllFolders()
        {
            return await GetRequest(RootAddress + "mediafolders/getfolders");
        }

        public async Task<string> GetMediaHash()
        {
            return await GetRequest(RootAddress + "mediafolders/GetMediaHash");
        }

        public async Task ResetPassword(string email)
        {
            await HttpRequest(RootAddress + "user/ForgottenPassword?email=" + email, HttpPut);
        }
    }
}