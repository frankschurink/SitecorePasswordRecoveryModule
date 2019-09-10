using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Sitecore.PasswordRecovery.Feature.Security
{
    public struct Templates
    {
        public struct PasswordReoverySettings
        {
            internal static readonly ID SenderEmailAddress = new ID("{1162A6FD-C1FF-45AF-BD5E-5A4FF8583C97}");
            internal static readonly ID EmailSubject = new ID("{29231C46-1DD8-4392-A1C3-2036CC634CDA}");
            internal static readonly ID EmailBody = new ID("{46A9A36F-65FB-4018-B546-255EF58F19D0}");
            internal static readonly ID TokenExpirationTime = new ID("{050D9516-1C36-4FBA-9F98-B191E55937F0}");

        }
    }
}