using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using Sitecore.Configuration;
using Sitecore.PasswordRecovery.Feature.Security.Enums;
using Sitecore.Security.Accounts;

namespace Sitecore.PasswordRecovery.Feature.Security.Helpers
{
    public class PasswordHelper
    {
        public static PasswordChangeStatus ChangePassword(string username, string token, string newPass)
        {
            var values = username.Split('|');
            var domainName = values[0];
            var userName = values[1];

            var user = UserHelper.GetUser(domainName, userName);
            var mUser = Membership.GetUser(user.Name);

            if (user.Profile.GetCustomProperty(Constants.PasswordRecoverToken) == token)
            {
                if (!string.IsNullOrWhiteSpace(user.Profile.GetCustomProperty(Constants.DateTimeRequest)))
                {
                    var dateTimeRequest = user.Profile.GetCustomProperty(Constants.DateTimeRequest);
                    var date = DateTime.ParseExact(dateTimeRequest, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    var difference = DateTime.Now - date;

                    var expirationHoursToken = Settings.GetSetting("ExpirationHoursToken");
                    var parseSuccessful = double.TryParse(expirationHoursToken, out var maxExpirationTime);

                    if (parseSuccessful)
                    {
                        if (difference.TotalHours < maxExpirationTime)
                        {
                            if (mUser != null)
                            {
                                if (mUser.IsLockedOut)
                                {
                                    mUser.UnlockUser();
                                }

                                var oldPassword = mUser.ResetPassword();
                                mUser.ChangePassword(oldPassword, newPass);

                                ResetCustomProperties(user);

                                return PasswordChangeStatus.PasswordChanged;
                            }
                        }
                        else
                        {
                            ResetCustomProperties(user);
                            return PasswordChangeStatus.TokenFoundLinkExpired;
                        }
                    }
                }
            }
            else
            {
                ResetCustomProperties(user);
                return PasswordChangeStatus.TokenNotValid;
            }

            return PasswordChangeStatus.TokenNotFound;
        }

        public static void ResetCustomProperties(User u)
        {
            u.Profile.SetCustomProperty(Constants.PasswordRecoverToken, string.Empty);
            u.Profile.Initialize(u.Name, true);
            u.Profile.Save();
        }

        public static bool IsValidPassword(string password)
        {
            var hasNonAlphanumericChar = new Regex(@"\W|_");
            var hasMinimumTenChars = new Regex(@".{10,}");

            return hasNonAlphanumericChar.IsMatch(password) && hasMinimumTenChars.IsMatch(password);
        }
    }
}