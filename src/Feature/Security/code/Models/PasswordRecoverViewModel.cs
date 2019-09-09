using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.PasswordRecovery.Feature.Security.Models
{
    public class PasswordRecoverViewModel
    {
        public PasswordRecoverViewModel()
        {
        }

        [CustomRequired("PasswordReset.NewPassRequired")]
        [DataType(DataType.Password)]
        [RegularExpression("((?=.*\\W|_).{10,})", "PasswordReset.NoStrongPass")]
        public string NewPassword { get; set; }

        [CustomRequired("PasswordReset.ConfirmPassRequired")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}