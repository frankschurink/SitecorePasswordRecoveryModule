using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.PasswordRecovery;
using Sitecore.Security.Accounts;
using Sitecore.Web;

namespace Sitecore.PasswordRecovery.Feature.Security.Infrastructure
{
    public class PopulatePasswordRecoverMail : PopulateMail
    {
        public override void Process(PasswordRecoveryArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            var token = args.CustomData[Constants.PasswordRecoverToken] as string;
            if (string.IsNullOrEmpty(token))
                return;

            var sender = Settings.GetSetting("PasswordRecoveryFromMail");
            var emailReceiver = args.UserEmail;
            var subject = Settings.GetSetting("PasswordRecoveryMailSubject");
            var user = User.FromName(args.Username, false);
            var confirmLink = GeneratePasswordRecoverLink(token, args.Username);

            var emailWithLink = new MailMessage(sender, emailReceiver, subject, GetHtmlEmailContent(user, confirmLink))
            {
                BodyEncoding = Encoding.UTF8,
                IsBodyHtml = true
            };

            MainUtil.SendMail(emailWithLink);
            args.AbortPipeline();
        }

        protected virtual string GeneratePasswordRecoverLink(string token, string userName)
        {
            var serverUrl = StringUtil.EnsurePostfix('/', WebUtil.GetServerUrl());
            return serverUrl + "sitecore/api/security/recoverpassword/" + userName.Replace('\\', '|') + "/" + token;
        }

        protected virtual string GetHtmlEmailContent(User user, string confirmLink)
        {
            var textLineOne = Settings.GetSetting("PasswordRecoveryMailBody1");
            var textLineTwo = Settings.GetSetting("PasswordRecoveryMailBody2");
            var textLineThree = Settings.GetSetting("PasswordRecoveryMailBody3");
            var textLineFour = Settings.GetSetting("PasswordRecoveryMailBody4");

            var sb = new StringBuilder();
            sb.AppendLine("<html><head><title>");
            sb.AppendLine("Sitecore wachtwoord herstellen");
            sb.AppendLine("</title></head><body>");
            sb.AppendLine("<p>" + textLineOne + "<br/>" + textLineTwo + "<br/><br/>" + textLineThree + "</p>");
            sb.AppendLine("<a href=\"" + confirmLink + "\">" + confirmLink + "</a>");
            sb.AppendLine("<p>" + textLineFour + "</p>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");
            return sb.ToString();
        }
    }
}