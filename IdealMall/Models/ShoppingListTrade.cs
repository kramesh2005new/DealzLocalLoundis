using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdealMall.Entities;
using IdealMall.Common;

namespace IdealMall.Models
{
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
