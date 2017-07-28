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

        public override IQueryable<客戶資料> All()
        {
            return base.All().Where(p => !p.是否已刪除);
        }

        public IQueryable<客戶資料> All(QueryCustomerViewModel queryModel)
        {
            var queryList = base.All();

            queryList = queryList.Where(p => !p.是否已刪除);

            if (!string.IsNullOrEmpty(queryModel.CustomerName))
                queryList = queryList.Where(p => p.客戶名稱.Contains(queryModel.CustomerName));
            if (!string.IsNullOrEmpty(queryModel.CustomerType))
                queryList = queryList.Where(p => p.客戶分類.Equals(queryModel.CustomerType));

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