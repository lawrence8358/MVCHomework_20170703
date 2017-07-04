namespace MVCHomework_20170703.Models
{
    using InputValidations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    [MetadataType(typeof(客戶聯絡人MetaData))]
    public partial class 客戶聯絡人 : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            客戶聯絡人Repository customerContactRepo = RepositoryHelper.Get客戶聯絡人Repository();
            var item = customerContactRepo.All()
                .Where(p => !p.Id.Equals(this.Id)) //排除自己
                .Where(p => !p.是否已刪除) //刪除的不要列入
                .Where(p => p.客戶Id.Equals(this.客戶Id)) //相同客戶
                .Where(p=> p.Email.Equals(this.Email))
                .FirstOrDefault(); 

            if (item == null) yield break;
            else yield return new ValidationResult("同一客戶下電子郵件不允許重複", new string[] { "Email" });
        }
    }

    public partial class 客戶聯絡人MetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int 客戶Id { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        public string 姓名 { get; set; }

        [StringLength(250, ErrorMessage = "欄位長度不得大於 250 個字元"), EmailAddress()]
        [Required]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        //[RegularExpression(@"^09\d{2}-\d{6}$", ErrorMessage = "手機格式必須為09xx-xxxxxx")] //簡單的驗證可以直接使用正則表達示即可
        [ValidateTaiwanMobile(ErrorMessage = "手機格式必須為09xx-xxxxxx")]
        public string 手機 { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; }

        public virtual 客戶資料 客戶資料 { get; set; }
    }
}
