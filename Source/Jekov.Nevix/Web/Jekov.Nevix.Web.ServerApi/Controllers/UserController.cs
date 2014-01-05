namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    using Jekov.Nevix.Common.ViewModels;
    using Jekov.Nevix.Common.Models;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class UserController : BaseController
    {
        /// <summary>
        /// Registers a new user account with the values passed in the model.
        /// </summary>
        /// <param name="model">Data values required for the new user.</param>
        /// <returns>Session key</returns>
        [HttpPost]
        public HttpResponseMessage Register([FromBody]UserRegisterViewModel model)
        {
            if (Data.Users.All().Any(u => u.Email.ToLower().Equals(model.Email.ToLower())))
            {
                var error = new ArgumentException("Email", "There is already a user with that email!");
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, error);
            }

            if (!model.Password.Equals(model.ConfirmPassword))
            {
                var error = new ArgumentException("confirmPassword", "Passwords do not match!");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }

            NevixUser newUser = new NevixUser
            {
                Email = model.Email,
                Password = model.Password,
                SessionKey = Guid.NewGuid().ToString()
            };

            Data.Users.Add(newUser);
            Data.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.Created, newUser.SessionKey);
        }

        [HttpPut]
        public HttpResponseMessage Login([FromBody]UserLoginViewModel model)
        {
            NevixUser user = Data.Users.All().FirstOrDefault(u => u.Email.ToLower().Equals(model.Email.ToLower()) && u.Password == model.Password);

            if (user == null)
            {
                var error = new ArgumentException("Wrong email or password!");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }

            user.SessionKey = Guid.NewGuid().ToString();
            Data.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, user.SessionKey);
        }

        [HttpPut]
        public HttpResponseMessage LogOff()
        {
            NevixUser user = GetCurrentUser();

            if (user == null)
            {
                return UnauthorizedErrorMessage();
            }

            user.SessionKey = null;
            Data.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
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

        [HttpPut]
        public HttpResponseMessage UpdateChannel(ChannelViewModel model)
        {
            if (model == null)
            {
                return NullModelErrorMessage();
            }

            NevixUser currentUser = GetCurrentUser();

            if (currentUser == null)
            {
                return UnauthorizedErrorMessage();
            }

            currentUser.ChannelName = model.Name;
            Data.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        public HttpResponseMessage GetChannel()
        {
            //NevixUser currentUser = GetCurrentUser();
            //if (currentUser == null)
            //{
            //    return UnauthorizedErrorMessage();
            //}

            NevixUser currentUser = Data.Users.All().FirstOrDefault();

            return Request.CreateResponse(HttpStatusCode.OK, currentUser.ChannelName);
        }
    }
}