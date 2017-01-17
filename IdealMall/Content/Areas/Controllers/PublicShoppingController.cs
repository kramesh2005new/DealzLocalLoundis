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
            int shopId = id.HasValue ? id.Value : 0;
            ViewData.Add("idHasValue", id.HasValue);
            ViewBag.view = "PublicShopping";

            List<ShoppingList> shopList = new List<ShoppingList>();
            List<LocalShop> localShops = new List<LocalShop>();
            if (ViewData["LocalShops"] == null || ViewData["shopList"] == null)
            {

                shopList = PublicShoppingBA.GetPublicShoppingList(Session["PublicUserName"].ToString());

                string UserName = Session["PublicUserName"] != null ? Session["PublicUserName"].ToString() : string.Empty;
                string PostCode = !string.IsNullOrEmpty(UserName) ? (UserName != Common.Common.GeneralUserName ? PublicBA.GetPostCode(UserName) : Common.Common.GeneralUserPostCode) : string.Empty;
                localShops = PublicShoppingBA.GetLocalShops(PostCode);

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
            ViewBag.PublicUserLastName = Convert.ToString(Session["PublicUserLastName"]);
            return View("ShoppingListV1", cco);
        }


        [OutputCache(Duration = 0)]
        [HttpPost, ValidateInput(false)]
        public JsonResult sendMail(string body)
        {
            try
            {

                string subject = "DealzLocal Shopping List - " + DateTime.UtcNow.ToString("dd-MM-yyyy");

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


                if (Session["PublicUserName"] != null)
                {
                    string sender, cc, recipient;
                    sender = Convert.ToString(ConfigurationManager.AppSettings["SenderMailId"]);
                    cc = Convert.ToString(ConfigurationManager.AppSettings["CC"]);

                    PublicUser objPublicUser = PublicBA.GetPublicUserDetails(Convert.ToString(Session["PublicUserName"]));
                    if (objPublicUser != null)
                    {
                        recipient = objPublicUser.EmailAddress;
                    }
                    else
                    { return Json("Email Address is not available for this user."); }

                    body = PopulateBody(objPublicUser.FirstName + " " + objPublicUser.LastName, body);

                    bool result = PublicShoppingBA.sendMail(sender, recipient, cc, subject, body);
                    return Json(result == true ? "Public shopping list successfully send to your registered mail id." :
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

        [HttpPost]
        public JsonResult ShoppingListByShop_Delete_Public(string Shop_id)
        {
            try
            {
                if (Session["PublicUserName"] != null)
                {
                    bool isSuccess = PublicShoppingBA.Delete_ShoppingListByShop(Convert.ToString(Session["PublicUserName"]), Shop_id);
                    if (isSuccess)
                        return Json("true");
                    else
                        return Json("false");

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
        public JsonResult ShoppingList_Delete_Public(string Product, string volume, string Shop_id)
        {
            try
            {
                if (Session["PublicUserName"] != null)
                {
                    bool isSuccess = PublicShoppingBA.Delete_ShoppingList(Product, Convert.ToString(Session["PublicUserName"]), volume, Shop_id);
                    if (isSuccess)
                        return Json("true");
                    else
                        return Json("false");

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
        public JsonResult ShoppingList_Update_Public(string Product, string volume, string Shop_id, string quantity)
        {
            try
            {
                if (Session["PublicUserName"] != null)
                {

                    bool isSuccess = PublicShoppingBA.Update_ShoppingList(Product, Convert.ToString(Session["PublicUserName"]), volume, Shop_id, quantity);
                    if (isSuccess)
                        return Json("true");
                    else
                        return Json("false");

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

    }
}
