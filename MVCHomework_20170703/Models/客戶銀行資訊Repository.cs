using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Web.Mvc;

namespace MVCHomework_20170703.Models
{
    public class 客戶銀行資訊Repository : EFRepository<客戶銀行資訊>, I客戶銀行資訊Repository
    {
        public override 客戶銀行資訊 Find(object id)
        {
            return base.All().Where(p => p.Id.Equals((int)id)).Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除).FirstOrDefault();
        }

        public override IQueryable<客戶銀行資訊> All()
        {
            return base.All().Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除);
        }

        public SelectList GetBankCodeList()
        {
            var bankSelectList = base.All()
            .Select(bank => new { Value = bank.銀行代碼, Text = bank.銀行代碼.ToString() })
            .Distinct()
            .ToList();

            bankSelectList.Insert(0, new { Value = -1, Text = "- 銀行代碼 -" });

            return new SelectList(bankSelectList, "Value", "Text");
        }
    }

    public interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
    {

    }
}