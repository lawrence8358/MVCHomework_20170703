using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCHomework_20170703.Models;
using MVCHomework_20170703.Models.ViewModels;
using PagedList;

namespace MVCHomework_20170703.Controllers
{
    [MyAuth]
    public class CustomerContactController : Controller
    {
        客戶資料Repository customerRepo = RepositoryHelper.Get客戶資料Repository();
        客戶聯絡人Repository customerContactRepo = RepositoryHelper.Get客戶聯絡人Repository();
        int _pageSize = 1;

        // GET: CustomerContact
        public ActionResult Index(int pageNo = 1)
        {
            //return View(customerContactRepo.All().Include(客 => 客.客戶資料));

            //增加分頁功能
            var tempData = customerContactRepo.All().Include(客 => 客.客戶資料).OrderBy(c => c.客戶Id);
            var data = tempData.ToPagedList(pageNo, this._pageSize);

            return View(data);
        } 

        [HttpPost]
        public ActionResult Index(QueryCustomerContactViewModel queryModel, int pageNo = 1)
        {
            if (ModelState.IsValid)
            {
                ViewBag.CustomerName = queryModel.CustomerName;
                ViewBag.CustomerContactName = queryModel.CustomerContactName;

                //IQueryable<客戶聯絡人> query = customerContactRepo.All().Include(客 => 客.客戶資料).AsQueryable();

                //if (!string.IsNullOrEmpty(queryModel.CustomerName))
                //    query = query.Where(p => p.客戶資料.客戶名稱.Contains(queryModel.CustomerName)); 
                //if (!string.IsNullOrEmpty(queryModel.CustomerContactName))
                //    query = query.Where(p => p.姓名.Contains(queryModel.CustomerContactName));

                //return View(query.ToList());

                //增加分頁功能 
                var tempData = customerContactRepo.All(queryModel).Include(客 => 客.客戶資料).OrderBy(c => c.客戶Id);
                var data = tempData.ToPagedList(pageNo, this._pageSize);

                return View(data); 
            }

            return View();
        }

        // GET: CustomerContact/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = customerContactRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // GET: CustomerContact/Create
        public ActionResult Create()
        { 
            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱");
            return View();
        }

        // POST: CustomerContact/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                customerContactRepo.Add(客戶聯絡人);
                customerContactRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }

            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: CustomerContact/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = customerContactRepo.Find(id.Value);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // POST: CustomerContact/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,客戶Id,職稱,姓名,Email,手機,電話")] 客戶聯絡人 客戶聯絡人)
        {
            if (ModelState.IsValid)
            {
                customerContactRepo.UnitOfWork.Context.Entry(客戶聯絡人).State = EntityState.Modified;
                customerContactRepo.UnitOfWork.Commit();
                return RedirectToAction("Index");
            }
            ViewBag.客戶Id = new SelectList(customerRepo.All(), "Id", "客戶名稱", 客戶聯絡人.客戶Id);
            return View(客戶聯絡人);
        }

        // GET: CustomerContact/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            客戶聯絡人 客戶聯絡人 = customerContactRepo.Find(id);
            if (客戶聯絡人 == null)
            {
                return HttpNotFound();
            }
            return View(客戶聯絡人);
        }

        // POST: CustomerContact/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            客戶聯絡人 客戶聯絡人 = customerContactRepo.Find(id);
            客戶聯絡人.是否已刪除 = true;
            customerContactRepo.UnitOfWork.Context.Entry(客戶聯絡人).State = EntityState.Modified;
            customerContactRepo.UnitOfWork.Commit();
            //customerContactRepo.Delete(客戶聯絡人);
            //customerContactRepo.UnitOfWork.Commit();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                customerContactRepo.UnitOfWork.Context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
