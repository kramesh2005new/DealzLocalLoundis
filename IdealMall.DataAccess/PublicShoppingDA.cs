using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DORM;
using IdealMall.Entities;


namespace IdealMall.DataAccess
{
    public static class PublicShoppingDA
    {

        public static List<ShoppingList> GetPublicShoppingList(string UserId)
        {
            var shoppingList = new List<ShoppingList>();
            try
            {
                using (var db = new IdealmallEntities())
                {
                    var enumerableList = (from a in db.ShoppingList_Public
                                          join s in db.Retailshops
                                              on a.Shop_Id equals s.Shop_Id
                                          where a.UserId == UserId
                                          group a by new { a.Product, a.Shop_Id, s.Shop_Name, s.Shop_ImageURL, a.Offer_Rate, a.Volume, a.VAT, s.Shop_Orginal_Name } into g
                                          select new
                                          {

                                              ShopId = g.Key.Shop_Id ?? 0,
                                              ShopName = g.Key.Shop_Name,
                                              ShopImgUrl = @"~/images/Retail/" + g.Key.Shop_Name + ".png",
                                              OfferRate = g.Key.Offer_Rate ?? 0,
                                              Product = g.Key.Product.Trim(),
                                              VAT = g.Key.VAT ?? 0,
                                              Qty = g.Sum(a => a.Qty),
                                              Volume = g.Key.Volume,
                                              Total = g.Sum(a => a.Total),
                                              Shop_Orginal_Name = g.Key.Shop_Orginal_Name

                                          });


                    var result = (from a in enumerableList
                                  join s in db.OfferProducts_Public on a.Product.Trim() equals s.Product.Trim()
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
                                      Volume = a.Volume,
                                      AdditionalInfo = s.Additional_Info,
                                      productImgUrl = @"~/images/Public/" + s.Product_ImageURL,
                                      Shop_Orginal_Name = a.Shop_Orginal_Name,
                                      Offer_Date = s.Offer_Enddate,
                                      Previous_Prize = s.Previous_Prize
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
                            Volume = item.Volume,
                            Qty = Convert.ToString(item.Qty),
                            Total = Convert.ToDecimal(item.Total),
                            Additional_Info = item.AdditionalInfo,
                            Product_ImgUrl = item.productImgUrl,
                            Shop_Orginal_Name = item.Shop_Orginal_Name,
                            Shop_CheckOut_UrL = string.Empty,
                            Offer_Date = Convert.ToString(item.Offer_Date.Value.ToString("dd-MMM-yyyy")),
                            Previous_Prize = Convert.ToString(item.Previous_Prize.Value)
                        });
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return shoppingList;
            //return new DynamicORM().GetEntity<List<ShoppingList>>("Get_ShoppingList_Public", new { UserId = UserId });

        }

        //public static List<CashandCarryShop> GetCashAndCarryShops()
        //{
        //    return new DynamicORM().GetEntity<List<CashandCarryShop>>("Get_CashAndCarrys",new { });

        //}
        public static List<LocalShop> GetLocalShops()
        {
            return new DynamicORM().GetEntity<List<LocalShop>>("Get_Retails", new { });

        }
        public static List<LocalShop> GetLocalShops(string Postcode)
        {
            return new DynamicORM().GetEntity<List<LocalShop>>("Get_Retails", new { Postcode });

        }

        public static bool Delete_ShoppingListbyShop(string Username, string Shop_id)
        {
            bool isDeleted = false;
            try
            {
                using (var db = new IdealmallEntities())
                {
                    int Shopid = Convert.ToInt32(Shop_id);
                    db.ShoppingList_Public.RemoveRange(db.ShoppingList_Public.Where(x => x.UserId.Trim() == Username.Trim() && x.Shop_Id.Value == Shopid));
                    db.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDeleted;
        }

        public static bool Delete_ShoppingList(string Product, string Username, string volume, string Shop_id)
        {
            bool isDeleted = false;
            try
            {
                using (var db = new IdealmallEntities())
                {
                    int Shopid = Convert.ToInt32(Shop_id);
                    db.ShoppingList_Public.RemoveRange(db.ShoppingList_Public.Where(x => x.Product.Trim() == Product.Trim() && x.UserId.Trim() == Username.Trim() && x.Volume.Trim() == volume.Trim() && x.Shop_Id.Value == Shopid));
                    db.SaveChanges();
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDeleted;
        }

        public static bool Update_ShoppingList(string Product, string Username, string volume, string Shop_id, string qty)
        {
            bool isAffected = false;
            try
            {
                using (var db = new IdealmallEntities())
                {
                    int Shopid = Convert.ToInt32(Shop_id);
                    int qty1 = Convert.ToInt32(qty);
                    List<ShoppingList_Public> lstOffer = db.ShoppingList_Public.Where(a => a.Shop_Id.Value == Shopid && a.Product.Trim() == Product.Trim() && a.Volume.Trim() == volume.Trim()).ToList();
                    if (lstOffer != null && lstOffer.Count > 0)
                    {
                        ShoppingList_Public offer = lstOffer[0];
                        decimal total = Math.Round(offer.Offer_Rate.Value * Convert.ToDecimal(qty), 2);
                        db.ShoppingList_Public.RemoveRange(db.ShoppingList_Public.Where(x => x.Product.Trim() == Product.Trim() && x.UserId.Trim() == Username.Trim() && x.Volume.Trim() == volume.Trim() && x.Shop_Id.Value == Shopid));
                        db.ShoppingList_Public.Add(new ShoppingList_Public { Shop_Id = Shopid, Product = Product, UserId = Username, Volume = volume, Total = total, VAT = offer.VAT, Offer_Rate = offer.Offer_Rate, Qty = qty1 });
                        db.SaveChanges();
                        isAffected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAffected;
        }


    }
}
