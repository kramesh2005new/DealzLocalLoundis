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

        public Boolean ValidateCookieSession()
        {
            if (Session["PublicUserName"] != null)
            {
                return true;
            }
            if (Request.Cookies["PublicUserName"] != null && Request.Cookies["PublicPassword"] != null)
            {
                if (PublicBA.IsValidPublicUser(new PublicUser() { EmailAddress = Request.Cookies["PublicUserName"].ToString(), Password = Request.Cookies["PublicPassword"].ToString() }) == "11")
                {
                    Session.Add("PublicUserName", Request.Cookies["PublicUserName"].ToString());
                    return true;
                }
            }
            return false;
        }

        private List<ShoppingSummary> GetShoppingSummaries(List<ShoppingList> shoppingLists)
        {
            var shopListSummary = new List<ShoppingSummary>();

            if (shoppingLists != null)
            {
                shopListSummary = (from s in shoppingLists
                                   group s by s.ShopId
                                       into g
                                       select new ShoppingSummary()
                                       {
                                           ShopName = g.FirstOrDefault() != null ? g.FirstOrDefault().ShopName : string.Empty,
                                           ShopImgUrl = g.FirstOrDefault() != null ? Url.Content(g.FirstOrDefault().ShopImgUrl) : string.Empty,
                                           TotalPurchaseAmount = g.Sum(s => s.Total)
                                       }).ToList();

            }
            return shopListSummary;
        }

        public ActionResult Index()
        {
            try
            {
                LocalShopsOffers localShopOffers = null;
                int cashandcarryID = 1;
                int pindex = 1;
                var shopList = new List<ShoppingList>();
                if (!string.IsNullOrWhiteSpace(Convert.ToString(Session["PublicUserName"])))
                {
                    shopList = PublicShoppingBA.GetPublicShoppingList(Session["PublicUserName"].ToString());
                }

                if (shopList != null && shopList.Count > 0)
                {
                    var shoppingCount = shopList.Sum(s => (string.IsNullOrWhiteSpace(s.Qty) ? 0 : Convert.ToInt32(s.Qty)));
                    ViewBag.ShoppingCount = shoppingCount;
                    ViewBag.ShopSummaries = GetShoppingSummaries(shopList);
                }

                ViewBag.PublicUserFirstName = Convert.ToString(Session["PublicUserFirstName"]);
                ViewBag.PublicUserLastName = Convert.ToString(Session["PublicUserLastName"]);
                string UserName = Session["PublicUserName"] != null ? Session["PublicUserName"].ToString() : string.Empty;
                ViewBag.PublicUserName = UserName;

                localShopOffers = new LocalShopsOffers();
                localShopOffers = PublicBA.GetHomeOffers(new PublicOffersFilterCriteria() { Product = string.Empty, ShopId = cashandcarryID, PageIndex = pindex });
                ViewBag.pages = localShopOffers.Pages;
                localShopOffers.UserName = UserName;

                return View("HomePageV1", localShopOffers);
            }

            catch (Exception ex)
            {
                return View("Error");
            }
        }

        public ActionResult Back()
        {
            return RedirectToAction("Index","Home");
        }

        public ActionResult SignOut()
        {
            Session["PublicUserName"] = null;
            Session["PublicUserFirstName"] = null;
            Session["PublicUserLastName"] = null;
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
