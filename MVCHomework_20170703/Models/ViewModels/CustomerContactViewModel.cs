using MVCHomework_20170703.Models.InputValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCHomework_20170703.Models.ViewModels
{
    public class QueryCustomerContactViewModel
    {
        public string CustomerName { get; set; }
        public string CustomerContactName { get; set; }
        public string JobTitle { get; set; }
    }

    public class CustomerContactBatchViewModel
    {  
        [Required]
        public int Id { get; set; } 

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        public string 職稱 { get; set; } 

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        //[RegularExpression(@"^09\d{2}-\d{6}$", ErrorMessage = "手機格式必須為09xx-xxxxxx")] //簡單的驗證可以直接使用正則表達示即可
        [ValidateTaiwanMobile(ErrorMessage = "手機格式必須為09xx-xxxxxx")]
        public string 手機 { get; set; }

        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string 電話 { get; set; } 
    }
}