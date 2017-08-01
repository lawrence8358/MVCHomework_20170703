using MVCHomework_20170703.Models;
using MVCHomework_20170703.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MVCHomework_20170703.Controllers
{
    public class HomeController : BaseController
    {
        客戶資料Repository customerRepo = RepositoryHelper.Get客戶資料Repository();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                string userData = string.Empty;

                if (login.UserName == "123")
                {
                    userData = "admin";
                }
                else
                {
                    //一般會員
                    userData = customerRepo.FindByAccount(login.UserName).Id.ToString();
                }

                //Web.config要設定
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                    login.UserName,
                    DateTime.Now,
                    DateTime.Now.AddMinutes(30),
                    false,
                    userData, //userData
                    FormsAuthentication.FormsCookiePath);

                //// Encrypt the ticket.
                //string encTicket = FormsAuthentication.Encrypt(ticket);
                //var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket); 
                //cookie.HttpOnly = true;

                //// Create the cookie.
                //Response.Cookies.Add(cookie);

                // Encrypt the ticket.
                string encTicket = FormsAuthentication.Encrypt(ticket);

                // Create the cookie.
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                return Redirect("/");
            }

            return View();
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Home");
        }
    }
}