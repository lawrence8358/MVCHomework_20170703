using System;
using System.Linq;
using System.Collections.Generic;
using MVCHomework_20170703.Models.ViewModels;

namespace MVCHomework_20170703.Models
{
    public class View_客戶資料統計Repository : EFRepository<View_客戶資料統計>, IView_客戶資料統計Repository
    {
        public override IQueryable<View_客戶資料統計> All()
        {
            return base.All();
        }

        public IQueryable<View_客戶資料統計> All(QueryCustomerReportViewModel queryModel)
        {
            var queryList = base.All();

            if (!string.IsNullOrEmpty(queryModel.CustomerName))
                queryList = queryList.Where(p => p.客戶名稱.Contains(queryModel.CustomerName));

            return queryList;
        }
    }

    public interface IView_客戶資料統計Repository : IRepository<View_客戶資料統計>
    {

    }
}