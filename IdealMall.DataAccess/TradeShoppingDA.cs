using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DORM;
using IdealMall.Entities;


namespace IdealMall.DataAccess
{
    public static class TradeShoppingDA
    {

        public static List<ShoppingList> GetTradeShoppingList(string UserId)
        {
            var shoppingList = new List<ShoppingList>();
            try
            {
                using (var db = new IdealmallEntities())
                {
                    var enumerableList = (from a in db.ShoppingList_Trade
                                          join s in db.CashAndCarrys
                                          on a.Shop_Id equals s.Shop_Id
                                          where a.UserId == UserId
                                          group a by new { a.Product, a.Shop_Id, s.Shop_Name, s.Shop_ImageURL, a.Offer_Rate, a.Volume, a.VAT } into g
                                          select new
                                          {
                                              ShopId = g.Key.Shop_Id ?? 0,
                                              ShopName = g.Key.Shop_Name,
                                              ShopImgUrl = @"~/images/CashAndCarry/" + g.Key.Shop_ImageURL,
                                              OfferRate = g.Key.Offer_Rate ?? 0,
                                              Product = g.Key.Product.Trim() + "(" + g.Key.Volume.Trim() + ")",
                                              VAT = g.Key.VAT ?? 0,
                                              Qty = g.Sum(a => a.Qty),
                                              Total = g.Sum(a => a.Total)
                                          });

                    var result = (from a in enumerableList
                                  join s in db.OfferProducts_Trade on a.Product equals s.Product.Trim() + "(" + s.Volume.Trim() + ")"
                                  where a.ShopId == s.Shop_Id
                                  select new
                                  {
                                      ShopId = a.ShopId,
                                      ShopName = a.ShopName,
                                      ShopImgUrl = a.ShopImgUrl,
                                      OfferRate = a.OfferRate,
                                      Product = a.Product,
                                      VAT = a.VAT,
                                      Qty = a.Qty,
                                      Total = a.Total,
                                      AdditionalInfo = s.Additional_Info
                                  });

                    foreach (var item in result)
                    {
                        shoppingList.Add(new ShoppingList()
                        {
                            ShopId = item.ShopId,
                            ShopName = item.ShopName,
                            ShopImgUrl = item.ShopImgUrl,
                            OfferRate = item.OfferRate,
                            Product = item.Product,
                            VAT = item.VAT,
                            Qty = Convert.ToString(item.Qty),
                            Total = Convert.ToDecimal(item.Total),
                            Additional_Info = item.AdditionalInfo
                        });
                    }
                }

                ////if (shoppingList != null && shoppingList.Count > 0)
                ////{
                ////   var result = shoppingList.GroupBy(sh => 
                ////              new { sh.Product,sh.ShopId } )
                ////             select
                ////                 (sh=>new ShoppingList {
                ////                 Qty = Convert.ToString (sh.Sum(p=>Convert.ToInt32  (p.Qty) )),
                ////                 Total = sh.Sum(p=>p.Total )
                ////             });


                ////}
            }
            catch (Exception ex)
            {

            }
            return shoppingList;
            //return new DynamicORM().GetEntity<List<ShoppingList>>("Get_ShoppingList_Trade", new { UserId = UserId });

        }

        public static List<CashandCarryShop> GetCashAndCarryShops()
        {
            return new DynamicORM().GetEntity<List<CashandCarryShop>>("Get_CashAndCarrys", new { });

        }
    }
}
