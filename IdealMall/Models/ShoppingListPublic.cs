using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdealMall.Entities;
using IdealMall.Common;

namespace IdealMall.Models
{
    public class ShoppingListPublic
    {

        public ShoppingListPublic()
        {
            shoppingList = new List<ShoppingList>();
            LocalShop = new List<LocalShop>();
        }

        public List<LocalShop> LocalShop { get; set; }
        public List<ShoppingList> shoppingList { get; set; }
        
    }
}
