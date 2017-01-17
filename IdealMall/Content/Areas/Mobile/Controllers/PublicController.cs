using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdealMall.Models;
using IdealMall.Common;
using IdealMall.Entities;
using IdealMall.BusinessAccess;
using IdealMall.DataAccess;
namespace IdealMall.Areas.Mobile.Controllers
{
    public class PublicController : Controller
    {
        //
        // GET: /Public/

        public ActionResult PublicLogin()
        {
            ViewBag.view = "Home";
            return View("../Home/HomePage", new PublicUser
            {
                EmailAddress = Request.Cookies["PublicUserName"] != null ? Request.Cookies["PublicUserName"].Value : string.Empty,
                Password = Request.Cookies["PublicPassword"] != null ? Request.Cookies["PublicPassword"].Value : string.Empty,
                RememberMe = Request.Cookies["PublicUserName"] != null
            });
        }
      

        public ActionResult PublicIndex()
        {
            ViewBag.browser = true;
            ViewBag.searchtext = string.Empty;
            ViewBag.view = "Public";
            ViewBag.cc = 0;
            if (Session["PublicUserName"] == null)
            {
                string PostCode = PublicBA.GetPostCode(Session["PublicUserName"].ToString());
                LocalShopsOffers cco = PublicBA.GetLocalShops(PostCode);
                ViewBag.pages = cco.Pages;
                ViewBag.ShoppingCount = 0;
                return View("PublicIndex", cco);
            }
            else
            {
                string PostCode = PublicBA.GetPostCode(Session["PublicUserName"].ToString());
                LocalShopsOffers cco = PublicBA.GetLocalShops(PostCode);
                ViewBag.pages = cco.Pages;
                ViewBag.ShoppingCount = PublicBA.GetShoppingListCount(Session["PublicUserName"].ToString());
                cco.PostCodeToDisplayInPublicIndex = PostCode;

                ViewBag.PublicUserFirstName = Convert.ToString(Session["PublicUserFirstName"]);
                ViewBag.PublicUserLastName = Convert.ToString(Session["PublicUserLastName"]);
                ViewBag.PublicUserPostCode = Convert.ToString(Session["PublicUserPostCode"]);
                ViewBag.PublicUserSelectedRadius = Convert.ToString(Session["PublicUserSelectedRadius"]);

                return View("PublicIndex", cco);
            }
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult DirectPublic(string PostCode, string radius)
        {
            if (string.IsNullOrWhiteSpace(radius))
            {
                var queryString = HttpContext.Request.QueryString.ToString();

                if (queryString.Contains("radius"))
                {
                    var radiusFromQryString = queryString.Substring(queryString.Length - 2, 2).Replace("=", "");

                    int result;
                    int.TryParse(radiusFromQryString, out result);

                    radius = result > 0 ? radiusFromQryString : "2";
                }
                else
                {
                    radius = "2";
                }

            }

            if (Session["PublicUserName"] == null)
            {
                Session.Add("PublicUserName", Common.Common.GeneralUserName);
           }
            if (Session["PublicUserSelectedRadius"] == null)
            {
                Session.Add("PublicUserSelectedRadius", radius);
            }
            else
            {
                Session["PublicUserSelectedRadius"] = radius;
            }
            try
            {
                ViewBag.browser = true;
                ViewBag.searchtext = string.Empty;
                ViewBag.view = "Public";
                ViewBag.cc = 0;

                LocalShopsOffers cco = PublicBA.GetLocalShops(PostCode, radius);
                cco.PostCodeToDisplayInPublicIndex = PostCode;
                Session.Add("PublicUserPostCode", PostCode.ToUpper());
                ViewBag.PublicUserPostCode = Convert.ToString(Session["PublicUserPostCode"]);
                ViewBag.PublicUserSelectedRadius = Convert.ToString(Session["PublicUserSelectedRadius"]);

                if (Convert.ToString(Session["PublicUserName"]) == Common.Common.GeneralUserName)
                {
                    ViewBag.ShoppingCount = 0;
                    ViewBag.PublicUserFirstName = "Anonymous";
                    ViewBag.PublicUserLastName = "User";
                }
                else
                {
                    var shopList = PublicShoppingBA.GetPublicShoppingList(Session["PublicUserName"].ToString());
                    if (shopList != null && shopList.Count > 0)
                    {
                        var shoppingCount =
                            shopList.Sum(s => (string.IsNullOrWhiteSpace(s.Qty) ? 0 : Convert.ToInt32(s.Qty)));
                        ViewBag.ShoppingCount = shoppingCount;
                        ViewBag.PublicUserFirstName = Convert.ToString(Session["PublicUserFirstName"]);
                        ViewBag.PublicUserLastName = Convert.ToString(Session["PublicUserLastName"]);
                    }
                }
                ViewBag.pages = cco.Pages;
                ViewBag.ShoppingCount = 0;
                return View("PublicIndex", cco);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [OutputCache(Duration = 0)]
        public JsonResult SignUp(PublicUser publicUser)
        {
            try
            {
                string result = PublicBA.CreateAccountPublicUser(publicUser);
                return Json(result == "1" ? "We will contact you shortly. Thanks for registering." :
                                            "Error creating Public User account.");
            }
            catch (Exception ex)
            {
                return Json("Error creating Public User account.");
            }
        }


        [OutputCache(Duration = 0)]
        [HttpPost]
        public JsonResult ValidateCredentials(PublicUser inputCredentials)
        {
            try
            {
                string result = PublicBA.IsValidPublicUser(inputCredentials);
                switch (result)
                {
                    case "11":
                        Session.Add("PublicUserName", inputCredentials.EmailAddress);
                        if (inputCredentials.RememberMe)
                        {
                            Response.Cookies["PublicUserName"].Expires = DateTime.Now.AddDays(30);
                            Response.Cookies["PublicPassword"].Expires = DateTime.Now.AddDays(30);
                            Response.Cookies["PublicUserName"].Value = inputCredentials.EmailAddress;
                            Response.Cookies["PublicPassword"].Value = inputCredentials.Password;
                        }
                        else
                        {
                            Response.Cookies["PublicUserName"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["PublicPassword"].Expires = DateTime.Now.AddDays(-1);
                        }
                         var publicuser = PublicBA.GetPublicUserDetails(inputCredentials.EmailAddress,
                            inputCredentials.Password);
                        if (publicuser != null)
                        {
                            Session.Add("PublicUserFirstName", publicuser.FirstName);
                            Session.Add("PublicUserLastName", publicuser.LastName);
                            Session.Add("PublicUserPostCode", publicuser.PostCode);
                        }
                        return Json("true");
                    case "10":
                        return Json("Invalid Password");
                    case "99":
                        return Json("Invalid User Id / Password");
                    default: return Json("false");
                }
            }
            catch (Exception ex)
            {
                return Json("Error Validating Credentials!");
            }
        }

        public ActionResult Index()
        {
            ViewBag.browser = true;
            ViewBag.searchtext = string.Empty;
            ViewBag.view = "Public";
            ViewBag.cc = 0;
            ViewBag.ShoppingCount = PublicBA.GetShoppingListCount("1");
            LocalShopsOffers cco = PublicBA.GetLocalShops(string.Empty);
            ViewBag.pages = cco.Pages;
            //cco.GensampleData();
            return View("CcOffers", cco);
        }

        //
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Autocomplete(string term)
        {
           var products= PublicBA.GetSuggestion(term);
            var x = (from pr in products
                     select new { Value = pr, Key = pr }).ToList();
            return Json(x, JsonRequestBehavior.AllowGet);
        }
        // GET: /Public/Details/5

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult Searchresult(string query, string browser, string view, string cashandcarry, string pageindex)
        {
            try
            {
                if (ValidateCookieSession())
                {
                    int cashandcarryID = string.IsNullOrWhiteSpace(cashandcarry) ? 0 : Convert.ToInt32(cashandcarry);
                    int pindex = string.IsNullOrWhiteSpace(pageindex) ? 1 : Convert.ToInt32(pageindex);
                    ViewBag.ShoppingCount = PublicBA.GetShoppingListCount(Session["PublicUserName"].ToString());
                    ViewBag.browser = false;
                    ViewBag.view = string.IsNullOrEmpty(view) ? "list" : "box";
                    ViewBag.searchtext = query;
                    ViewBag.cc = cashandcarryID;
                    string UserName = Session["PublicUserName"] != null ? Session["PublicUserName"].ToString() : string.Empty;
                    string CurrPostCode = string.Empty;
                    if (Session["PublicUserPostCode"] == null)
                    {
                        CurrPostCode = !string.IsNullOrEmpty(UserName) ? (UserName != Common.Common.GeneralUserName ? PublicBA.GetPostCode(UserName) : Common.Common.GeneralUserPostCode) : string.Empty;
                    }
                    else
                    {
                        CurrPostCode = Convert.ToString(Session["PublicUserPostCode"]);
                    }
                    //string CurrPostCode = !string.IsNullOrEmpty(UserName) ?  (UserName != Common.Common.GeneralUserName ? PublicBA.GetPostCode(UserName) : Common.Common.GeneralUserPostCode) : string.Empty;
                    string radius = Session["PublicUserSelectedRadius"] != null
                         ? Session["PublicUserSelectedRadius"].ToString()
                         : Common.Common.PublicUserGeneralRadius;
                    LocalShopsOffers localShopOffers = new LocalShopsOffers();
                    localShopOffers = PublicBA.GetPublicOffers(new PublicOffersFilterCriteria() { Product = query, ShopId = cashandcarryID, PageIndex = pindex, PostCode = CurrPostCode, Radius = Convert.ToDouble(radius) });
                    localShopOffers.AllLocalShops = PublicBA.GetLocalShops(CurrPostCode, radius).AllLocalShops;
                    ViewBag.pages = localShopOffers.Pages;
                    localShopOffers.UserName = UserName;
                    if (!string.IsNullOrEmpty(browser))
                    {
                        ViewBag.browser = browser.Contains("full");
                        return View("CcOffers", localShopOffers);
                    }

                    return View("RetailersBigdiv", localShopOffers);
                }
                else
                {
                    if (Request.HttpMethod.ToUpper().Equals("GET"))
                    {
                        return PublicLogin();
                    }
                    else
                    {
                        return new HttpUnauthorizedResult("Session time out");
                    }
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }


        // Added by Varadha - Begin
        public ActionResult PriceCompare(string Product,string volume, string imageURL, string PostCode)
        {
            try
            {
                string strproductvol = Product.Trim() + "(" + volume.Trim() + ")";

                var latLng = GoogleMapsApiHelper.GetLatLng(PostCode + ",UK");

                if (latLng != null && latLng.Latitude == 0 && latLng.Longitude == 0)
                {
                    var postCodes = PublicDA.GetPostCodesLike(PostCode);
                    if (postCodes.Count > 0)
                    {
                        latLng.Latitude = Convert.ToDecimal(postCodes.Average(a => a.latitude));
                        latLng.Longitude = Convert.ToDecimal(postCodes.Average(a => a.longitude));
                    }
                }
                string radius = Session["PublicUserSelectedRadius"] != null
                         ? Session["PublicUserSelectedRadius"].ToString()
                         : Common.Common.PublicUserGeneralRadius;

                List<Offer> offers = PublicBA.GetPriceCompare(strproductvol, Common.Common.GeneralUserPostCode, radius, latLng);
                ViewBag.imageURL = imageURL;
                ViewData.Add("ProductName", Product);
                return View(offers.AsEnumerable());
            }
            catch (Exception)
            {
                return View("Error");
            }
        }
        // Added by Varadha - End

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
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult AddToShoppingList(ShoppingItem item)
        {
            try
            {
                if (ValidateCookieSession())
                {
            item.UserId = Session["PublicUserName"].ToString();
            var totalcount = PublicBA.AddToShoppingList(item);
            return Json(totalcount, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Session Time out", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json("Error occured");
            }
        }

        [HttpPost]
        public JsonResult ClearShoppingList(string id)
        {
            try
            {
                if (Session["PublicUserName"] != null)
            {
                if (PublicBA.ClearShoppingList(Session["PublicUserName"].ToString()))
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
        public JsonResult ChangePassword(PublicUser publicUser)
        {
            try
            {
                string result = PublicBA.IsValidPublicUser(publicUser);
                switch (result)
                {
                    case "11":
                        PublicBA.ChangePassword(publicUser);
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

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetPostCodeSuggestions(string PostCode)
        {
            var list = PublicBA.GetPostCodeSuggestions(PostCode);
            var x = (from elt in list
                     select new { Value = elt, Key = elt }).ToList();
            return Json(x, JsonRequestBehavior.AllowGet);
        }
    }
}
