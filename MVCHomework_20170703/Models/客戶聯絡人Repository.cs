using System.Linq;
using MVCHomework_20170703.Models.ViewModels;
using System.Data.Entity;

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
            return base.All().Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除);
        }

        public IQueryable<客戶聯絡人> All(QueryCustomerContactViewModel queryModel)
        { 
            var queryList = base.All();

            queryList = queryList.Where(p => !p.是否已刪除 && !p.客戶資料.是否已刪除);

            if (!string.IsNullOrEmpty(queryModel.CustomerName))
                queryList = queryList.Where(p => p.客戶資料.客戶名稱.Contains(queryModel.CustomerName));
            if (!string.IsNullOrEmpty(queryModel.CustomerContactName))
                queryList = queryList.Where(p => p.姓名.Contains(queryModel.CustomerContactName));
              
            return queryList;
        }
    }

    public interface I客戶聯絡人Repository : IRepository<客戶聯絡人>
    {

    }
}