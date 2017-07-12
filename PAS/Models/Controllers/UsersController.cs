using PAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAS.Controllers
{
    public class UsersController : Controller
    {
        //
        // GET: /Users/

        public ActionResult Userhome()
        {
            if (Session["uname"] == null)
            {
                return RedirectToAction("index", "Home");
            }

            return View();
        }

        public ActionResult SignUp()
        {
            if (Session["uname"] != null)
            {
                return RedirectToAction("userhome");
            }

            return View();
        }

        public JsonResult CheckUserName(string username)
        {
            PAS_DatabaseEntities1 db = new PAS_DatabaseEntities1();

            string usernameToCheck = username;

            var query = (from u in db.Users
                         where u.FirstName == username
                         select new
                         {
                             u.UserId,
                             u.FirstName
                         });

            if (query.Count() == 0)
            {
                return this.Json(false, JsonRequestBehavior.AllowGet);
            }
            
            return this.Json(true, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SignUp(User user)
        {
            if (Session["uname"] != null)
            {
                return RedirectToAction("userhome");
            }

            if (ModelState.IsValid)
            {
                PAS_DatabaseEntities1 db = new PAS_DatabaseEntities1();
                User newUser = new User();
                newUser.FirstName = user.FirstName;
                newUser.LastName = user.LastName;
                newUser.Email = user.Email;
                newUser.Password = user.Password;

                db.Users.Add(newUser);
                db.SaveChanges();

                Session["uname"] = newUser.FirstName;

                return RedirectToAction("userhome");
            }

            return View(user);
        }

        public ActionResult SignInUser(User user)
        {
            if (Session["uname"] != null)
            {
                return RedirectToAction("userhome");
            }

            PAS_DatabaseEntities1 db = new PAS_DatabaseEntities1();

            User userToLogin = new User();

            var query = (from u in db.Users
                        where u.Email == user.Email
                        && u.Password == user.Password
                        select new 
                        {
                            u.UserId,
                            u.Email,
                            u.FirstName,
                            u.LastName,
                            u.Password
                        });
            
            if (query.Count() == 0)
            {
                Console.WriteLine("User Not Found");
                // display on view
                return RedirectToAction("signin", "Home");
            }

            foreach (var u in query)
            {
                userToLogin.Email = u.Email;
                userToLogin.FirstName = u.FirstName;
                userToLogin.LastName = u.LastName;
                userToLogin.Password = u.Password;
            }

            Session["uname"] = userToLogin.FirstName;
            
            return RedirectToAction("userhome");

        }

        public ActionResult SubmitApplication ()
        {
            if (Session["uname"] == null)
            {
                return RedirectToAction("index", "Home");
            }

            PAS_DatabaseEntities1 db = new PAS_DatabaseEntities1();

            Application newApplication = new Application();
            newApplication.Subject = Request["subject"];
            newApplication.Body = Request["body"];
            newApplication.Approved = false;

            string s = Session["uname"].ToString();

            var query2 = from u in db.Users
                         where u.FirstName == s
                         select new
                         {
                             u.UserId
                         };

            User user = new User();

            foreach (var u in query2)
            {
                user.UserId = u.UserId;
            }
            
            newApplication.UserId = user.UserId;

            db.Applications.Add(newApplication);
            db.SaveChanges();
            
            return RedirectToAction("SentApplications");
        }

        public ActionResult SentApplications ()
        {
            if (Session["uname"] == null)
            {
                return RedirectToAction("index", "Home");
            }

            PAS_DatabaseEntities1 db = new PAS_DatabaseEntities1();

            List<Application> applicationList = new List<Application>();

            string s = Session["uname"].ToString();

            var query2 = from u in db.Users
                         where u.FirstName == s
                         select new
                         {
                             u.UserId
                         };

            User user = new User();

            foreach (var u in query2)
            {
                user.UserId = u.UserId;
            }

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
                if (item.UserId == user.UserId)
                {
                    Application temp = new Application();
                    temp.ApplicationId = item.ApplicationId;
                    temp.Subject = item.Subject;
                    temp.Body = item.Body;
                    temp.Approved = item.Approved;
                    temp.UserId = item.UserId;

                    applicationList.Add(temp);

                }
            }

            return View(applicationList);
        }
    }
}
