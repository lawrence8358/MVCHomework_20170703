using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVCHomework_20170703.Models;
using MVCHomework_20170703.Models.ViewModels;
using PagedList;
using ClosedXML.Excel;

namespace MVCHomework_20170703.Controllers
{
    [MyAuth]
    public class CustomerBankController : BaseController
    {
        客戶資料Repository customerRepo = RepositoryHelper.Get客戶資料Repository();
        客戶銀行資訊Repository customerBankRepo = RepositoryHelper.Get客戶銀行資訊Repository();
        int _pageSize = 5;

        // GET: CustomerBank
        public ActionResult Index(int pageNo = 1)
        {
            ViewBag.BankId = customerBankRepo.GetBankCodeList();

            //return View(customerBankRepo.All().Include(客 => 客.客戶資料));

            //增加分頁功能
            var tempData = customerBankRepo.All();
            var data = tempData.ToPagedList(pageNo, this._pageSize);

            return View(data);
        }

        [HttpPost]
        public ActionResult Index(QueryCustomerBankViewModel queryModel, int pageNo = 1, string sortName = "", string currentSortName = "")
        {
            if (ModelState.IsValid)
            {
                ViewBag.BankId = customerBankRepo.GetBankCodeList();
                ViewBag.CustomerName = queryModel.CustomerName;

                var tempData = customerBankRepo.All(queryModel, sortName, currentSortName);
                var data = tempData.ToPagedList(pageNo, this._pageSize);

                ViewData["CurrentSortName"] = currentSortName;
                ViewData["SortName"] = sortName;
                ViewData["PageNo"] = pageNo;

                return View(data);
            }

            return View();
        }

        //增加JsonResult使用範例
        public FileResult Excel(QueryCustomerBankViewModel queryModel)
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                var data = customerBankRepo.All(queryModel).Select(c => new { c.銀行名稱, c.銀行代碼, c.分行代碼, c.帳戶名稱, c.帳戶號碼, c.客戶資料.客戶名稱 });

                var ws = wb.Worksheets.Add("客戶銀行資料", 1);
                ws.Cell(1, 1).Value = "銀行名稱";
                ws.Cell(1, 2).Value = "銀行代碼";
                ws.Cell(1, 3).Value = "分行代碼";
                ws.Cell(1, 4).Value = "帳戶名稱";
                ws.Cell(1, 5).Value = "帳戶號碼";
                ws.Cell(1, 6).Value = "客戶名稱";

                ws.Cell(2, 1).InsertData(data);

                using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    return File(memoryStream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, "客戶銀行資料.xlsx");
                }
            }
        }

        //增加JsonResult使用範例
        public JsonResult CheckExcel(QueryCustomerBankViewModel queryModel)
        {
            var cnt = customerBankRepo.All(queryModel).Count();

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
                customerBankRepo.Create(客戶銀行資訊);
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
                customerBankRepo.Modify(客戶銀行資訊);
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
            customerBankRepo.Delete(id);
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
