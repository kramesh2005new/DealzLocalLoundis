using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using IdealMall.BusinessAccess;
using IdealMall.DataAccess;
using IdealMall.Entities;
using IdealMall.Common;
using IdealMall.Models;

namespace IdealMall.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DealzLocalService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DealzLocalService.svc or DealzLocalService.svc.cs at the Solution Explorer and start debugging.
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class DealLocalService : IDealLocalService
    {
        public DealLocalService()
        {
            IdealMall.BusinessAccess.Database.ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
        }

        public string PublicLogin_Validuser(string username, string password)
        {

            PublicUser objPublicUser = new PublicUser();
            objPublicUser.EmailAddress = username;
            objPublicUser.Password = password;
            return PublicBA.IsValidPublicUser(objPublicUser);
        }

        public class SearchCriteria
        {
            public string PostCode { get; set; }
            public string radius { get; set; }
        }

        public List<AllLocalShops> GetLocalShops(string PostCode, string radius)
        {
            LocalShopsOffers objLocalShopOffer = PublicBA.GetLocalShops(PostCode, radius);

            if (objLocalShopOffer != null)
            {
                if (objLocalShopOffer.AllLocalShops != null && objLocalShopOffer.AllLocalShops.Count > 0)
                {
                    foreach (AllLocalShops objAllLocalShop in objLocalShopOffer.AllLocalShops)
                    {
                        objAllLocalShop.ImgUrl = objAllLocalShop.ImgUrl.Replace("~", Convert.ToString(ConfigurationManager.AppSettings["AllShopsImagePath"]));
                    }
                }

                if (objLocalShopOffer.Offers != null && objLocalShopOffer.Offers.Count > 0)
                {
                    foreach (PublicOffer objPublicOffer in objLocalShopOffer.Offers)
                    {

                        objPublicOffer.ImageURL = objPublicOffer.ImageURL.Replace("~", Convert.ToString(ConfigurationManager.AppSettings["PublicOfferImagePath"])); ;
                    }
                }
            }

            return objLocalShopOffer.AllLocalShops;
        }

        public PublicUser GetPublicUserDetails(string objUserName, string objPassword)
        {
            return PublicBA.GetPublicUserDetails(objUserName, objPassword);
        }

        #region IDealLocalService Members

        public LocalShopsOffers GetPublicShopsOffers(string product, int shopID, int pageIndex, string postCode, string radius, int pagesize)
        {
            LocalShopsOffers objLocalShopOffer = new LocalShopsOffers();
            if (objLocalShopOffer != null)
            {
                if (objLocalShopOffer.AllLocalShops != null && objLocalShopOffer.AllLocalShops.Count > 0)
                {
                    foreach (AllLocalShops objAllLocalShop in objLocalShopOffer.AllLocalShops)
                    {
                        objAllLocalShop.ImgUrl = objAllLocalShop.ImgUrl.Replace("~", Convert.ToString(ConfigurationManager.AppSettings["AllShopsImagePath"]));
                    }
                }

                if (objLocalShopOffer.Offers != null && objLocalShopOffer.Offers.Count > 0)
                {
                    foreach (PublicOffer objPublicOffer in objLocalShopOffer.Offers)
                    {

                        objPublicOffer.ImageURL = objPublicOffer.ImageURL.Replace("~", Convert.ToString(ConfigurationManager.AppSettings["PublicOfferImagePath"])); ;
                    }
                }
            }
            return objLocalShopOffer;
        }

        #endregion

        #region IDealLocalService Members


        public ShoppingListPublic GetPublicShoppingList(string username)
        {
            int shopId = 0;
            string postcode = !string.IsNullOrEmpty(username) ? (username != Common.Common.GeneralUserName ? PublicBA.GetPostCode(username) : Common.Common.GeneralUserPostCode) : string.Empty;
            ShoppingListPublic shoppingList_Public = new ShoppingListPublic();
            List<ShoppingList> lstPublicShoppingList = PublicShoppingBA.GetPublicShoppingList(username);
            List<LocalShop> lstLocalShops = new List<LocalShop>();
            if (lstPublicShoppingList != null && lstPublicShoppingList.Count > 0)
            {
                foreach (ShoppingList objShopping in lstPublicShoppingList)
                {
                    objShopping.ShopImgUrl = objShopping.ShopImgUrl.Replace("~", Convert.ToString(ConfigurationManager.AppSettings["AllShopsImagePath"]));
                }
            }

            if (lstLocalShops != null && lstLocalShops.Count > 0)
            {
                foreach (LocalShop objLocalShop in lstLocalShops)
                {
                    objLocalShop.ImgUrl = objLocalShop.ImgUrl.Replace("~", Convert.ToString(ConfigurationManager.AppSettings["PublicOfferImagePath"])); ;
                }
            }
            shoppingList_Public.shoppingList = lstPublicShoppingList.Where(m => m.ShopId == shopId || shopId == 0).ToList();
            shoppingList_Public.LocalShop = lstLocalShops.Where(m => shoppingList_Public.shoppingList.Where(m1 => m1.ShopId == m.ID).Count() > 0).ToList();


            return shoppingList_Public;
        }

        #endregion

        #region IDealLocalService Members


        public int Public_AddToShoppingList(string UserId, int Shop_ID, float Offer_Rate, string Product, int Qty, float VAT, string Volume)
        {
            ShoppingItem objShoppingItem = new ShoppingItem()
            {
                UserId = UserId,
                Shop_ID = Shop_ID,
                Offer_Rate = Offer_Rate,
                Product = Product,
                Qty = Qty,
                VAT = VAT,
                Volume = Volume
            };
            return PublicBA.AddToShoppingList(objShoppingItem);
        }

        public string CreateAccountPublicUser(string FirstName, string LastName, string ContactNumber, string UserName, string Postcode, string Password)
        {
            PublicUser publicUser = new PublicUser() { FirstName = FirstName, LastName = LastName, ContactNumber = ContactNumber, EmailAddress = UserName, PostCode = Postcode, Password = Password };
            return PublicBA.CreateAccountPublicUser(publicUser);
        }

        public List<Department> GetMenuDetails()
        {
            string excelPath = System.Web.Hosting.HostingEnvironment.MapPath("/Content/CategoryMenu/MenuProducts.xls");
            return MenuBA.GetMenuDetails(excelPath);
        }

        public LocalShopsOffers Public_SearchResults(string query, int cashandcarryID, int pindex, string CurrPostCode, string radius)
        {
            LocalShopsOffers objlocalShopOffer = new LocalShopsOffers();
            //objlocalShopOffer = PublicBA.GetPublicOffers(new PublicOffersFilterCriteria() { Product = query, ShopId = cashandcarryID, PageIndex = pindex, PostCode = CurrPostCode, Radius = Convert.ToDouble(radius) });
            objlocalShopOffer.AllLocalShops = PublicBA.GetLocalShops(CurrPostCode, radius).AllLocalShops;
            if (objlocalShopOffer != null)
            {
                if (objlocalShopOffer.AllLocalShops != null && objlocalShopOffer.AllLocalShops.Count > 0)
                {
                    foreach (AllLocalShops objAllLocalShop in objlocalShopOffer.AllLocalShops)
                    {
                        objAllLocalShop.ImgUrl = objAllLocalShop.ImgUrl.Replace("~", Convert.ToString(ConfigurationManager.AppSettings["AllShopsImagePath"]));
                    }
                }

                if (objlocalShopOffer.Offers != null && objlocalShopOffer.Offers.Count > 0)
                {
                    foreach (PublicOffer objPublicOffer in objlocalShopOffer.Offers)
                    {

                        objPublicOffer.ImageURL = objPublicOffer.ImageURL.Replace("~", Convert.ToString(ConfigurationManager.AppSettings["PublicOfferImagePath"])); ;
                    }
                }
            }

            return objlocalShopOffer;
        }

        public bool Public_ClearShoppingList(string username)
        {
            return PublicBA.ClearShoppingList(username);
        }

        public List<string> Public_GetSuggestion(string query)
        {
            List<string> lstString = new List<string>();
            lstString = PublicBA.GetSuggestion(query);
            return lstString;
        }
        #endregion

        #region Trade Methods


        public string TradeLogin_Validuser(string username, string password)
        {
            TradeUser objTradeUser = new TradeUser();
            objTradeUser.UserName = username;
            objTradeUser.TradePassword = password;
            return TradeBA.IsValidTradeUser(objTradeUser);
        }

        public TradeUser GetTradeUserDetails(string objUserName, string objPassword)
        {
            return TradeBA.GetTradeUserDetails(objUserName);
        }

        public string CreateAccountTradeUser(string FirstName, string LastName, string ContactNumber, string EmailAddress, string BusinessName)
        {
            TradeUser tradeUser = new TradeUser() { FirstName = FirstName, LastName = LastName, ContactNumber = ContactNumber, EmailAddress = EmailAddress, BusinessName = BusinessName };
            return TradeBA.CreateAccountTradeUser(tradeUser);
        }

        public List<CashandCarryShop> GetTradeShops()
        {
            List<CashandCarryShop> lstCashandCarryShop = new List<CashandCarryShop>();
            CashandCarryOffers objCashandCarryOffers = new CashandCarryOffers();
            objCashandCarryOffers = TradeBA.GetTradeOffers();
            if (objCashandCarryOffers != null && objCashandCarryOffers.CashandCarry != null)
            {
                foreach (CashandCarryShop objCashandCarryShop in objCashandCarryOffers.CashandCarry)
                {
                    objCashandCarryShop.ImgUrl = objCashandCarryShop.ImgUrl.Replace("~", Convert.ToString(ConfigurationManager.AppSettings["AllShopsImagePath"]));
                }
            }
            lstCashandCarryShop = objCashandCarryOffers.CashandCarry;
            return lstCashandCarryShop;
        }

        public CashandCarryOffers GetTradeOffers(int PageIndex, int ShopId, string Product, int PageSize)
        {
            TradeOffersFilterCriteria objTradeOffersFilter = new TradeOffersFilterCriteria() { PageIndex = PageIndex, ShopId = ShopId, Product = Product, Pagesize = PageSize };
            CashandCarryOffers objCashandCarryOffers = new CashandCarryOffers();
            objCashandCarryOffers = TradeBA.GetTradeOffers(objTradeOffersFilter);
            if (objCashandCarryOffers != null && objCashandCarryOffers.CashandCarry != null)
            {
                foreach (CashandCarryShop objCashandCarryShop in objCashandCarryOffers.CashandCarry)
                {
                    objCashandCarryShop.ImgUrl = objCashandCarryShop.ImgUrl.Replace("~", Convert.ToString(ConfigurationManager.AppSettings["AllShopsImagePath"]));
                }
            }
            return objCashandCarryOffers;
        }

        public List<string> Trade_GetSuggestion(string query)
        {
            List<string> lstString = new List<string>();
            lstString = TradeBA.GetSuggestion(query);
            return lstString;
        }

        public bool Trade_ClearShoppingList(string username)
        {
            return TradeBA.ClearShoppingList(username);
        }

        public int Trade_AddToShoppingList(string UserId, int Shop_ID, float Offer_Rate, string Product, int Qty, float VAT, string Volume)
        {
            ShoppingItem objShoppingItem = new ShoppingItem()
            {
                UserId = UserId,
                Shop_ID = Shop_ID,
                Offer_Rate = Offer_Rate,
                Product = Product,
                Qty = Qty,
                VAT = VAT,
                Volume = Volume
            };
            return TradeBA.AddToShoppingList(objShoppingItem);
        }

        public ShoppingListTrade GetTradeShoppingList(string username)
        {
            int shopId = 0;
            string postcode = !string.IsNullOrEmpty(username) ? (username != Common.Common.GeneralUserName ? PublicBA.GetPostCode(username) : Common.Common.GeneralUserPostCode) : string.Empty;
            ShoppingListTrade shoppingList_Trade = new ShoppingListTrade();
            List<ShoppingList> lstTradeShoppingList = TradeShoppingBA.GetTradeShoppingList(username);
            List<CashandCarryShop> lstCashAndCarryShops = TradeShoppingBA.GetCashAndCarryShops();
            if (lstTradeShoppingList != null && lstTradeShoppingList.Count > 0)
            {
                foreach (ShoppingList objShopping in lstTradeShoppingList)
                {
                    objShopping.ShopImgUrl = objShopping.ShopImgUrl.Replace("~", Convert.ToString(ConfigurationManager.AppSettings["AllShopsImagePath"]));
                }
            }

            if (lstCashAndCarryShops != null && lstCashAndCarryShops.Count > 0)
            {
                foreach (CashandCarryShop  objLocalShop in lstCashAndCarryShops)
                {
                    objLocalShop.ImgUrl = objLocalShop.ImgUrl.Replace("~", Convert.ToString(ConfigurationManager.AppSettings["PublicOfferImagePath"])); ;
                }
            }
            shoppingList_Trade.shoppingList  = lstTradeShoppingList.Where(m => m.ShopId == shopId || shopId == 0).ToList();
            shoppingList_Trade.CashandCarry = lstCashAndCarryShops.Where(m => shoppingList_Trade.shoppingList.Where(m1 => m1.ShopId == m.ID).Count() > 0).ToList();

            return shoppingList_Trade;
        }


        #endregion
    }


    public class ShoppingListPublic
    {

        public ShoppingListPublic()
        {
            shoppingList = new List<ShoppingList>();
            LocalShop = new List<LocalShop>();
        }

        public List<LocalShop> LocalShop { get; set; }
        public List<ShoppingList> shoppingList { get; set; }

    }


    public class ShoppingListTrade
    {

        public ShoppingListTrade()
        {
            shoppingList = new List<ShoppingList>();
            CashandCarry = new List<CashandCarryShop>();
        }

        public List<CashandCarryShop> CashandCarry { get; set; }
        public List<ShoppingList> shoppingList { get; set; }

    }
}
