using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdealMall.Models;
using IdealMall.Common;
using IdealMall.Entities;
using IdealMall.BusinessAccess;
using System.Configuration;
using System.IO;
namespace IdealMall.Controllers
{
    public class TradeController : Controller
    {
        //
        // GET: /Trade/

        [OutputCache(Duration = 0)]
        public ActionResult TradeLogin()
        {
            return View("TradeLogin1", new TradeUser
            {
                UserName = Request.Cookies["UserName"] != null ? Request.Cookies["UserName"].Value : string.Empty,
                TradePassword = Request.Cookies["TradePassword"] != null ? Request.Cookies["TradePassword"].Value : string.Empty,
                TradeRememberMe = Request.Cookies["TradeRememberMe"] != null ? Convert.ToBoolean(Request.Cookies["TradeRememberMe"].Value) : false,
            });
        }


        [OutputCache(Duration = 0)]
        public ActionResult TradeSignIn()
        {
            return View("TradeSignIn", new TradeUser
            {
            });
        }

        [OutputCache(Duration = 0)]
        public ActionResult ForgotPasswordIndex()
        {
            return View("ForgotPassword", new TradeUser
            {
            });
        }


        [OutputCache(Duration = 0)]
        public JsonResult ForgotPassword(string userid)
        {
            try
            {

                string subject = "DealzLocal Trade Password";

                if (string.IsNullOrWhiteSpace(userid))
                {
                    return Json("Trade User ID is empty.");
                }

                TradeUser objTradeUser = TradeBA.GetTradeUserDetails(userid);

                if (objTradeUser == null)
                    return Json("Please enter valid trade user id.");


                if (string.IsNullOrWhiteSpace(objTradeUser.EmailAddress))
                    return Json("Email Address is not available for this user.");


                string sender, cc, recipient, body;
                string Resetpass = Convert.ToString(ConfigurationManager.AppSettings["ResetPasswordStart"]) + objTradeUser.TradePassword + Convert.ToString(ConfigurationManager.AppSettings["ResetPasswordEnd"]);
                TradeUser objChangePass = new TradeUser();
                objChangePass.UserName = userid;
                objChangePass.TradePassword = objTradeUser.TradePassword;
                objChangePass.NewPassword = Resetpass;
                string strChangePassword = TradeBA.ChangePassword(objChangePass);

                body = PopulateBody(objTradeUser.FirstName + " " + objTradeUser.LastName, "Change Password", "", Resetpass);

                sender = Convert.ToString(ConfigurationManager.AppSettings["SenderMailId"]);
                cc = Convert.ToString(ConfigurationManager.AppSettings["CC"]);
                recipient = objTradeUser.EmailAddress;
                bool result = TradeShoppingBA.sendMail(sender, recipient, cc, subject, body);
                return Json(result == true ? "Trade password successfully send to your registered mail id." :
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
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/PasswordReset.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{Title}", title);
            body = body.Replace("{Url}", url);
            body = body.Replace("{password}", password);
            return body;
        }
        public ActionResult TradeIndex()
        {
            if (Session["TradeUserName"] == null)
            {
                return View("TradeLogin1", new TradeUser());
            }
            else
            {
                ViewBag.browser = true;
                ViewBag.searchtext = string.Empty;
                ViewBag.view = "Trade";
                ViewBag.cc = 0;
                CashandCarryOffers cco = TradeBA.GetTradeOffers();
                ViewBag.pages = cco.Pages;

                // ViewBag.ShoppingCount = TradeBA.GetShoppingListCount(Session["TradeUserName"].ToString());
                var shopList = TradeShoppingBA.GetTradeShoppingList(Session["TradeUserName"].ToString());
                if (shopList != null && shopList.Count > 0)
                {
                    var shoppingCount =
                        shopList.Sum(s => (string.IsNullOrWhiteSpace(s.Qty) ? 0 : Convert.ToInt32(s.Qty)));
                    ViewBag.TradeUserFirstName = Convert.ToString(Session["TradeUserFirstName"]);
                    ViewBag.TradeUserLastName = Convert.ToString(Session["TradeUserLastName"]);
                    ViewBag.TradeBusinessName = Convert.ToString(Session["TradeBusinessName"]);
                    ViewBag.ShoppingCount = shoppingCount;
                    ViewBag.ShopSummaries = GetShoppingSummaries(shopList);
                }

                return View("TradeIndex", cco);
            }
        }


        [OutputCache(Duration = 0)]
        public JsonResult SignUp(TradeUser tradeUser)
        {
            try
            {
                string resultText = string.Empty;
                string result = TradeBA.CreateAccountTradeUser(tradeUser);
                if (result == "1")
                    resultText = "We will contact you shortly. Thanks for registeration.";
                else if (result == "2")
                    resultText = "Email Address already exists.";
                else
                    resultText = "Error creating Trade User account.";

                return Json(resultText);
            }
            catch (Exception ex)
            {
                return Json("Error creating Trade User account.");
            }
        }


        [OutputCache(Duration = 0)]
        [HttpPost]
        public JsonResult ValidateCredentials(TradeUser inputCredentials)
        {
            try
            {
                string result = TradeBA.IsValidTradeUser(inputCredentials);

                switch (result)
                {
                    // Pass only User id --- Validates User id exist or not and returns 1 or 0.

                    // Pass User id&Pwd --- Validates credential exist or not and returns 11 or 00.
                    case "11":
                        Session.Add("TradeUserName", inputCredentials.UserName);
                        if (inputCredentials.TradeRememberMe)
                        {
                            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(30);
                            Response.Cookies["TradePassword"].Expires = DateTime.Now.AddDays(30);
                            Response.Cookies["UserName"].Value = inputCredentials.UserName;
                            Response.Cookies["TradePassword"].Value = inputCredentials.TradePassword;
                            Response.Cookies["TradeRememberMe"].Value = Convert.ToString(inputCredentials.TradeRememberMe);
                        }
                        else
                        {
                            Response.Cookies["UserName"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["TradePassword"].Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies["TradeRememberMe"].Expires = DateTime.Now.AddDays(-1);
                        }
                        TradeUser objTradeUser = TradeBA.GetTradeUserDetails(inputCredentials.UserName);
                        if (objTradeUser != null)
                        {
                            Session["TradeUserFirstName"] = objTradeUser.FirstName;
                            Session["TradeUserLastName"] = objTradeUser.LastName;
                            Session["TradeBusinessName"] = objTradeUser.BusinessName;
                        }
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



                // Pass User id&Pwd --- Validates credential exist or not and returns 11 or 00.
                // If User Id alone is correct  and pwd wrong -- Returns 10

                return Json("Error Validating Credentials!");
            }

        }

        public ActionResult Index()
        {
            ViewBag.browser = true;
            ViewBag.searchtext = string.Empty;
            ViewBag.view = "Trade";
            ViewBag.cc = 0;
            ViewBag.ShoppingCount = TradeBA.GetShoppingListCount("Idealmall");
            CashandCarryOffers cco = TradeBA.GetTradeOffers();
            ViewBag.pages = cco.Pages;
            cco.GensampleData();
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
        public ActionResult Searchresult(string query, string browser, string view, string cashandcarry,
            string pageindex)
        {
            try
            {
                if (ValidateCookieSession())
                {
                    int cashandcarryID = string.IsNullOrWhiteSpace(cashandcarry) ? 0 : Convert.ToInt32(cashandcarry);
                    int pindex = string.IsNullOrWhiteSpace(pageindex) ? 1 : Convert.ToInt32(pageindex);

                    //ViewBag.ShoppingCount = TradeBA.GetShoppingListCount(Session["TradeUserName"].ToString());
                    var shopList = TradeShoppingBA.GetTradeShoppingList(Session["TradeUserName"].ToString());
                    if (shopList != null && shopList.Count > 0)
                    {
                        var shoppingCount =
                            shopList.Sum(s => (string.IsNullOrWhiteSpace(s.Qty) ? 0 : Convert.ToInt32(s.Qty)));
                        ViewBag.ShoppingCount = shoppingCount;
                        ViewBag.ShopSummaries = GetShoppingSummaries(shopList);
                    }

                    ViewBag.browser = false;
                    ViewBag.view = string.IsNullOrEmpty(view) ? "list" : "box";
                    ViewBag.searchtext = query;
                    ViewBag.cc = cashandcarryID;

                    string UserName = Session["TradeUserName"] != null
                        ? Session["TradeUserName"].ToString()
                        : string.Empty;
                    ViewBag.TradeUserName = UserName;

                    CashandCarryOffers cco = new CashandCarryOffers();
                    cco =
                        TradeBA.GetTradeOffers(new TradeOffersFilterCriteria()
                                               {
                                                   Product = query,
                                                   ShopId = cashandcarryID,
                                                   PageIndex = pindex
                                               });
                    cco.AllCashandCarrys = TradeBA.GetAllCashAndCarrys();
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

        public ActionResult SignOut()
        {
            Session["TradeUserName"] = null;
            return RedirectToAction("TradeLogin");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetShoppingList()
        {
            try
            {
                if (ValidateCookieSession())
                {
                    var shopList = TradeShoppingBA.GetTradeShoppingList(Session["TradeUserName"].ToString());
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
                                           ShopImgUrl = (g.FirstOrDefault() != null && !string.IsNullOrWhiteSpace(g.FirstOrDefault().ShopImgUrl)) ? Url.Content(g.FirstOrDefault().ShopImgUrl) : string.Empty,
                                           TotalPurchaseAmount = g.Sum(s => s.Total)
                                       }).ToList();

            }
            return shopListSummary;
        }

        // Added by Varadha - Begin

        public ActionResult PriceCompare(string Product, string imageURL)
        {
            try
            {
                string[] strArray = Product.Split('=');
                Product = Product.Replace("=", string.Empty);
                List<Offer> offers = TradeBA.GetPriceCompare(Product.Trim()); //new List<Offer>();
                //offers.Add(new Offer { Shop = new CashandCarryShop { Name = "Chetan" }, OfferRate = 3.59f, Volume = "24*250ml", OfferRatePercent = 48.7f });
                //offers.Add(new Offer { Shop = new CashandCarryShop { Name = "Chetan" }, OfferRate = 3.59f, Volume = "12*500ml", OfferRatePercent = 48.7f });
                //offers.Add(new Offer { Shop = new CashandCarryShop { Name = "Chetan" }, OfferRate = 3.59f, Volume = "24*250ml", OfferRatePercent = 48.7f });
                //offers.ForEach(m =>
                //{
                //    m.Product = Product;
                //    m.ImageURL = "/images/" + imageURL;
                //});
                ViewBag.imageURL = imageURL;
                ViewData.Add("ProductName", strArray[0]);
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
            if (Session["TradeUserName"] != null)
            {
                return true;
            }
            if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
            {
                if (TradeBA.IsValidTradeUser(new TradeUser() { UserName = Request.Cookies["UserName"].ToString(), TradePassword = Request.Cookies["Password"].ToString() }) == "11")
                {
                    Session.Add("TradeUserName", Request.Cookies["UserName"].ToString());
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

        [OutputCache(Duration = 0)]
        public ActionResult ChangePasswordIndex()
        {
            return View("ChangePassword", new TradeUser
            {
            });
        }


        [HttpPost]
        [OutputCache(Duration = 0)]
        public JsonResult ChangePassword(string username, string password, string newpassword, string confirmpassword)
        {
            TradeUser tradeUser = null;
            try
            {
                tradeUser = new TradeUser() { UserName = username, TradePassword = password, NewPassword = newpassword, ConfirmPassword = confirmpassword };
                if (string.IsNullOrWhiteSpace(Convert.ToString(tradeUser.UserName)) ||
                    string.IsNullOrWhiteSpace(Convert.ToString(tradeUser.TradePassword)) ||
                    string.IsNullOrWhiteSpace(Convert.ToString(tradeUser.NewPassword)) ||
                    string.IsNullOrWhiteSpace(Convert.ToString(tradeUser.ConfirmPassword)))
                {
                    return Json("Please enter all the fields.");
                }
                string result = TradeBA.IsValidTradeUser(tradeUser);
                switch (result)
                {
                    case "11":
                        TradeBA.ChangePassword(tradeUser);
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
                TradeUser objTradeUser = TradeBA.GetTradeUserDetails(yourname);

                if (objTradeUser != null && string.IsNullOrWhiteSpace(objTradeUser.EmailAddress))
                    return Json("Please enter valid Trade User ID.");

                string sender, cc, recipient, body;

                body = PopulateContactBody(objTradeUser.FirstName + " " + objTradeUser.LastName, email, subject, message);

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

        [OutputCache(Duration = 0)]
        public ActionResult ContactUsIndex()
        {
            return View("ContactUs", new TradeUser
            {
            });
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



    }
}
