using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Globalization;
using Sitecore.Mvc.Controllers;
using Sitecore.Creates.Feature.Security.Enums;
using Sitecore.Creates.Feature.Security.Helpers;
using Sitecore.Creates.Feature.Security.Models;

namespace Sitecore.Creates.Feature.Security.Controllers
{
    public class SecurityController : SitecoreController
    {
        [HttpGet]
        public ActionResult RecoverPassword(string userName, string token)
        {
            var viewModel = new PasswordRecoverViewModel();
            return View("~/Views/Feature/Security/PasswordRecover.cshtml", viewModel);
        }

        [HttpPost]
        public ActionResult RecoverPassword(string username, string token, PasswordRecoverViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                ModelState.AddModelError("", Translate.Text("PasswordReset.TokenRequired"));
            }

            if (ModelState.IsValid)
            {
                if (PasswordHelper.IsValidPassword(viewModel.ConfirmPassword))
                {
                    switch (PasswordHelper.ChangePassword(username, token, viewModel.ConfirmPassword))
                    {
                        case PasswordChangeStatus.PasswordChanged:
                            return View("~/Views/Feature/Security/PasswordRecoverSuccess.cshtml", viewModel);
                        case PasswordChangeStatus.TokenNotFound:
                            ModelState.AddModelError("", "Something went wrong. Please contact your administrator.");
                            break;
                        case PasswordChangeStatus.TokenFoundLinkExpired:
                            ModelState.AddModelError("", "Your password reset link is expired. Request a new password via Sitecore and try again with the new link.");
                            break;
                        case PasswordChangeStatus.TokenNotValid:
                            ModelState.AddModelError("", "Invalid token used. Request a new password via Sitecore and try again with the new link.");
                            break;
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The chosen password does not match the password policy. Please choose a new password.");
                }
            }

            return View("~/Views/Feature/Security/PasswordRecover.cshtml", viewModel);
        }
    }
}