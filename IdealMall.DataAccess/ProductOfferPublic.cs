using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdealMall.DataAccess
{
    public class ProductOfferPublic
    {
        public int ShopId { get; set; }
        public string ShopName { get; set; }
        public string ShopImgUrl { get; set; }
        public string ShopPostCode { get; set; }

        public string ProductName { get; set; }

        public decimal OfferRate { get; set; }
        public string Volume { get; set; }
        public decimal SellPrice { get; set; }
        public decimal POR { get; set; }
        public string AdditionalOffer { get; set; }
        public string ProductImageUrl { get; set; }
        public DateTime? OfferEndDate { get; set; }
        public decimal? Previous_Prize { get; set; }
        public string Shop_Orginal_Name { get; set; }
        public string Offer_Mode { get; set; }
    }
}
