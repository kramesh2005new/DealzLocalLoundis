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
    public class PublicShoppingController : Controller
    {

       [OutputCache(Duration=0)]
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
           cco.LocalShop = localShops.Where(m=> cco.shoppingList.Where(m1=> m1.ShopId == m.ID).Count() > 0 ).ToList();            
           return View("ShoppingList", cco);
        }

    }
}
