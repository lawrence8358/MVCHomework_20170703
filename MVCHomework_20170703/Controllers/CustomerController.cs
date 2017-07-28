using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCHomework_20170703.Models;
using PagedList;
using MVCHomework_20170703.Models.ViewModels;

namespace MVCHomework_20170703.Controllers
{
    [MyAuth]
    public class CustomerController : Controller
    {
        客戶資料Repository customerRepo = RepositoryHelper.Get客戶資料Repository();
        View_客戶資料統計Repository customerViewRepo = RepositoryHelper.GetView_客戶資料統計Repository();
        int _pageSize = 1;

        // GET: Customer
        public ActionResult Index(int pageNo = 1)
        {
            //return View(customerRepo.All());

            //增加分頁功能
            var tempData = customerRepo.All().OrderBy(c => c.Id);
            var data = tempData.ToPagedList(pageNo, this._pageSize);

            return View(data);
        }

        [HttpPost]
        public ActionResult Index(QueryCustomerViewModel queryModel, int pageNo = 1)
        {
            ViewBag.CustomerName = queryModel.CustomerName;

            //return View(customerRepo.Where( p=> p.客戶名稱.Contains(CustomerName)));

            //增加分頁功能 
            var tempData = customerRepo.All(queryModel).OrderBy(c => c.Id);
            var data = tempData.ToPagedList(pageNo, this._pageSize);

            return View(data);
        }

        // GET: Customer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,帳號,密碼,客戶分類,客戶名稱,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                customerRepo.Add(客戶資料);
                customerRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            return View(客戶資料);
        }

        // GET: Customer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerRepo.Find(id.Value);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: Customer/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,帳號,密碼,客戶分類,客戶名稱,統一編號,電話,傳真,地址,Email")] 客戶資料 客戶資料)
        {
            if (ModelState.IsValid)
            {
                customerRepo.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
                customerRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            return View(客戶資料);
        }

        // GET: Customer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶資料 客戶資料 = customerRepo.Find(id);
            if (客戶資料 == null)
            {
                return HttpNotFound();
            }
            return View(客戶資料);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶資料 客戶資料 = customerRepo.Find(id);
            客戶資料.是否已刪除 = true;
            customerRepo.UnitOfWork.Context.Entry(客戶資料).State = EntityState.Modified;
            customerRepo.UnitOfWork.Commit();
            //customerRepo.Delete(客戶資料);
            //customerRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        public ActionResult Report(int pageNo = 1)
        {
            //var 客戶資料統計 = ((CRMDbContext)customerRepo.UnitOfWork.Context).View_客戶資料統計;
            //return View(客戶資料統計);

            //增加分頁功能
            var tempData = customerViewRepo.All().OrderBy(c => c.客戶Id);
            var data = tempData.ToPagedList(pageNo, this._pageSize);

            return View(data);
        }

        [HttpPost]
        public ActionResult Report(QueryCustomerReportViewModel queryModel, int pageNo = 1)
        {
            ViewBag.CustomerName = queryModel.CustomerName;

            //增加分頁功能 
            var tempData = customerViewRepo.All(queryModel).OrderBy(c => c.客戶Id);
            var data = tempData.ToPagedList(pageNo, this._pageSize);

            return View(data);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                customerRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
