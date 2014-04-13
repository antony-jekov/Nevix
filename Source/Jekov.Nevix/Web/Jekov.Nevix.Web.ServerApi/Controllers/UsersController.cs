using Jekov.Nevix.Common.Models;
using Jekov.Nevix.Web.ServerApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace Jekov.Nevix.Web.ServerApi.Controllers
{
    public class UsersController : BaseController
    {
        public ActionResult Confirm(string id)
        {
            ConfirmUser confirm = Data.ConfirmUser.All().FirstOrDefault(c => c.SecretKey == id);

            if (confirm != null)
            {
                Data.ConfirmUser.Delete(confirm);

                Data.SaveChanges();
                ViewBag.confirmed = true;
            }
            else
            {
                ViewBag.confirmed = false;
            }

            return View();
        }

        public ActionResult ResetPass(string id)
        {
            ForgottenPasswordRequest passReq = Data.ForgottenPasswordRequests.All().FirstOrDefault(r => r.SecretKey == id);

            if (passReq == null)
            {
                ViewBag.valid = false;
            }
            else
            {
                ViewBag.valid = true;
            }

            ResetPassViewModel resetModel = new ResetPassViewModel();
            resetModel.SecretKey = id;

            return View(resetModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Edit (ResetPassViewModel model)
        {
            if (ModelState.IsValid)
            {
                ForgottenPasswordRequest resetRequest = Data.ForgottenPasswordRequests.All().FirstOrDefault(r => r.SecretKey == model.SecretKey);

                if (resetRequest != null)
                {
                    NevixUser user = resetRequest.User;
                    user.Password = StringToSha1(model.Password);
                    Data.ForgottenPasswordRequests.Delete(resetRequest);
                    Data.SaveChanges();
                }
            }

            return Redirect("/");
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