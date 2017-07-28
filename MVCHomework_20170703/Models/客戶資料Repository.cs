using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using MVCHomework_20170703.Models.ViewModels;

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

            return queryList; 
        }
    }

    public interface I客戶資料Repository : IRepository<客戶資料>
    {

    }
}