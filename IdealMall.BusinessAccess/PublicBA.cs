using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdealMall.Common;
using IdealMall.DataAccess;
using IdealMall.Entities;
namespace IdealMall.BusinessAccess
{
    //public static class Database
    //{
    //    public static string ConnectionString
    //    {
    //        set
    //        {
    //            DbConnection.ConnectionString = value;
    //        }
    //    }

    //}
    public class PublicBA
    {
        public static List<string> GetSuggestion(string searchText)
        {
            return PublicDA.GetSuggestion(searchText);
        }

        public static LocalShopsOffers GetHomeOffers(PublicOffersFilterCriteria filter)
        {
            return PublicDA.GetHomeOffers(filter);
        }

        public static LocalShopsOffers GetPublicOffers(PublicOffersFilterCriteria filter)
        {
            return PublicDA.GetPublicOffers(filter);

        }
        public static LocalShopsOffers GetLocalShops(string Postcode)
        {
            return PublicDA.GetLocalShops(Postcode);
        }
        public static LocalShopsOffers GetLocalShops(string PostCode, string radius)
        {
            LatLng latLng = GoogleMapsApiHelper.GetLatLng(PostCode + ",UK");

            if (latLng != null && latLng.Latitude == 0 && latLng.Longitude == 0)
            {
                var postCodes = PublicDA.GetPostCodesLike(PostCode);
                if (postCodes.Count > 0)
                {
                    latLng.Latitude = Convert.ToDecimal(postCodes.Average(a => a.latitude));
                    latLng.Longitude = Convert.ToDecimal(postCodes.Average(a => a.longitude));
                }
            }
            //  return new LocalShopsOffers();
            return PublicDA.GetLocalShops(latLng, radius);
        }
        //public static LocalShopsOffers GetLocalShops()
        //{
        //    return PublicDA.GetLocalShops("RM94SH");
        //}

        public static List<Offer> GetProductOffers(string Product)
        {
            return PublicDA.GetProductOffers(Product);

        }

        public static string CreateAccountPublicUser(PublicUser publicUser)
        {
            return PublicDA.CreateAccountPublicUser(publicUser);

        }

        public static PublicUser GetPublicUserDetails(string username, string password)
        {
            return PublicDA.GetPublicUserDetails(username, password);
        }

        public static PublicUser GetPublicUserDetails(string username)
        {
            return PublicDA.GetPublicUserDetails(username);
        }

        public static Int32 GetShoppingListCount(string userID)
        {
            return PublicDA.GetShoppingListCount(userID);


        }

        public static Int32 AddToShoppingList(ShoppingItem item)
        {
            return PublicDA.AddToShoppingList(item);
        }

        public static string IsValidPublicUser(PublicUser publicUser)
        {
            return PublicDA.IsValidPublicUser(publicUser);
        }

        //public static List<AllLocalShops> GetAllLocalShops()
        //{
        //    return PublicDA.GetAllLocalShops();
        //}
        public static bool ClearShoppingList(string UserId)
        {
            return PublicDA.ClearShoppingList(UserId);
        }

        public static List<Offer> GetPriceCompare(string Product, string PostCode, string radius, LatLng lanlat)
        {
            return PublicDA.GetPriceCompare(Product, PostCode, radius, lanlat);
        }

        public static string ChangePassword(PublicUser publicUser)
        {
            return PublicDA.ChangePassword(publicUser);
        }
        public static string GetPostCode(string EmailAddress)
        {
            return PublicDA.GetPostCode(EmailAddress);
        }
        public static List<string> GetPostCodeSuggestions(string PostCode)
        {
            return PublicDA.GetPostCodeSuggestions(PostCode);
        }
    }
}
