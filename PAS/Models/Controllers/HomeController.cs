using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAS.Models;

namespace PAS.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        
        public ActionResult Signin()
        {
            if (Session["uname"] != null)
            {
                return RedirectToAction("userhome");
            }

            return View();
        }

        public ActionResult LoginFactory()
        {
            if (ModelState.IsValid)
            {

            }
            else
                return RedirectToAction("signin", "Home");

            if (int.Parse(Request["login-type"]) == 1)
            {
                User user = new User();
                user.Email = Request["email"];
                user.Password = Request["password"];

                return RedirectToAction("signinuser", "Users", user);
            }
            else if (int.Parse(Request["login-type"]) == 0)
            {
                Admin admin = new Admin();
                admin.Email = Request["email"];
                admin.Password = Request["password"];

                return RedirectToAction("signinadmin", "Admin", admin);
            }

            return RedirectToAction("signin");
        }

        public ActionResult Logout ()
        {
            Session["uname"] = null;
            Session["aname"] = null; 

            return RedirectToAction("index");
        }

        public ActionResult Index()
        {
            //Session["uname"] = "muhammad";

            
            ViewData["Message1"] = "VD Hellow";
            ViewBag.Message = "VB Hellow";

            return View();
        }
        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }


    }
}
