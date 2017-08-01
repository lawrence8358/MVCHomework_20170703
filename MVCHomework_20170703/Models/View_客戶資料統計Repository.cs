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
            return base.All().OrderBy(c => c.客戶Id);
        }

        public IQueryable<View_客戶資料統計> All(QueryCustomerReportViewModel queryModel, string sortName = "", string currentSortName = "")
        {
            var queryList = base.All();

            if (!string.IsNullOrEmpty(queryModel.CustomerName))
                queryList = queryList.Where(p => p.客戶名稱.Contains(queryModel.CustomerName));

            switch (currentSortName)
            {
                case "customerName":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.客戶名稱);
                    else
                        queryList = queryList.OrderBy(c => c.客戶名稱);
                    break;

                case "contactCount":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.聯絡人數量);
                    else
                        queryList = queryList.OrderBy(c => c.聯絡人數量);
                    break;

                case "bankCount":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.銀行帳戶數量);
                    else
                        queryList = queryList.OrderBy(c => c.銀行帳戶數量);
                    break;

                default:
                    queryList = queryList.OrderBy(c => c.客戶Id);
                    break;
            }

            return queryList;
        }
    }

    public interface IView_客戶資料統計Repository : IRepository<View_客戶資料統計>
    {

    }
}