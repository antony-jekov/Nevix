namespace Jekov.Nevix.Desktop.Common
{
    using Jekov.Nevix.Common.ViewModels;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;

    public class PersisterManager
    {
        private const string RootAddress = "http://localhost:50906/api/";
        private const string HttpPost = "POST";
        private const string HttpPut = "PUT";

        public string SessionKey { get; protected set; }

        public void Login(string email, string password)
        {
            UserLoginViewModel loginData = new UserLoginViewModel
            {
                Email = email.ToLower(),
                Password = StringToSha1(password)
            };

            string serializedRequestBody = JsonConvert.SerializeObject(loginData);

            SessionKey = HttpRequest(RootAddress + "user/login", HttpPut, serializedRequestBody).Trim(new char[]{'"', '\\'});
        }

        public void LogOff()
        {
            HttpRequest(RootAddress + "user/logoff", HttpPut);
        }

        public void Register(string email, string password, string confirmPassword)
        {
            UserRegisterViewModel registerData = new UserRegisterViewModel
            {
                Email = email.ToLower(),
                Password = StringToSha1(password),
                ConfirmPassword = StringToSha1(confirmPassword)
            };

            string serializedRequestBody = JsonConvert.SerializeObject(registerData);

            SessionKey = HttpRequest(RootAddress + "user/register", HttpPost, serializedRequestBody).Trim(new char[] { '"', '\\' });
        }

        public void SendMediaDatabase(MediaFolderViewModel rootFolder)
        {
            string requestBody = JsonConvert.SerializeObject(rootFolder);

            string result = HttpRequest(RootAddress + "mediafolders/addfolder", HttpPost, requestBody);
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
            WebResponse resp = req.GetResponse();

            if (resp == null) return null;

            StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        protected string GetRequest(string url)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        protected string StringToSha1(string text)
        {
            System.Text.ASCIIEncoding encoder = new System.Text.ASCIIEncoding();

            byte[] buffer = encoder.GetBytes(text);
            SHA1CryptoServiceProvider cryptoTransformSHA1 = new SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }
    }
}