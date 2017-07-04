using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCHomework_20170703.Models.ViewModels
{
    public class LoginViewModel : IValidatableObject
    {
        [Required]
        [DisplayName("帳號")]
        public string UserName { get; set; }

        [Required]
        [DisplayName("密碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.UserName == "123" && this.Password == "123")
            {
                yield break;
            }
            else
            {
                yield return new ValidationResult("登入帳號或密碼錯誤", new string[] { "UserName" });
            }
        }
    }
}