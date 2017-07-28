namespace MVCHomework_20170703.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(View_客戶資料統計MetaData))]
    public partial class View_客戶資料統計
    {
    }
    
    public partial class View_客戶資料統計MetaData
    {
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 客戶名稱 { get; set; }
        public Nullable<int> 聯絡人數量 { get; set; }
        public Nullable<int> 銀行帳戶數量 { get; set; }
        [Required]
        public int 客戶Id { get; set; }
    }
}
