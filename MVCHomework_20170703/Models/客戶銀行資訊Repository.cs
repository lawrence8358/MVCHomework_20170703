using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.Web.Mvc;
using System.Linq.Expressions;
using MVCHomework_20170703.Models.ViewModels;
using System.Data.Entity;

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
            return base.All().Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除).Include(客 => 客.客戶資料).OrderBy(c => c.客戶Id);
        }

        public IQueryable<客戶銀行資訊> All(QueryCustomerBankViewModel queryModel, string sortName = "", string currentSortName = "")
        {
            var queryList = base.All();

            queryList = queryList.Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除);

            if (queryModel.BankId > 0)
                queryList = queryList.Where(p => p.銀行代碼.Equals(queryModel.BankId));
            if (!string.IsNullOrEmpty(queryModel.CustomerName))
                queryList = queryList.Where(p => p.客戶資料.客戶名稱.Contains(queryModel.CustomerName));

            queryList = queryList.Include(客 => 客.客戶資料);

            switch (currentSortName)
            {
                case "customerName":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.客戶資料.客戶名稱);
                    else
                        queryList = queryList.OrderBy(c => c.客戶資料.客戶名稱);
                    break;

                case "bankName":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.銀行名稱);
                    else
                        queryList = queryList.OrderBy(c => c.銀行名稱);
                    break;

                case "bankCode":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.銀行代碼);
                    else
                        queryList = queryList.OrderBy(c => c.銀行代碼);
                    break;

                case "bankSubCode":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.分行代碼);
                    else
                        queryList = queryList.OrderBy(c => c.分行代碼);
                    break;

                case "accountName":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.帳戶名稱);
                    else
                        queryList = queryList.OrderBy(c => c.帳戶名稱);
                    break;

                case "account":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.帳戶號碼);
                    else
                        queryList = queryList.OrderBy(c => c.帳戶號碼);
                    break;
                     
                default:
                    queryList = queryList.OrderBy(c => c.客戶Id);
                    break;
            } 

            return queryList;
        }

        public SelectList GetBankCodeList()
        {
            var bankSelectList = base.All()
             .Where(p => !p.是否已刪除)
            .Select(bank => new { Value = bank.銀行代碼, Text = bank.銀行代碼.ToString() })
            .Distinct()
            .ToList();

            bankSelectList.Insert(0, new { Value = -1, Text = "< 全部 >" });

            return new SelectList(bankSelectList, "Value", "Text");
        }

        public void Create(客戶銀行資訊 客戶銀行資訊)
        {
            this.Add(客戶銀行資訊);
            this.UnitOfWork.Commit(); 
        }

        public void Modify(客戶銀行資訊 客戶銀行資訊)
        { 
            this.UnitOfWork.Context.Entry(客戶銀行資訊).State = EntityState.Modified;
            this.UnitOfWork.Commit();
        }

        public void Delete(int? id)
        { 
            客戶銀行資訊 客戶銀行資訊 = this.Find(id);
            客戶銀行資訊.是否已刪除 = true;
            this.UnitOfWork.Context.Entry(客戶銀行資訊).State = EntityState.Modified;
            this.UnitOfWork.Commit();
            //this.Delete(客戶銀行資訊);
            //this.UnitOfWork.Commit();
        }
    }

    public interface I客戶銀行資訊Repository : IRepository<客戶銀行資訊>
    {

    }
}