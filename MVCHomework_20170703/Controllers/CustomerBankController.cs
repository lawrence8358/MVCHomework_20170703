using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net; 
using System.Web.Mvc;
using MVCHomework_20170703.Models;
using MVCHomework_20170703.Models.ViewModels;

namespace MVCHomework_20170703.Controllers
{
    [MyAuth]
    public class CustomerBankController : Controller
    {
        客戶資料Repository customerRepo = RepositoryHelper.Get客戶資料Repository();
        客戶銀行資訊Repository customerBankRepo = RepositoryHelper.Get客戶銀行資訊Repository();

        // GET: CustomerBank
        public ActionResult Index()
        { 
            ViewBag.BankId = customerBankRepo.GetBankCodeList(); 

            return View(customerBankRepo.All().Include(客 => 客.客戶資料));
        }

        [HttpPost]
        public ActionResult Index(QueryCustomerBankViewModel queryModel)
        { 
            if (ModelState.IsValid)
            {
                ViewBag.BankId = customerBankRepo.GetBankCodeList();
                ViewBag.CustomerName = queryModel.CustomerName;

                IQueryable<客戶銀行資訊> query = customerBankRepo.All().Include(客 => 客.客戶資料).AsQueryable();

                if (queryModel.BankId > 0)
                    query = query.Where(p => p.銀行代碼.Equals(queryModel.BankId));
                if (!string.IsNullOrEmpty(queryModel.CustomerName))
                    query = query.Where(p => p.客戶資料.客戶名稱.Contains(queryModel.CustomerName));

                return View(query.ToList());
            }

            return View();
        }



        // GET: CustomerBank/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = customerBankRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // GET: CustomerBank/Create
        public ActionResult Create()
        {
            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: CustomerBank/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                customerBankRepo.Add(客戶銀行資訊);
                customerBankRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: CustomerBank/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = customerBankRepo.Find(id.Value);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // POST: CustomerBank/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,銀行名稱,銀行代碼,分行代碼,帳戶名稱,帳戶號碼")] 客戶銀行資訊 客戶銀行資訊)
        {
            if (ModelState.IsValid)
            {
                customerBankRepo.UnitOfWork.Context.Entry(客戶銀行資訊).State = EntityState.Modified;
                customerBankRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", 客戶銀行資訊.客戶Id);
            return View(客戶銀行資訊);
        }

        // GET: CustomerBank/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶銀行資訊 客戶銀行資訊 = customerBankRepo.Find(id);
            if (客戶銀行資訊 == null)
            {
                return HttpNotFound();
            }
            return View(客戶銀行資訊);
        }

        // POST: CustomerBank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶銀行資訊 客戶銀行資訊 = customerBankRepo.Find(id);
            客戶銀行資訊.是否已刪除 = true;
            customerBankRepo.UnitOfWork.Context.Entry(客戶銀行資訊).State = EntityState.Modified;
            customerBankRepo.UnitOfWork.Commit();
            //customerBankRepo.Delete(客戶銀行資訊);
            //customerBankRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                customerBankRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
