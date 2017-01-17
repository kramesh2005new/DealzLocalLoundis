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
using System.Text;
using System.IO;

namespace IdealMall.Controllers
{
    public class TradeShoppingController : Controller
    {

        [OutputCache(Duration = 0)]
        public ActionResult Index(int? id)
        {
            int shopId = id.HasValue ? id.Value : 0;
            ViewData.Add("idHasValue", id.HasValue);
            ViewBag.view = "TradeShopping";

            List<ShoppingList> shopList = new List<ShoppingList>();
            List<CashandCarryShop> cashCarrys = new List<CashandCarryShop>();
            if (ViewData["CashAndCarryShops"] == null || ViewData["shopList"] == null)
            {

                if (Session["TradeUserName"] == null)
                {
                    return null;
                }
                shopList = TradeShoppingBA.GetTradeShoppingList(Session["TradeUserName"].ToString());

                cashCarrys = TradeShoppingBA.GetCashAndCarryShops();

                ViewData.Add("CashAndCarryShops", cashCarrys);
                ViewData.Add("shopList", shopList);

            }
            else
            {
                cashCarrys = ViewData["CashAndCarryShops"] as List<CashandCarryShop>;
                ViewData["CashAndCarryShops"] = cashCarrys;

                shopList = ViewData["shopList"] as List<ShoppingList>;
                ViewData["shopList"] = shopList;
            }

            if (Session["TradeBusinessName"] != null)
            {
                ViewBag.TradeBusinessName = Session["TradeBusinessName"];
            }
            ShoppingListTrade cco = new ShoppingListTrade();

            cco.shoppingList = shopList.Where(m => m.ShopId == shopId || shopId == 0).ToList();
            cco.CashandCarry = cashCarrys.Where(m => cco.shoppingList.Where(m1 => m1.ShopId == m.ID).Count() > 0).ToList();
            return View("ShoppingListV1", cco);
        }

        [OutputCache(Duration = 0)]
        [HttpPost, ValidateInput(false)]
        public JsonResult sendMail(string body)
        {
            try
            {

                string subject = "DealzLocal Trade Shopping List - " + DateTime.UtcNow.ToString("dd-MM-yyyy");

                if (!string.IsNullOrWhiteSpace(body))
                {
                    body = body.Replace("../..", string.Empty);
                    body = body.Replace("~", string.Empty);
                    body = body.Replace("/Content", Convert.ToString(ConfigurationManager.AppSettings["MailImagePath"]) + "/Content");
                    body = body.Replace("/content", Convert.ToString(ConfigurationManager.AppSettings["MailImagePath"]) + "/content");
                    body = body.Replace("/Images", Convert.ToString(ConfigurationManager.AppSettings["MailImagePath"]) + "/Images");
                    body = body.Replace("/images", Convert.ToString(ConfigurationManager.AppSettings["MailImagePath"]) + "/images");
                    body = body.Replace("/Scripts", Convert.ToString(ConfigurationManager.AppSettings["MailImagePath"]) + "/Scripts");
                    body = body.Replace("logo.png", "emaillogo.png");
                    
                }


                if (Session["TradeUserName"] != null)
                {
                    string sender, cc, recipient;
                    sender = Convert.ToString(ConfigurationManager.AppSettings["SenderMailId"]);
                    cc = Convert.ToString(ConfigurationManager.AppSettings["CC"]);

                    TradeUser objTradeUser = TradeBA.GetTradeUserDetails(Convert.ToString(Session["TradeUserName"]));
                    if (objTradeUser != null)
                    {
                        recipient = objTradeUser.EmailAddress;
                    }
                    else
                    { return Json("Email Address is not available for this user."); }

                    body = PopulateBody(objTradeUser.FirstName + " " + objTradeUser.LastName, body);

                    bool result = TradeShoppingBA.sendMail(sender, recipient, cc, subject, body);
                    return Json(result == true ? "Trade shopping list successfully send to your registered mail id." :
                                "Error while sending mail.");

                }
                return Json("Session Expired.");
            }
            catch (Exception ex)
            {
                return Json("Error while sending mail.");
            }
        }
        private string PopulateBody(string userName, string shoppinglist)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/EmailTemplate/ShoppingList.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{UserName}", userName);
            body = body.Replace("{body}", shoppinglist);
            return body;
        }

    }
}
