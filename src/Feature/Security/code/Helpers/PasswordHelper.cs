using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Creates.Feature.Security.Enums;
using Sitecore.Security.Accounts;

namespace Sitecore.Creates.Feature.Security.Helpers
{
    public class PasswordHelper
    {
        public static PasswordChangeStatus ChangePassword(string username, string token, string newPass)
        {
            var values = username.Split(',');
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

                                    DeleteToken(user);
                                    Log.Info($"Password of user {user.Profile.UserName} has been changed successfully ", new PasswordHelper());

                                    return PasswordChangeStatus.PasswordChanged;
                                }
                            }
                            else
                            {
                                DeleteToken(user);
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
                DeleteToken(user);
                Log.Info($"Token not valid. Password of user {user.Profile.UserName} could not been changed.", new PasswordHelper());

                return PasswordChangeStatus.TokenNotValid;
            }
            Log.Info($"No token found. Password of user {user.Profile.UserName} could not been changed.", new PasswordHelper());

            return PasswordChangeStatus.TokenNotFound;
        }

        private static void DeleteToken(User u)
        {
            u.Profile.SetCustomProperty(Constants.CustomProfileProperties.PasswordRecoverToken, string.Empty);
            u.Profile.Initialize(u.Name, true);
            u.Profile.Save();
        }

        public static bool IsValidPassword(string password)
        {
            var passwordRecoverySettings = Context.Database.GetItem(Constants.SitecoreItemIds.PasswordRecoverySettings);
            if (passwordRecoverySettings != null)
            {
                Double.TryParse(passwordRecoverySettings.Fields[Templates.PasswordReoverySettings.PasswordLength].Value, out var passwordLength);
                var minimumCharsRegex = new Regex(@".{" + passwordLength + @",}");
                   
                Double.TryParse(passwordRecoverySettings.Fields[Templates.PasswordReoverySettings.UpperCaseCharacters].Value, out var upperCaseLength);
                var upperCaseRegex = new Regex(@"[A-Z]{" + upperCaseLength + @",}");

                Double.TryParse(passwordRecoverySettings.Fields[Templates.PasswordReoverySettings.LowerCaseCharacters].Value, out var lowerCaseLength);
                var lowerCaseRegex = new Regex(@"[a-z]{" + lowerCaseLength + @",}");
                
                Double.TryParse(passwordRecoverySettings.Fields[Templates.PasswordReoverySettings.NonAlphanumericCharaters].Value, out var nonAlphanumericLength);
                var nonAlphanumericRegex = new Regex(@"\W|_{" + nonAlphanumericLength + @",}");

                Double.TryParse(passwordRecoverySettings.Fields[Templates.PasswordReoverySettings.NumberCharacters].Value, out var numberLength);

                return minimumCharsRegex.IsMatch(password) && upperCaseRegex.IsMatch(password) &&
                       lowerCaseRegex.IsMatch(password) && nonAlphanumericRegex.IsMatch(password) && MatchNumberPolicy(password, numberLength);
            }

            Log.Info("Password Recovery Settings item not found.", new PasswordHelper());

            return false;
        }

        private static bool MatchNumberPolicy(string password, double numberLength)
        {
            double digits = 0;
            foreach (char c in password)
            {
                if (char.IsDigit(c))
                {
                    digits++;
                }
            }

            return digits >= numberLength;
        }
    }
}