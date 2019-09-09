using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Sitecore.Globalization;

namespace Sitecore.PasswordRecovery.Feature.Security.Models
{
    public class RegularExpressionAttribute : ValidationAttribute
    {
        private readonly string _regularExpression;
        private readonly string _translateKey;

        public RegularExpressionAttribute(string regularExpression, string translateKey)
        {
            _regularExpression = regularExpression;
            _translateKey = translateKey;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var reg = new Regex(_regularExpression);

            var stringValue = (string)value;

            if (string.IsNullOrWhiteSpace(stringValue))
                return new ValidationResult(Translate.Text(_translateKey));

            var m1 = reg.Match(stringValue);
            return !m1.Success
                ? new ValidationResult(Translate.Text(_translateKey))
                : ValidationResult.Success;
        }
    }
}