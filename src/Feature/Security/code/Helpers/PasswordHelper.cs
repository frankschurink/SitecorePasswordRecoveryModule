using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
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

            if (user.Profile.GetCustomProperty(Constants.CustomProfileProperties.PasswordRecoverToken) == token)
            {
                if (!string.IsNullOrWhiteSpace(user.Profile.GetCustomProperty(Constants.CustomProfileProperties.DateTimeRequest)))
                {
                    DateTime.TryParse(user.Profile.GetCustomProperty(Constants.CustomProfileProperties.DateTimeRequest), out var dateTimeRequest);
                    var difference = DateTime.Now - dateTimeRequest;

                    var passwordRecoverySettings = Context.Database.GetItem(Constants.SitecoreItemIds.PasswordRecoverySettings);
                    if (passwordRecoverySettings != null)
                    {
                        var success = Double.TryParse(passwordRecoverySettings.Fields[Templates.PasswordReoverySettings.TokenExpirationTime].Value, out var expirationHoursToken);

                        if (success)
                        {
                            if (difference.TotalHours < expirationHoursToken)
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
                                    Log.Info($"Password of user {user.Profile.UserName} has been changed successfully ", new PasswordHelper());

                                    return PasswordChangeStatus.PasswordChanged;
                                }
                            }
                            else
                            {
                                ResetCustomProperties(user);
                                Log.Info($"Token expired. Password of user {user.Profile.UserName} could not been changed.", new PasswordHelper());

                                return PasswordChangeStatus.TokenFoundLinkExpired;
                            }
                        }
                        else
                        {
                            Log.Info($"Error in parsing value of token expiration time.", new PasswordHelper());
                        }
                    }
                    else
                    {
                        Log.Info($"Password Recovery Settings item not published. Password of user {user.Profile.UserName} could not been changed.", new PasswordHelper());
                    }
                }
            }
            else
            {
                ResetCustomProperties(user);
                Log.Info($"Token not valid. Password of user {user.Profile.UserName} could not been changed.", new PasswordHelper());

                return PasswordChangeStatus.TokenNotValid;
            }
            Log.Info($"No token found. Password of user {user.Profile.UserName} could not been changed.", new PasswordHelper());

            return PasswordChangeStatus.TokenNotFound;
        }

        public static void ResetCustomProperties(User u)
        {
            u.Profile.SetCustomProperty(Constants.CustomProfileProperties.PasswordRecoverToken, string.Empty);
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