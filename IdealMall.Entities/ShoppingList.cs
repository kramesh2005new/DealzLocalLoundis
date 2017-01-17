using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using DORM;

namespace IdealMall.Entities
{
    public class ShoppingList
    {
        public int ShopId { get; set; }

        [Display(Name = "Product")]
        public string Product { get; set; }

        [Display(Name = "Price (£)")]
        public decimal OfferRate { get; set; }

        [Display(Name = "Volume")]
        public string Volume { get; set; }

        [Display(Name = "Quantity")]
        public string Qty { get; set; }

        [Display(Name = "VAT")]
        public decimal VAT { get; set; }

        [Display(Name = "SubTotal (£)")]
        public decimal Total { get; set; }

        [Display(Name = "Was (£)")]
        public string Previous_Prize { get; set; }

        public string ShopName { get; set; }

        public string ShopImgUrl { get; set; }

        public string Additional_Info { get; set; }

        public string Product_ImgUrl { get; set; }

        public string Shop_Orginal_Name { get; set; }

        public string Shop_CheckOut_UrL { get; set; }

        public string Offer_Date { get; set; }
    }

    public class ShoppingItem
    {
        public string UserId { get; set; }
        public Int32 Shop_ID { get; set; }
        public float Offer_Rate { get; set; }
        public string Product { get; set; }
        public int Qty { get; set; }
        public float VAT { get; set; }
        public string Volume { get; set; }

    }

    public class ShoppingSummary
    {
        public string ShopName { get; set; }
        public string ShopImgUrl { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
    }
}
