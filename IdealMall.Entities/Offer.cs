using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations; // Added by Varadha
using DORM;

namespace IdealMall.Entities
{
    [SP("GetOfferProducts_Trade")]
    public class TradeOffers
    {
        [RSIndex(1)]
        public List<Offer> Offers { get; set; }
        [RSIndex(2)]
        public int Totalcount { get; set; }
        [RSIndex(3)]
        public List<CashandCarryShop> CashandCarry { get; set; }
    }

    public class TradeOffersFilterCriteria
    {
        public int PageIndex { get; set; }
        public int ShopId { get; set; }
        public string Product { get; set; }
        public int Pagesize { get; set; }
    }
    
    public class Offer
    {
        public int ID { get; set; }

       
        public CashandCarryShop Shop { get; set; }
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
    }
    [SP("GetOfferProducts_Trade")]
    public class CashandCarryOffers
    {
        public CashandCarryOffers()
        {
            Offers = new List<Offer>();
            Products = new List<Product>();
            CashandCarry = new List<CashandCarryShop>();
            AllCashandCarrys = new List<AllCashandCarrys>();
        }
         [RSIndex(1)]
        public List<Offer> Offers { get; set; }
         [RSIndex(2)]
         public int Totalcount { get; set; }
         [RSIndex(3)]
         public List<CashandCarryShop> CashandCarry { get; set; }
        public List<Product> Products { get; set; }
        public List<AllCashandCarrys> AllCashandCarrys { get; set; }

        public int Pages
        {
            get
            {
              return  (Totalcount / 10) + ((Totalcount % 10) > 0 ? 1 : 0);
            }
        }
        public void GensampleData()
        {
            CashandCarry.Add(new CashandCarryShop()
            {
                ID = 1,
                Name = "British Grocery",
                ImgUrl = "british.png"
            });
            CashandCarry.Add(new CashandCarryShop()
            {
                ID = 2,
                Name = "Best Way",
                ImgUrl = "bestway.png"
            });
            CashandCarry.Add(new CashandCarryShop()
            {
                ID = 3,
                Name = "parfets",
                ImgUrl = "parfets.png"
            });
            CashandCarry.Add(new CashandCarryShop()
            {
                ID = 4,
                Name = "Millenium Group",
                ImgUrl = "GroupLogo.gif"
            });
            Products.Add(new Product()
            {
                ID = 1,
                Name = "Tropicana"
            });
            Products.Add(new Product()
            {
                ID = 2,
                Name = "Turmeric powder"
            });
            Products.Add(new Product()
            {
                ID = 3,
                Name = "Appy"
            });
            Products.Add(new Product()
            {
                ID = 4,
                Name = "Tooth paste"
            });
            Products.Add(new Product()
            {
                ID = 5,
                Name = "Tropical Fruit Mixes"
            });
            Products.Add(new Product()
            {
                ID = 6,
                Name = "Tapioca chips"
            });
            Products.Add(new Product()
            {
                ID = 14,
                Name = "Coca cola"
            });


            Offers.Add(new Offer()
            {
                ID = 1,
                Category = "juice",
                ImageURL = "1.jpg",
                OfferRate = 2.30f,
                PostCode = "LS2 7QS",
                Product = "Tropicana",
                Shop = new CashandCarryShop()
                {
                    ID = 2,
                    Name = "Best Way",
                    ImgUrl = "bestway.png"
                },
                Volume = "2*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 2,
                Category = "juice",
                ImageURL = "1.jpg",
                OfferRate = 2.00f,
                PostCode = "LE2 7QS",
                Product = "Tropicana",
                Shop = new CashandCarryShop()
                {
                    ID = 2,
                    Name = "Best Way",
                    ImgUrl = "bestway.png"
                },
                Volume = "12*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 3,
                Category = "juice",
                ImageURL = "1.jpg",
                OfferRate = 2.35f,
                PostCode = "LS2 7QS",
                Product = "Tropicana",
                Shop = new CashandCarryShop()
                {
                    ID = 4,
                    Name = "Millenium Group",
                    ImgUrl = "GroupLogo.gif"
                },
                Volume = "4*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 4,
                Category = "juice",
                ImageURL = "1.jpg",
                OfferRate = 2.30f,
                PostCode = "LS2 7QS",
                Product = "Tropicana",
                Shop = new CashandCarryShop()
                {
                    ID = 1,
                    Name = "British Grocery",
                    ImgUrl = "british.png"
                },
                Volume = "2*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 5,
                Category = "juice",
                ImageURL = "1.jpg",
                OfferRate = 2.20f,
                PostCode = "LS2 7QS",
                Product = "Tropicana",
                Shop = new CashandCarryShop()
                {
                    ID = 1,
                    Name = "British Grocery",
                    ImgUrl = "british.png"
                },
                Volume = "3*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 6,
                Category = "juice",
                ImageURL = "1.jpg",
                OfferRate = 1.90f,
                PostCode = "LS2 7QS",
                Product = "Tropicana",
                Shop = new CashandCarryShop()
                {
                    ID = 3,
                    Name = "parfets",
                    ImgUrl = "parfets.png"
                },
                Volume = "5*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 2,
                Category = "juice",
                ImageURL = "1.jpg",
                OfferRate = 2.00f,
                PostCode = "LE2 7QS",
                Product = "Tropicana",
                Shop = new CashandCarryShop()
                {
                    ID = 2,
                    Name = "Best Way",
                    ImgUrl = "bestway.png"
                },
                Volume = "12*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 3,
                Category = "juice",
                ImageURL = "1.jpg",
                OfferRate = 2.35f,
                PostCode = "LS2 7QS",
                Product = "Tropicana",
                Shop = new CashandCarryShop()
                {
                    ID = 4,
                    Name = "Millenium Group",
                    ImgUrl = "GroupLogo.gif"
                },
                Volume = "4*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 4,
                Category = "juice",
                ImageURL = "1.jpg",
                OfferRate = 2.30f,
                PostCode = "LS2 7QS",
                Product = "Tropicana",
                Shop = new CashandCarryShop()
                {
                    ID = 1,
                    Name = "British Grocery",
                    ImgUrl = "british.png"
                },
                Volume = "2*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 2,
                Category = "juice",
                ImageURL = "1.jpg",
                OfferRate = 2.00f,
                PostCode = "LE2 7QS",
                Product = "Tropicana",
                Shop = new CashandCarryShop()
                {
                    ID = 2,
                    Name = "Best Way",
                    ImgUrl = "bestway.png"
                },
                Volume = "12*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 3,
                Category = "juice",
                ImageURL = "1.jpg",
                OfferRate = 2.35f,
                PostCode = "LS2 7QS",
                Product = "Tropicana",
                Shop = new CashandCarryShop()
                {
                    ID = 4,
                    Name = "Millenium Group",
                    ImgUrl = "GroupLogo.gif"
                },
                Volume = "4*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 4,
                Category = "juice",
                ImageURL = "1.jpg",
                OfferRate = 2.30f,
                PostCode = "LS2 7QS",
                Product = "Tropicana",
                Shop = new CashandCarryShop()
                {
                    ID = 1,
                    Name = "British Grocery",
                    ImgUrl = "british.png"
                },
                Volume = "2*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 14,
                Category = "juice",
                ImageURL = "coke.jpg",
                OfferRate = 2.30f,
                PostCode = "LS2 7QS",
                Product = "Coca cola",
                Shop = new CashandCarryShop()
                {
                    ID = 1,
                    Name = "British Grocery",
                    ImgUrl = "british.png"
                },
                Volume = "2*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 14,
                Category = "juice",
                ImageURL = "coke.jpg",
                OfferRate = 2.30f,
                PostCode = "LS2 7QS",
                Product = "Coca cola",
                Shop = new CashandCarryShop()
                {
                    ID = 4,
                    Name = "Millenium Group",
                    ImgUrl = "GroupLogo.gif"
                },
                Volume = "2*100ml"
            });
            Offers.Add(new Offer()
            {
                ID = 14,
                Category = "juice",
                ImageURL = "coke.jpg",
                OfferRate = 2.30f,
                PostCode = "LS2 7QS",
                Product = "Coca cola",
                Shop = new CashandCarryShop()
                {
                    ID = 3,
                    Name = "parfets",
                    ImgUrl = "parfets.png"
                },
                Volume = "2*100ml"
            });
        }
    }
}
