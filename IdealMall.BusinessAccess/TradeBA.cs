using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdealMall.DataAccess;
using IdealMall.Entities;
namespace IdealMall.BusinessAccess
{
    public static class Database
    {
        public static string ConnectionString
        {
            set
            {
                DbConnection.ConnectionString = value;
            }
        }

    }
    public class TradeBA
    {
        public static List<string> GetSuggestion(string searchText)
        {
            return TradeDA.GetSuggestion(searchText);
        }
        public static CashandCarryOffers GetTradeOffers(TradeOffersFilterCriteria filter)
        {
            return TradeDA.GetTradeOffers(filter);
        }
        public static CashandCarryOffers GetTradeOffers()
        {
            return TradeDA.GetTradeOffers();
        }

        public static List<Offer> GetProductOffers(string Product)
        {
            return TradeDA.GetProductOffers(Product);

        }

        public static string CreateAccountTradeUser(TradeUser tradeUser)
        {
            return TradeDA.CreateAccountTradeUser(tradeUser);

        }
        public static Int32 GetShoppingListCount(string userID)
        {
            return TradeDA.GetShoppingListCount(userID);


        }

        public static Int32 AddToShoppingList(ShoppingItem item)
        {
            return TradeDA.AddToShoppingList(item);
        }

        public static string IsValidTradeUser(TradeUser tradeUser)
        {
            return TradeDA.IsValidTradeUser(tradeUser);
        }

        public static List<AllCashandCarrys> GetAllCashAndCarrys()
        {
            return TradeDA.GetAllCashAndCarrys();
        }
        public static bool ClearShoppingList(string UserId)
        {
            return TradeDA.ClearShoppingList(UserId);
        }

        public static List<Offer> GetPriceCompare(string Product)
        {
            return TradeDA.GetPriceCompare(Product);

        }
        public static string ChangePassword(TradeUser tradeUser)
        {
            return TradeDA.ChangePassword(tradeUser);
        }

        public static TradeUser GetTradeUserDetails(string username)
        {
            return TradeDA.GetTradeUserDetails(username);
        }
    }
}
