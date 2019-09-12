using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Creates.Feature.Security.Models
{
    public class PasswordRecoverViewModel
    {
        public PasswordRecoverViewModel()
        {
        }

        [Required(ErrorMessage = "The field 'New password' is required")]
        //You can sitecore dictionary items for the error message text instead of hardcoded text
        //[CustomRequired("")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "The field 'New password' is required")]
        //You can sitecore dictionary items for the error message text instead of hardcoded text
        //[CustomRequired("")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}