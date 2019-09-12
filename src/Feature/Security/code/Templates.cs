using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Sitecore.Creates.Feature.Security
{
    public struct Templates
    {
        public struct PasswordReoverySettings
        {
            internal static readonly ID SenderEmailAddress = new ID("{1162A6FD-C1FF-45AF-BD5E-5A4FF8583C97}");
            internal static readonly ID EmailSubject = new ID("{29231C46-1DD8-4392-A1C3-2036CC634CDA}");
            internal static readonly ID EmailBody = new ID("{46A9A36F-65FB-4018-B546-255EF58F19D0}");
            internal static readonly ID TokenExpirationTime = new ID("{050D9516-1C36-4FBA-9F98-B191E55937F0}");
            internal static readonly ID PasswordLength = new ID("{05866409-5C5F-4FFC-B55E-A2DFDECC31CB}");
            internal static readonly ID NonAlphanumericCharaters = new ID("{AA59CA4F-289E-4B06-A62E-7FB848CAF935}");
            internal static readonly ID UpperCaseCharacters = new ID("{3D660B4A-814F-4259-921E-BF0A2CC86BFF}");
            internal static readonly ID LowerCaseCharacters = new ID("{9BCB67ED-789B-46E0-9744-48B2AE9C8FB5}");
            internal static readonly ID NumberCharacters = new ID("{BF3BF6E9-A4C3-42A7-9D56-3720190E1839}");         
        }
    }
}