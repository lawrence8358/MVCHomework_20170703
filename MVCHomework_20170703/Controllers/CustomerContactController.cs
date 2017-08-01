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
using ClosedXML.Excel;

namespace MVCHomework_20170703.Controllers
{
    [MyAuth]
    public class CustomerContactController : BaseController
    {
        客戶資料Repository customerRepo = RepositoryHelper.Get客戶資料Repository();
        客戶聯絡人Repository customerContactRepo = RepositoryHelper.Get客戶聯絡人Repository();
        int _pageSize = 5;

        // GET: CustomerContact
        public ActionResult Index(int pageNo = 1)
        {
            //return View(customerContactRepo.All().Include(客 => 客.客戶資料));

            //增加分頁功能
            var tempData = customerContactRepo.All();
            var data = tempData.ToPagedList(pageNo, this._pageSize);

            return View(data);
        } 

        [HttpPost]
        public ActionResult Index(QueryCustomerContactViewModel queryModel, int pageNo = 1, string sortName = "", string currentSortName = "")
        {
            if (ModelState.IsValid)
            {
                ViewBag.CustomerName = queryModel.CustomerName;
                ViewBag.CustomerContactName = queryModel.CustomerContactName;
                ViewBag.JobTitle = queryModel.JobTitle;
                 
                var tempData = customerContactRepo.All(queryModel, sortName, currentSortName);
                var data = tempData.ToPagedList(pageNo, this._pageSize);

                ViewData["CurrentSortName"] = currentSortName;
                ViewData["SortName"] = sortName;
                ViewData["PageNo"] = pageNo;

                return View(data); 
            }

            return View();
        }

        //增加JsonResult使用範例
        public FileResult Excel(QueryCustomerContactViewModel queryModel)
        {
            using (XLWorkbook wb = new XLWorkbook())
            { 
                var data = customerContactRepo.All(queryModel).Select(c => new { c.職稱, c.姓名, c.Email, c.手機, c.電話, c.客戶資料.客戶名稱 });

                var ws = wb.Worksheets.Add("客戶聯絡人資料", 1);
                ws.Cell(1, 1).Value = "職稱";
                ws.Cell(1, 2).Value = "姓名";
                ws.Cell(1, 3).Value = "Email";
                ws.Cell(1, 4).Value = "手機";
                ws.Cell(1, 5).Value = "電話";
                ws.Cell(1, 6).Value = "客戶名稱";

                ws.Cell(2, 1).InsertData(data);

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    return File(memoryStream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, "客戶聯絡人資料.xlsx");
                }
            }
        }

        //增加JsonResult使用範例
        public JsonResult CheckExcel(QueryCustomerContactViewModel queryModel)
        {
            var cnt = customerContactRepo.All(queryModel).Count();

            var routeValues = new System.Web.Routing.RouteValueDictionary(queryModel);
            var queryString = string.Empty;
            foreach (var item in routeValues)
            {
                if (!string.IsNullOrEmpty(queryString)) queryString += "&";
                queryString += item.Key + "=" + item.Value;
            }
            if (!string.IsNullOrEmpty(queryString)) queryString = "?" + queryString;

            var result = new
            {
                ExcelCount = cnt,
                ExcelUrl = string.Format("/{0}/Excel{1}", this.ControllerContext.RouteData.Values["controller"].ToString(), queryString)
            };

            return Json(result, JsonRequestBehavior.AllowGet);
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
                customerContactRepo.Create(客戶聯絡人);
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
                customerContactRepo.Modify(客戶聯絡人);
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
            customerContactRepo.Delete(id); 
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
