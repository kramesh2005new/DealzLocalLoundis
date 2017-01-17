using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DORM;

namespace IdealMall.Entities
{
    [SP("Get_Retails")]
    public class AllLocalShops
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
        public string Shop_ImgUrl { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public decimal Miles { get; set; }
        public string Shop_Orginal_Name { get; set; }
        public string Services { get; set; }
        public string Services_Text { get; set; }
        public string Shopping_Timing { get; set; }
        public string PhoneNumber { get; set; }
    }
}


