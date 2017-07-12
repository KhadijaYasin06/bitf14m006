using PAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAS.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult SignInAdmin(Admin admin)
        {
            if (Session["aname"] != null)
            {
                return RedirectToAction("index");
            }

            PAS_DatabaseEntities1 db = new PAS_DatabaseEntities1();

            Admin adminToLogin = new Admin();

            var query = (from a in db.Admins
                         where a.Email == admin.Email
                         && a.Password == admin.Password
                         select new
                         {
                             a.AdminId,
                             a.Email,
                             a.Name,
                             a.Password
                         });

            if (query.Count() == 0)
            {
                Console.WriteLine("User Not Found");
                // display on view
                return RedirectToAction("signin", "Home");
            }

            foreach (var a in query)
            {
                adminToLogin.Email = a.Email;
                adminToLogin.Name = a.Name;
                adminToLogin.Password = a.Password;
            }

            Session["aname"] = adminToLogin.Name;

            return RedirectToAction("index");
        }

        public ActionResult Index()
        {
            if (Session["aname"] == null)
            {
                return RedirectToAction("index", "Home");
            }

            PAS_DatabaseEntities1 db = new PAS_DatabaseEntities1();

            List<Application> applicationList = new List<Application>();

            var query = from app in db.Applications
                        select new
                        {
                            app.ApplicationId,
                            app.Subject,
                            app.Body,
                            app.Approved,
                            app.UserId
                        };

            


            foreach (var item in query)
            {
                Application temp = new Application();
                temp.ApplicationId = item.ApplicationId;
                temp.Subject = item.Subject;
                temp.Body = item.Body;
                temp.Approved = item.Approved;
                temp.UserId = item.UserId;

                applicationList.Add(temp);
            }
          
            applicationList.Reverse();
            return View(applicationList);

        }

        public ActionResult DeleteApplication(int applicationId = -1)
        {
            if (Session["aname"] == null)
            {
                return RedirectToAction("index", "Home");
            }

            if (applicationId == -1)
            {
                return RedirectToAction("index");
            }

            PAS_DatabaseEntities1 db = new PAS_DatabaseEntities1();

            Application application = db.Applications.Find(applicationId);

            db.Applications.Remove(application);
            db.SaveChanges();

            ViewBag.deleted = 1; 


            return RedirectToAction("index", ViewBag);
        }

        public ActionResult UpdateApplication ()
        {
            if (Session["aname"] == null)
            {
                return RedirectToAction("index", "Home");
            }

            PAS_DatabaseEntities1 db = new PAS_DatabaseEntities1();

            Application application = db.Applications.Find(int.Parse(Request["applicationId"]));
            application.Subject = Request["subject"];
            application.Body = Request["body"];
            application.Approved = Convert.ToBoolean(int.Parse(Request["approval-status"]));

            db.SaveChanges();
            ViewBag.updated = 1;

            return RedirectToAction("index", ViewBag);
            
        }

        public ActionResult Update(int applicationId)
        {
            if (Session["aname"] == null)
            {
                return RedirectToAction("index", "Home");
            }

            PAS_DatabaseEntities1 db = new PAS_DatabaseEntities1();

            Application application = db.Applications.Find(applicationId);

            return View(application);
        }

    }
}
