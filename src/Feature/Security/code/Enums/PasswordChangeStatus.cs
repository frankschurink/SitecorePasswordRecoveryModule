using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.PasswordRecovery.Feature.Security.Enums
{
    public enum PasswordChangeStatus
    {
        PasswordChanged,
        TokenNotFound,
        TokenFoundLinkExpired,
        TokenNotValid
    }
}