using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Profile;
using Sitecore.Diagnostics;
using Sitecore.PasswordRecovery.Feature.Security.Helpers;
using Sitecore.Pipelines.PasswordRecovery;
using Sitecore.Security.Accounts;

namespace Sitecore.PasswordRecovery.Feature.Security.Infrastructure
{
    public class GenerateToken : PasswordRecoveryProcessor
    {
        public override void Process(PasswordRecoveryArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            var user = User.FromName(args.Username, true);
            if (user == null)
            {
                args.AbortPipeline();
                return;
            }

            var token = Guid.NewGuid().ToString();

            UserHelper.SetProfileProperty(user, Constants.PasswordRecoverToken, token);
            UserHelper.SetProfileProperty(user, Constants.DateTimeRequest, DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));

            args.CustomData.Add(Constants.PasswordRecoverToken, token);
        }
    }
}