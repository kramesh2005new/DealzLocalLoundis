using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdealMall.Models;
using IdealMall.Common;
using IdealMall.Entities;
using IdealMall.BusinessAccess;
using System.IO;
using System.Configuration;

namespace IdealMall.Controllers
{
    public class PublicShoppingController : Controller
    {

        [OutputCache(Duration = 0)]
        public ActionResult Index(int? id)
        {
            if (Session["PublicUserName"] == null)
            {
                return RedirectToAction("LoginIndex", "Home");
            }

            int shopId = id.HasValue ? id.Value : 0;
            ViewData.Add("idHasValue", id.HasValue);
            ViewBag.view = "PublicShopping";

            List<ShoppingList> shopList = new List<ShoppingList>();
            List<LocalShop> localShops = new List<LocalShop>();
            if (ViewData["LocalShops"] == null || ViewData["shopList"] == null)
            {

                shopList = PublicShoppingBA.GetPublicShoppingList(Session["PublicUserName"].ToString());

                string UserName = Session["PublicUserName"] != null ? Session["PublicUserName"].ToString() : string.Empty;
                localShops = PublicShoppingBA.GetLocalShops();

                ViewData.Add("LocalShops", localShops);
                ViewData.Add("shopList", shopList);

            }
            else
            {
                localShops = ViewData["LocalShops"] as List<LocalShop>;
                ViewData["LocalShops"] = localShops;

                shopList = ViewData["shopList"] as List<ShoppingList>;
                ViewData["shopList"] = shopList;
            }

            ShoppingListPublic cco = new ShoppingListPublic();

            cco.shoppingList = shopList.Where(m => m.ShopId == shopId || shopId == 0).ToList();
            cco.LocalShop = localShops.Where(m => cco.shoppingList.Where(m1 => m1.ShopId == m.ID).Count() > 0).ToList();
            ViewBag.PublicUserFirstName = Convert.ToString(Session["PublicUserFirstName"]);
            ViewBag.PublicUserName = Convert.ToString(Session["PublicUserName"]);
            ViewBag.PublicUserLastName = Convert.ToString(Session["PublicUserLastName"]);
            return View("ShoppingListV1", cco);
        }

        [OutputCache(Duration = 0)]
        public ActionResult Back()
        {
            return RedirectToAction("HomeDirectPublic", "Public", new { Mode = "N", query = string.Empty });
        }


        [OutputCache(Duration = 0)]
        [HttpPost, ValidateInput(false)]
        public JsonResult sendMail(string body)
        {
            try
            {

                string subject = "Shopping List - " + DateTime.UtcNow.ToString("dd-MM-yyyy");

                if (!string.IsNullOrWhiteSpace(body))
                {
                    body = body.Replace("../..", string.Empty);
                    body = body.Replace("~", string.Empty);
                    body = body.Replace("/Content", Convert.ToString(ConfigurationManager.AppSettings["MailImagePath"]) + "/Content");
                    body = body.Replace("/content", Convert.ToString(ConfigurationManager.AppSettings["MailImagePath"]) + "/content");
                    body = body.Replace("/Images", Convert.ToString(ConfigurationManager.AppSettings["MailImagePath"]) + "/Images");
                    body = body.Replace("/images", Convert.ToString(ConfigurationManager.AppSettings["MailImagePath"]) + "/images");
                    body = body.Replace("/Scripts", Convert.ToString(ConfigurationManager.AppSettings["MailImagePath"]) + "/Scripts");
                    body = body.Replace("/img", Convert.ToString(ConfigurationManager.AppSettings["MailImagePath"]) + "/img");
                  //  body = body.Replace("logo.png", "emaillogo.png");
                }


                if (Session["PublicUserName"] != null)
                {
                    string sender, cc, recipient;
                    sender = Convert.ToString(ConfigurationManager.AppSettings["SenderMailId"]);
                    cc = Convert.ToString(ConfigurationManager.AppSettings["CC"]);

                    PublicUser objPublicUser = PublicBA.GetPublicUserDetails(Convert.ToString(Session["PublicUserName"]));
                    if (objPublicUser != null)
                    {
                      //  recipient = Convert.ToString(ConfigurationManager.AppSettings["SenderMailId"]); 
                        recipient = objPublicUser.EmailAddress;
                    }
                    else
                    { return Json("Email Address is not available for this user."); }

                    body = PopulateBody(objPublicUser.FirstName + " " + objPublicUser.LastName, body);

                    bool result = PublicShoppingBA.sendMail(sender, recipient, cc, subject, body);
                    return Json(result == true ? "Shopping details sent successfully to your registered mail id." :
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
        [OutputCache(Duration = 0)]
        [HttpPost, ValidateInput(false)]
        public JsonResult ShoppingList_Delete_Public(string Product, string volume, string Shop_id)
        {
            PublicShoppingBA.Delete_ShoppingList(Product, Session["PublicUserName"].ToString(), volume, Shop_id);
            return Json("true");
        }

        [OutputCache(Duration = 0)]
        [HttpPost, ValidateInput(false)]
        public JsonResult ShoppingList_Shop_Public(string Shop_id)
        {
            PublicShoppingBA.Delete_ShoppingListByShop(Session["PublicUserName"].ToString(), Shop_id);
            return Json("true");
        }


        [OutputCache(Duration = 0)]
        [HttpPost, ValidateInput(false)]
        public JsonResult Update_ShoppingList(string Product, string volume, string Shop_id, string qty)
        {
            PublicShoppingBA.Update_ShoppingList(Product, Session["PublicUserName"].ToString(), volume, Shop_id, qty);
            return Json("true");
        }

    }
}
