using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations; // Added by Varadha
using DORM;

namespace IdealMall.Entities
{
    [SP("GetOfferProducts_Public")]
    public class PublicOffers
    {
        [RSIndex(1)]
        public List<PublicOffer> Offers { get; set; }
        [RSIndex(2)]
        public int Totalcount { get; set; }
        [RSIndex(3)]
        public List<LocalShop> local { get; set; }
    }

    public class PublicOffersFilterCriteria
    {
        public int PageIndex { get; set; }
        public int ShopId { get; set; }
        public string Product { get; set; }
        public int Pagesize { get; set; }
        public string Offer_Mode { get; set; }
    }
    public class PublicOffer
    {
        public int ID { get; set; }


        public LocalShop Shop { get; set; }
        public string Category { get; set; }
        public string Product { get; set; }

        [Display(Name = "OFFER RATE (£)")] // Added by Varadha
        public float OfferRate { get; set; }

        [Display(Name = "Volume")] // Added by Varadha
        public string Volume { get; set; }
        public string PostCode { get; set; }
        public string ImageURL { get; set; }

        [Display(Name = "Offer Rate%")] // Added by Varadha
        public float OfferRatePercent { get; set; }
        [Display(Name = "POR (%)")]
        public float POR { get; set; }

        public string Sell_Price { get; set; }
        public float VAT { get; set; }
        [Display(Name = "ADDITIONAL OFFER")]
        public string Additional_offer { get; set; }
        public string OfferEnddate { get; set; }
        public decimal? Previous_Prize { get; set; }
        public string Shop_Orginal_Name { get; set; }
        public string Offer_Mode { get; set; }
    }
    [SP("GetOfferProducts_Public")]
    public class LocalShopsOffers
    {
        public string PostCode { get; set; }
        public LocalShopsOffers()
        {
            Offers = new List<PublicOffer>();
            Products = new List<Product>();
            local = new List<LocalShop>();
            AllLocalShops = new List<AllLocalShops>();
        }
        [RSIndex(1)]
        public List<PublicOffer> Offers { get; set; }
        [RSIndex(2)]
        public int Totalcount { get; set; }
        [RSIndex(3)]
        public List<LocalShop> local { get; set; }
        public List<Product> Products { get; set; }
        public List<AllLocalShops> AllLocalShops { get; set; }
        public string PostCodeToDisplayInPublicIndex { get; set; }
        public string UserName { get; set; }
        public int Pages
        {
            get
            {
                return (Totalcount / 10) + ((Totalcount % 10) > 0 ? 1 : 0);
            }
        }
        public Pager Pager { get; set; }
    }
    public class Pager
    {
        public Pager(int totalItems, int? page, int pageSize = 12)
        {
            // calculate total, start and end pages
            var totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            var currentPage = page != null ? (int)page : 1;
            var startPage = currentPage - 5;
            var endPage = currentPage + 4;
            if (startPage <= 0)
            {
                endPage -= (startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }

        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
    }
}
