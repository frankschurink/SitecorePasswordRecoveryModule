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
            var token = args.CustomData[Constants.CustomProfileProperties.PasswordRecoverToken] as string;
            if (string.IsNullOrEmpty(token))
            {
                return;
            }

            var webDbName = Settings.GetSetting("WebDatabaseName");
            if (!string.IsNullOrWhiteSpace(webDbName))
            {
                var webDb = Sitecore.Data.Database.GetDatabase(webDbName);
                var passwordRecoverySettings = webDb.GetItem(Constants.SitecoreItemIds.PasswordRecoverySettings);
                if (passwordRecoverySettings != null)
                {
                    var subject = passwordRecoverySettings.Fields[Templates.PasswordReoverySettings.EmailSubject].Value;
                    var sender = passwordRecoverySettings.Fields[Templates.PasswordReoverySettings.SenderEmailAddress].Value;

                    var emailReceiver = args.UserEmail;
                    var user = User.FromName(args.Username, false);

                    var confirmLink = GeneratePasswordRecoverLink(token, args.Username);
                    var username = args.Username.Substring(args.Username.LastIndexOf('\\') + 1);
                    var body = string.Format(passwordRecoverySettings.Fields[Templates.PasswordReoverySettings.EmailBody].Value, username, confirmLink);

                    try
                    {
                        var emailWithLink = new MailMessage(sender, emailReceiver, subject, body)
                        {
                            BodyEncoding = Encoding.UTF8,
                            IsBodyHtml = true
                        };
                        MainUtil.SendMail(emailWithLink);
                        Log.Info($"Password recovery mail send to user: {args.Username}", this);
                    }
                    catch (Exception ex)
                    {
                        Log.Error($"Error when sending password recovery mail to user: {args.Username} | Exception: {ex.Message}", this);
                    }
                }
            }       
            
            args.AbortPipeline();
        }

        protected virtual string GeneratePasswordRecoverLink(string token, string userName)
        {
            var serverUrl = StringUtil.EnsurePostfix('/', WebUtil.GetServerUrl());
            return serverUrl + "sitecore/api/security/recoverpassword/" + userName.Replace('\\', '|') + "/" + token;
        }    
    }
}