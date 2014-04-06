namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    using Jekov.Nevix.Common.Models;
    using Jekov.Nevix.Common.ViewModels;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;

    public class UserController : BaseApiController
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
                return Request.CreateResponse(HttpStatusCode.Conflict, "There is already a user with that email!");
            }

            if (!model.Password.Equals(model.ConfirmPassword))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Passwords do not match!");
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
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Wrong email or password!");
            }

            if (user.SessionKey == null)
            {
                user.SessionKey = Guid.NewGuid().ToString();
                Data.SaveChanges();
            }

            return Request.CreateResponse(HttpStatusCode.OK, user.SessionKey);
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