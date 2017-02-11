using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DORM;
using IdealMall.Common;
using IdealMall.Entities;
using LinqKit;
using PredicateBuilder = IdealMall.Common.PredicateBuilder;

namespace IdealMall.DataAccess
{
    //public static class DbConnection
    //{
    //    public static string ConnectionString
    //    {
    //        set
    //        {
    //            DynamicORM.ConnectionString = value;                
    //        }
    //    }
    //}
    public static class PublicDA
    {

        public static Int32 GetShoppingListCount(string userID)
        {
            return new DynamicORM().GetEntity<Int32>("Get_CountCart_Public", new { UserId = userID });
        }

        public static Int32 AddToShoppingList(ShoppingItem item)
        {
            return new DynamicORM().GetEntity<Int32>("Create_ShoppingList_Public", item);

        }

        public static Int32 GetProductId(string productName, string volume)
        {
            return new DynamicORM().GetEntity<Int32>("Get_ProductId", new { Product = productName, Volume = volume });

        }

        public static List<string> GetSuggestion(string searchText)
        {
            return new DynamicORM().GetEntity<List<string>>("Get_SearchSuggession_Public", new { Product = searchText });
        }

        public static LocalShopsOffers GetPublicOffers(PublicOffersFilterCriteria filter)
        {
            if (filter.Pagesize == 0)
                filter.Pagesize = 15;
            if (filter.PageIndex == 0)
                filter.PageIndex = 1;
            if (!string.IsNullOrEmpty(filter.Product))
            {
                filter.Product = filter.Product.Trim();
            }

            return GetPublicOffersBasedOnProductAndShop(filter);

        }

        public static LocalShopsOffers GetHomeOffers(PublicOffersFilterCriteria filter)
        {
            LocalShopsOffers objLocalShopOffers = new LocalShopsOffers();

            if (filter.Pagesize == 0)
                filter.Pagesize = 15;
            if (filter.PageIndex == 0)
                filter.PageIndex = 1;
            if (!string.IsNullOrEmpty(filter.Product))
            {
                filter.Product = filter.Product.Trim();
            }

            filter.Offer_Mode = "N";
            LocalShopsOffers New_Offer = GetPublicOffersBasedOnProductAndShop(filter);

            filter.Offer_Mode = "W";
            LocalShopsOffers Weekly_Offer = GetPublicOffersBasedOnProductAndShop(filter);

            filter.Offer_Mode = "M";
            LocalShopsOffers Monthly_Offer = GetPublicOffersBasedOnProductAndShop(filter);

            filter.Offer_Mode = "B";
            LocalShopsOffers Best_Offer = GetPublicOffersBasedOnProductAndShop(filter);
            objLocalShopOffers.Offers = New_Offer.Offers;

            objLocalShopOffers.Offers.AddRange(New_Offer.Offers);
            objLocalShopOffers.Offers.AddRange(Weekly_Offer.Offers);
            objLocalShopOffers.Offers.AddRange(Monthly_Offer.Offers);
            objLocalShopOffers.Offers.AddRange(Best_Offer.Offers);
            return objLocalShopOffers;

        }


        public static LocalShopsOffers GetLocalShops(LatLng latLng, string radius)
        {
            LocalShopsOffers localShopsOffers = new LocalShopsOffers();
            if (string.IsNullOrWhiteSpace(radius))
            {
                radius = Common.Common.PublicUserGeneralRadius;
            }
            localShopsOffers.AllLocalShops = new DynamicORM().GetEntity<List<AllLocalShops>>("Get_Retails_ByRadius",
                new { Latitude = latLng.Latitude, Longitude = latLng.Longitude, Radius = Convert.ToDecimal(radius) });
            return localShopsOffers;
        }

        public static LocalShopsOffers GetLocalShops(string Postcode)
        {
            LocalShopsOffers localShopsOffers = new LocalShopsOffers();
            //return new DynamicORM().GetEntity<LocalShopsOffers>(new { Pagesize = 15 });
            localShopsOffers.AllLocalShops = new DynamicORM().GetEntity<List<AllLocalShops>>("Get_Retails",
                new { Postcode });
            //return new DynamicORM().GetEntity<List<LocalShops>>("Get_Retails", new { Postcode});
            return localShopsOffers;
        }

        public static List<Offer> GetProductOffers(string Product)
        {
            Product = Product.Trim();
            return new DynamicORM().GetEntity<List<Offer>>("GetOfferProducts_Public", new { Product });
        }


        public static String CreateAccountPublicUser(PublicUser publicUser)
        {
            var user = new PublicUser();
            string result = string.Empty;
            try
            {
                using (var db = new IdealmallEntities())
                {
                    var userFromLoginTrade =
                        db.Login_Public.FirstOrDefault(
                            f => f.UserId.ToUpper() == publicUser.EmailAddress.ToUpper());
                    if (userFromLoginTrade != null && userFromLoginTrade.UserId.ToUpper() == publicUser.EmailAddress.ToUpper())
                    {
                        return "2"; // Email Address already exists.
                    }

                }
                return new DynamicORM().GetEntity<Int32>("USP_CreateAccount_Public",
                    new
                    {
                        FirstName = publicUser.FirstName,
                        LastName = publicUser.LastName,
                        ContactNo = publicUser.ContactNumber,
                        UserId = publicUser.EmailAddress,
                        Postcode = publicUser.PostCode.Replace(" ", string.Empty),
                        Password = publicUser.Password
                    }).ToString();
            }
            catch (Exception)
            {
                result = "-1";
            }
            return result;
        }

        public static String IsValidPublicUser(PublicUser publicUser)
        {
            return new DynamicORM().GetEntity<Int32>("IsValidUser_public",
                new
                {
                    UserName = publicUser.EmailAddress,
                    Password = publicUser.Password
                }).ToString();
        }

        public static PublicUser GetPublicUserDetails(string userName)
        {
            var user = new PublicUser();
            try
            {
                using (var db = new IdealmallEntities())
                {
                    var userFromDb =
                        db.CreateAccount_Public.FirstOrDefault(
                            f => f.UserId.ToUpper() == userName.ToUpper());
                    if (userFromDb != null)
                    {
                        user.FirstName = userFromDb.FirstName;
                        user.LastName = userFromDb.LastName;
                        user.EmailAddress = userFromDb.UserId;
                        user.PostCode = userFromDb.PostCode;
                        user.ContactNumber = userFromDb.ContactNo;
                        user.Password = userFromDb.User_Password;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return user;
        }

        public static PublicUser GetPublicUserDetails(string userName, string password)
        {
            var user = new PublicUser();
            try
            {
                using (var db = new IdealmallEntities())
                {
                    var userFromDb =
                        db.CreateAccount_Public.FirstOrDefault(
                            f => f.UserId.ToUpper() == userName.ToUpper() && f.User_Password == password);
                    if (userFromDb != null)
                    {
                        user.FirstName = userFromDb.FirstName;
                        user.LastName = userFromDb.LastName;
                        user.EmailAddress = userFromDb.UserId;
                        user.PostCode = userFromDb.PostCode;
                        user.ContactNumber = userFromDb.ContactNo;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return user;
        }

        public static String ChangePassword(PublicUser publicUser)
        {
            return new DynamicORM().GetEntity<Int32>("Update_PasswordPublic",
                new
                {
                    @userid = publicUser.EmailAddress,
                    @prevPwd = publicUser.Password,
                    @newPwd = publicUser.NewPassword
                }).ToString();
        }

        public static string GetPostCode(string EmailAddress)
        {
            var postCodes = new DynamicORM().GetEntity<List<DBTransactionResult>>("Get_User_PostCode",
                new
                {
                    @UserName = EmailAddress
                });

            if (postCodes != null && postCodes.Count > 0)
            {
                return postCodes.First().DBResult;
            }
            return string.Empty;
        }

        //public static List<AllLocalShops> GetAllLocalShops()
        //{
        //    return new DynamicORM().GetEntity<List<AllLocalShops>>("Get_Retails");
        //}
        public static bool ClearShoppingList(string UserId)
        {
            return
                (new DynamicORM().GetEntity<Int32>("Delete_ShoppingList_Public", new { @UserId = UserId }).ToString() ==
                 "1");
        }

        public static List<Offer> GetPriceCompare(string Product, string Postcode, string radius, LatLng latLng)
        {
            if (string.IsNullOrWhiteSpace(radius))
            {
                radius = "0.5";
            }
            return new DynamicORM().GetEntity<List<Offer>>("GetPriceComparisonByRadius_Public", new { Product, Postcode, radius, latLng.Latitude, latLng.Longitude });

        }

        public static List<string> GetPostCodeSuggestions(string PostCode)
        {
            return new DynamicORM().GetEntity<List<string>>("Get_PostCodeSuggession_Public", new { @PostCode = PostCode });
        }

        public static IList<postcodelatlng> GetPostCodesLike(string postCode)
        {
            try
            {
                using (var db = new IdealmallEntities())
                {
                    return db.postcodelatlngs.Where(w => w.postcode.StartsWith(postCode)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return null;
        }


        public static double GetDistanceInMiles(double actualLat, double actualLng, double dbLat, double dbLng)
        {
            double R = 3960;
            var lat = (dbLat - actualLat).ToRadians();
            var lng = (dbLng - actualLng).ToRadians();
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                     Math.Cos(actualLat.ToRadians()) * Math.Cos(dbLat.ToRadians()) *
                     Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return R * h2;
        }

        private static LocalShopsOffers ConvertToLocalShopOffersList(IList<ProductOfferPublic> offers)
        {
            if (null == offers || offers.Count == 0)
                return new LocalShopsOffers();

            var lso = new LocalShopsOffers();
            foreach (var off in offers)
            {
                var publicOffer = new PublicOffer
                                  {
                                      Product = off.ProductName,
                                      OfferRate = Convert.ToSingle(off.OfferRate),
                                      Volume = off.Volume,
                                      Sell_Price = Convert.ToString(off.SellPrice),
                                      POR = Convert.ToSingle(off.POR),
                                      Additional_offer = off.AdditionalOffer,
                                      ImageURL = @"~/images/Public/" + off.ProductImageUrl,
                                      OfferEnddate = (off.OfferEndDate == null
                                          ? string.Empty
                                          : off.OfferEndDate.Value.ToString("dd MMM yyyy")),
                                      Previous_Prize = off.Previous_Prize,
                                      Shop_Orginal_Name = off.Shop_Orginal_Name,
                                      Offer_Mode = off.Offer_Mode
                                  };
                publicOffer.Shop = new LocalShop
                                   {
                                       ID = off.ShopId,
                                       ImgUrl = @"~/images/Retail/" + off.ShopName + ".png",
                                       Name = off.ShopName
                                   };
                lso.Offers.Add(publicOffer);
            }

            lso.Totalcount = offers.Count;

            var groupedShops = from of in offers
                               group of by new
                                           {
                                               of.ShopId,
                                               of.ShopName,
                                               of.ShopImgUrl
                                           }
                                   into gof
                                   select new
                                          {
                                              gof.Key.ShopId,
                                              gof.Key.ShopName,
                                              gof.Key.ShopImgUrl
                                          };

            groupedShops.ToList().ForEach(f =>
                                          {
                                              var localShop = new LocalShop
                                                              {
                                                                  ID = f.ShopId,
                                                                  ImgUrl = @"~/images/Retail/" + f.ShopName + ".png",
                                                                  Name = f.ShopName
                                                              };
                                              lso.local.Add(localShop);
                                          });
            return lso;
        }

        public static LocalShopsOffers GetPublicOffersBasedOnProductAndShop(PublicOffersFilterCriteria filterCriteria)
        {
            var lso = new LocalShopsOffers();
            try
            {
                using (var db = new IdealmallEntities())
                {
                    var predicate = IdealMall.Common.PredicateBuilder.True<OfferProducts_Public>();

                    if (false == string.IsNullOrWhiteSpace(filterCriteria.Product))
                    {
                        predicate = PredicateBuilder.And(predicate,
                            p1 => p1.Product.StartsWith(filterCriteria.Product) ||
                                  p1.Category.StartsWith(filterCriteria.Product) ||
                                  p1.Department.StartsWith(filterCriteria.Product));
                    }

                    if (filterCriteria.ShopId != 0)
                    {
                        predicate = PredicateBuilder.And(predicate, p1 => p1.Shop_Id == filterCriteria.ShopId);
                    }
                    if (!string.IsNullOrWhiteSpace(filterCriteria.Offer_Mode))
                    {
                        predicate = PredicateBuilder.And(predicate, p1 => p1.Offer_Mode == filterCriteria.Offer_Mode);
                    }
                    var pubOffers = (from opp in Extensions.AsExpandable(db.OfferProducts_Public).Where(predicate)
                                     select new ProductOfferPublic
                                            {
                                                ShopId = opp.Shop_Id ?? 0,
                                                ShopName = opp.Retailshop.Shop_Name,
                                                ShopImgUrl = opp.Retailshop.Shop_ImageURL,
                                                ProductName = opp.Product,
                                                OfferRate = opp.Offer_Rate ?? 0,
                                                Volume = opp.Volume,
                                                SellPrice = opp.SellAt ?? 0,
                                                POR = opp.POR ?? 0,
                                                AdditionalOffer = opp.Additional_Offer,
                                                ProductImageUrl = opp.Product_ImageURL,
                                                OfferEndDate = opp.Offer_Enddate,
                                                Previous_Prize = opp.Previous_Prize,
                                                Shop_Orginal_Name = opp.Retailshop.Shop_Orginal_Name,
                                                Offer_Mode = opp.Offer_Mode
                                            }).ToList();
                    var offersList = pubOffers.Skip((filterCriteria.PageIndex - 1) * filterCriteria.Pagesize)
                        .Take(filterCriteria.Pagesize)
                        .ToList();
                    lso = ConvertToLocalShopOffersList(pubOffers);
                    lso.Totalcount = pubOffers.Count;

                    //lso = ConvertToLocalShopOffersList(pubOffers);

                }

            }
            catch (Exception)
            {
                throw;
            }
            return lso;
        }

        public static LocalShopsOffers GetPublicOffersBasedOnShop(PublicOffersFilterCriteria filterCriteria)
        {
            var lso = new LocalShopsOffers();
            try
            {
                using (var db = new IdealmallEntities())
                {
                    if (filterCriteria.ShopId != 0)
                    {

                        var pubOffers = (from opp in db.OfferProducts_Public
                                         where opp.Shop_Id == filterCriteria.ShopId
                                         select new ProductOfferPublic
                                                {
                                                    ShopId = opp.Shop_Id ?? 0,
                                                    ShopName = opp.Retailshop.Shop_Name,
                                                    ShopImgUrl = opp.Retailshop.Shop_ImageURL,
                                                    ProductName = opp.Product,
                                                    OfferRate = opp.Offer_Rate ?? 0,
                                                    Volume = opp.Volume,
                                                    SellPrice = opp.SellAt ?? 0,
                                                    POR = opp.POR ?? 0,
                                                    AdditionalOffer = opp.Additional_Offer,
                                                    ProductImageUrl = opp.Product_ImageURL,
                                                    OfferEndDate = opp.Offer_Enddate,
                                                    Previous_Prize = opp.Previous_Prize,
                                                    Shop_Orginal_Name = opp.Retailshop.Shop_Orginal_Name
                                                }).ToList();
                        var offersList = pubOffers.Skip((filterCriteria.PageIndex - 1) * filterCriteria.Pagesize)
                            .Take(filterCriteria.Pagesize)
                            .ToList();
                        lso = ConvertToLocalShopOffersList(pubOffers);
                        lso.Totalcount = pubOffers.Count;

                        //lso = ConvertToLocalShopOffersList(pubOffers);
                    }

                }

            }
            catch (Exception)
            {
                throw;
            }
            return lso;
        }
    }
}

