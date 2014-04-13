namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    using Jekov.Nevix.Common.Models;
    using Jekov.Nevix.Common.ViewModels;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Mail;
    using System.Text;
    using System.Web.Http;

    public class UserController : BaseApiController
    {
        private const string SignEmail =
@"<br />
<p>Have a nice day!</p>
<p>The NeviX Team</p>
<img alt='nevix logo' title='NeviX Remote Control' width='160px' src='http://antonyjekov.com/nevix/NevixLogo.png' />";

        private const string GreetEmail =
@"<h2 style='color:#555257'><img src='http://antonyjekov.com/nevix/NevixLogoOnly16.png' /> Hello {0},</h2>";

        /// <summary>
        /// Registers a new user account with the values passed in the model.
        /// </summary>
        /// <param name="model">Data values required for the new user.</param>
        /// <returns>Session key</returns>
        [HttpPost]
        public HttpResponseMessage Register([FromBody]UserRegisterViewModel model)
        {
            try
            {
                if (Data.Users.All().Any(u => u.Email.ToLower().Equals(model.Email.ToLower())))
                {
                    return Request.CreateResponse(HttpStatusCode.Conflict, "There is already an user with that email!");
                }

                if (!model.Password.Equals(model.ConfirmPassword))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, "Passwords do not match!");
                }

                NevixUser newUser = new NevixUser
                {
                    Email = model.Email.ToLower(),
                    Password = model.Password,
                    SessionKey = Guid.NewGuid().ToString()
                };

                Data.Users.Add(newUser);

                string secretKey = Guid.NewGuid().ToString().Replace("-", "");
                Data.ConfirmUser.Add(new ConfirmUser(newUser, secretKey));

                //try
                //{
                SendActivationEmail(newUser.Email, secretKey);
                Data.SaveChanges();
                //}
                //catch (Exception)
                //{
                //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Email address could not be reached!");
                //}

                return Request.CreateResponse(HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                SendEmail("antony.jekov@gmail.com", "NeviX EXCEPTION", ex.Message);
            }
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        private void SendActivationEmail(string emailTo, string secretKey)
        {
            string message = String.Format(
@"{0}
<h3>Welcome to the NeviX family!</h3>
<p>We are excited to have you and we will make our best to offer you a problem free service.</p>
<p>Please take the time to visit this address in order to activate your account and to start using our services: <br /><b>{1}</b><br />
Security matters to us, so we need to verify that this email address is real and that it belongs to you.</p>
<p>Thank you for your time and happy media browsing!</p>
{2}",
    String.Format(GreetEmail, emailTo.Substring(0, emailTo.IndexOf("@"))),
    String.Format("http://nevix.antonyjekov.com/users/confirm/{0}", secretKey),
    SignEmail);

            SendEmail(emailTo, "Activate Your Account", message);
        }

        //[HttpGet]
        //public HttpResponseMessage Test()
        //{
        //    string r = "lainaci";
        //    try
        //    {
        //        SendForgottenEmail("antony.jekov@gmail.com", "batman");
        //    }
        //    catch (Exception ex)
        //    {
        //        r = ex.Message;
        //    }

        //    return Request.CreateResponse(HttpStatusCode.OK, r);
        //}

        [HttpPut]
        public HttpResponseMessage Login([FromBody]UserLoginViewModel model)
        {
            NevixUser user = Data.Users.All().FirstOrDefault(u => u.Email.ToLower().Equals(model.Email.ToLower()) && u.Password == model.Password);

            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Wrong email or password!");
            }

            if (Data.ConfirmUser.All().Any(u => u.UserId == user.Id))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Email address for this accoumt is not confirmed!");
            }

            //if (user.SessionKey == null)
            //{
            //    user.SessionKey = Guid.NewGuid().ToString();
            //    Data.SaveChanges();
            //}

            return Request.CreateResponse(HttpStatusCode.OK, user.SessionKey);
        }

        [HttpPut]
        public HttpResponseMessage ForgottenPassword(string email)
        {
            NevixUser user = Data.Users.All().FirstOrDefault(u => u.Email.ToLower() == email.ToLower());

            if (user != null)
            {
                ForgottenPasswordRequest request = Data.ForgottenPasswordRequests.All().FirstOrDefault(r => r.User.Email == email.ToLower());

                string secretKey = string.Empty;
                if (request == null)
                {
                    secretKey = Guid.NewGuid().ToString().Replace("-", "");
                    Data.ForgottenPasswordRequests.Add(new ForgottenPasswordRequest(user, secretKey));
                }
                else
                {
                    secretKey = request.SecretKey;
                }

                try
                {
                    SendForgottenEmail(email, secretKey);

                    if (request == null)
                    {
                        Data.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    SendEmail("antony.jekov@gmail.com", "NeviX EXCEPTION", ex.Message);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private void SendForgottenEmail(string emailTo, string secretKey)
        {
            string message = String.Format(
@"{0}
<p>You have requested your password to be changed.</p>
<p>Please follow this link to proceed with the change: <b>{1}</b></p>
{2}",
    String.Format(GreetEmail, emailTo.Substring(0, emailTo.IndexOf("@"))),
    String.Format("http://nevix.antonyjekov.com/users/resetpass/{0}", secretKey),
    SignEmail
    );

            SendEmail(emailTo, "Requested Password Change", message);
        }

        private void SendEmail(string emailTo, string title, string msg)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("nevix@antonyjekov.com", "NeviX Remote Control");
            message.To.Add(new MailAddress(emailTo, "NeviX User"));
            message.Subject = title;
            message.IsBodyHtml = true;
            message.Body = msg;

            SmtpClient client = new SmtpClient();
            client.Host = "antonyjekov.com";
            client.Port = 25;
            client.Credentials = new System.Net.NetworkCredential("nevix@antonyjekov.com", "MeLlOn!@#");

            client.Send(message);
        }

        [HttpGet]
        public HttpResponseMessage LastMediaUpdate()
        {
            NevixUser currentUser = GetCurrentUser();

            if (currentUser == null)
            {
                return UnauthorizedErrorMessage();
            }

            return Request.CreateResponse(HttpStatusCode.OK, currentUser.LastFilesUpdate);
        }
    }
}