using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sitecore.Globalization;

namespace Sitecore.Creates.Feature.Security.Models
{
    public class CustomRequiredAttribute : ValidationAttribute
    {
        private readonly string _translateKey;

        public CustomRequiredAttribute()
        {
        }

        public CustomRequiredAttribute(string translateKey)
        {
            _translateKey = translateKey;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return string.IsNullOrWhiteSpace((string)value)
                ? new ValidationResult(Translate.Text(this._translateKey))
                : ValidationResult.Success;
        }
    }
}