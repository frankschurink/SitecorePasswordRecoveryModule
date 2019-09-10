using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Sitecore.PasswordRecovery.Feature.Security
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
            internal static readonly ID PasswordRecoverySettings = new ID("{ACCF79D2-8631-46CB-9B82-D326CCF353D8}");         
        }
    }
}