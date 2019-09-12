using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Sitecore.Creates.Feature.Security
{
    public struct Constants
    {
        public struct CustomProfileProperties
        {
            internal const string PasswordRecoverToken = "PasswordRecoverToken";
            internal const string DateTimeRequest = "NewPasswordRequest";
        }

        public struct SitecoreItemIds
        {
            internal static readonly ID PasswordRecoverySettings = new ID("{4B04EC19-2626-408B-9419-248E62CD42E7}");         
        }
    }
}