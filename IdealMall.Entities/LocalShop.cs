using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;  // Added by Varadha

namespace IdealMall.Entities
{
    public class LocalShop
    {
        public int ID { get; set; }
        [Display(Name="SHOP")] // Added by Varadha
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public string Shop_CheckOut_UrL { get; set; }
        public string Shop_Orginal_Name { get; set; }

        public class Comparer : IEqualityComparer<LocalShop>
        {
            public bool Equals(LocalShop x, LocalShop y)
            {
                return x.ID == y.ID;
                   
            }

            public int GetHashCode(LocalShop obj)
            {
                unchecked  // overflow is fine
                {
                    int hash = 17;
                    hash = hash * 23 + (obj.ID.ToString() ?? "").GetHashCode();
                    hash = hash * 23 + (obj.Name ?? "").GetHashCode();
                    hash = hash * 23 + (obj.ImgUrl ?? "").GetHashCode();
                    return hash;
                }
            }
        }
    }
    
}
