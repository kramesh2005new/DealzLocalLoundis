using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using IdealMall.DataAccess;
using IdealMall.Entities;

namespace IdealMall.BusinessAccess
{
    public static class PublicShoppingBA
    {
        public static List<ShoppingList> GetPublicShoppingList(string UserId)
        {
            List<ShoppingList> lstShoppingLst = new List<ShoppingList>();
            lstShoppingLst = PublicShoppingDA.GetPublicShoppingList(UserId);

            if (lstShoppingLst != null)
            {
                foreach (ShoppingList objList in lstShoppingLst)
                {
                    if (!string.IsNullOrWhiteSpace(objList.Additional_Info))
                    {
                        //HtmlString strHtmlString = new HtmlString("Additional Offer : " + objList.Additional_Info);
                        objList.Additional_Info = objList.Additional_Info;
                    }
                }
            }
            return lstShoppingLst;
        }

        public static List<LocalShop> GetLocalShops()
        {
            return PublicShoppingDA.GetLocalShops();
        }

        public static bool sendMail(string sender, string recipient, string cc, string subject, string body)
        {
            MailMessage objMessage = new MailMessage();
            objMessage.From = new MailAddress(sender);
            objMessage.To.Add(new MailAddress(recipient));
            objMessage.CC.Add(new MailAddress(cc));
            objMessage.Subject = subject;
            objMessage.Body = body;
            objMessage.IsBodyHtml = true;
            Mailer objMailer = new Mailer();
            return objMailer.sendMail(objMessage);
        }

        public static bool Delete_ShoppingList(string Product, string Username, string volume, string Shop_id)
        {
            return PublicShoppingDA.Delete_ShoppingList(Product, Username, volume, Shop_id);
        }

        public static bool Delete_ShoppingListByShop(string Username, string Shop_id)
        {
            return PublicShoppingDA.Delete_ShoppingListbyShop(Username, Shop_id);
        }

        public static bool Update_ShoppingList(string Product, string Username, string volume, string Shop_id, string qty)
        {
            return PublicShoppingDA.Update_ShoppingList(Product, Username, volume, Shop_id, qty);
        }

    }
}
