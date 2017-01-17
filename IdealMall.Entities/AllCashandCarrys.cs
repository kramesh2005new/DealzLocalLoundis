using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DORM;

namespace IdealMall.Entities
{
    [SP("Get_CashAndCarrys")]
    public class AllCashandCarrys
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string ImgUrl { get; set; }
    }

}
