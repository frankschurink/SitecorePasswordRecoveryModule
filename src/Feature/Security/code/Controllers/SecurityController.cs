using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Globalization;
using Sitecore.Mvc.Controllers;
using Sitecore.PasswordRecovery.Feature.Security.Enums;
using Sitecore.PasswordRecovery.Feature.Security.Helpers;
using Sitecore.PasswordRecovery.Feature.Security.Models;

namespace Sitecore.PasswordRecovery.Feature.Security.Controllers
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
                switch (PasswordHelper.ChangePassword(username, token, viewModel.ConfirmPassword))
                {
                    case PasswordChangeStatus.PasswordChanged:
                        return View("~/Views/Feature/Security/PasswordRecoverSuccess.cshtml", viewModel);
                    case PasswordChangeStatus.TokenNotFound:
                        ModelState.AddModelError("", Translate.Text("PasswordReset.TokenNotFound"));
                        break;
                    case PasswordChangeStatus.TokenFoundLinkExpired:
                        ModelState.AddModelError("", Translate.Text("PasswordReset.LinkExpired"));
                        break;
                    case PasswordChangeStatus.TokenNotValid:
                        ModelState.AddModelError("", Translate.Text("PasswordReset.InvalidToken"));
                        break;
                }
            }

            return View("~/Views/Feature/Security/PasswordRecover.cshtml", viewModel);
        }
    }
}