using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using MVCHomework_20170703.Models.ViewModels;
using System.Data.Entity;
using System.Web.Mvc;

namespace MVCHomework_20170703.Models
{
    public class 客戶資料Repository : EFRepository<客戶資料>, I客戶資料Repository
    {
        public override 客戶資料 Find(object id)
        {
            return base.All().Where(p => p.Id.Equals((int)id)).Where(p => !p.是否已刪除).FirstOrDefault();
        }

        public 客戶資料 FindByAccount(string account)
        {
            return base.All().Where(p => p.帳號.Equals(account)).Where(p => !p.是否已刪除).FirstOrDefault();
        }

        public 客戶資料 Login(string account, string password)
        {
            var data = base.All().Where(p => p.帳號.Equals(account)).Where(p => !p.是否已刪除).FirstOrDefault();

            if (data != null)
            {
                if ((System.Web.Helpers.Crypto.Hash(password).Equals(data.密碼))) return data; 
            }

            return null;
        }


        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(p => !p.是否已刪除).OrderBy(c => c.Id);
        }

        public IQueryable<客戶資料> All(QueryCustomerViewModel queryModel, string sortName = "", string currentSortName = "")
        {
            var queryList = base.All();

            queryList = queryList.Where(p => !p.是否已刪除);

            if (!string.IsNullOrEmpty(queryModel.CustomerName))
                queryList = queryList.Where(p => p.客戶名稱.Contains(queryModel.CustomerName));
            if (!string.IsNullOrEmpty(queryModel.CustomerType))
                queryList = queryList.Where(p => p.客戶分類.Equals(queryModel.CustomerType));

            switch (currentSortName)
            {
                case "customerName":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.客戶名稱);
                    else
                        queryList = queryList.OrderBy(c => c.客戶名稱);
                    break;

                case "customerType":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.客戶分類);
                    else
                        queryList = queryList.OrderBy(c => c.客戶分類);
                    break;

                case "texNo":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.統一編號);
                    else
                        queryList = queryList.OrderBy(c => c.統一編號);
                    break;

                case "phone":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.電話);
                    else
                        queryList = queryList.OrderBy(c => c.電話);
                    break;

                case "fax":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.傳真);
                    else
                        queryList = queryList.OrderBy(c => c.傳真);
                    break;

                case "address":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.地址);
                    else
                        queryList = queryList.OrderBy(c => c.地址);
                    break;

                case "email":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.Email);
                    else
                        queryList = queryList.OrderBy(c => c.Email);
                    break;

                default:
                    queryList = queryList.OrderBy(c => c.Id);
                    break;
            }

            return queryList;
        }

        public SelectList GetCustomerTypeList()
        {
            var selectList = base.All()
            .Where(p => !p.是否已刪除)
            .Select(p => new { Value = p.客戶分類, Text = p.客戶分類 })
            .Distinct()
            .ToList();

            selectList.Insert(0, new { Value = "", Text = "< 全部 >" });

            return new SelectList(selectList, "Value", "Text");
        }

        public void Create(客戶資料 客戶資料)
        {
            this.Add(客戶資料);
            this.UnitOfWork.Commit();
        }

        public void Modify(客戶資料 客戶資料)
        {
            this.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
            this.UnitOfWork.Commit();
        }

        public void Delete(int? id)
        {
            客戶資料 客戶資料 = this.Find(id);
            客戶資料.是否已刪除 = true;
            this.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
            this.UnitOfWork.Commit();
            //this.Delete(客戶資料);
            //this.UnitOfWork.Commit();
        }
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}