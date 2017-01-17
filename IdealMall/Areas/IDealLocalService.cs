using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using IdealMall.BusinessAccess;
using IdealMall.Entities;
using IdealMall.Common;
using IdealMall.Models;

namespace IdealMall.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IDealLocalService
    {

        [OperationContract]
        [WebInvoke(UriTemplate = "/PublicLogin_Validuser", ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped)]
        string PublicLogin_Validuser(string username, string password);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetPublicUserDetails", ResponseFormat = WebMessageFormat.Json, Method = "POST", BodyStyle = WebMessageBodyStyle.Wrapped)]
        PublicUser GetPublicUserDetails(string objUserName, string objPassword);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetLocalShops", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        List<AllLocalShops> GetLocalShops(string PostCode, string radius);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetPublicShoppingList", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        ShoppingListPublic GetPublicShoppingList(string username);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetPublicShopsOffers", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        LocalShopsOffers GetPublicShopsOffers(string product, int shopID, int pageIndex, string postCode, string radius, int pagesize);


        [OperationContract]
        [WebInvoke(UriTemplate = "/Public_AddToShoppingList", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int Public_AddToShoppingList(string UserId, int Shop_ID, float Offer_Rate, string Product, int Qty, float VAT, string Volume);


        [OperationContract]
        [WebInvoke(UriTemplate = "/CreateAccountPublicUser", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        string CreateAccountPublicUser(string FirstName, string LastName, string ContactNumber, string UserName, string PostCode, string Password);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetMenuDetails", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        List<Department> GetMenuDetails();

        [OperationContract]
        [WebInvoke(UriTemplate = "/Public_SearchResults", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        LocalShopsOffers Public_SearchResults(string query, int cashandcarryID, int pindex, string CurrPostCode, string radius);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Public_ClearShoppingList", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        bool Public_ClearShoppingList(string username);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Public_GetSuggestion", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        List<string> Public_GetSuggestion(string query);


        [OperationContract]
        [WebInvoke(UriTemplate = "/TradeLogin_Validuser", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        string TradeLogin_Validuser(string username, string password);


        [OperationContract]
        [WebInvoke(UriTemplate = "/GetTradeUserDetails", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        TradeUser GetTradeUserDetails(string objUserName, string objPassword);


        [OperationContract]
        [WebInvoke(UriTemplate = "/CreateAccountTradeUser", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        string CreateAccountTradeUser(string FirstName, string LastName, string ContactNumber, string EmailAddress, string BusinessName);

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetTradeShops", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        List<CashandCarryShop> GetTradeShops();

        [OperationContract]
        [WebInvoke(UriTemplate = "/GetTradeOffers", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        CashandCarryOffers GetTradeOffers(int PageIndex, int ShopId, string Product, int PageSize);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Trade_GetSuggestion", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        List<string> Trade_GetSuggestion(string query);

        [OperationContract]
        [WebInvoke(UriTemplate = "/Trade_ClearShoppingList", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        bool Trade_ClearShoppingList(string username);


        [OperationContract]
        [WebInvoke(UriTemplate = "/Trade_AddToShoppingList", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        int Trade_AddToShoppingList(string UserId, int Shop_ID, float Offer_Rate, string Product, int Qty, float VAT, string Volume);

        [WebInvoke(UriTemplate = "/GetTradeShoppingList", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST")]
        ShoppingListTrade GetTradeShoppingList(string username);
        
    }



}
