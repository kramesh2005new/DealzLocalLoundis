using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdealMall.Entities;
using IdealMall.BusinessAccess;

namespace IdealMall.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult LoginIndex(string msg)
        {
            ViewBag.view = "Home";

            if (false == string.IsNullOrWhiteSpace(msg))
            {
                ViewBag.MsgAfterLog = msg;
            }

            return View("Loginpage", new PublicUser
            {

                EmailAddress = Request.Cookies["EmailAddress"] != null ? Request.Cookies["EmailAddress"].Value : string.Empty,
                Password = Request.Cookies["Password"] != null ? Request.Cookies["Password"].Value : string.Empty,
                RememberMe = Request.Cookies["RememberMe"] != null ? Convert.ToBoolean(Request.Cookies["RememberMe"].Value) : false
            });

            //return View("HomePage");
        }

        public ActionResult Index(string msg)
        {
            ViewBag.view = "Home";


            if (false == string.IsNullOrWhiteSpace(msg))
            {
                ViewBag.MsgAfterLog = msg;
            }

            if (Request.Cookies["Home_PostCode"] != null)
            {
                ViewBag.RetainPostCode = Request.Cookies["Home_PostCode"].Value;
            }
            else
            {
                ViewBag.RetainPostCode = string.Empty;
            }

            return View("HomePageV1", new PublicUser
            {
                EmailAddress = Request.Cookies["EmailAddress"] != null ? Request.Cookies["EmailAddress"].Value : string.Empty,
                Password = Request.Cookies["Password"] != null ? Request.Cookies["Password"].Value : string.Empty,
                RememberMe = Request.Cookies["RememberMe"] != null ? Convert.ToBoolean(Request.Cookies["RememberMe"].Value) : false,
            });

            //return View("HomePage");
        }

        public ActionResult Back()
        {
            if (Convert.ToString(Session["Public_Back"]) == "Shops")
            {
                ViewBag.view = "Home";
                if (Request.Cookies["Home_PostCode"] != null)
                {
                    ViewBag.RetainPostCode = Request.Cookies["Home_PostCode"].Value;
                }
                else
                {
                    ViewBag.RetainPostCode = string.Empty;
                }

                return View("HomePageV1", new PublicUser
                {
                    EmailAddress = Request.Cookies["EmailAddress"] != null ? Request.Cookies["EmailAddress"].Value : string.Empty,
                    Password = Request.Cookies["Password"] != null ? Request.Cookies["Password"].Value : string.Empty,
                    RememberMe = Request.Cookies["RememberMe"] != null ? Convert.ToBoolean(Request.Cookies["RememberMe"].Value) : false,
                });
            }
            else
            {

                return RedirectToAction("DirectPublic", "Public", new { PostCode = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["PostCode"], radius = Convert.ToString(HttpUtility.ParseQueryString(Request.UrlReferrer.Query)[HttpUtility.ParseQueryString(Request.UrlReferrer.Query).AllKeys[1]]) });
            }

        }

        public ActionResult SignOut()
        {
            Session["PublicUserName"] = null;
            return RedirectToAction("Index");
        }
        //
        // GET: /Home/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Home/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Home/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Home/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Home/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Home/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Home/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
