using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DORM;
using IdealMall.Entities;

namespace IdealMall.DataAccess
{
    public static class DbConnection
    {
        public static string ConnectionString
        {
            set
            {
                DynamicORM.ConnectionString = value;
            }
        }
    }
    public static class TradeDA
    {

        public static Int32 GetShoppingListCount(string userID)
        {
            return new DynamicORM().GetEntity<Int32>("Get_CountCart_Trade", new { UserId = userID });

        }

        public static Int32 AddToShoppingList(ShoppingItem item)
        {
            return new DynamicORM().GetEntity<Int32>("Create_ShoppingList_Trade", item);

        }
        public static List<string> GetSuggestion(string searchText)
        {
            return new DynamicORM().GetEntity<List<string>>("Get_SearchSuggession_Trade", new { Product = searchText });
        }
        public static CashandCarryOffers GetTradeOffers(TradeOffersFilterCriteria filter)
        {
            if (filter.Pagesize == 0)
                filter.Pagesize = 15;
            if (filter.PageIndex == 0)
                filter.PageIndex = 1;

            if (!string.IsNullOrEmpty(filter.Product))
            {
                filter.Product = filter.Product.Trim();
            }

            if (!string.IsNullOrEmpty(filter.Product) && filter.ShopId != 0)
                return new DynamicORM().GetEntity<CashandCarryOffers>(filter);
            else if (!string.IsNullOrEmpty(filter.Product))
                return new DynamicORM().GetEntity<CashandCarryOffers>(new { filter.PageIndex, filter.Product, filter.Pagesize });
            else if (filter.ShopId != 0)
                return new DynamicORM().GetEntity<CashandCarryOffers>(new { filter.PageIndex, filter.ShopId, filter.Pagesize });
            else
                return new DynamicORM().GetEntity<CashandCarryOffers>(new { filter.PageIndex, filter.Pagesize });

        }
        public static CashandCarryOffers GetTradeOffers()
        {
            return new DynamicORM().GetEntity<CashandCarryOffers>(new { Pagesize = 15 });
        }
        public static List<Offer> GetProductOffers(string Product)
        {
            Product = Product.Trim();
            return new DynamicORM().GetEntity<List<Offer>>("GetOfferProducts_Trade", new { Product });
        }


        public static String CreateAccountTradeUser(TradeUser tradeUser)
        {
            var user = new TradeUser();
            string result = string.Empty;
            try
            {
               using (var db = new IdealmallEntities())
                {
                    var userFromLoginTrade =
                        db.Login_Trade.FirstOrDefault(
                            f => f.UserName.ToUpper() == tradeUser.EmailAddress.ToUpper());
                    if (userFromLoginTrade != null && userFromLoginTrade.UserName.ToUpper() == tradeUser.EmailAddress.ToUpper())
                    {
                        return "2"; // Email Address already exists.
                    }

                }
                result = new DynamicORM().GetEntity<Int32>("USP_CreateAccount_Trade",
                                                        new
                                                        {
                                                            FirstName = tradeUser.FirstName,
                                                            LastName = tradeUser.LastName,
                                                            EmailId = tradeUser.EmailAddress,
                                                            ContactNo = tradeUser.ContactNumber,
                                                            BusinessName = tradeUser.BusinessName
                                                        }).ToString();
            }
            catch (Exception)
            {
                result = "-1";
            }
            return result;
        }
        public static String IsValidTradeUser(TradeUser tradeUser)
        {
            return new DynamicORM().GetEntity<Int32>("IsValidUser_Trade",
                                                                    new
                                                                    {
                                                                        UserName = tradeUser.UserName,
                                                                        Password = tradeUser.TradePassword
                                                                    }).ToString();
        }

        public static TradeUser GetTradeUserDetails(string userName)
        {
            var user = new TradeUser();
            try
            {
                using (var db = new IdealmallEntities())
                {
                    var userFromLoginTrade =
                        db.Login_Trade.FirstOrDefault(
                            f => f.UserId.ToUpper() == userName.ToUpper());
                    if (userFromLoginTrade != null)
                    {
                        var userFromDb = db.CreateAccount_Trade.FirstOrDefault(f => f.EmailId.ToUpper() == userFromLoginTrade.UserName.ToUpper());
                        if (userFromDb != null)
                        {
                            user.UserName = userFromLoginTrade.UserId;
                            user.FirstName = userFromDb.FirstName;
                            user.LastName = userFromDb.LastName;
                            user.EmailAddress = userFromDb.EmailId;
                            user.PostCode = userFromLoginTrade.PostCode;
                            user.ContactNumber = userFromDb.ContactNo;
                            user.TradePassword = userFromLoginTrade.User_Password;
                            user.BusinessName = userFromDb.BusinessName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return user;
        }


        public static List<AllCashandCarrys> GetAllCashAndCarrys()
        {
            return new DynamicORM().GetEntity<List<AllCashandCarrys>>("Get_CashAndCarrys");
        }
        public static bool ClearShoppingList(string UserId)
        {
            return (new DynamicORM().GetEntity<Int32>("Delete_ShoppingList_Trade", new { @UserId = UserId }).ToString() == "1");
        }

        public static List<Offer> GetPriceCompare(string Product)
        {
            return new DynamicORM().GetEntity<List<Offer>>("GetPriceComparison_Trade", new { Product });
        }
        public static String ChangePassword(TradeUser tradeUser)
        {
            return new DynamicORM().GetEntity<Int32>("Update_PasswordTrade",
                                                                    new
                                                                    {
                                                                        @userid = tradeUser.UserName,
                                                                        @prevPwd = tradeUser.TradePassword,
                                                                        @newPwd = tradeUser.NewPassword
                                                                    }).ToString();
        }
    }
}
