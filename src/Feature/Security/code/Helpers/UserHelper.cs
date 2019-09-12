using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;

namespace Sitecore.Creates.Feature.Security.Helpers
{
    public class UserHelper
    {
        public static User GetUser(string domainName, string userName)
        {
            if (User.Exists(domainName + @"\" + userName))
            {
                return User.FromName(domainName + @"\" + userName, true);
            }
            return null;
        }
        
        public static void SetProfileProperty(Sitecore.Security.Accounts.User user, string propertyName, string value)
        {
            using (new SecurityDisabler())
            {
                Sitecore.Security.Accounts.User authUser = Sitecore.Security.Accounts.User.FromName(user.Profile.UserName, true);
                authUser.Profile.SetCustomProperty(propertyName, value);
                authUser.Profile.Save();
            }
        }
    }
}