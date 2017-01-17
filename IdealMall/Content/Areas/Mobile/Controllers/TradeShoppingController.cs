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

            ShoppingListTrade cco = new ShoppingListTrade();

            cco.shoppingList = shopList.Where(m => m.ShopId == shopId || shopId == 0).ToList();
            cco.CashandCarry = cashCarrys.Where(m => cco.shoppingList.Where(m1 => m1.ShopId == m.ID).Count() > 0).ToList();
            return View("ShopKart", cco.shoppingList);
        }
    }
}
