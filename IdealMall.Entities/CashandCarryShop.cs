using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;  // Added by Varadha

namespace IdealMall.Entities
{
    public class CashandCarryShop
    {
        public int ID { get; set; }
        [Display(Name="SHOP")] // Added by Varadha
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public string Type { get; set; }
        public string LiveAvailable { get; set; }
        public string Expiry_Date { get; set; }

        public class Comparer : IEqualityComparer<CashandCarryShop>
        {
            public bool Equals(CashandCarryShop x, CashandCarryShop y)
            {
                return x.ID == y.ID;
                   
            }

            public int GetHashCode(CashandCarryShop obj)
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
