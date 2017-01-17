using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using IdealMall.Models;
using IdealMall.Common;
using IdealMall.Entities;
using IdealMall.BusinessAccess;
using MvcPaging;
using System.Configuration;
using IdealMall.DataAccess;
namespace IdealMall.Controllers
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
                EmailAddress = Request.Cookies["EmailAddress"] != null ? Request.Cookies["EmailAddress"].Value : string.Empty,
                Password = Request.Cookies["Password"] != null ? Request.Cookies["Password"].Value : string.Empty,
                RememberMe = Request.Cookies["PublicUserName"] != null
            });
        }

        public ActionResult PublicIndex()
        {
            ViewBag.Back = "Home";
            ViewBag.browser = true;
            ViewBag.searchtext = string.Empty;
            ViewBag.view = "Public";
            ViewBag.cc = 0;

            Session["Public_Back"] = "Shops";

            if (Session["PublicUserName"] == null)
            {
                string PostCode = PublicBA.GetPostCode(Session["PublicUserName"].ToString());
                Response.Cookies["Home_PostCode"].Value = PostCode;
                LocalShopsOffers cco = PublicBA.GetLocalShops(PostCode, "0.5");
                ViewBag.pages = cco.Pages;
                ViewBag.ShoppingCount = 0;
                ViewBag.PublicUserName = string.Empty;
                return View("PublicIndex", cco);
            }
            else
            {
                string PostCode = PublicBA.GetPostCode(Session["PublicUserName"].ToString());
                Response.Cookies["Home_PostCode"].Value = PostCode;
                LocalShopsOffers cco = PublicBA.GetLocalShops(PostCode, "0.5");
                ViewBag.pages = cco.Pages;

                var shopList = PublicShoppingBA.GetPublicShoppingList(Session["PublicUserName"].ToString());
                if (shopList != null && shopList.Count > 0)
                {
                    var shoppingCount =
                        shopList.Sum(s => (string.IsNullOrWhiteSpace(s.Qty) ? 0 : Convert.ToInt32(s.Qty)));
                    ViewBag.ShoppingCount = shoppingCount;
                    ViewBag.ShopSummaries = GetShoppingSummaries(shopList);
                }

                cco.PostCodeToDisplayInPublicIndex = PostCode;

                ViewBag.PublicUserName = Convert.ToString(Session["PublicUserName"]);
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
            Session["Public_Back"] = "Shops";
            Response.Cookies["Home_PostCode"].Value = PostCode;
            if (string.IsNullOrWhiteSpace(radius))
            {
                var queryString = HttpContext.Request.QueryString.ToString();

                if (queryString.Contains("radius"))
                {
                    var radiusFromQryString = queryString.Substring(queryString.Length - 2, 2).Replace("=", "");

                    decimal result;
                    decimal.TryParse(radiusFromQryString, out result);

                    radius = result > 0 ? radiusFromQryString : Common.Common.PublicUserGeneralRadius;
                }
                else
                {
                    radius = Common.Common.PublicUserGeneralRadius;
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
                cco.PostCodeToDisplayInPublicIndex = PostCode.ToUpper();
                Session.Add("PublicUserPostCode", PostCode.ToUpper());
                ViewBag.PublicUserPostCode = Convert.ToString(Session["PublicUserPostCode"]);
                ViewBag.PublicUserSelectedRadius = Convert.ToString(Session["PublicUserSelectedRadius"]);
                if (Convert.ToString(Session["PublicUserName"]) == Common.Common.GeneralUserName)
                {
                    ViewBag.ShoppingCount = 0;
                    ViewBag.PublicUserFirstName = "Anonymous";
                    ViewBag.PublicUserLastName = "User";
                    ViewBag.PublicUserName = string.Empty;
                }
                else
                {
                    var shopList = PublicShoppingBA.GetPublicShoppingList(Session["PublicUserName"].ToString());
                    if (shopList != null && shopList.Count > 0)
                    {
                        var shoppingCount =
                            shopList.Sum(s => (string.IsNullOrWhiteSpace(s.Qty) ? 0 : Convert.ToInt32(s.Qty)));
                        ViewBag.ShoppingCount = shoppingCount;
                        ViewBag.ShopSummaries = GetShoppingSummaries(shopList);
                        ViewBag.PublicUserFirstName = Convert.ToString(Session["PublicUserFirstName"]);
                        ViewBag.PublicUserLastName = Convert.ToString(Session["PublicUserLastName"]);
                        ViewBag.PublicUserName = Convert.ToString(Session["PublicUserName"]);
                    }
                }


                ViewBag.pages = cco.Pages;


                if (Request.IsAjaxRequest())
                {
                    return View("PublicPartial", cco);
                }

                return View("PublicIndex", cco);

            }
            catch (Exception ex)
            {
                return View("Error");
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
                            Response.Cookies["EmailAddress"].Expires = DateTime.Now.AddDays(30);
                            Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
                            Response.Cookies["EmailAddress"].Value = inputCredentials.EmailAddress;
                            Response.Cookies["Password"].Value = inputCredentials.Password;
                            Response.Cookies["RememberMe"].Value = Convert.ToString(inputCredentials.RememberMe);
                        }
                        else
                        {
                            Response.Cookies["EmailAddress"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["RememberMe"].Expires = DateTime.Now.AddDays(-1);
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

        [OutputCache(Duration = 0)]
        public JsonResult SignUp(PublicUser publicUser)
        {
            string outputString = string.Empty;
            try
            {
                string result = PublicBA.CreateAccountPublicUser(publicUser);
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
                return Json("Error creating Public User account.");
            }
        }



        // Pass only User id --- Validates User id exist or not and returns 1 or 0.

        // Pass User id&Pwd --- Validates credential exist or not and returns 11 or 00.
        // If User Id alone is correct  and pwd wrong -- Returns 10




        // Pass User id&Pwd --- Validates credential exist or not and returns 11 or 00.
        // If User Id alone is correct  and pwd wrong -- Returns 10

        public JsonResult GetUserDetails(PublicUser inputCredentials)
        {
            if (inputCredentials == null || string.IsNullOrWhiteSpace(inputCredentials.EmailAddress) ||
                string.IsNullOrWhiteSpace(inputCredentials.Password))
            {
                return Json("not valid");
            }

            return Json(PublicBA.GetPublicUserDetails(inputCredentials.EmailAddress, inputCredentials.Password),
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            ViewBag.browser = true;
            ViewBag.searchtext = string.Empty;
            ViewBag.view = "Public";
            ViewBag.cc = 0;
            ViewBag.ShoppingCount = PublicBA.GetShoppingListCount("1");
            var radius = Convert.ToString(Session["PublicUserSelectedRadius"]);
            LocalShopsOffers cco = PublicBA.GetLocalShops(string.Empty, radius);
            ViewBag.pages = cco.Pages;
            //cco.GensampleData();
            return View("CcOffers", cco);
        }

        //
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Autocomplete(string term)
        {
            var products = PublicBA.GetSuggestion(term);
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
                Session["Public_Back"] = "Products";
                if (ValidateCookieSession())
                {
                    int cashandcarryID = string.IsNullOrWhiteSpace(cashandcarry) ? 0 : Convert.ToInt32(cashandcarry);
                    int pindex = string.IsNullOrWhiteSpace(pageindex) ? 1 : Convert.ToInt32(pageindex);

                    var shopList = PublicShoppingBA.GetPublicShoppingList(Session["PublicUserName"].ToString());
                    if (shopList != null && shopList.Count > 0)
                    {
                        var shoppingCount = shopList.Sum(s => (string.IsNullOrWhiteSpace(s.Qty) ? 0 : Convert.ToInt32(s.Qty)));
                        ViewBag.ShoppingCount = shoppingCount;
                        ViewBag.ShopSummaries = GetShoppingSummaries(shopList);
                    }

                    ViewBag.browser = false;
                    ViewBag.view = string.IsNullOrEmpty(view) ? "list" : "box";
                    ViewBag.searchtext = query;
                    ViewBag.cc = cashandcarryID;

                    ViewBag.PublicUserFirstName = Convert.ToString(Session["PublicUserFirstName"]);
                    ViewBag.PublicUserLastName = Convert.ToString(Session["PublicUserLastName"]);
                    ViewBag.PublicUserPostCode = Convert.ToString(Session["PublicUserPostCode"]);
                    ViewBag.PublicUserSelectedRadius = Convert.ToString(Session["PublicUserSelectedRadius"]);

                    string UserName = Session["PublicUserName"] != null ? Session["PublicUserName"].ToString() : string.Empty;
                    ViewBag.PublicUserName = UserName;
                    string CurrPostCode = string.Empty;
                    if (Session["PublicUserPostCode"] == null)
                    {
                        CurrPostCode = !string.IsNullOrEmpty(UserName) ? (UserName != Common.Common.GeneralUserName ? PublicBA.GetPostCode(UserName) : Common.Common.GeneralUserPostCode) : string.Empty;
                    }
                    else
                    {
                        CurrPostCode = Convert.ToString(Session["PublicUserPostCode"]);
                    }
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

                    if (Request.Browser.IsMobileDevice)
                    {
                        return View("RetailersBigdiv", localShopOffers);
                    }

                    return View("RetailersBigdiv", localShopOffers);
                    //var pageNo = pindex > 0 ? pindex - 1 : 0;
                    //return View("RetailersBigDivPaged", localShopOffers.Offers.ToPagedList(pageNo, 6));
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
        public ActionResult PriceCompare(string Product, string volume, string imageURL, string PostCode)
        {
            try
            {
                string strproductvol = Product.Trim() + "(" + volume.Trim() + ")";

                if (Request.Cookies["Home_PostCode"] != null)
                {
                    PostCode = Request.Cookies["Home_PostCode"].Value;
                }
                else
                {
                    PostCode = string.Empty;
                }

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
            catch (Exception ex)
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
                    if (Convert.ToString(Session["PublicUserName"]) == "POST@CODE")
                    {
                        return Json("-1");
                    }
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

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetShoppingList()
        {
            try
            {
                if (ValidateCookieSession())
                {
                    var shopList = PublicShoppingBA.GetPublicShoppingList(Session["PublicUserName"].ToString());
                    var shopSummaries = GetShoppingSummaries(shopList);
                    return Json(shopSummaries, JsonRequestBehavior.AllowGet);
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
            var requestUri = new Uri("https://api.postcodes.io/postcodes/:" + PostCode + "/autocomplete");
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUri);
            httpWebRequest.Method = WebRequestMethods.Http.Get;
            httpWebRequest.Accept = "application/json";
            PostCodeJson myojb = new PostCodeJson();
            using (var twitpicResponse = (HttpWebResponse)httpWebRequest.GetResponse())
            {

                using (var reader = new StreamReader(twitpicResponse.GetResponseStream()))
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var objText = reader.ReadToEnd();
                    myojb = (PostCodeJson)js.Deserialize(objText, typeof(PostCodeJson));
                }

            }

            var list = (myojb != null && myojb.status == 200 && myojb.result != null) ? myojb.result : new List<string>();
            var x = (from elt in list
                     select new { Value = elt, Key = elt }).ToList();
            return Json(x, JsonRequestBehavior.AllowGet);
        }


        [OutputCache(Duration = 0)]
        public ActionResult ForgotPasswordIndex()
        {
            return View("ForgotPassword", new PublicUser
            {
            });
        }


        [OutputCache(Duration = 0)]
        public JsonResult ForgotPassword(string userid)
        {
            try
            {

                string subject = "DealzLocal Password";

                if (string.IsNullOrWhiteSpace(userid))
                {
                    return Json("Email Address is empty.");
                }

                PublicUser objPublicUser = PublicBA.GetPublicUserDetails(userid);

                if (objPublicUser == null)
                    return Json("Please enter valid public user name.");


                if (string.IsNullOrWhiteSpace(objPublicUser.EmailAddress))
                    return Json("Email Address is not available for this user.");


                string sender, cc, recipient, body;
                string Resetpass = Convert.ToString(ConfigurationManager.AppSettings["ResetPasswordStart"]) + objPublicUser.Password + Convert.ToString(ConfigurationManager.AppSettings["ResetPasswordEnd"]);
                PublicUser objChangePass = new PublicUser();
                objChangePass.EmailAddress = userid;
                objChangePass.Password = objPublicUser.Password;
                objChangePass.NewPassword = Resetpass;
                string strChangePassword = PublicBA.ChangePassword(objChangePass);

                body = PopulateBody(objPublicUser.FirstName + " " + objPublicUser.LastName, "Change Password", "", Resetpass);

                sender = Convert.ToString(ConfigurationManager.AppSettings["SenderMailId"]);
                cc = Convert.ToString(ConfigurationManager.AppSettings["CC"]);
                recipient = objPublicUser.EmailAddress;
                bool result = TradeShoppingBA.sendMail(sender, recipient, cc, subject, body);
                return Json(result == true ? "Password has been sent successfully to your registered mail id." :
                            "Error while sending mail.");
            }
            catch (Exception ex)
            {
                return Json("Error while sending mail.");
            }
        }
        private string PopulateBody(string userName, string title, string url, string password)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/PublicPasswordReset.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{Title}", title);
            body = body.Replace("{Url}", url);
            body = body.Replace("{password}", password);
            return body;
        }

        private string PopulateContactBody(string userName, string mailid, string subject, string message)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/ContactUs.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{mailID}", mailid);
            body = body.Replace("{subject}", subject);
            body = body.Replace("{message}", message);
            return body;
        }


        [OutputCache(Duration = 0)]
        public ActionResult ChangePasswordIndex()
        {
            return View("ChangePassword", new PublicUser
            {
            });
        }


        [OutputCache(Duration = 0)]
        public ActionResult ContactUsIndex()
        {
            ViewBag.Captcha = GenerateRandomCode();
            return View("ContactUs", new PublicUser
            {
            });
        }


        [HttpPost]
        [OutputCache(Duration = 0)]
        public JsonResult ChangePassword(string username, string password, string newpassword, string confirmpassword)
        {
            PublicUser publicUser = null;
            try
            {
                publicUser = new PublicUser() { UserName = username, Password = password, NewPassword = newpassword, ConfirmPassword = confirmpassword, EmailAddress = username };
                if (string.IsNullOrWhiteSpace(Convert.ToString(publicUser.UserName)) ||
                    string.IsNullOrWhiteSpace(Convert.ToString(publicUser.Password)) ||
                    string.IsNullOrWhiteSpace(Convert.ToString(publicUser.NewPassword)) ||
                    string.IsNullOrWhiteSpace(Convert.ToString(publicUser.ConfirmPassword)))
                {
                    return Json("Please enter all the fields.");
                }
                string result = PublicBA.IsValidPublicUser(publicUser);
                switch (result)
                {
                    case "11":
                        PublicBA.ChangePassword(publicUser);
                        return Json("Password has been changed successfully!");
                    case "10":
                        return Json("Wrong old password");
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

        [HttpPost]
        [OutputCache(Duration = 0)]
        public JsonResult ContactUs(string yourname, string email, string subject, string message)
        {

            try
            {

                if (string.IsNullOrWhiteSpace(Convert.ToString(yourname)) ||
                    string.IsNullOrWhiteSpace(Convert.ToString(email)) ||
                    string.IsNullOrWhiteSpace(Convert.ToString(subject)) ||
                    string.IsNullOrWhiteSpace(Convert.ToString(message)))
                {
                    return Json("Please enter all the fields.");
                }
                string sender, cc, recipient, body;

                body = PopulateContactBody(yourname, email, subject, message);

                sender = Convert.ToString(ConfigurationManager.AppSettings["SenderMailId"]);
                cc = Convert.ToString(ConfigurationManager.AppSettings["CC"]);
                recipient = Convert.ToString(ConfigurationManager.AppSettings["recipientMailId"]); ;
                bool result = TradeShoppingBA.sendMail(sender, recipient, cc, subject, body);
                return Json(result == true ? "Your message has been received to us. We will contact you shortly." :
                            "Please give your correct mail id.");
            }
            catch (Exception ex)
            {
                return Json("Please give your correct mail id.!");
            }
        }

        private string GenerateRandomCode()
        {
            Random r = new Random();
            string s = "";
            for (int j = 0; j < 5; j++)
            {
                int i = r.Next(3);
                int ch;
                switch (i)
                {
                    case 1:
                        ch = r.Next(0, 9);
                        s = s + ch.ToString();
                        break;
                    case 2:
                        ch = r.Next(65, 90);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    case 3:
                        ch = r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                    default:
                        ch = r.Next(97, 122);
                        s = s + Convert.ToChar(ch).ToString();
                        break;
                }
                r.NextDouble();
                r.Next(100, 1999);
            }
            return s;
        }


    }
}
