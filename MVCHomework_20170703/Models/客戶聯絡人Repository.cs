using System.Linq;
using MVCHomework_20170703.Models.ViewModels;
using System.Data.Entity;
using System.Collections.Generic;

namespace MVCHomework_20170703.Models
{
    public class 客戶聯絡人Repository : EFRepository<客戶聯絡人>, I客戶聯絡人Repository
    {
        public override 客戶聯絡人 Find(object id)
        {
            return base.All().Where(p => p.Id.Equals((int)id)).Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除).FirstOrDefault();
        }

        public override IQueryable<客戶聯絡人> All()
        {
            return base.All().Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除).Include(客 => 客.客戶資料).OrderBy(c => c.客戶Id);
        }

        public IQueryable<客戶聯絡人> All(QueryCustomerContactViewModel queryModel, string sortName = "", string currentSortName = "")
        { 
            var queryList = base.All();

            queryList = queryList.Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除);

            if (!string.IsNullOrEmpty(queryModel.CustomerName))
                queryList = queryList.Where(p => p.客戶資料.客戶名稱.Contains(queryModel.CustomerName));
            if (!string.IsNullOrEmpty(queryModel.CustomerContactName))
                queryList = queryList.Where(p => p.姓名.Contains(queryModel.CustomerContactName));
            if (!string.IsNullOrEmpty(queryModel.JobTitle))
                queryList = queryList.Where(p => p.職稱.Contains(queryModel.JobTitle));

            queryList = queryList.Include(客 => 客.客戶資料);

            switch (currentSortName)
            {
                case "customerName":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.客戶資料.客戶名稱);
                    else
                        queryList = queryList.OrderBy(c => c.客戶資料.客戶名稱);
                    break;

                case "jobTitle":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.職稱);
                    else
                        queryList = queryList.OrderBy(c => c.職稱);
                    break;

                case "name":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.姓名);
                    else
                        queryList = queryList.OrderBy(c => c.姓名);
                    break;
                    
                case "email":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.Email);
                    else
                        queryList = queryList.OrderBy(c => c.Email);
                    break;
                    
                case "mobile":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.手機);
                    else
                        queryList = queryList.OrderBy(c => c.手機);
                    break;

                case "phone":
                    if (sortName.Equals(currentSortName))
                        queryList = queryList.OrderByDescending(c => c.電話);
                    else
                        queryList = queryList.OrderBy(c => c.電話);
                    break;

                default:
                    queryList = queryList.OrderBy(c => c.客戶Id);
                    break;
            }

            return queryList;
        }

        public IQueryable<客戶聯絡人> FormCustomer(int customerId)
        {
            return base.All().Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除 && p.客戶Id.Equals(customerId));
        }

        public void Create(客戶聯絡人 客戶聯絡人)
        { 
            this.Add(客戶聯絡人);
            this.UnitOfWork.Commit();
        }

        public void Modify(客戶聯絡人 客戶聯絡人)
        { 
            this.UnitOfWork.Context.Entry(客戶聯絡人).State = EntityState.Modified;
            this.UnitOfWork.Commit();
        }

        public void Delete(int? id)
        {
            客戶聯絡人 客戶聯絡人 = this.Find(id);
            客戶聯絡人.是否已刪除 = true;
            this.UnitOfWork.Context.Entry(客戶聯絡人).State = EntityState.Modified;
            this.UnitOfWork.Commit();
            //this.Delete(客戶聯絡人);
            //this.UnitOfWork.Commit();
        }

        public void BatchUpdate(List<CustomerContactBatchViewModel> data)
        {
            //ProductBatchView[] 此寫法在前端必須對應data[i].ProductId的格式，但在C# 6.0有問題
            //因此目前的解法式移除Microsoft.CodeDom.Providers.DotNetCompilerPlatform
            //解法可參考 http://haacked.com/archive/2008/10/23/model-binding-to-a-list.aspx/
            foreach (var item in data)
            {
                //不需要檢查是否有異動，EF的機制會自動檢查
                var contact = this.Find(item.Id);
                contact.職稱 = item.職稱;
                contact.手機 = item.手機;
                contact.電話 = item.電話;
            }

            this.UnitOfWork.Commit();
        }
    }

    public interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
    {

    }
}