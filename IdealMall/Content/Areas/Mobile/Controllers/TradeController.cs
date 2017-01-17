using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdealMall.Models;
using IdealMall.Common;
using IdealMall.Entities;
using IdealMall.BusinessAccess;
namespace IdealMall.Areas.Mobile.Controllers
{
    public class TradeController : Controller
    {
        //
        // GET: /Trade/

        public ActionResult TradeLogin()
        {
            return View("TradeLogin");
        }

        public ActionResult Index()
        {
            if (Session["TradeUserName"] == null)
            {
                return View("TradeLogin");
            }
           
            ViewBag.browser = true;
            ViewBag.searchtext = string.Empty;
            ViewBag.view = "Trade";
            ViewBag.cc = 0;
            ViewBag.ShoppingCount = TradeBA.GetShoppingListCount(Session["TradeUserName"].ToString());
            CashandCarryOffers cco = TradeBA.GetTradeOffers();
            ViewBag.pages = cco.Pages;
            //cco.GensampleData();
            return View("CcOffers", cco);
        }

        //
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Autocomplete(string term)
        {
            var products = TradeBA.GetSuggestion(term);
            var x = (from pr in products
                     select new { Value = pr, Key = pr }).ToList();
            return Json(x, JsonRequestBehavior.AllowGet);
        }
        // GET: /Trade/Details/5

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult Searchresult(string query, string browser, string view, string cashandcarry, string pageindex)
        {
            try
            {
                if (Session["TradeUserName"] != null)
                {
            int cashandcarryID = string.IsNullOrWhiteSpace(cashandcarry) ? 0 : Convert.ToInt32(cashandcarry);
            int pindex = string.IsNullOrWhiteSpace(pageindex) ? 1 : Convert.ToInt32(pageindex);
            ViewBag.ShoppingCount = TradeBA.GetShoppingListCount(Session["TradeUserName"].ToString());
            ViewBag.browser = false;
            ViewBag.view = string.IsNullOrEmpty(view) ? "list" : "box";
            ViewBag.searchtext = query;
            ViewBag.cc = cashandcarryID;
            CashandCarryOffers cco = new CashandCarryOffers();
            cco = TradeBA.GetTradeOffers(new TradeOffersFilterCriteria() { Product = query, ShopId = cashandcarryID, PageIndex = pindex });
            ViewBag.pages = cco.Pages;
            if (!string.IsNullOrEmpty(browser))
            {
                ViewBag.browser = browser.Contains("full");
                return View("CcOffers", cco);
            }

            return View("RetailersBigdiv", cco);
                }
                else
                {
                    if (Request.HttpMethod.ToUpper().Equals("GET"))
                    {
                        return TradeLogin();
                    }
                    else
                    {
                        return new HttpUnauthorizedResult("Session time out");
                    }
                }
            }
            catch (Exception)
            {
                return View("Error");
            }
        }


        // Added by Varadha - Begin
         [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult PriceCompare(string Product, string imageURL)
        {

            #region Test Data
            List<Offer> offers = TradeBA.GetPriceCompare(Product); //new List<Offer>();
            //offers.Add(new Offer { Shop = new CashandCarryShop { Name = "Chetan" }, OfferRate = 3.59f, Volume = "24*250ml", OfferRatePercent = 48.7f });
            //offers.Add(new Offer { Shop = new CashandCarryShop { Name = "Chetan" }, OfferRate = 3.59f, Volume = "12*500ml", OfferRatePercent = 48.7f });
            //offers.Add(new Offer { Shop = new CashandCarryShop { Name = "Chetan" }, OfferRate = 3.59f, Volume = "24*250ml", OfferRatePercent = 48.7f });
            //offers.ForEach(m =>
            //{
            //    m.Product = Product;
            //    m.ImageURL = "/images/" + imageURL;
            //});
            #endregion
            ViewBag.imageURL = imageURL;
            ViewData.Add("ProductName", Product);
            return View(offers.AsEnumerable());
        }
        // Added by Varadha - End

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddToShoppingList(ShoppingItem item)
        {
             try
            {
               if (Session["TradeUserName"] != null)
                {
            item.UserId = Session["TradeUserName"].ToString();
            var totalcount = TradeBA.AddToShoppingList(item);
            return Json(totalcount, JsonRequestBehavior.AllowGet);
        }
                else
                {
                    return Json("Session Time out", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                Response.StatusCode = 500;
                return Json("Error occured");
            }
          }

        [OutputCache(Duration = 0)]
        public JsonResult SignUp(TradeUser tradeUser)
        {
            string outputString = string.Empty;
            try
            {
                string result = TradeBA.CreateAccountTradeUser(tradeUser);
                if (result == "!")
                {
                    outputString = "Your details are registered successfully.";
                }
                else if (result == "2")
                {
                    outputString = "Email Address already exists.";
                }
                else
                {
                    outputString = "Error creating Public User account.";
                }
                return Json(outputString);
            }
            catch (Exception ex)
            {
                return Json("Error creating Trade User account.");
            }
        }


        [HttpPost]
        public JsonResult ValidateCredentials(TradeUser inputCredentials)
        {
            try
            {
                string result = TradeBA.IsValidTradeUser(inputCredentials);

                switch (result)
                {
                    // Pass User id&Pwd --- Validates credential exist or not and returns 11 or 00.
                    case "11":
                        Session.Add("TradeUserName", inputCredentials.UserName);
                        return Json("true");
                    case "10":
                        return Json("Invalid Password");
                    case "99":
                        return Json("Invalid User Id / Password");
                    // If User Id alone is correct  and pwd wrong -- Returns 10
                    default: return Json("false");
                }

            }
            catch (Exception ex)
            {
                return Json("Error Validating Credentials!");
            }

        }

        [HttpPost]
        public JsonResult ClearShoppingList(string id)
        {
            try
            {
                if (Session["TradeUserName"] != null)
                {
                if (TradeBA.ClearShoppingList(Session["TradeUserName"].ToString()))
                {
                    return Json("true");
                }
                else
                {
                    return Json("error");
                    }
                }
                else
                {
                    return Json("Session Time out");
                }
            }
            catch (Exception ex)
            {
                return Json("error");
            }
        }

        [HttpPost]
        [OutputCache(Duration = 0)]
        public JsonResult ChangePassword(TradeUser tradeUser)
        {
            try
            {
                string result = TradeBA.IsValidTradeUser(tradeUser);
                switch (result)
                {
                    case "11":
                        TradeBA.ChangePassword(tradeUser);
                        return Json("Password changed successfully!");
                    case "10":
                        return Json("Invalid Password");
                    case "99":
                        return Json("Invalid User Id / Password");
                    default: return Json("Error validating credentials!");
                }
            }
            catch (Exception ex)
            {
                return Json("Error changing password!");
            }
        }
    }
}
