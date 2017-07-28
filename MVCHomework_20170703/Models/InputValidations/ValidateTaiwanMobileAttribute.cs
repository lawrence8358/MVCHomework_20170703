using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MVCHomework_20170703.Models.InputValidations
{
    public class ValidateTaiwanMobileAttribute : DataTypeAttribute
    {
        public ValidateTaiwanMobileAttribute() : base(DataType.Text)
        {
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            return Regex.IsMatch((string)value, @"^09\d{2}-\d{6}$");
        }
    }
}